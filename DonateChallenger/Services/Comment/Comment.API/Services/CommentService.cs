using Comment.API.Data;
using Comment.API.Models.DTOs;
using Comment.API.Models.Responses;
using Comment.API.Repositories.Abstractions;
using Comment.API.Services.Abstractions;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Infrastructure.Services.Abstractions;

namespace Comment.API.Services;

public class CommentService : BaseDataService<AppDbContext>, ICommentService
{
    private readonly ICommentRepository _repository;
    public CommentService(
        IDbContextWrapper<AppDbContext> dbContext,
        ILogger<BaseDataService<AppDbContext>> logger,
        ICommentRepository repository)
        : base(dbContext, logger)
    {
        _repository = repository;
    }

    public async Task<GetPaginatedCommentsResponse<CommentDto>> GetPaginatedCommentsAsync(int currentPage, int commentsPerPage, long challengeId, string? userId = null)
    {
        return await ExecuteSafeAsync(async () =>
        {
            Logger.LogInformation($"{nameof(GetPaginatedCommentsAsync)} ---> {nameof(currentPage)}: {currentPage}; {nameof(commentsPerPage)}: {commentsPerPage}; {nameof(challengeId)}: {challengeId}");
            if (!GetPaginatedCommentsStateIsValid(currentPage, commentsPerPage, challengeId, userId))
            {
                Logger.LogError($"{nameof(GetPaginatedCommentsAsync)} ---> State is not valid");
                return new GetPaginatedCommentsResponse<CommentDto>
                {
                    Data = Enumerable.Empty<CommentDto>()
                };
            }

            var paginatedComments = await _repository.GetPaginated(currentPage, commentsPerPage, challengeId, userId);

            var totalCount = paginatedComments.TotalCount;
            var totalPages = PageCounter.Count(totalCount, commentsPerPage);
            var data = paginatedComments.Comments.Select(s => new CommentDto
            {
                CommentId = s.CommentId,
                ChallengeId = s.ChallengeId,
                Message = s.Message,
                UserId = s.UserId
            }).ToList();

            Logger.LogInformation($"{nameof(GetPaginatedCommentsAsync)} ---> {nameof(data)} amount: {data.Count()}; {nameof(totalPages)}: {totalPages}");
            return new GetPaginatedCommentsResponse<CommentDto>
            {
                Data = data,
                TotalCount = totalCount,
                CommentsPerPage = commentsPerPage,
                CurrentPage = currentPage,
                TotalPages = totalPages
            };
        });
    }

    public async Task<GetCommentByCommentIdResponse<CommentDto?>> GetCommentByCommentIdAsync(long commentId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            Logger.LogInformation($"{nameof(GetCommentByCommentIdAsync)} ---> {nameof(commentId)}: {commentId}");
            if (commentId <= 0)
            {
                Logger.LogError($"{nameof(GetCommentByCommentIdAsync)} ---> State is not valid");
                return new GetCommentByCommentIdResponse<CommentDto?> { Data = null };
            }

            var comment = await _repository.GetByCommentId(commentId);
            if (comment == null)
            {
                Logger.LogError($"{nameof(GetCommentByCommentIdAsync)} ---> Comment's not found");
                return new GetCommentByCommentIdResponse<CommentDto?> { Data = null };
            }

            Logger.LogInformation($"{nameof(GetCommentByCommentIdAsync)} ---> {nameof(comment)}: {nameof(comment.Message)}: {comment.Message}");
            return new GetCommentByCommentIdResponse<CommentDto?>
            {
                Data = new CommentDto
                {
                    CommentId = comment.CommentId,
                    ChallengeId = comment.ChallengeId,
                    Message = comment.Message,
                    UserId = comment.UserId
                }
            };
        });
    }

    public async Task<AddCommentResponse<long?>> AddCommentAsync(string userId, long challengeId, string message)
    {
        return await ExecuteSafeAsync(async () =>
        {
            Logger.LogInformation($"{nameof(AddCommentAsync)} ---> {nameof(userId)}: {userId}; {nameof(challengeId)}: {challengeId}; {nameof(message)}: {message}; ");
            if (!AddCommentStateIsValid(userId, challengeId, message))
            {
                Logger.LogError($"{nameof(AddCommentAsync)} ---> State is not valid");
                return new AddCommentResponse<long?> { Data = null };
            }

            var result = await _repository.Add(userId, challengeId, message);

            Logger.LogInformation(!result.HasValue ?
                $"{nameof(AddCommentAsync)} ---> Error! Comment's not added"
                : $"{nameof(AddCommentAsync)} ---> Added comment id: {result.Value}");

            return new AddCommentResponse<long?> { Data = result };
        });
    }

    public async Task<UpdateCommentResponse<long?>> UpdateCommentAsync(long commentId, string userId, long challengeId, string message)
    {
        return await ExecuteSafeAsync(async () =>
        {
            Logger.LogInformation($"{nameof(UpdateCommentAsync)} ---> {nameof(commentId)}: {commentId}; {nameof(userId)}: {userId}; {nameof(challengeId)}: {challengeId}; {nameof(message)}: {message}; ");
            if (!UpdateCommentStateIsValid(commentId, userId, challengeId, message))
            {
                Logger.LogError($"{nameof(UpdateCommentAsync)} ---> State is not valid");
                return new UpdateCommentResponse<long?> { Data = null };
            }

            var result = await _repository.Update(commentId, userId, challengeId, message);

            Logger.LogInformation(!result.HasValue ?
                $"{nameof(UpdateCommentAsync)} ---> Error! Comment's not updated"
                : $"{nameof(UpdateCommentAsync)} ---> Updated comment id: {result.Value}");

            return new UpdateCommentResponse<long?> { Data = result };
        });
    }

    public async Task<DeleteCommentResponse<bool>> DeleteCommentAsync(long commentId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            Logger.LogInformation($"{nameof(DeleteCommentAsync)} ---> {nameof(commentId)}: {commentId}");
            if (commentId <= 0)
            {
                Logger.LogError($"{nameof(UpdateCommentAsync)} ---> State is not valid");
                return new DeleteCommentResponse<bool> { Data = false };
            }

            var result = await _repository.Delete(commentId);
            Logger.LogInformation($"{nameof(DeleteCommentAsync)} ---> {nameof(result)}: {result}");

            return new DeleteCommentResponse<bool> { Data = result };
        });
    }

    private bool GetPaginatedCommentsStateIsValid(int currentPage, int commentsPerPage, long challengeId, string? userId = null)
    {
        return currentPage >= 0
               && commentsPerPage > 0
               && challengeId > 0
               && (userId == null || userId.Length > 0);
    }

    private bool AddCommentStateIsValid(string userId, long challengeId, string message)
    {
        return !string.IsNullOrWhiteSpace(userId)
               && !string.IsNullOrWhiteSpace(message)
               && challengeId > 0;
    }

    private bool UpdateCommentStateIsValid(long commentId, string userId, long challengeId, string message)
    {
        return commentId > 0
               && !string.IsNullOrWhiteSpace(userId)
               && !string.IsNullOrWhiteSpace(message)
               && challengeId > 0;
    }
}
using Comment.API.Data;
using Comment.API.Data.Entities;
using Comment.API.Repositories.Abstractions;
using Infrastructure.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Comment.API.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CommentRepository> _logger;

    public CommentRepository(IDbContextWrapper<AppDbContext> context, ILogger<CommentRepository> logger)
    {
        _context = context.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedComments> GetPaginated(int currentPage, int commentsPerPage, long challengeId, string? userId = null)
    {
        _logger.LogInformation($"{nameof(GetPaginated)} ---> {nameof(currentPage)}: {currentPage}; {nameof(commentsPerPage)}: {commentsPerPage}; {nameof(challengeId)}: {challengeId}");
        var query = _context.Comments.AsQueryable();

        query = query.Where(q => q.ChallengeId == challengeId);

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(q => q.UserId == userId);
        }

        var totalCount = await query.LongCountAsync();

        query = query
            .Skip(currentPage * commentsPerPage)
            .Take(commentsPerPage);

        var usersIdsForFindingNicknames = await query
            .Select(s => s.UserId)
            .Distinct()
            .ToListAsync();

        var comments = await query.ToListAsync();

        _logger.LogInformation($"{nameof(GetPaginated)} ---> {nameof(totalCount)}: {totalCount}; {nameof(comments)} amount: {comments.Count};");

        return new PaginatedComments
        {
            Comments = comments,
            TotalCount = totalCount,
            UsersIdsForFindingNicknames = usersIdsForFindingNicknames
        };
    }

    public async Task<CommentEntity?> GetByCommentId(long commentId)
    {
        _logger.LogInformation($"{nameof(GetByCommentId)} ---> {nameof(commentId)}: {commentId}");
        var comment = await _context.Comments.FirstOrDefaultAsync(f => f.CommentId == commentId);
        if (comment == null)
        {
            _logger.LogWarning($"{nameof(GetByCommentId)} ---> Comment is not found");
            return null;
        }

        _logger.LogInformation($"{nameof(GetByCommentId)} ---> comment: {nameof(comment.Message)}: {comment.Message}; {nameof(comment.ChallengeId)}: {comment.ChallengeId}; {nameof(comment.UserId)}: {comment.UserId}; {nameof(comment.CommentId)}: {comment.CommentId};");
        return comment;
    }

    public async Task<long?> Add(string userId, long challengeId, string message)
    {
        _logger.LogInformation($"{nameof(Add)} ---> {nameof(userId)}: {userId}; {nameof(challengeId)}: {challengeId}; {nameof(message)}: {message}; ");
        var result = await _context.AddAsync(new CommentEntity
        {
            UserId = userId,
            ChallengeId = challengeId,
            Message = message,
            Date = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return result?.Entity?.CommentId;
    }

    public async Task<long?> Update(long commentId, string userId, long challengeId, string message)
    {
        _logger.LogInformation($"{nameof(Update)} ---> {nameof(commentId)}: {commentId}; {nameof(userId)}: {userId}; {nameof(challengeId)}: {challengeId}; {nameof(message)}: {message}; ");

        var comment = await GetByCommentId(commentId);
        if (comment == null)
        {
            _logger.LogError($"{nameof(Update)} ---> Comment's not found. Returned null");
            return null;
        }

        comment.UserId = userId;
        comment.ChallengeId = challengeId;
        comment.Message = message;
        var result = _context.Update(comment);
        await _context.SaveChangesAsync();
        return result?.Entity?.CommentId;
    }

    public async Task<bool> Delete(long commentId)
    {
        _logger.LogInformation($"{nameof(Delete)} ---> {nameof(commentId)}: {commentId}");
        var comment = new CommentEntity { CommentId = commentId };

        _context.Entry(comment).State = EntityState.Deleted;
        await _context.SaveChangesAsync();

        return true;
    }
}
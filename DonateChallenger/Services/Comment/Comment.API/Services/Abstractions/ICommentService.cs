using Comment.API.Models.DTOs;
using Comment.API.Models.Responses;

namespace Comment.API.Services.Abstractions;

public interface ICommentService
{
    Task<GetPaginatedCommentsResponse<CommentDto>> GetPaginatedCommentsAsync(int currentPage, int commentsPerPage, long challengeId, string? userId = null);
    Task<GetCommentByCommentIdResponse<CommentDto?>> GetCommentByCommentIdAsync(long commentId);
    Task<AddCommentResponse<long?>> AddCommentAsync(string userId, long challengeId, string message);
    Task<UpdateCommentResponse<long?>> UpdateCommentAsync(long commentId, string userId, long challengeId, string message);
    Task<DeleteCommentResponse<bool>> DeleteCommentAsync(long commentId);
}
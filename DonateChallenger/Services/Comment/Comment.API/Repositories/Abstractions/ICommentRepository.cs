using Comment.API.Data;
using Comment.API.Data.Entities;

namespace Comment.API.Repositories.Abstractions;

public interface ICommentRepository
{
    Task<PaginatedComments> GetPaginated(int currentPage, int commentsPerPage, long challengeId, string? userId = null);
    Task<CommentEntity?> GetByCommentId(long commentId);
    Task<long?> Add(string userId, long challengeId, string message);
    Task<long?> Update(long commentId, string userId, long challengeId, string message);
    Task<bool> Delete(long commentId);
}
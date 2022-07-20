using Comment.API.Data.Entities;

namespace Comment.API.Data;

public class PaginatedComments
{
    public long TotalCount { get; set; }

    public IEnumerable<CommentEntity> Comments { get; set; } = null!;

    public IEnumerable<string> UsersIdsForFindingNicknames { get; set; } = null!;
}
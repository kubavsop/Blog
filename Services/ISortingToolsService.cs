using Blog.API.Common.Enums;
using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Services;

public interface ISortingToolsService
{
    Task<PostPagedList> GetPostPagedListAsync(IQueryable<Post> queryable, PostSorting sorting, int page, int size);
    IQueryable<Post> SortPosts(IQueryable<Post> queryable, PostSorting sorting);
    Task<List<Tag>> GetTagsAsync(IEnumerable<Guid> tagsId);
    Task<bool> HasUserLikedPostAsync(Guid postId);
    Task<User> GetUserWithLikedPostsAsync();
}
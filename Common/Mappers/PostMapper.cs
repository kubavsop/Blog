using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities;
using Blog.API.Entities.Database;

namespace Blog.API.Common.Mappers;

internal static class PostMapper
{
    public static PostPagedListDto PagedListPagedListDto(PostPagedList pagedList)
    {
        return new PostPagedListDto
        {
            Posts = PostsToPostsDto(pagedList.Posts),
            Pagination = new PageInfoDto
            {
                Count = pagedList.Pagination.Count,
                Current = pagedList.Pagination.Current,
                Size = pagedList.Pagination.Size
            }
        };
    }
    
    private static IEnumerable<PostDto> PostsToPostsDto(IEnumerable<PostInformation> posts)
    {
        return posts.Select(PostFullToPostDto);
    }

    public static PostFullDto PostFullToPostFullDto(PostInformation post)
    {
        return new PostFullDto
        {
            Id = post.Id,
            CreateTime = post.CreateTime,
            Title = post.Title,
            Description = post.Description,
            ReadingTime = post.ReadingTime,
            Image = post.Image,
            AuthorId = post.AuthorId,
            Author = post.Author,
            CommunityId = post.CommunityId,
            CommunityName = post.CommunityName,
            AddressId = post.AddressId,
            Likes = post.Likes,
            HasLike = post.HasLike,
            CommentsCount = post.CommentsCount,
            Tags = TagMapper.TagsToTagsDto(post.Tags),
            Comments = CommentMapper.CommentsToCommentsDto(post.Comments)
        };
    }
    public static PostResponseDto PostResponseToPostResponseDto(PostResponse postResponse)
    {
        return new PostResponseDto
        {
            PostId = postResponse.PostId
        };
    }

    public static CreatePost CreatePostDtoToCreatePost(CreatePostDto createPostDto)
    {
        return new CreatePost
        {
            Title = createPostDto.Title,
            Description = createPostDto.Description,
            ReadingTime = createPostDto.ReadingTime,
            Image = createPostDto.Image,
            AddressId = createPostDto.AddressId,
            Tags = createPostDto.Tags
        };
    }
    
    private static PostDto PostFullToPostDto(PostInformation post)
    {
        return new PostDto
        {
            Id = post.Id,
            CreateTime = post.CreateTime,
            Title = post.Title,
            Description = post.Description,
            ReadingTime = post.ReadingTime,
            Image = post.Image,
            AuthorId = post.AuthorId,
            Author = post.Author,
            CommunityId = post.CommunityId,
            CommunityName = post.CommunityName,
            AddressId = post.AddressId,
            Likes = post.Likes,
            HasLike = post.HasLike,
            CommentsCount = post.CommentsCount,
            Tags = TagMapper.TagsToTagsDto(post.Tags)
        };
    }
}
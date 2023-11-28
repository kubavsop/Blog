using Blog.API.Controllers.Dto.Requests;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities;

namespace Blog.API.Controllers.Mappers;

internal static class PostMapper
{
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
            Tags = createPostDto.Tags
        };
    }
}
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities;

namespace Blog.API.Controllers.Mappers;

internal static class AuthorMapper
{
    public static IEnumerable<AuthorDto> AuthorsToAuthorsDto(IEnumerable<Author> authors)
    {
        return authors.Select(AuthorToAuthorDto);
    }
    
    private static AuthorDto AuthorToAuthorDto(Author author)
    {
        return new AuthorDto
        {
            FullName = author.FullName,
            BirthDate = author.BirthDate,
            Gender = author.Gender,
            Posts = author.Posts,
            Likes = author.Likes,
            Created = author.Created
        };
    }
}
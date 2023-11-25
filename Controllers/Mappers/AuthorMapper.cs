using Blog.API.Controllers.Dto.Responses;
using Blog.API.Entities;

namespace Blog.API.Controllers.Mappers;

internal static class AuthorMapper
{
    public static AuthorDto AuthorToAuthorDto(Author author)
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

    public static IEnumerable<AuthorDto> AuthorsToAuthorsDto(IEnumerable<Author> authors)
    {
        return authors.Select(author => new AuthorDto
        {
            FullName = author.FullName,
            BirthDate = author.BirthDate,
            Gender = author.Gender,
            Posts = author.Posts,
            Likes = author.Likes,
            Created = author.Created
        });
    }
}
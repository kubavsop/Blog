using Blog.API.Entities;

namespace Blog.API.Services;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAuthorsAsync();
}
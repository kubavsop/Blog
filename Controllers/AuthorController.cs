using Blog.API.Common.Mappers;
using Blog.API.Controllers.Dto.Responses;
using Blog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;


[Route("api/author")]
[ApiController]
public class AuthorController: ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthorsAsync()
    {
        var authorsDto = AuthorMapper.AuthorsToAuthorsDto(await _authorService.GetAuthorsAsync());
        return Ok(authorsDto);
    }
}
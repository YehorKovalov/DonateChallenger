using System.Net;
using Comment.API.Models.DTOs;
using Comment.API.Models.Requests;
using Comment.API.Models.Responses;
using Comment.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Comment.API.Controllers;

[ApiController]
[Route(Defaults.DefaultRoute)]
[Scope("comment.bff")]
public class CommentBffController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentBffController(ICommentService commentService) => _commentService = commentService;

    [HttpPost]
    [ProducesResponseType(typeof(GetPaginatedCommentsResponse<CommentDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> PaginatedComments(GetPaginatedCommentsRequest request)
    {
        var result = await _commentService.GetPaginatedCommentsAsync(request.CurrentPage, request.CommentsPerPage, request.ChallengeId);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddCommentResponse<long?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Add(AddCommentRequest request)
    {
        var result = await _commentService.AddCommentAsync(request.UserId, request.ChallengeId, request.Message);
        return Ok(result);
    }
}
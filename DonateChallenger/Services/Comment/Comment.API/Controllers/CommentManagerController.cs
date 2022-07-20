using System.Net;
using Comment.API.Models.DTOs;
using Comment.API.Models.Requests;
using Comment.API.Models.Responses;
using Comment.API.Services.Abstractions;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Comment.API.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.ManagerMinimumPolicy)]
[Route(Defaults.DefaultRoute)]
[Scope("comment.manager")]
public class CommentManagerController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentManagerController(ICommentService commentService) => _commentService = commentService;

    [HttpGet]
    [ProducesResponseType(typeof(DeleteCommentResponse<bool>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(long commentId)
    {
        var result = await _commentService.DeleteCommentAsync(commentId);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UpdateCommentResponse<long?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateCommentRequest request)
    {
        var result = await _commentService.UpdateCommentAsync(request.CommentId, request.UserId, request.ChallengeId, request.Message);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetCommentByCommentIdResponse<CommentDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UserPaginatedComments(GetPaginatedCommentsRequest request)
    {
        var result = await _commentService.GetPaginatedCommentsAsync(request.CurrentPage, request.CommentsPerPage, request.ChallengeId, request.UserId);
        return Ok(result);
    }
}
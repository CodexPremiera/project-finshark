using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    // INITIAlIZE REPOSITORY  
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;
    private readonly UserManager<AppUser> _userManager;
    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
    {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
        _userManager = userManager;
    }
    
    // CONTROL HTTP METHODS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comments = await _commentRepo.GetAllAsync();
        comments.Select(s => s.ToCommentDto());
        
        return Ok(comments);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comment = await _commentRepo.GetByIdAsync(id);

        if (comment == null)
            return NotFound();

        return Ok(comment.ToCommentDto());
    }
    
    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (!await _stockRepo.StockExistAsync(stockId))
            return BadRequest("Stock does not exist");

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var newComment = commentDto.ToComment(stockId);
        newComment.AppUserId = appUser.Id;
        await _commentRepo.CreateAsync(newComment);
        
        return CreatedAtAction (
            nameof(GetById),  // Call `GetById` from above
            new { id = newComment.Id }, 
            newComment.ToCommentDto()  // Convert result to ToCommentDto 
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateCommentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comment = await _commentRepo.UpdateAsync(id, updateCommentDto.ToComment(id));
        return comment == null 
            ? NotFound("Comment not found") 
            : Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comment = await _commentRepo.DeleteAsync(id);
        return comment == null 
            ? NotFound("Comment not found") 
            : Ok(comment);
    }
}

using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    // INITIAlIZE REPOSITORY  
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;
    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
    }
    
    // CONTROL HTTP METHODS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepo.GetAllAsync();
        comments.Select(s => s.ToCommentDto());
        
        return Ok(comments);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await _commentRepo.GetByIdAsync(id);

        if (comment == null)
            return NotFound();

        return Ok(comment.ToCommentDto());
    }
    
    [HttpPost("{stockId}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
    {
        if (!await _stockRepo.StockExistAsync(stockId))
            return BadRequest("Stock does not exist");

        var newComment = commentDto.ToComment(stockId);
        await _commentRepo.CreateAsync(newComment);
        
        return CreatedAtAction (
            nameof(GetById),  // Call `GetById` from above
            new { id = newComment.Id }, 
            newComment.ToCommentDto()  // Convert result to ToCommentDto 
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateCommentDto)
    {
        var comment = await _commentRepo.UpdateAsync(id, updateCommentDto.ToComment(id));
        return comment == null 
            ? NotFound("Comment not found") 
            : Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var comment = await _commentRepo.DeleteAsync(id);
        return comment == null 
            ? NotFound("Comment not found") 
            : Ok(comment);
    }
}
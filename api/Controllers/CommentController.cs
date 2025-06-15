using api.Dtos.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IFMPService _fmpService;
    public CommentController(ICommentRepository commentRepo,
        IStockRepository stockRepo, UserManager<AppUser> userManager, IFMPService fmpService)
    {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
        _userManager = userManager;
        _fmpService = fmpService;
    }
    
    // CONTROL HTTP METHODS
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject queryObject)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comments = await _commentRepo.GetAllAsync(queryObject);
        var commentDto = comments.Select(s => s.ToCommentDto());

        return Ok(commentDto);
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
    
    [HttpPost("{symbol:alpha}")]
    public async Task<IActionResult> Create([FromRoute] string symbol, CreateCommentDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check if stock exist in local database
        var stock = await _stockRepo.GetBySymbolAsync(symbol);

        // Check if stock exists in IRL (via Financial Modeling API)
        if (stock == null)
        {
            stock = await _fmpService.FindStockBySymbolAsync(symbol);
            
            if (stock == null)
                return BadRequest("Stock does not exists");
            await _stockRepo.CreateAsync(stock);
        }
        
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var newComment = commentDto.ToComment(stock.Id);
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

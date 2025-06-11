using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    private readonly IStockRepository _stockRepo;
    public StockController(ApplicationDBContext context, IStockRepository stockRepo)
    {
        _stockRepo = stockRepo;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto createDto) // get id from body 
    {
        var newStock = createDto.ToStockFromCreateDto();
        await _stockRepo.CreateAsync(newStock);
        
        return CreatedAtAction (
            nameof(GetById),  // Call `GetById` from above
            new { id = newStock.Id }, 
            newStock.ToStockDto()  // Convert result to StockDto 
            );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _stockRepo.GetAllAsync();
        var stockDto = stocks.Select(s => s.ToStockDto());
        
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id) // get id from url
    {
        var stock = await _stockRepo.GetByIdAsync(id);
        return stock == null ? NotFound() : Ok(stock.ToStockDto());
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
    {
        var stock = await _stockRepo.UpdateAsync(id, updateDto);
        return stock == null ? NotFound() : Ok(stock.ToStockDto());
    }   

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stock = await _stockRepo.DeleteAsync(id);
        return stock == null ? NotFound() : NoContent();
    }
}
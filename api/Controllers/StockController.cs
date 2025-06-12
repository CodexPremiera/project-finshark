using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    // INITIAlIZE REPOSITORY  
    private readonly IStockRepository _stockRepo;
    public StockController(IStockRepository stockRepo)
    {
        _stockRepo = stockRepo;
    }

    
    // CONTROL HTTP METHODS
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto createDto) // get id from body 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var newStock = createDto.ToStock();
        await _stockRepo.CreateAsync(newStock);
        
        return CreatedAtAction (
            nameof(GetById),  // Call `GetById` from above
            new { id = newStock.Id }, 
            newStock.ToStockDto()  // Convert result to StockDto 
            );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var stocks = await _stockRepo.GetAllAsync(query);
        var stockDto = stocks.Select(s => s.ToStockDto());
        
        return Ok(stocks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id) // get id from url
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var stock = await _stockRepo.GetByIdAsync(id);
        return stock == null ? NotFound() : Ok(stock.ToStockDto());
    }
    
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stock = await _stockRepo.UpdateAsync(id, updateDto);
        return stock == null ? NotFound() : Ok(stock.ToStockDto());
    }   

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var stock = await _stockRepo.DeleteAsync(id);
        return stock == null ? NotFound() : NoContent();
    }
}
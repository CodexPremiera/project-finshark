using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _context.Stocks.ToListAsync();
        var stockDto = stocks.Select(s => s.ToStockDto());
        
        return Ok(stockDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id) // get id from url
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
            return NotFound();
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto createDto) // get id from body 
    {
        var newStock = createDto.ToStockFromCreateDto();
        
        await _context.Stocks.AddAsync(newStock);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction (
            nameof(GetById),  // Call `GetById` from above
            new { id = newStock.Id }, 
            newStock.ToStockDto()  // Convert result to StockDto 
            );
    }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
        if (stock == null)
            return NotFound();

        stock.Symbol = updateDto.Symbol;
        stock.CompanyName = updateDto.CompanyName;
        stock.Purchase = updateDto.Purchase;
        stock.LastDiv = updateDto.LastDiv;
        stock.Industry = updateDto.Industry;
        stock.MarketCap = updateDto.MarketCap;
        
        await _context.SaveChangesAsync();
        return Ok(stock.ToStockDto());
    }   

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
        if (stock == null)
            return NotFound();
        
        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}
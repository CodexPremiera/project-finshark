using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetAll()
    {
        var stocks = _context.Stocks.ToList()
            .Select(s => s.ToStockDto());
        
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id) // get id from url
    {
        var stock = _context.Stocks.Find(id);

        if (stock == null)
            return NotFound();
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockDto createDto) // get id from body 
    {
        var newStock = createDto.ToStockFromCreateDto();
        _context.Stocks.Add(newStock);
        _context.SaveChanges();
        return CreatedAtAction(
            nameof(GetById),  // Call `GetById` from above
            new { id = newStock.Id }, 
            newStock.ToStockDto()  // Convert result to StockDto 
            );
    }
    
    [HttpPut]
    [Route("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
    {
        var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
        if (stock == null)
            return NotFound();

        stock.Symbol = updateDto.Symbol;
        stock.CompanyName = updateDto.CompanyName;
        stock.Purchase = updateDto.Purchase;
        stock.LastDiv = updateDto.LastDiv;
        stock.Industry = updateDto.Industry;
        stock.MarketCap = updateDto.MarketCap;
        
        _context.SaveChanges();
        return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
        if (stock == null)
            return NotFound();
        
        _context.Stocks.Remove(stock);
        _context.SaveChanges();
        
        return NoContent();
    }
}
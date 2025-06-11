using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context;
    
    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<Stock> CreateAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stocks.Include(c => c.Comments).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockDto updateStockDto)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

        if (existingStock == null)
            return null;

        existingStock.Symbol = updateStockDto.Symbol;
        existingStock.CompanyName = updateStockDto.CompanyName;
        existingStock.Purchase = updateStockDto.Purchase;
        existingStock.LastDiv = updateStockDto.LastDiv;
        existingStock.Industry = updateStockDto.Industry;
        existingStock.MarketCap = updateStockDto.MarketCap;

        await _context.SaveChangesAsync();

        return existingStock;
    }
    
    public async Task<Stock?> DeleteAsync(int id)
    {
        var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
        if (stock == null)
            return null;
        
        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();
        
        return stock;
    }
}
﻿using api.Data;
using api.Dtos.Stock;
using api.Helpers;
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
    
    public async Task<Stock?> CreateAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks =  _context.Stocks
            .Include(stock => stock.Comments)
            .ThenInclude(comment => comment.AppUser)
            .AsQueryable();
        
        // Filtering
        if (!string.IsNullOrWhiteSpace(query.CompanyName))
            stocks = stocks.Where(s => s.CompanyName.ToLower().Contains(query.CompanyName.ToLower()));
        
        if (!string.IsNullOrWhiteSpace(query.Symbol))
            stocks = stocks.Where(s => s.Symbol.ToLower().Contains(query.Symbol.ToLower()));

        // Sorting
        if (!string.IsNullOrWhiteSpace(query.sortBy))
        {
            if (query.sortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                stocks = query.IsDescending 
                    ? stocks.OrderByDescending(s => s.Symbol) 
                    : stocks.OrderBy(s => s.Symbol);
        }
        
        // Pagination
        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }
    
    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(stock => stock.Symbol == symbol);
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(stock => stock.Id == id);
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

    public async Task<bool> StockExistAsync(int id)
    {
        return await _context.Stocks.AnyAsync(s => s.Id == id);
    }
}
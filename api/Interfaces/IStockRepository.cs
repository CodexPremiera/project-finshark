using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
    Task<Stock?> CreateAsync(Stock stock);
    Task<List<Stock>> GetAllAsync(QueryObject query);
    Task<Stock?> GetBySymbolAsync(string symbol);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock?> UpdateAsync(int id, UpdateStockDto updateStockDto);
    Task<Stock?> DeleteAsync(int id);
    Task<bool> StockExistAsync(int id);
}
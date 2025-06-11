using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
    Task<Stock?> CreateAsync(Stock stock);
    Task<List<Stock>> GetAllAsync();
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock?> UpdateAsync(int id, UpdateStockDto updateStockDto);
    Task<Stock?> DeleteAsync(int id);
}
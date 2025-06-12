using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;


[Route("api/portfolio")]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepo;
    private readonly IPortfolioRepository _portfolioRepo;
    private readonly IFMPService _fmpService;
    public PortfolioController(UserManager<AppUser> userManager,
        IStockRepository stockRepo, IPortfolioRepository portfolioRepo, IFMPService fmpService)
    {
        _userManager = userManager;
        _stockRepo = stockRepo;
        _portfolioRepo = portfolioRepo;
        _fmpService = fmpService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepo.GetBySymbolAsync(symbol);
        
        // Check if stock exists in IRL (via Financial Modeling API)
        if (stock == null)
        {
            stock = await _fmpService.FindStockBySymbolAsync(symbol);
            if (stock == null)
                return BadRequest("Stock does not exists");
            await _stockRepo.CreateAsync(stock);
        }
        
        // Check if user already has the stock
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) 
            return BadRequest("Cannot add same stock to portfolio");

        // Create portfolio object
        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };

        // Add portfolio to the database
        await _portfolioRepo.CreateAsync(portfolioModel);
        return portfolioModel == null
            ? StatusCode(500, "Could not create")
            : Created();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        
        // Find the stock in the portfolio that matches the input symbol
        var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();
        if (filteredStock.Count() != 1)
            return BadRequest("Stock not in your portfolio");
        
        // Delete the stock from the user's portfolio
        await _portfolioRepo.DeletePortfolio(appUser, symbol);
        return Ok();
    }
    
}
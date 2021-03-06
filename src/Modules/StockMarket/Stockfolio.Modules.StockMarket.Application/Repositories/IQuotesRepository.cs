using Stockfolio.Modules.StockMarket.Application.Dto;
using Stockfolio.Modules.StockMarket.Application.DTO;

namespace Stockfolio.Modules.StockMarket.Application.Repositories;

internal interface IQuotesRepository
{
    Task<SearchQuotesDto> SearchQuotes(string searchQuery, int quotesCount = 6, CancellationToken cancellationToken = default);

    Task<IEnumerable<QuoteDetailsDto>> GetQuotes(IEnumerable<string> symbols, CancellationToken cancellationToken = default);

    Task<QuoteDetailsDto> GetQuote(string symbol, CancellationToken cancellationToken = default);
}
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using zixieWebAPI_Stocks;
using zixieWebAPI_Stocks.Data;
using zixieWebAPI_Stocks.Models;

namespace zixieWebAPI_Stocks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly ILogger<StocksController> _logger;
        private readonly InvestApiClient _investApi;
        private readonly zixieContext _context;

        public StocksController(ILogger<StocksController> logger, InvestApiClient investApi, zixieContext context)
        {
            _investApi = investApi;
            _logger = logger;
            _context = context;
        }
        [HttpGet("GetStock")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet("GetStock/{id}")]
        public string Get(int id)
        {
            return $"value {id}";
        }
        [HttpGet("GetPrices/LoadPrices")]
        public async Task<IActionResult> LoadPrices(CancellationToken stoppingToken)
        {
            List<Prices> prices = new List<Prices>();
            //var instrumentsPrices = await new InstrumentsServiceSample(_investApi.Instruments).get
            LastPrice lp = new LastPrice();
            string[] get_prices = new string[1];
            get_prices.Append("BBG000BPH459");
            GetOrderBookRequest req = new GetOrderBookRequest();
            req.Depth = 1;
            req.Figi = "BBG000B9XRY4";
            var last_prices =  _investApi.MarketData.GetOrderBook(req);
            var price = last_prices.LastPrice;
            //var a = _investApi.MarketData.GetLastPrices(b);
            return Ok(prices);
        }
        [HttpGet("GetStock/LoadStocks")]
        public async Task<IActionResult> LoadStocks(CancellationToken stoppingToken)
        {
            List<Shares> share = new List<Shares>();
            var instrumentsDescription = await new InstrumentsServiceSample(_investApi.Instruments).GetInstrumentByTicker(stoppingToken);
            System.Diagnostics.Debug.WriteLine("test4: " + instrumentsDescription.Instruments);
            var selectedShares = from p in instrumentsDescription.Instruments select p;
            foreach (var i in selectedShares)
            {
                System.Diagnostics.Debug.WriteLine(i);
                share.Add(new Shares { Name = i.Name });
                var share_find = await _context.Shares.FirstOrDefaultAsync(s => s.Name == i.Name);
                if (share_find == null)
                {
                    _context.Add(new Shares
                    {
                        Name = i.Name
                        ,
                        Ticker = i.Ticker
                        ,
                        Currency = i.Currency
                        ,
                        DivYieldFlag = i.DivYieldFlag
                        ,
                        Exchange = i.Exchange
                        ,
                        Figi = i.Figi
                        ,
                        Isin = i.Isin
                        ,
                        BuyAvailableFlag = i.BuyAvailableFlag
                        ,
                        Nominal = i.Nominal.Currency.ToString()
                        ,
                        Sector = i.Sector
                    });
                }
            }
            _context.SaveChanges();
            return Ok(share);
        }
    }
}
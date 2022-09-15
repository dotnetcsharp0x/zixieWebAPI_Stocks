using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Globalization;
using System.Linq;
using System.Web.Helpers;
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

        public StocksController(ILogger<StocksController> logger
            , InvestApiClient investApi
            , zixieContext context)
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
            var figies = (from s in _context.Shares select s);
            //string[] get_prices = new string[figies.Count()];
            Prices price_insert = new Prices();
            string path = "Logs.log";
            GetOrderBookRequest req = new GetOrderBookRequest();

            try
            {
                foreach (var s in figies)
                {
                    Thread.Sleep(300);
                    //get_prices.Append(s.Figi);
                    req.Depth = 1;
                    req.Figi = s.Figi;
                    var last_prices = _investApi.MarketData.GetOrderBook(req);
                    
                    var price = last_prices.LastPrice;
                    //if (last_prices != null)
                    //{
                        prices.Add(new Prices
                        {
                            Figi = s.Figi
                        ,
                            Price = float.Parse((price.Units.ToString() + "." + price.Nano.ToString()), CultureInfo.InvariantCulture.NumberFormat)
                        ,
                            Date = DateTime.Now.ToString()
                        });
                    _context.Add(new Prices
                    {
                        Figi = s.Figi,
                        Date = DateTime.Now.ToString(),
                        Price = float.Parse((price.Units.ToString() + "." + price.Nano.ToString()), CultureInfo.InvariantCulture.NumberFormat)
                    });
                    //price_insert.Figi = s.Figi;
                    //price_insert.Price = float.Parse((price.Units.ToString() + "." + price.Nano.ToString()), CultureInfo.InvariantCulture.NumberFormat);
                    //price_insert.Date = DateTime.Now.ToString();
                    //_context.Add(price_insert);
                    //_context.SaveChanges();
                    //}
                    //price_insert.Price = float.Parse((price.Units.ToString() + "." + price.Nano.ToString()), CultureInfo.InvariantCulture.NumberFormat);
                    //price_insert.Date = DateTime.Now.ToString();
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        await writer.WriteLineAsync(DateTime.Now.ToString() + " | " + s.Figi + " | " + float.Parse((price.Units.ToString() + "." + price.Nano.ToString()), CultureInfo.InvariantCulture.NumberFormat) + " | ");
                    }
                }
                //var test =  prices;
                //foreach (var p in prices)
                //{
                   
                //}
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                var test = ex.Message + " | " + prices;
                //using (StreamWriter writer = new StreamWriter(path, true))
                //{
                //    await writer.WriteLineAsync(DateTime.Now.ToString() + " | " + ex.Message.ToString() + " | " + s);
                //    //await writer.WriteAsync("4,5");
                //}
            }
            finally
            {

            }
            return Ok(prices);
        }
         
        [HttpGet("GetStock/LoadStocks")]
        public async Task<IActionResult> LoadStocks(CancellationToken stoppingToken)
        {
            List<Shares> share = new List<Shares>();
            var instrumentsDescription = await new InstrumentsServiceSample(_investApi.Instruments).GetInstrumentByTicker(stoppingToken);
            //var instrumentsDescription = await _service.SharesAsync(stoppingToken);
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
        [HttpGet("GetCrypto/LoadExchanges")]
        public async Task<IActionResult> LoadExchanges()
        {
            var client = new RestClient($"https://api.diadata.org/v1/exchanges");
            var request = new RestRequest();
            var response = await client.ExecuteAsync(request);
            List<Exchanges> exchange = new List<Exchanges>();
            var objects = JArray.Parse(response.Content);
         
            foreach (var i in objects)
            {
                exchange.Add(new Exchanges { Name = i.ToString() });
            }
            var query = from item in _context.Exchange
                        select item;
            exchange.Except(query);
            foreach (var i in exchange)
            {
                System.Diagnostics.Debug.WriteLine("New exchange has beed founded!: " + i.Name);
                _context.Add(new Exchanges
                {
                    Name = i.Name
                });
            }
            _context.SaveChanges();
            return Ok(exchange);
        }
        [HttpGet("GetCrypto/LoadSymbols")]
        public async Task<IActionResult> LoadSymbols()
        {
            List<Symbols> cyrpto = new List<Symbols>();
            var selectedCryptos = (from p in _context.Symbol select p);
            foreach (var i in selectedCryptos)
            {
                System.Diagnostics.Debug.WriteLine(i);
                cyrpto.Add(i);
            }
            return Ok(cyrpto);
        }
        [HttpGet("GetCrypto/LoadCrypto")]
        public async Task<IActionResult> LoadCrypto()
        {
            List<Cryptos> cyrpto = new List<Cryptos>();
            var selectedCryptos = (from p in _context.Cryptos select p);
            foreach (var i in selectedCryptos)
            {
                System.Diagnostics.Debug.WriteLine(i);
                cyrpto.Add(i);
            }
            return Ok(cyrpto);
        }
    }
}
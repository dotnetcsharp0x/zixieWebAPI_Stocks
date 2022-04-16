namespace zixieWebAPI_Stocks.Models
{
    public class Prices
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public float Price { get; set; }
        public string Date { get; set; }
    }
}

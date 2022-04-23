namespace zixieWebAPI_Stocks.Models
{
    public class Cryptos
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Blockchain { get; set; }
        public float? Price { get; set; }
        public DateTime? Time { get; set; }
    }
}

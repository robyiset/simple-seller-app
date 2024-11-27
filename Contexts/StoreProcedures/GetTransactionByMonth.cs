
namespace simple_seller_app.Contexts.StoreProcedures
{
    public class GetTransactionByMonth
    {
        public string? transaction_code { get; set; }
        public string? product_code { get; set; }
        public string? product_name { get; set; }
        public int? price { get; set; }
        public int? quantity { get; set; }
        public int? total { get; set; }
    }
}
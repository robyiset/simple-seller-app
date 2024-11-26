using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_seller_app.Contexts.Tables
{
    [Table("m_product")]
    public class m_product
    {
        [Key]
        [Column(TypeName = "varchar(50)")]
        public string id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string product_code { get; set; }
        [Column(TypeName = "varchar(-1)")]
        public string product_name { get; set; }
        public int price { get; set; }
        public DateTime created_date { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string created_by { get; set; }
        public DateTime? updated_date { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? updated_by { get; set; }
        public DateTime? deleted_date { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? deleted_by { get; set; }
    }
}
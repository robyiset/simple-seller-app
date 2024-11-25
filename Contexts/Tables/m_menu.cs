using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_seller_app.Contexts.Tables
{
    public class m_menu
    {
        [Key]
        public int id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string menu_name { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string menu_url { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string role { get; set; }
    }
}
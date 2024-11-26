using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simple_seller_app.Contexts.Tables
{
    [Table("u_user")]
    public class u_user
    {
        [Key]
        [Column(TypeName = "varchar(50)")]
        public string id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string username { get; set; }
        [Column(TypeName = "varchar(-1)")]
        public string password { get; set; }
        [Column(TypeName = "varchar(-1)")]
        public string full_name { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string role { get; set; }
        public DateTime register_date { get; set; }
        public DateTime? login_date { get; set; }
    }
}
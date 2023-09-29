using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyClass.Model
{
    [Table("Orderdetails")]
    public class Orderdetails
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int ProductID { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }

    }
}

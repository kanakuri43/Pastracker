using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastracker.Models
{
    [Table("companies")]
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastracker.Models
{
    [Table("branches")]
    public class Branch
    {
        public int Id { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastracker.Models
{
    public class MoveContent
    {
        public int Id { get; set; }
        [Column("branch_id")]
        public int BranchId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("pickup_date")]
        public DateTime PickupDate { get; set; }
        [Column("delivery_date")]
        public DateTime DeliveryDate { get; set; }
    }
}

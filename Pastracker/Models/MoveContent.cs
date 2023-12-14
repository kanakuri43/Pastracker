using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastracker.Models
{
    [Table("move_contents")]
    public class MoveContent
    {
        public int Id { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        [Column("branch_id")]
        public int BranchId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("employee_name")]
        public string EmployeeName { get; set; }

        [Column("pickup_date")]
        public DateTime PickupDate { get; set; }
        [Column("pickup_name")]
        public string PickupName { get; set; }
        [Column("pickup_tel")]
        public string PickupTel { get; set; }
        [Column("pickup_address1")]
        public string PickupAddress1 { get; set; }
        [Column("pickup_address2")]
        public string PickupAddress2 { get; set; }

        [Column("delivery_date")]
        public DateTime DeliveryDate { get; set; }
        [Column("delivery_name")]
        public string DeliveryName { get; set; }
        [Column("delivery_tel")]
        public string DeliveryTel { get; set; }
        [Column("delivery_address1")]
        public string DeliveryAddress1 { get; set; }
        [Column("delivery_address2")]
        public string DeliveryAddress2 { get; set; }

        [Column("truck_type")]
        public int TruckType { get; set; }
        [Column("delivery_branch_id")]
        public int DeliveryBranchId { get; set; }
        [Column("memo")]
        public string Memo { get; set; }
        [Column("document_directory")]
        public string DocumentDirectory { get; set; }

        public int Item01 { get; set; }
        public int Item02 { get; set; }
        public int Item03 { get; set; }
        public int Item04 { get; set; }
        public int Item05 { get; set; }
        public int Item06 { get; set; }
        public int Item07 { get; set; }
        public int Item08 { get; set; }
        public int Item09 { get; set; }
        public int Item10 { get; set; }

    }
}

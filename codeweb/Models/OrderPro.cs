namespace codeweb.Models
{
    using codeweb.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderPro")]
    public partial class OrderPro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderPro()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOrder { get; set; }

        public string NameCus { get; set; }
        public int? IDCus { get; set; }
        public int PhoneNumber { get; set; }
        public string AddressDelivery { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public string Status { get; set; } = "Waiting for confirmation";


        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}


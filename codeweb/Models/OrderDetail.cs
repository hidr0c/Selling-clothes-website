//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace codeweb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public int Quantity { get; set; }
        public decimal SoldPrice { get; set; }
        public string NameClothes { get; set; }
        public string IDClothes { get; set; }
        public string IDOrder { get; set; }
    
        public virtual Cloth Cloth { get; set; }
        public virtual UserOrder UserOrder { get; set; }
    }
}

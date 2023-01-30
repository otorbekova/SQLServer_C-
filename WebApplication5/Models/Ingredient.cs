using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class Ingredient
    {
        public int Id { get; set; }
        public int? Продукция { get; set; }
        public int? Сырьё { get; set; }
        public decimal? Количество { get; set; }

        public virtual FinishedProduct Продукция_ { get; set; }
        public virtual RawMaterial Сырьё_ { get; set; }
        //public virtual ICollection<FinishedProduct> Продукция_ { get; set; }
    }
}

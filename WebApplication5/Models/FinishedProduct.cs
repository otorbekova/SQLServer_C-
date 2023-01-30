using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class FinishedProduct
    {
        public FinishedProduct()
        {
            Ingredients = new HashSet<Ingredient>();
            Productions = new HashSet<Production>();
            SaleOfProducts = new HashSet<SaleOfProduct>();
        }

        public int Id { get; set; }
        public string Наименование { get; set; }
        public int? ЕдиницаИзмерения { get; set; }
        public decimal? Сумма { get; set; }
        public decimal? Количество { get; set; }

        public virtual Unit ЕдиницаИзмерения_ { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Production> Productions { get; set; }
        public virtual ICollection<SaleOfProduct> SaleOfProducts { get; set; }
    }
}

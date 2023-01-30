using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class RawMaterial
    {
        public RawMaterial()
        {
            Ingredients = new HashSet<Ingredient>();
            PurchaseOfRawMaterials = new HashSet<PurchaseOfRawMaterial>();
        }

        public int Id { get; set; }
        public string Наименование { get; set; }
        public int? ЕдиницаИзмерения { get; set; }
        public decimal? Сумма { get; set; }
        public decimal? Количество { get; set; }

        public virtual Unit ЕдиницаИзмерения_ { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<PurchaseOfRawMaterial> PurchaseOfRawMaterials { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class Unit
    {
        public Unit()
        {
            FinishedProducts = new HashSet<FinishedProduct>();
            RawMaterials = new HashSet<RawMaterial>();
        }

        public int Id { get; set; }
        public string Наименование { get; set; }

        public virtual ICollection<FinishedProduct> FinishedProducts { get; set; }
        public virtual ICollection<RawMaterial> RawMaterials { get; set; }
    }
}

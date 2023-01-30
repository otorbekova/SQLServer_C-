using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class Employee
    {
        public Employee()
        {
        
            PurchaseOfRawMaterials = new HashSet<PurchaseOfRawMaterial>();
            Salaries = new HashSet<Salary>();
            SaleOfProducts = new HashSet<SaleOfProduct>();
            Productions = new HashSet<Production>();
        }

        public int Id { get; set; }
        public string Фио { get; set; }
        public int? Должность { get; set; }
        public decimal? Оклад { get; set; }
        public string Адрес { get; set; }
        public string Телефон { get; set; }

        public virtual Post Должность_ { get; set; }
        public virtual ICollection<PurchaseOfRawMaterial> PurchaseOfRawMaterials { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<SaleOfProduct> SaleOfProducts { get; set; }
        public virtual ICollection<Production> Productions { get; set; }
    }
}

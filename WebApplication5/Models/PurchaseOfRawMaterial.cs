using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class PurchaseOfRawMaterial
    {
        public int Id { get; set; }
        public int? Сырьё { get; set; }
        public decimal? Количество { get; set; }
        public decimal? Сумма { get; set; }
        public DateTime? Дата { get; set; }
        public int? Сотрудник { get; set; }

        public virtual Employee Сотрудник_ { get; set; }
        public virtual RawMaterial Сырьё_ { get; set; }
    }
}

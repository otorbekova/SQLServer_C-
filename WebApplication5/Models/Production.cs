using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class Production
    {
        public int Id { get; set; }
        public int? Продукция { get; set; }
        public decimal? Количество { get; set; }
        public DateTime? Дата { get; set; }
        public int? Сотрудник { get; set; }

        public virtual FinishedProduct Продукция_{ get; set; }
        public virtual Employee Сотрудники{ get; set; }
    }
}

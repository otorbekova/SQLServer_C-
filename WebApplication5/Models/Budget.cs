using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class Budget
    {
        public int Id { get; set; }
        public decimal? СуммаБюджета { get; set; }
        public int Бонус { get; set; }
        public int Процент { get; set; }

    }
}

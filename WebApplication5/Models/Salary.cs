using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class Salary
    {
        public int IdЗарплата { get; set; }
        public int? Год { get; set; }
        public int? Месяц { get; set; }
        public int Сотрудник { get; set; }
        public decimal? Зарплата { get; set; }
        public string Производство { get; set; }
        public string Закупка { get; set; }
        public string Продажа { get; set; }
        public int? ОбщееКоличество { get; set; }
        public int? Оклад { get; set; }
        public int? Бонус { get; set; }

      //  public virtual Empl Сотрудник_ { get; set; }
        public virtual Employee Сотрудник_ { get; set; }
    }
}

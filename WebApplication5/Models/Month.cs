using System;
using System.Collections.Generic;

namespace WebApplication5.Models
{
    public class Month
    {
        public Month()
        {
            Salaries = new HashSet<Salary>();
        }

        public int Id { get; set; }
        public string? Month1 { get; set; }

        public virtual ICollection<Salary> Salaries { get; set; }
    }
}

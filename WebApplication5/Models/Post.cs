using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication5.Models
{
    public partial class Post
    {
        public Post()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Должность { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
       // [Column("id")]  // Если столбец называется "id" в базе
        public int id { get; set; }

        public string Name { get; set; }

        // Навигационное свойство для связи с расходами
        public ICollection<Expense> Expenses { get; set; }
    }
}

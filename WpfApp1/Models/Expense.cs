using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models
{
    [Table("Expenses")]
    public class Expense
    {
        [Key]
        [Column("ExpensesId")]  // Убедитесь, что имя поля соответствует таблице
        public int Id { get; set; }

        [ForeignKey("Category")]
  
        public int CategoryId { get; set; }

        public DateTime ExpenseDate { get; set; }

        public decimal Cost { get; set; }

        public string Coment { get; set; }

        // Навигационное свойство для связи с категорией
        public Category Categoryy { get; set; }
    }
}

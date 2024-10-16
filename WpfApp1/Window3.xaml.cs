using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class Window3 : Window
    {
        private readonly AppDbContext _context;

        public Window3()
        {
            InitializeComponent();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=qwerty123;Database=home");

            _context = new AppDbContext(optionsBuilder.Options);

            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _context.Categories.ToList();
                CategoryComboBox.ItemsSource = categories;
                CategoryComboBox.DisplayMemberPath = "Name";
                CategoryComboBox.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке категорий: {ex.Message}");
            }
        }

        private void SaveExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem != null)
            {
                var selectedCategory = (Category)CategoryComboBox.SelectedItem;

                // Получение даты с учетом часового пояса
                DateTime expenseDate = (ExpenseDatePicker.SelectedDate ?? DateTime.Now).ToUniversalTime(); // Конвертация в UTC
                decimal cost;

                if (!decimal.TryParse(CostTextBox.Text, out cost))
                {
                    MessageBox.Show("Пожалуйста, введите корректную стоимость.");
                    return;
                }

                string comment = CommentTextBox.Text;

                var expense = new Expense
                {
                    CategoryId = selectedCategory.id,
                    ExpenseDate = expenseDate, 
                    Cost = cost,
                    Coment = comment
                };

                
                _context.Expenses.Add(expense);

              
                _context.SaveChanges();

                MessageBox.Show("Расход успешно сохранен!");
                ExpenseDatePicker.SelectedDate = null;
                CostTextBox.Clear();
                CommentTextBox.Clear();
                CategoryComboBox.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите категорию.");
            }
        }



        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}

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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=qwerty123;Database=home"))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("SELECT \"Name\" FROM categories", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        CategoryComboBox.Items.Clear(); 
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Таблица categories пуста!");
                            return;
                        }

                        while (reader.Read())
                        {
                            CategoryComboBox.Items.Add(reader.GetString(0)); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке категорий: {ex.Message}");
            }
        }

        private void SaveExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что категория выбрана
            if (CategoryComboBox.SelectedItem != null)
            {
                // Получаем имя выбранной категории
                string selectedCategory = CategoryComboBox.SelectedItem.ToString();
                DateTime expenseDate = ExpenseDatePicker.SelectedDate ?? DateTime.Now; // Используем текущую дату, если не выбрана
                decimal cost;

                // Проверяем корректность введенной стоимости
                if (!decimal.TryParse(CostTextBox.Text, out cost))
                {
                    MessageBox.Show("Пожалуйста, введите корректную стоимость.");
                    return;
                }

                string comment = CommentTextBox.Text;

                try
                {
                    using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=qwerty123;Database=home"))
                    {
                        conn.Open();

                        // Получаем ID категории на основе её имени
                        int categoryId;
                        using (var cmdCategoryId = new NpgsqlCommand("SELECT \"id\" FROM \"categories\" WHERE \"Name\" = @categoryName", conn))
                        {
                            cmdCategoryId.Parameters.AddWithValue("categoryName", selectedCategory);
                            var result = cmdCategoryId.ExecuteScalar();

                            if (result != null)
                            {
                                categoryId = Convert.ToInt32(result);
                            }
                            else
                            {
                                MessageBox.Show("Ошибка: выбранная категория не найдена в базе данных.");
                                return;
                            }
                        }

                        // Вставляем новый расход в таблицу Expenses
                        using (var cmd = new NpgsqlCommand("INSERT INTO \"Expenses\" (\"CategoryId\", \"ExpenseDate\", \"Cost\", \"Coment\") VALUES (@categoryId, @expenseDate, @cost, @comment)", conn))
                        {
                            cmd.Parameters.AddWithValue("categoryId", categoryId);
                            cmd.Parameters.AddWithValue("expenseDate", expenseDate);
                            cmd.Parameters.AddWithValue("cost", cost);
                            cmd.Parameters.AddWithValue("comment", comment);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Расход успешно сохранен!");
                    ExpenseDatePicker.SelectedDate = null;
                    CostTextBox.Clear();
                    CommentTextBox.Clear();
                    CategoryComboBox.SelectedIndex = -1; // Сбрасываем выбор категории
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении расхода: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите категорию.");
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem != null)
            {
                string selectedCategory = CategoryComboBox.SelectedItem.ToString();

                MessageBox.Show($"Вы выбрали категорию: {selectedCategory}");
            }
        }
    }
}

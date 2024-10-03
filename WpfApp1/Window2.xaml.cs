using System;
using System.Collections.Generic;
using System.Linq;
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
using Npgsql;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private string selectedCategoryName;
        public Window2()
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
                        CategoryListBox.Items.Clear();


                        while (reader.Read())
                        {
                            CategoryListBox.Items.Add(reader.GetString(0));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке категорий: {ex.Message}");
            }
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryListBox.SelectedItem != null)
            {
                string selectedCategory = CategoryListBox.SelectedItem.ToString();

                try
                {
                    using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=qwerty123;Database=home"))
                    {
                        conn.Open();

                        using (var cmd = new NpgsqlCommand("DELETE FROM categories WHERE \"Name\" = @name", conn))
                        {
                            cmd.Parameters.AddWithValue("name", selectedCategory);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show($"Категория '{selectedCategory}' удалена!");
                    LoadCategories();
                    CategoryNameTextBox.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении категории: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите категорию для удаления.");
            }
        }


        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = CategoryNameTextBox.Text;

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                try
                {
                    using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=qwerty123;Database=home"))
                    {
                        conn.Open();

                        using (var cmd = new NpgsqlCommand("INSERT INTO categories (\"Name\")  VALUES (@name)", conn))
                        {
                            cmd.Parameters.AddWithValue("name", categoryName);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadCategories();

                    MessageBox.Show($"Категория '{categoryName}' добавлена!");
                    CategoryNameTextBox.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении категории: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название категории.");
            }
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedCategoryName))
            {
                MessageBox.Show("Пожалуйста, выберите категорию для редактирования.");
                return;
            }

            string newCategoryName = CategoryNameTextBox.Text;

            if (!string.IsNullOrWhiteSpace(newCategoryName))
            {
                try
                {
                    using (var conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=qwerty123;Database=home"))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand("UPDATE categories SET \"Name\" = @newName WHERE \"Name\" = @oldName", conn))
                        {
                            cmd.Parameters.AddWithValue("newName", newCategoryName);
                            cmd.Parameters.AddWithValue("oldName", selectedCategoryName);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"Категория '{selectedCategoryName}' изменена на '{newCategoryName}'!");
                    LoadCategories();
                    CategoryNameTextBox.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при редактировании категории: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите новое название категории.");
            }
        }
    }
}
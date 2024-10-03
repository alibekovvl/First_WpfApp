using Npgsql;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Window1 : Window
    {
        private readonly string connectionString = "Host=localhost;Username=postgres;Password=qwerty123;Database=home";

        public Window1()
        {
            InitializeComponent();
            LoadMonths();
            LoadYears();
        }

        // Метод для загрузки месяцев в ComboBox
        private void LoadMonths()
        {
            for (int month = 1; month <= 12; month++)
            {
                MonthComboBox.Items.Add(new DateTime(2024, month, 1).ToString("MMMM", CultureInfo.InvariantCulture)); 
            }
            MonthComboBox.SelectedIndex = 0; 
        }

        
        private void LoadYears()
        {
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 10; year <= currentYear; year++)
            {
                YearComboBox.Items.Add(year);
            }
            YearComboBox.SelectedItem = currentYear; 
        }

       
        private void ShowStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (MonthComboBox.SelectedItem != null && YearComboBox.SelectedItem != null)
            {
                string selectedMonth = MonthComboBox.SelectedItem.ToString();
                int selectedYear = (int)YearComboBox.SelectedItem;

                MessageBox.Show($"Выбранный месяц: {selectedMonth}, Год: {selectedYear}", "Отладка", MessageBoxButton.OK);

                LoadStatistics(selectedMonth, selectedYear);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите месяц и год.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void LoadStatistics(string selectedMonth, int selectedYear)
        {
            MonthlyExpensesListBox.Items.Clear();

            // Получаем номер месяца
            int monthNumber = DateTime.ParseExact(selectedMonth, "MMMM", CultureInfo.InvariantCulture).Month;

            // Определяем границы дат
            DateTime startDate = new DateTime(selectedYear, monthNumber, 1);
            DateTime endDate = startDate.AddMonths(1);

            
            string query = @"
                SELECT ""Name"", ""ExpenseDate"", ""Cost"", ""Coment"" 
                FROM ""Expenses"" 
                JOIN ""categories"" ON ""CategoryId"" = ""id"" 
                WHERE ""ExpenseDate"" >= @startDate AND ""ExpenseDate"" < @endDate";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);

                    try
                    {
                        conn.Open();
                        using (var reader = command.ExecuteReader())
                        {
                           
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Нет данных для выбранного месяца и года.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }

                            while (reader.Read())
                            {
                                
                                var item = new
                                {
                                    CategoryName = reader["Name"].ToString(),
                                    ExpenseDate = (DateTime)reader["ExpenseDate"],
                                    Cost = reader["Cost"],
                                    Comment = reader["Coment"].ToString() // Изменено на "Coment"
                                };

                                MonthlyExpensesListBox.Items.Add(item);
                            }
                        }
                    }
                    catch (NpgsqlException npgsqlEx)
                    {
                        MessageBox.Show($"Ошибка подключения к базе данных: {npgsqlEx.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке статистики: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

       
        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }
    }
}

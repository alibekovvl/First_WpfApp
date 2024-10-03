using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void Button_category(object sender, RoutedEventArgs e)
        {
            var add = new Window3();
            add.ShowDialog();
        }
        private void Button_trafic_Click(object sender, RoutedEventArgs e)
        {
            var add = new Window2();
            add.ShowDialog();

        }

        private void Button_statistic(object sender, RoutedEventArgs e)
        {
            try
            {
                var show = new Window1();
                show.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна статистики: {ex.Message}");
            }
        }
    }
}
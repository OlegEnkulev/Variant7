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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Variant7.Pages
{
    public partial class LoginPage : Page
    {
        bool Mistacked = false;
        DispatcherTimer Timer = new DispatcherTimer();
        int TimerTicks = 0;

        public LoginPage()
        {
            InitializeComponent();
            TimerSetup();
        }

        private void LoginBTN_Click(object sender, RoutedEventArgs e)
        {
            if (Mistacked == true && CapchaBox.Text != "35R234")
            {
                Mistake("Неправильно введена капча");
                return;
            }

            User user = Core.DB.User.Where(u => u.UserLogin == LoginBox.Text && u.UserPassword == PassBox.Password).FirstOrDefault();
            if(user == null)
            {
                Mistake("Проверьте правильность введенных данных");
                return;
            }

            Core.currentUser = user;

            if(user.UserRole == 1)
            {
                Core.mainWindow.MainFrame.Navigate(new Pages.AdminPage());
            }
            else
            {
                Core.mainWindow.MainFrame.Navigate(new Pages.ProductPage());
            }
        }

        private void GuestBTN_Click(object sender, RoutedEventArgs e)
        {
            Core.mainWindow.MainFrame.Navigate(new Pages.ProductPage());
        }

        void Mistake(string errStr)
        {
            MessageBox.Show(errStr, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            CapchaPanel.Visibility = Visibility.Visible;
            Mistacked = true;
            TimerTicks = 10;
            LoginBTN.Content = TimerTicks.ToString();
            LoginBTN.IsEnabled = false;
        }

        void TimerSetup()
        {
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(TimerTicks == 0)
            {
                LoginBTN.Content = "Войти";
                LoginBTN.IsEnabled = true;
            }
            else
            {
                TimerTicks--;
                LoginBTN.Content = TimerTicks.ToString();
                LoginBTN.IsEnabled = false;
            }
        }
    }
}

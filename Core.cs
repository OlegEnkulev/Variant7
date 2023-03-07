using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Variant7
{
    public static class Core
    {
        public static MainWindow mainWindow;
        public static Variant7DBEntities DB = new Variant7DBEntities();
        public static User currentUser;

        public static void ExitSystem()
        {
            mainWindow.MainFrame.Navigate(new Pages.LoginPage());
            currentUser = null;
        }
    }
}

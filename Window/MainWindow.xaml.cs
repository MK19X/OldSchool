using OSProject.DAL;
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

namespace OSProject
{
    public partial class MainWindow : Window
    {
        public OSUser user;
        public MainWindow(OSUser u)
        {
            user = u;
            InitializeComponent();
            
            UserName.Text = $"{u.user_name}";
            UserScore.Text = $"{u.user_score}";
        }
        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            SystemEnterWindow logout = new SystemEnterWindow();
            logout.Show();
            Close();
        }
    }
}
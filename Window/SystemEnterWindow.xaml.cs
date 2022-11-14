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
using System.Windows.Shapes;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Npgsql;
using System.Data;

namespace OSProject
{
    public partial class SystemEnterWindow : Window
    {
        public SystemEnterWindow()
        {
            InitializeComponent();
        }

        private string UserName;
        private string UserPassword;
        private void LoginButtonClick(object sender, RoutedEventArgs e) {
            UserName = LoginText.Text;
            UserPassword = PasswordText.Text;
            if (UserName != "" && UserPassword != "")
            {
                UserDataBaseFunc o = new UserDataBaseFunc();
                string c = o.ReturnConnectionString();
                using (NpgsqlConnection conn = new NpgsqlConnection(c))
                {

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        using (NpgsqlCommand cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            try
                            {
                                cmd.CommandText = $"SELECT COUNT(1) FROM OSUser WHERE user_name = '{UserName}' AND user_password = '{UserPassword}'";
                                int count = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Parameters.Clear();
                                if (count == 1)
                                {
                                    cmd.CommandText = $"SELECT id FROM OSUser WHERE user_name = '{UserName}' AND user_password = '{UserPassword}'";
                                    int i = Convert.ToInt32(cmd.ExecuteScalar());
                                    cmd.Parameters.Clear();

                                    cmd.CommandText = $"SELECT stype FROM OSUser WHERE id = {i}";
                                    string st = cmd.ExecuteScalar().ToString();
                                    cmd.Parameters.Clear();

                                    cmd.CommandText = $"SELECT user_score FROM OSUser WHERE id = {i}";
                                    int sc = Convert.ToInt32(cmd.ExecuteScalar());
                                    cmd.Parameters.Clear();

                                    cmd.CommandText = $"SELECT image FROM OSUser WHERE id = {i}";
                                    string im = cmd.ExecuteScalar().ToString();
                                    cmd.Parameters.Clear();

                                    OSUser user = new OSUser(i, UserName, st, UserPassword, im, sc);

                                    GrantAccess(user);
                                    Close();
                                }
                                else
                                {
                                    MessageBox.Show("Wrong Input!");
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected Error!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error! Servers are down");
                    }
                }
            }
            else
            {
                MessageBox.Show("Error! You cannot leave empty fields!");
            }
        }
        private void RegisterButtonClick(object sender, RoutedEventArgs e) {
            UserName = LoginText.Text;
            UserPassword = PasswordText.Text;
            if (UserName != "" && UserPassword != "")
            {
                UserDataBaseFunc o = new UserDataBaseFunc();
                string c = o.ReturnConnectionString();
                using (NpgsqlConnection conn = new NpgsqlConnection(c))
                {

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        using (NpgsqlCommand cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            try
                            {
                                o.FuncCheckLogin(UserName, c);
                            }
                            catch
                            {
                                MessageBox.Show("Error! Login already exists!");
                            }
                            try
                            {
                                o.FuncCheckPass(UserName, UserPassword, c);
                            }
                            catch
                            {
                                MessageBox.Show("Error! Enter another password!");
                            }
                            try
                            {
                                int newId = o.FuncCreateID(c);

                                cmd.CommandText = $"INSERT INTO OSUser VALUES ({newId}, '{UserName}','{UserPassword}','Sys', 'default', 0)";
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();

                                o.CreateRating(c, newId);
                                cmd.Parameters.Clear();

                                cmd.CommandText = $"SELECT id FROM OSUser WHERE user_name = '{UserName}' AND user_password = '{UserPassword}'";
                                int i = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Parameters.Clear();

                                cmd.CommandText = $"SELECT stype FROM OSUser WHERE id = {i}";
                                string st = cmd.ExecuteScalar().ToString();
                                cmd.Parameters.Clear();

                                cmd.CommandText = $"SELECT user_score FROM OSUser WHERE id = {i}";
                                int sc = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Parameters.Clear();

                                cmd.CommandText = $"SELECT image FROM OSUser WHERE id = {i}";
                                string im = cmd.ExecuteScalar().ToString();
                                cmd.Parameters.Clear();

                                OSUser user = new OSUser(i, UserName, st, UserPassword, im, sc);
                                GrantAccess(user);
                                Close();
                            }
                            catch
                            {
                                MessageBox.Show("Unexpected Error!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error! Servers are down");
                    }
                }
            }
            else {
                MessageBox.Show("Error! You cannot leave empty fields!");
            }
        }
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void GrantAccess(OSUser u) {
            MainWindow main = new MainWindow(u);
            main.Show();
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using balance.DataBase;


namespace balance.Views
{
    /// <summary>
    /// LoginForm.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginForm : Window
    {
        public balance.Views.Certification.Refresh rf = null;

        public LoginForm()
        {
            InitializeComponent();
        }

        public LoginForm(balance.Views.Certification.Refresh pRefresh)
        {
            InitializeComponent();

            rf = pRefresh;
        }

        String tlogin_id;
        String tpassword;
        String SQL;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tlogin_id = "'" + login_id.Text + "'";
            tpassword = "'" + password.Password + "'";
//            SQL = "SELECT * FROM t_user WHERE login_id = 'sasaki' AND password = 'sasaki'";
            SQL = "SELECT * FROM t_user WHERE login_id = " + tlogin_id + " AND password = " + tpassword;

            DBConnect.Connect("kasiihara.db");
            DBConnect.ExecuteReader(SQL);

            if (DBConnect.Reader.Read() == true)
            {
                Application.Current.Properties["t_id"] = DBConnect.Reader[0].ToString();
                Application.Current.Properties["t_na"] = DBConnect.Reader[1].ToString();
                DBConnect.Dispose();
                rf();
                this.Close();
            }
            else
            {
                //MessageBox.Show("ユーザIDまたはパスワードが間違ってます。");
                Nologin.Content = "ユーザIDまたはパスワードが間違ってます。";
                DBConnect.Dispose();
//                this.Close();
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

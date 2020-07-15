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
using System.Windows.Navigation;
using System.Windows.Shapes;

using balance.DataBase;

namespace balance.Views
{
    /// <summary>
    /// PasswordEdit.xaml の相互作用ロジック
    /// </summary>
    public partial class PasswordEdit : Page
    {
        int UserId = -1;
        String SQL = "";
        public PasswordEdit()
        {
            InitializeComponent();

            UserId = int.Parse(Application.Current.Properties["u_id"].ToString());

            SQL = "SELECT * FROM t_user WHERE user_id  = '" + UserId + "'";

            WrapPanel backbuttons = new WrapPanel();

            Button top_b = new Button() { Content = "トップ画面", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            top_b.Click += bmodo;
            backbuttons.Children.Add(top_b);

            Button logout_b = new Button() { Content = "ログアウト", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            logout_b.Click += bout;
            backbuttons.Children.Add(logout_b);

            grid10.Children.Add(backbuttons);

            DBConnect.Connect("kasiihara.db");
            DBConnect.ExecuteReader(SQL);
            DBConnect.Reader.Read();

            DBConnect.Dispose();

        }

        void bout(object sender, EventArgs e)
        {
            var nextPage = new MainTitle();
            NavigationService.Navigate(nextPage);
        }

        void bmodo(object sender, EventArgs e)
        {
            var nextPage = new t_top();
            NavigationService.Navigate(nextPage);
        }

        void Edi(object sender, EventArgs e)
        {
            if (!cpass.Password.Equals("") && !npass.Password.Equals("") && !rpass.Password.Equals(""))
            {
                DBConnect.Connect("kasiihara.db");
                DBConnect.ExecuteReader(SQL);
                DBConnect.Reader.Read();
                if (cpass.Password.ToString().Equals(DBConnect.Reader[3]))
                {
                    if (npass.Password.ToString().Equals(rpass.Password.ToString()))
                    {
                        SQL = "UPDATE t_user SET password = '" + npass.Password.ToString() + "' WHERE user_id = '" + UserId + "'";
                        DBConnect.ExecuteReader(SQL);
                        MessageBox.Show("パスワードを変更しました。");
                    }
                    else
                    {
                        MessageBox.Show("新しいパスワードと新しいパスワード（確認）の内容が違います。");
                    }
                }
                else
                {
                    MessageBox.Show("現在のパスワードが違います。");
                }
                DBConnect.Dispose();
//                MessageBox.Show("パスワードを変更しました。");
            }
            else
            {
                MessageBox.Show("入力に誤りがあります。");
            }
        }
        
    }
}

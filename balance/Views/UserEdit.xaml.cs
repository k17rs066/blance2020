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
    /// UserEdit.xaml の相互作用ロジック
    /// </summary>
    public partial class UserEdit : Page
    {
        String e_id = "";
        String SQL = "";

        String type;
        
        public UserEdit()
        {
            InitializeComponent();

            WrapPanel backbuttons = new WrapPanel();

            Button top_b = new Button() { Content = "トップ画面", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            top_b.Click += bmodo;
            backbuttons.Children.Add(top_b);

            Button logout_b = new Button() { Content = "ログアウト", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            logout_b.Click += bout;
            backbuttons.Children.Add(logout_b);

            Button bac_b = new Button() { Content = "戻る", FontSize = 50, HorizontalAlignment = HorizontalAlignment.Left, Height = 99.2, Margin = new Thickness(1550, 50, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 250 };
            bac_b.Click += bac;

            grid8.Children.Add(bac_b);
            grid8.Children.Add(backbuttons);

            e_id = Application.Current.Properties["e_id"].ToString();
            SQL = "SELECT * FROM t_user WHERE user_id  = '" + e_id + "'";
            DBConnect.Connect("kasiihara.db");
            DBConnect.ExecuteReader(SQL);
            DBConnect.Reader.Read();
            if (!DBConnect.Reader[1].ToString().Equals(""))
            {
                umei.Text = DBConnect.Reader[1].ToString();
            }

            if (!DBConnect.Reader[2].ToString().Equals(""))
            {
                uid.Text = DBConnect.Reader[2].ToString();
            }
            if (!DBConnect.Reader[6].ToString().Equals(""))
            {
                umei_kana.Text = DBConnect.Reader[6].ToString();
            }

            type = DBConnect.Reader[4].ToString();
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

        void bac(object sender, EventArgs e)
        {
            var nextPage = new UserManagiment();
            NavigationService.Navigate(nextPage);
        }

        void edit(object sender, EventArgs e)
        {
            if (!umei.Text.Equals(null) && !uid.Text.Equals(null) && !umei.Text.Equals("") && !uid.Text.Equals(""))
            {
                DBConnect.Connect("kasiihara.db");
//                SQL = "UPDATE t_user WHERE user_id = '"+ Application.Current.Properties["e_id"].ToString() + "' SET user_name = '" + umei.Text + "',login_id = '" + uid + "'";
                SQL = "UPDATE t_user SET user_name = '" + umei.Text + "',login_id = '" + uid.Text + "',user_name_kana = '" + umei_kana.Text + "' WHERE user_id = '" + int.Parse(Application.Current.Properties["e_id"].ToString()) + "'";
                DBConnect.ExecuteReader(SQL);
                DBConnect.Dispose();


                //送信するデータ格納
                Application.Current.Properties["henshuu"] = "2";
                Application.Current.Properties["rename"] = umei.Text;
                Application.Current.Properties["reid"] = uid.Text;
                Application.Current.Properties["rename_kana"] = umei_kana.Text;
                Application.Current.Properties["rename_type"] = type;
                //送信先、遷移先を設定
                var nextPage = new UserManagiment();
                NavigationService.Navigate(nextPage);
                
                //MessageBox.Show("編集が完了しました");
            }
            else
            {
                MessageBox.Show("編集が完了できませんでした");
            }
//                Dispatcher.Invoke(new Action(() =>
//                    {}));
        }
    }
}

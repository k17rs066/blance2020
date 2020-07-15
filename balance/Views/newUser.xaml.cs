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
    /// newUser.xaml の相互作用ロジック
    /// </summary>
    public partial class newUser : Page
    {
        String SQL = "";
        public newUser()
        {
            InitializeComponent();

            WrapPanel backbuttons = new WrapPanel();

            Button top_b = new Button() { Content = "トップ画面", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            top_b.Click += bmodo;
            backbuttons.Children.Add(top_b);

            Button logout_b = new Button() { Content = "ログアウト", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            logout_b.Click += bout;
            backbuttons.Children.Add(logout_b);

            Button bac_b = new Button(){Content="戻る", FontSize=50, HorizontalAlignment=HorizontalAlignment.Left, Height=99.2, Margin=new Thickness(1550,50,0,0), VerticalAlignment=VerticalAlignment.Top, Width=250};
            bac_b.Click += bac;

            

            grid8.Children.Add(bac_b);
            grid8.Children.Add(backbuttons);



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

        void Registration(object sender, EventArgs e)
        {
            if((bool)tan.IsChecked == true){
                if (!username.Text.Equals("") && !username_kana.Text.Equals("") && !userid.Text.Equals("") && !pass.Password.Equals("") && !repass.Password.Equals(""))
                {
                    if (pass.Password.ToString().Equals(repass.Password.ToString()))
                    {
                        DBConnect.Connect("kasiihara.db");
                        SQL = "INSERT INTO t_user (user_name,user_name_kana,login_id,password,usertype)VALUES('" + username.Text.ToString() + "','"+ username_kana.Text.ToString() + "','" + userid.Text.ToString() + "','"+ pass.Password.ToString() +"','担当者')";
                        DBConnect.ExecuteReader(SQL);
                        DBConnect.Dispose();

                        //送信するデータ格納
                        Application.Current.Properties["henshuu"] = "1";
                        Application.Current.Properties["rename"] = username.Text;
                        Application.Current.Properties["rename_kana"] = username_kana.Text;
                        Application.Current.Properties["reid"] = userid.Text;
                        //送信先、遷移先を設定
                        var nextPage = new UserManagiment();
                        NavigationService.Navigate(nextPage);

                        //MessageBox.Show("担当者の登録が完了しました");

                    }
                    else
                    {
                        MessageBox.Show("パスワード入力欄と確認用パスワード入力欄の入力内容が異なります。");
                    }
                }
                else
                {
                    MessageBox.Show("入力項目に誤りがあります。");
                }
            }

            if((bool)pai.IsChecked == true){
                if (!username.Text.Equals("") && !username_kana.Text.Equals(""))
                {
                    DBConnect.Connect("kasiihara.db");
                    SQL = "INSERT INTO t_user (user_name,user_name_kana,login_id,password,usertype)VALUES('" + username.Text.ToString() +"','" + username_kana.Text.ToString() + "','abc','abc','患者')";
                    DBConnect.ExecuteReader(SQL);
                    SQL = "SELECT * FROM t_user ORDER BY user_id DESC";
                    DBConnect.ExecuteReader(SQL);
                    DBConnect.Reader.Read();
                    SQL = "UPDATE t_user SET login_id = 'kanjya" + DBConnect.Reader[0].ToString() + "',password = 'kanjya" + DBConnect.Reader[0].ToString() + "' WHERE user_id = '" + DBConnect.Reader[0].ToString() + "'";
                    DBConnect.ExecuteReader(SQL);
                    DBConnect.Dispose();

                    //送信するデータ格納
                    Application.Current.Properties["henshuu"] = "11";
                    Application.Current.Properties["rename"] = username.Text;
                    Application.Current.Properties["rename_kana"] = username_kana.Text;
                    Application.Current.Properties["reid"] = userid.Text;
                    //送信先、遷移先を設定
                    var nextPage = new UserManagiment();
                    NavigationService.Navigate(nextPage);

                    //MessageBox.Show("患者の登録が完了しました。");

                }
                else
                {
                    MessageBox.Show("入力項目に誤りがあります。");
                }
            }
        }

        void nyu(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Name.Equals("pai"))
            {
//                pass.IsReadOnly = true;
//                repass.IsReadOnly = true;
                userid.IsReadOnly = true;
                pass.Background = Brushes.Gainsboro;
                repass.Background = Brushes.Gainsboro;
                userid.Background = Brushes.Gainsboro;
                paiu.Background = Brushes.Coral;
                tanu.Background = null;
            }
            else
            {
//                pass.IsReadOnly = false;
//                repass.IsReadOnly = false;
                userid.IsReadOnly = false;
                pass.Background = Brushes.White;
                repass.Background = Brushes.White;
                userid.Background = Brushes.White;
                paiu.Background = null;
                tanu.Background = Brushes.Coral;
            }
        }

        void nyuclear(object sender, EventArgs e)
        {
            username.Text = null;
            username_kana.Text = null;
            userid.Text = null;
            pass.Password = null;
            repass.Password = null;
        }

    }
}

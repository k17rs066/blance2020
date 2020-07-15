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
    /// UserManagiment.xaml の相互作用ロジック
    /// </summary>
    public partial class UserManagiment : Page
    {

        public delegate void Refresh5(object sender, EventArgs e);

        int labelNum = 100;
        int Lnum = 0;

        int buttonNum = 100;
        int ButtonNum = 0;

        int heightm = 0;

        String dele = "";
        String SQL = "";

        int judge;

        int sortname2 = 0;//0id昇順、1id降順、2.名前昇順　3.名前降順
        int sortfin2 = 0;//何もしない、1:昇順 2:降順

       

        public UserManagiment()
        {
            InitializeComponent();

            try
            {
                sortname2 = int.Parse(Application.Current.Properties["sortnamee2"].ToString());
            }
            catch
            {
                sortname2 = 0;
            }
            try
            {
                sortfin2 = int.Parse(Application.Current.Properties["sortfinn2"].ToString());
            }
            catch
            {
                sortfin2 = 0;
            }

            if (!(sortname2 == 0) && !(sortname2 == 1))
            {
                yuzaname2.Background = Brushes.RoyalBlue;
                if (sortname2 == 2)
                {
                    yuzaname2.Content = "▲";
                }
                else if (sortname2 == 3)
                {
                    yuzaname2.Content = "▼";
                }
            }
            else
            {
                if(!(sortfin2 == 0))
                {
                    yuzaname2.Background = Brushes.Gainsboro;
                }
                else
                {
                    yuzaname2.Background = Brushes.Coral;
                }
                if (sortname2 == 0)
                {
                    yuzaname2.Content = "▲";
                }
                else if (sortname2 == 1)
                {
                    yuzaname2.Content = "▼";
                }
            }

            if (!(sortfin2 == 0))
            {
                finlog2.Background = Brushes.RoyalBlue;
                if (sortfin2 == 1)
                {
                    finlog2.Content = "▲";
                }
                else if (sortfin2 == 2)
                {
                    finlog2.Content = "▼";
                }
            }
            else
            {
                finlog2.Background = Brushes.Gainsboro;
                finlog2.Content = "▲";
            }

            Label[] label = new Label[labelNum];
            Button[] button = new Button[buttonNum];

            WrapPanel backbuttons = new WrapPanel();

            Button top_b = new Button() { Content = "トップ画面", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            top_b.Click += bmodo;
            backbuttons.Children.Add(top_b);

            Button logout_b = new Button() { Content = "ログアウト", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            logout_b.Click += bout;
            backbuttons.Children.Add(logout_b);

            grid7.Children.Add(backbuttons);

            DBConnect.Connect("kasiihara.db");

            DBConnect.ExecuteReader("SELECT COUNT(*) as num FROM t_user");
            DBConnect.Reader.Read();
            grid7.Height = 1080 + (int.Parse(DBConnect.Reader[0].ToString())-3) * 124;



            if (sortname2 == 0)
            {
                if (sortfin2 == 0)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_id";
                }
                else if (sortfin2 == 1)
                {
                    SQL = "SELECT * FROM t_user ORDER BY fainal_logindate";
                }
                else if (sortfin2 == 2)
                {
                    SQL = "SELECT * FROM t_user ORDER BY fainal_logindate DESC";
                }
            }

            else if (sortname2 == 1)
            {
                if (sortfin2 == 0)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_id DESC";
                }
                else if (sortfin2 == 1)
                {
                    SQL = "SELECT * FROM t_user ORDER BY fainal_logindate";
                }
                else if (sortfin2 == 2)
                {
                    SQL = "SELECT * FROM t_user ORDER BY fainal_logindate DESC";
                }
            }
            else if (sortname2 == 2)
            {
                if (sortfin2 == 0)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana";
                }
                else if (sortfin2 == 1)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana, fainal_logindate";
                }
                else if (sortfin2 == 2)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana, fainal_logindate DESC";
                }

            }
            else if (sortname2 == 3)
            {
                if (sortfin2 == 0)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana DESC";
                }
                else if (sortfin2 == 1)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana DESC, fainal_logindate";
                }
                else if (sortfin2 == 2)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana DESC, fainal_logindate DESC";
                }

            }

            DBConnect.ExecuteReader(SQL);
           
            while (DBConnect.Reader.Read())
            {
                label[Lnum] = new Label() { Content = DBConnect.Reader[1].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(106.2, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 427, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                grid7.Children.Add(label[Lnum]);
                Lnum++;
                
                label[Lnum] = new Label() { Content = DBConnect.Reader[5].ToString(), FontSize = 45, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(533.2, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                grid7.Children.Add(label[Lnum]);
                Lnum++;

                button[ButtonNum] = new Button() { Content = "", FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 80, Margin = new Thickness(980, 633.653 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 400, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                button[ButtonNum].Tag = DBConnect.Reader[0];
                button[ButtonNum].Click += edit;
                grid7.Children.Add(button[ButtonNum]);
                ButtonNum++;

                button[ButtonNum] = new Button() { Content = "", FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 80, Margin = new Thickness(1410, 633.653 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 400, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                button[ButtonNum].Tag = DBConnect.Reader[0];
                button[ButtonNum].Click += delete;
                grid7.Children.Add(button[ButtonNum]);
                ButtonNum++;

                heightm += 124;
            }

            DBConnect.Dispose();
            try
            {
                judge = int.Parse(Application.Current.Properties["henshuu"].ToString());
            }
            catch
            {
                judge = 0;

            }

            if (judge==0)
            {
                recivename.Content = " ";
            }
            else if (judge == 1)
            {
                recivechange.Content = "登録しました！";
                recivechange.Background = Brushes.Azure;
                recivename.Content = "名前 : " + Application.Current.Properties["rename"].ToString() + "\n名前カナ : " + Application.Current.Properties["rename_kana"].ToString() + "\nユーザID : " + Application.Current.Properties["reid"].ToString() + "\n種別 : 担当者";
                recivename.Background = Brushes.Azure;
                Application.Current.Properties["henshuu"] = "0";
            }
            else if (judge == 11)
            {
                recivechange.Content = "登録しました！";
                recivechange.Background = Brushes.Azure;
                recivename.Content = "名前：" + Application.Current.Properties["rename"].ToString() + "\n名前カナ : " + Application.Current.Properties["rename_kana"].ToString() + "\nユーザID : " + Application.Current.Properties["reid"].ToString() + "\n種別 : 患者";
                recivename.Background = Brushes.Azure;
                Application.Current.Properties["henshuu"] = "0";
            }
            else if (judge == 2)
            {
                recivechange.Content = "編集しました！";
                recivechange.Background = Brushes.Azure;
                recivename.Content = "名前：" + Application.Current.Properties["rename"].ToString() + "\n名前カナ : " + Application.Current.Properties["rename_kana"].ToString() + "\nユーザID : " + Application.Current.Properties["reid"].ToString() + "\n種別 : " + Application.Current.Properties["rename_type"].ToString();
                recivename.Background = Brushes.Azure;
                Application.Current.Properties["henshuu"] = "0";
            }
            else if (judge == 3)
            {
                recivechange.Content = "削除しました！";
                recivechange.Background = Brushes.Azure;
                recivename.Content = "名前：" + Application.Current.Properties["rename"].ToString() + "\n名前カナ : " + Application.Current.Properties["rename_kana"].ToString() + "\nユーザID : " + Application.Current.Properties["reid"].ToString() + "\n種別 : " + Application.Current.Properties["rename_type"].ToString();
                recivename.Background = Brushes.Azure;
                Application.Current.Properties["henshuu"] = "0";
            }


            Console.WriteLine(judge);
        }

        void deleteno(object sender, EventArgs e)
        {
            var nextPage = new UserManagiment();
            NavigationService.Navigate(nextPage);
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

        void nu(object sender, EventArgs e)
        {
            var nextPage = new newUser();
            NavigationService.Navigate(nextPage);
        }

        void edit(object sender, EventArgs e)
        {
            Application.Current.Properties["e_id"] = (sender as Button).Tag.ToString();
            var nextPage = new UserEdit();
            NavigationService.Navigate(nextPage);
        }

        void delete(object sender, EventArgs e)
        {
            DBConnect.Connect("kasiihara.db");
            SQL = "SELECT * FROM t_user WHERE user_id = '" + int.Parse((sender as Button).Tag.ToString()) + "'";
            DBConnect.ExecuteReader(SQL);
            DBConnect.Reader.Read();

            Application.Current.Properties["deletename"] = DBConnect.Reader[1].ToString(); 
            Application.Current.Properties["deletekananame"] = DBConnect.Reader[6].ToString(); 
            Application.Current.Properties["deletetype"] = DBConnect.Reader[4].ToString();
            Application.Current.Properties["deleteid"] = DBConnect.Reader[0].ToString();

            //dele = DBConnect.Reader[1].ToString();
            DBConnect.Dispose();

            User_Delete w = new User_Delete(this.deleteno);
            w.Title = "User_Delete";
            w.ShowDialog();


            /* MessageBoxResult result = MessageBox.Show(dele + "を削除しますか？", "ユーザ削除", MessageBoxButton.YesNo, MessageBoxImage.Warning);

             if (result == MessageBoxResult.Yes)
             {
                 DBConnect.Connect("kasiihara.db");
                 SQL = "DELETE FROM t_user WHERE user_id = '" + int.Parse((sender as Button).Tag.ToString()) + "'";
                 DBConnect.ExecuteReader(SQL);
                 DBConnect.Dispose();
                 MessageBox.Show(dele + "を削除しました。");
                 var nextPage = new UserManagiment();
                 NavigationService.Navigate(nextPage);
             }
             */

        }


        void sorttt(object sender, EventArgs e)
        {
            if ((sender as Button).Name.Equals("yuzaname2"))
            {
                if (sortname2 == 0)
                {
                    Application.Current.Properties["sortnamee2"] = 1;
                }
                else if (sortname2 == 1)
                {
                    Application.Current.Properties["sortnamee2"] = 2;
                }
                else if(sortname2 == 2)
                {
                    Application.Current.Properties["sortnamee2"] = 3;
                }
                else
                {
                    Application.Current.Properties["sortnamee2"] = 0;
                }
            }
            else
            {
                if (sortfin2 == 0)
                {
                    Application.Current.Properties["sortfinn2"] = 1;
                }
                else if (sortfin2 == 1)
                {
                    Application.Current.Properties["sortfinn2"] = 2;
                }
                else
                {
                    Application.Current.Properties["sortfinn2"] = 0;
                }
            }

            var nextPage = new UserManagiment();
            NavigationService.Navigate(nextPage);
        }
    }
}

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
    /// UserRecordCheck.xaml の相互作用ロジック
    /// </summary>
    public partial class UserRecordCheck : Page
    {
        int labelNum = 100;
        int Lnum = 0;

        int buttonNum = 100;
        int ButtonNum = 0;

        int heightm = 0;

        int t_id = -1;

        int sortname = 0;//何もしない、１、昇順　2:降順
        int sortfin = 0;//何もしない、1:昇順 2:降順

        String SQL = "";

        public UserRecordCheck()
        {
            InitializeComponent();

            try
            {
                sortname = int.Parse(Application.Current.Properties["sortnamee"].ToString());
            }
            catch
            {
                sortname = 0;
            }
            try
            {
                sortfin = int.Parse(Application.Current.Properties["sortfinn"].ToString());
            }
            catch
            {
                sortfin = 0;
            }

            if (!(sortname == 0) && !(sortname == 1))
            {
                yuzaname.Background = Brushes.RoyalBlue;
                if (sortname == 2)
                {
                    yuzaname.Content = "▲";
                }
                else if (sortname == 3)
                {
                    yuzaname.Content = "▼";
                }
            }
            else
            {
                if (!(sortfin == 0))
                {
                    yuzaname.Background = Brushes.Gainsboro;
                }
                else
                {
                    yuzaname.Background = Brushes.Coral;
                }
                if (sortname == 0)
                {
                    yuzaname.Content = "▲";
                }
                else if (sortname == 1)
                {
                    yuzaname.Content = "▼";
                }
            }

            if (!(sortfin == 0))
            {
                finlog.Background = Brushes.RoyalBlue;
                if (sortfin == 1)
                {
                    finlog.Content = "▲";
                }
                else if (sortfin == 2)
                {
                    finlog.Content = "▼";
                }
            }
            else
            {
                finlog.Background = Brushes.Gainsboro;
                finlog.Content = "▲";
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

            grid5.Children.Add(backbuttons);

            t_id = int.Parse(Application.Current.Properties["u_id"].ToString());

            DBConnect.Connect("kasiihara.db");

            DBConnect.ExecuteReader("SELECT COUNT(*) as num FROM t_user");
            DBConnect.Reader.Read();
            grid5.Height = 1080 + (int.Parse(DBConnect.Reader[0].ToString()) - 4) * 124;


            if (sortname == 0)
            {
                if (sortfin == 0)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_id";
                }
                else if (sortfin == 1)
                {
                    SQL = "SELECT * FROM t_user ORDER BY fainal_logindate";
                }
                else if (sortfin == 2)
                {
                    SQL = "SELECT * FROM t_user ORDER BY fainal_logindate DESC";
                }
            }
            else if (sortname == 1)
            {
                if (sortfin == 0)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_id DESC";
                }
                else if (sortfin == 1)
                {
                    SQL = "SELECT * FROM t_user ORDER BY fainal_logindate";
                }
                else if (sortfin == 2)
                {
                    SQL = "SELECT * FROM t_user ORDER BY fainal_logindate DESC";
                }
            }
            else if (sortname == 2)
            {
                if (sortfin == 0)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana";
                }
                else if (sortfin == 1)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana, fainal_logindate";
                }
                else if (sortfin == 2)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana, fainal_logindate DESC";
                }

            }
            else if (sortname == 3)
            {
                if (sortfin == 0)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana DESC";
                }
                else if (sortfin == 1)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana DESC, fainal_logindate";
                }
                else if (sortfin == 2)
                {
                    SQL = "SELECT * FROM t_user ORDER BY user_name_kana DESC, fainal_logindate DESC";
                }

            }


            DBConnect.ExecuteReader(SQL);

            while (DBConnect.Reader.Read())
            {
                label[Lnum] = new Label() { Content = DBConnect.Reader[1].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(238, 534.253 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 487, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                grid5.Children.Add(label[Lnum]);
                Lnum++;
                if (!DBConnect.Reader[5].ToString().Equals(""))
                {
                    label[Lnum] = new Label() { Content = DBConnect.Reader[5].ToString(), FontSize = 45, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(725, 534.253 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 487, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid5.Children.Add(label[Lnum]);
                    Lnum++;
                }
                else
                {
                    label[Lnum] = new Label() { Content = "", FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(725, 534.253 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 487, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid5.Children.Add(label[Lnum]);
                    Lnum++;
                }

                button[ButtonNum] = new Button() { Content = "", FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 90, Margin = new Thickness(1227, 551.253 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 442.8, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                button[ButtonNum].Tag = DBConnect.Reader[0];
                button[ButtonNum].Click += record;
                grid5.Children.Add(button[ButtonNum]);
                ButtonNum++;


                heightm += 124;
            }

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

        void record(object sender, EventArgs e)
        {
            Application.Current.Properties["u_id"] = (sender as Button).Tag;
            Application.Current.Properties["t_id"] = t_id;
            var nextPage = new k_RecordConfirmation();
            NavigationService.Navigate(nextPage);
        }

        void sortt(object sender, EventArgs e)
        {
            if ((sender as Button).Name.Equals("yuzaname"))
            {
                if (sortname == 0)
                {
                    Application.Current.Properties["sortnamee"] = 1;
                }
                else if (sortname == 1)
                {
                    Application.Current.Properties["sortnamee"] = 2;
                }
                else if(sortname == 2)
                {
                    Application.Current.Properties["sortnamee"] = 3;
                }
                else
                {
                    Application.Current.Properties["sortnamee"] = 0;
                }
            }
            else
            {
                if (sortfin == 0)
                {
                    Application.Current.Properties["sortfinn"] = 1;
                }
                else if (sortfin == 1)
                {
                    Application.Current.Properties["sortfinn"] = 2;
                }
                else
                {
                    Application.Current.Properties["sortfinn"] = 0;
                }
            }

            var nextPage = new UserRecordCheck();
            NavigationService.Navigate(nextPage);
        }
    }
}

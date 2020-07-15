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
using System.Windows.Threading;

namespace balance.Views
{
    /// <summary>
    /// t_top.xaml の相互作用ロジック
    /// </summary>
    public partial class t_top : Page
    {
        String SQL;
        public t_top()
        {
            InitializeComponent();

            name.Content = Application.Current.Properties["kname"].ToString();

            int buttonNum = 15;
            Button[] button = new Button[buttonNum];
            int LeftMargin = 175;
            int TopMargin = 600;
            int ButtonNum = 0;
            String[] ButtonContent = {"患者記録閲覧","ユーザ管理","キャリブレーション","パスワード変更"};
            String kari;

            kari =Application.Current.Properties["k_id"].ToString();
            int k_id = int.Parse(kari);
            SQL = "SELECT * FROM t_user WHERE user_id = " + k_id;
            DBConnect.Connect("kasiihara.db");
            DBConnect.ExecuteReader(SQL);
            DBConnect.Reader.Read();
            if (!DBConnect.Reader[5].ToString().Equals(""))
            {
                datetimedb.Text = DBConnect.Reader[5].ToString();
            }
            else
            {
                las.Content = "";
            }

            DBConnect.Dispose();

            DBConnect.Connect("kasiihara.db");
            SQL = "UPDATE t_user SET fainal_logindate = '" + DateTime.Now.ToString() + "' WHERE user_id = '" + int.Parse(Application.Current.Properties["k_id"].ToString()) + "'";
            DBConnect.ExecuteReader(SQL);
            DBConnect.Dispose();

            Application.Current.Properties["u_id"] = k_id;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick +=timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();


            WrapPanel wrapPanel = new WrapPanel();
            WrapPanel wrapPanel2 = new WrapPanel();
            WrapPanel logout = new WrapPanel();

            Grid grid = new Grid();

            Thickness margin = new Thickness(LeftMargin, TopMargin, 0, 0);
            Thickness margin2 = new Thickness(LeftMargin, TopMargin + 200, 0, 0);
            Thickness marginl = new Thickness(100, 50, 0, 0);

            for (int i = 0; i < 2; i++ )
            {

                button[ButtonNum] = new Button() { Content = ButtonContent[i], FontSize = 70, Margin = margin, Width = 700, Height = 150, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.Coral };
                if (i == 1)
                {
                    button[ButtonNum].Click += manage;
                }
                else if (i == 0)
                {
                    button[ButtonNum].Click += reco;
                }
                wrapPanel.Children.Add(button[ButtonNum]);

                ButtonNum++;
            }

            for (int i = 2; i < 4; i++)
            {

                button[ButtonNum] = new Button() { Content = ButtonContent[i], FontSize = 70, Margin = margin2, Width = 700, Height = 150, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.Coral };
                if (i == 2)
                {
                    button[ButtonNum].Click += calib;
                }
                else if (i == 3)
                {
                    button[ButtonNum].Click += p_edit;
                }
                wrapPanel2.Children.Add(button[ButtonNum]);

                ButtonNum++;
            }


            Button logoutb = new Button() { Content = "ログアウト", FontSize = 30,Margin = marginl, Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press};
            logoutb.Click += logouta;

            logout.Children.Add(logoutb);
 

            grid4.Children.Add(wrapPanel);
            grid4.Children.Add(wrapPanel2);
            grid4.Children.Add(logout);
        }

        void logouta(object sender,EventArgs e)
        {
            Application.Current.Properties["t_id"] = null;
            var nextPage = new MainTitle();
            NavigationService.Navigate(nextPage);
        }

        void timer_Tick(object sender,EventArgs e)
        {
            datetimeb.Text = DateTime.Now.ToString();
        }

        void reco(object sender, EventArgs e)
        {
            var nextPage = new UserRecordCheck();
            NavigationService.Navigate(nextPage);
        }

        void manage(object sender, EventArgs e)
        {
            var nextPage = new UserManagiment();
            NavigationService.Navigate(nextPage);
        }

        void p_edit(object sender, EventArgs e)
        {
            var nextPage = new PasswordEdit();
            NavigationService.Navigate(nextPage);
        }

        void calib(object sender, EventArgs e)
        {
            var nextPage = new Calibration();
            NavigationService.Navigate(nextPage);
        }
    }
}

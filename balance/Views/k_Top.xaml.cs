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

using MySql.Data.MySqlClient;

//記録表彰のボタン追加

namespace balance.Views
{
    /// <summary>
    /// Login.xaml の相互作用ロジック
    /// </summary>
    public partial class k_Top : Page
    {
        String SQL;

        public k_Top()
        {
            InitializeComponent();

            
            name.Content = Application.Current.Properties["UserName"].ToString();

            int buttonNum = 15;
            Button[] button = new Button[buttonNum];
            int LeftMargin = 120;
            int TopMargin = 600;
            int ButtonNum = 0;
            String[] ButtonContent = {"バランスモード","ゲーム選択","全記録確認","個人表彰"};
            String kari;

            kari =Application.Current.Properties["User_id"].ToString();
            if (!kari.Equals("guest"))
            {
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
                SQL = "UPDATE t_user SET fainal_logindate = '" + DateTime.Now.ToString() + "' WHERE user_id = '" + int.Parse(Application.Current.Properties["User_id"].ToString()) + "'";
                DBConnect.ExecuteReader(SQL);
                DBConnect.Dispose();

                Application.Current.Properties["u_id"] = k_id;
            }
            else
            {
                las.Content = "";
                Application.Current.Properties["u_id"] = "guest";
            }


            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick +=timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();


            WrapPanel wrapPanel = new WrapPanel();
            WrapPanel wrapPanel2 = new WrapPanel();
            WrapPanel logout = new WrapPanel();


            Thickness margin = new Thickness(LeftMargin,TopMargin, 0, 0);
            Thickness margin2 = new Thickness(LeftMargin, TopMargin + 200, 0, 0);
            Thickness marginl = new Thickness(80, 50, 0, 0);

            for (int i = 0; i <2; i++ )
            {

                button[ButtonNum] = new Button() { Content = ButtonContent[i], FontSize = 70, Margin = margin, Width = 700, Height = 150, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.Coral };
                if (i == 1)
                {
                    button[ButtonNum].Click += game;
                }
                else if (i == 0)
                {
                    button[ButtonNum].Click += train;
                }
               

                wrapPanel.Children.Add(button[ButtonNum]);

                ButtonNum++;
            }

            if (!kari.Equals("guest"))
            {

                for (int i = 2; i < 4; i++)
                {

                    button[ButtonNum] = new Button() { Content = ButtonContent[i], FontSize = 70, Margin = margin2, Width = 700, Height = 150, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.Coral };

                    if (i == 3)
                    {
                        button[ButtonNum].Click += newrecord;
                    }
                    else
                    {
                        if (!kari.ToString().Equals("guest"))
                        {
                            button[ButtonNum].Click += record;
                        }
                        else
                        {
                            button[ButtonNum].Click += guestrecord;
                        }
                    }


                    wrapPanel2.Children.Add(button[ButtonNum]);

                    ButtonNum++;

                }
            }

            Button logoutb = new Button() { Content = "ログアウト", FontSize = 30,Margin = marginl, Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press};
            logoutb.Click += logouta;

            logout.Children.Add(logoutb);
 

            grid2.Children.Add(wrapPanel);
            grid2.Children.Add(wrapPanel2);
            grid2.Children.Add(logout);
        }


        void logouta(object sender,EventArgs e)
        {
            var nextPage = new MainTitle();
            NavigationService.Navigate(nextPage);
        }

        void timer_Tick(object sender,EventArgs e)
        {
            datetimeb.Text = DateTime.Now.ToString();
        }

        void game(object sender, EventArgs e)
        {
            var nextPage = new GameSelect();
            NavigationService.Navigate(nextPage);
        }

        void train(object sender, EventArgs e)
        {
            var nextPage = new LoadTraining();
            NavigationService.Navigate(nextPage);
        }

        void record(object sender, EventArgs e)
        {
            var nextPage = new k_RecordConfirmation();
            NavigationService.Navigate(nextPage);
        }

        void guestrecord(object sender, EventArgs e)
        {
            MessageBox.Show("Gusetは記録確認できません。");
        }

        void newrecord(object sender, EventArgs e)
        {
            var nextPage = new New_k_record();
            NavigationService.Navigate(nextPage);
        }


    }
}

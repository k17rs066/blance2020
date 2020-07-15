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

using WiimoteLib;
using balance.DataBase;

namespace balance.Views
{
    /// <summary>
    /// Calibration.xaml の相互作用ロジック
    /// </summary>
    public partial class Calibration : Page
    {
        String SQL = "";
        Wiimote wiimote = new Wiimote();

        double change = 0;

        public Calibration()
        {
            InitializeComponent();

            wiimote.Connect();
            wiimote.WiimoteChanged += OnWiimoteChanged;

            chan.TextChanged += ch;

            DBConnect.Connect("kasiihara.db");
            SQL = "SELECT * FROM t_Calibration ORDER BY calibration_id DESC";
            DBConnect.ExecuteReader(SQL);
            if (DBConnect.Reader.Read() == true)
            {
                change = int.Parse(DBConnect.Reader[1].ToString());
                chan.Text = (int.Parse(DBConnect.Reader[1].ToString())).ToString();
            }
            DBConnect.Dispose();

            WrapPanel backbuttons = new WrapPanel();

            Button top_b = new Button() { Content = "トップ画面", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            top_b.Click += bmodo;
            backbuttons.Children.Add(top_b);

            Button logout_b = new Button() { Content = "ログアウト", FontSize = 30, Margin = new Thickness(100, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            logout_b.Click += bout;
            backbuttons.Children.Add(logout_b);

            grid9.Children.Add(backbuttons);
        }

        void OnWiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            BalanceBoardState bbs = e.WiimoteState.BalanceBoardState;
            Dispatcher.Invoke(new Action(() =>
            {
//                nowtai.Text = ((double)(((int)(bbs.WeightKg*10)))/10).ToString();
//                afetai.Text = ((double)(((int)(bbs.WeightKg*10 + change*10)))/10).ToString();
                nowtai.Text = ((int)(((int)(bbs.WeightKg*10)))/10).ToString();
                afetai.Text = ((int)(((int)(bbs.WeightKg*10 + change*10)))/10).ToString();

            }));
        }
        
        void bout(object sender, EventArgs e)
        {
            wiimote.Disconnect();
            var nextPage = new MainTitle();
            NavigationService.Navigate(nextPage);
        }

        void bmodo(object sender, EventArgs e)
        {
            wiimote.Disconnect();
            var nextPage = new t_top();
            NavigationService.Navigate(nextPage);
        }

        void Page_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           wiimote.Disconnect();
        }

 /*       void chou(object sender, EventArgs e)
        {
            change = double.Parse(chan.Text.ToString());
        }
        */
        void mem(object sender, EventArgs e)
        {
            if (!chan.Text.ToString().Equals(""))
            {
                DBConnect.Connect("kasiihara.db");
                SQL = "INSERT INTO t_Calibration (change_kg,change_date)VALUES('" + change.ToString() + "','" + DateTime.Now.ToString() + "')";
                DBConnect.ExecuteReader(SQL);
                DBConnect.Dispose();

                MessageBox.Show("登録が完了しました。");
            }
        }

        private void ch(object sender, TextChangedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    change = (double)int.Parse(chan.Text);
                }));
            }
            catch
            {
            }
        }
    }
}

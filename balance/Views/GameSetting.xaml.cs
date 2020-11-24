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
    /// GameSetting.xaml の相互作用ロジック
    /// </summary>
    public partial class GameSetting : Window
    {

        public GameMode.Refresh rf = null;
        int bnum = -1;

        String SQL = "";
        int c_line = 70;
        int seti = 10;
        int user_id;
        public GameSetting()
        {
            /*InitializeComponent();

            //gamemode.Text = Application.Current.Properties["gamemodename"].ToString();
            han.Text = Application.Current.Properties["hantei"].ToString() + ":";
            tan.Text = Application.Current.Properties["tani"].ToString();*/

            

        }

        public GameSetting(GameMode.Refresh pRefresh)
        {
            InitializeComponent();

            
            //gamemode.Text = Application.Current.Properties["gamemodename"].ToString();
            han.Text = Application.Current.Properties["hantei"].ToString() + ":";
            tan.Text = Application.Current.Properties["tani"].ToString();
            user_id = int.Parse(Application.Current.Properties["setuser_id"].ToString());

            timebutton.Background = Brushes.Gainsboro;
            scorebutton.Background = Brushes.Coral;

            rf = pRefresh;


            DBConnect.Connect("kasiihara.db");
            SQL = "SELECT * FROM t_userrecord NATURAL JOIN t_gametrain WHERE user_id ='" + user_id +"' AND traintype ='" + Application.Current.Properties["gamemodename"].ToString() + "' ORDER BY userrecord_id DESC";
            DBConnect.ExecuteReader(SQL);
            if (DBConnect.Reader.Read())
            {
                c_line = int.Parse(DBConnect.Reader[7].ToString());
                seti = int.Parse(DBConnect.Reader[5].ToString());
            }
            DBConnect.Dispose();

            for (int i = 50; i <= 100; i+=10)
            {
                if (i == c_line)
                {
                    combo.Items.Add(new ComboBoxItem() {IsSelected = true, Content = i});
                }
                else
                {
                    combo.Items.Add(i);
                }
            }
            for (int i = 10; i <= 100; i+=10)
            {
                if (i == seti)
                {
                    combokei.Items.Add(new ComboBoxItem() { IsSelected = true, Content = i });
                }
                else
                {
                    combokei.Items.Add(i);
                }
            }


        }



        void Back_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        void ok_Click(object sender, EventArgs e)
        {
            if (!combo.Text
                .ToString().Equals("") && !combokei.Text.ToString().Equals(""))
            {
                if (han.Text.Equals("計測時間:"))
                {
                    bnum = 0;
                }
                else
                {
                    bnum = 1;
                }
                Application.Current.Properties["line"] = combo.Text;
                Application.Current.Properties["settei"] = han.Text + combokei.Text + tan.Text;
                Application.Current.Properties["sette"] =combokei.Text;
                rf(bnum);
            }
            this.Close();
        }


         void scoreButton_Click(object sender, EventArgs e)
        {
            timebutton.Background = Brushes.Gainsboro;
            scorebutton.Background = Brushes.Coral;
            han.Text = "計測時間:";
            tan.Text = "秒";
            Application.Current.Properties["gamemodename"] = "スコアアタック";

        }

         void timeButton_Click(object sender, EventArgs e)
        {
            timebutton.Background = Brushes.Coral;
            scorebutton.Background = Brushes.Gainsboro;
            han.Text = "計測回数:";
            tan.Text = "回";
            Application.Current.Properties["gamemodename"] = "タイムアタック";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// New_k_record.xaml の相互作用ロジック
    /// 設定のOKボタンを画面に配置、コードではない
    /// </summary>
    public partial class New_k_record : Page
    {


        int labelNum = 100;
        int Lnum = 0; //タイムアタック列
        int Lnum2 = 0;//スコアアタック列

        int t_id = -1;

        int heightm = 0; ///タイムアタック表のマスの高さ
        int heighta = 0; ///スコアアタック表のマスの高さ
        private Button bac_b = null;

        int rank = 1;　　//タイム順位
        int scorerank = 1; //スコア順位


        int line = 60;//ゲームモードのクリアライン

        int settei = 10; //ゲームモードの秒数。回数
        int fallse = 30;　//落下ゲームの秒数


        int u_id;

        int gamejudge;

        public New_k_record()
        {
            InitializeComponent();

           
            //グラフメソッド呼び出し
            //showColumnChart();

            /* if (bac_b != null) //nullは表示せずにエラー、閉じる
             {
                 grid33.Children.Remove(bac_b);
                 bac_b = null;
             }*/


            ///Label[] label = new Label[labelNum];

            ///Label[] label2 = new Label[labelNum];　　　///生成列の配列
            WrapPanel backbuttons1 = new WrapPanel();

            Button top_b = new Button() { Content = "トップ画面", FontSize = 30, Margin = new Thickness(80, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            top_b.Click += bmodo;
            backbuttons1.Children.Add(top_b);

            Button logout_b = new Button() { Content = "ログアウト", FontSize = 30, Margin = new Thickness(80, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            logout_b.Click += bout;
            backbuttons1.Children.Add(logout_b);

            grid33.Children.Add(backbuttons1);
            if (Application.Current.Properties["t_id"] != null)
            {
                t_id = int.Parse(Application.Current.Properties["t_id"].ToString());
            }

            if (t_id != -1)
            {
                bac_b = new Button() { Content = "戻る", FontSize = 50, HorizontalAlignment = HorizontalAlignment.Left, Height = 99.2, Margin = new Thickness(1550, 50, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 250 };
                bac_b.Click += bac;

                grid33.Children.Add(bac_b);
            }

            //int u_id;

            u_id = int.Parse(Application.Current.Properties["u_id"].ToString());

            DBConnect.Connect("kasiihara.db");

            DBConnect.ExecuteReader("SELECT COUNT(*) as num FROM t_userrecord WHERE user_id = '" + u_id + "'");
            DBConnect.Reader.Read();
            //////////表の列数が3以上
            /*
            if (int.Parse(DBConnect.Reader[0].ToString()) > 3)
            {
                grid33.Height = 1080 + (int.Parse(DBConnect.Reader[0].ToString()) - 3) * 124;
                //                scroll1.Height = grid3.Height;
            }
            */
            DBConnect.Dispose();


            String SQL;
            SQL = "SELECT * FROM t_user WHERE user_id = " + u_id;
            DBConnect.Connect("kasiihara.db");

            DBConnect.ExecuteReader(SQL);
            DBConnect.Reader.Read();
            name.Content = DBConnect.Reader[1].ToString();

            /*
            if (t_id == -1)
            {
                if (!DBConnect.Reader[5].ToString().Equals(""))
                {
                    datetimedb.Text = DBConnect.Reader[5].ToString();
                }
                else
                {
                    las.Content = "";
                }
            }
            else
            {
                las.Content = "";
            }

            gamejudge = 1;

            /*
            hyouname.Content = "タイムアタック";
            chartname.Content = "タイムアタック";
            */

            percent.Text = "50";
            setting_fall.Text = "30";

            DBConnect.Dispose();

            for (int i = 50; i <= 100; i += 10)
            {
                if (i == line)
                {
                    percent.Items.Add(new ComboBoxItem() { IsSelected = true, Content = i });
                }
                else
                {
                    percent.Items.Add(i);
                }
            }

            for (int i = 30; i <= 90; i += 30)
            {
                if (i == fallse)
                {
                    setting_fall.Items.Add(new ComboBoxItem() { IsSelected = true, Content = i });
                }
                else
                {
                    setting_fall.Items.Add(i);
                }
            }

            for (int i = 10; i <= 100; i += 10)
            {
                if (i == settei)
                {
                    setting.Items.Add(new ComboBoxItem() { IsSelected = true, Content = i });
                }
                else
                {
                    setting.Items.Add(i);
                }
            }


            gamejudge = 1;
            //表メソッド呼び出し
            hyou(gamejudge);
            //グラフメソッド呼び出し
            showColumnChart(gamejudge);
            //設定の有無
            combo(gamejudge);
            //advice
            hyouadvice.Content = "回数が多いほど\r\n好記録ですよ！";
            chartadvice.Content = "値が高い所が\r\n好記録ですよ！";
            scorebutton.Background = Brushes.Coral;

            titlechart.Content = "スコアアタック";

        }

        private void scorebutton_Click(object sender, RoutedEventArgs e)
        {
            gamejudge = 1;
            /*
            hyouname.Content = "スコアアタック";
            chartname.Content = "スコアアタック";
            */
            //表メソッド呼び出し
            hyou(gamejudge);
            //グラフメソッド呼び出し
            showColumnChart(gamejudge);
            //設定の有無
            combo(gamejudge);
            //advice
            hyouadvice.Content = "回数が多いほど\r\n好記録ですよ！";
            chartadvice.Content = "値が高い所が\r\n好記録ですよ！";

            scorebutton.Background = Brushes.Coral;
            timebutton.Background = Brushes.Gainsboro;
            targetbutton.Background = Brushes.Gainsboro;
            fallbutton.Background = Brushes.Gainsboro;


            titlechart.Content = "スコアアタック";
        }

        private void timebutton_Click(object sender, RoutedEventArgs e)
        {
            gamejudge = 2;
            /*
            hyouname.Content = "タイムアタック";
            chartname.Content = "タイムアタック";
            */
            //表メソッド呼び出し
            hyou(gamejudge);
            //グラフメソッド呼び出し
            showColumnChart(gamejudge);
            //設定の有無
            combo(gamejudge);
            //advice
            hyouadvice.Content = "秒数が早いほど\r\n好記録ですよ！";
            chartadvice.Content = "値が低い所が\r\n好記録ですよ！";

            timebutton.Background = Brushes.Coral;
            scorebutton.Background = Brushes.Gainsboro;
            targetbutton.Background = Brushes.Gainsboro;
            fallbutton.Background = Brushes.Gainsboro;

            titlechart.Content = "タイムアタック";

        }


        private void targetbutton_Click(object sender, RoutedEventArgs e)
        {
            gamejudge = 3;
            /*
            hyouname.Content = "ターゲットゲーム";
            chartname.Content = "ターゲットゲーム";
            */
            //表メソッド呼び出し
            hyou(gamejudge);
            //グラフメソッド呼び出し
            showColumnChart(gamejudge);
            //設定の有無
            combo(gamejudge);
            //advice
            hyouadvice.Content = "秒数が早いほど\r\n好記録ですよ！";
            chartadvice.Content = "値が低い所が\r\n好記録ですよ！";

            targetbutton.Background = Brushes.Coral;
            timebutton.Background = Brushes.Gainsboro;
            scorebutton.Background = Brushes.Gainsboro;
            fallbutton.Background = Brushes.Gainsboro;


            titlechart.Content = "ターゲットゲーム";
        }

        private void fallbutton_Click(object sender, RoutedEventArgs e)
        {
            gamejudge = 4;
            /*
            hyouname.Content = "落下ゲーム";
            chartname.Content = "落下ゲーム";
            */
            //表メソッド呼び出し
            hyou(gamejudge);
            //グラフメソッド呼び出し
            showColumnChart(gamejudge);
            //設定の有無
            combo(gamejudge);
            //advice
            hyouadvice.Content = "得点が高いほど\r\n好記録ですよ！";
            chartadvice.Content = "値が高い所が\r\n好記録ですよ！";

            fallbutton.Background = Brushes.Coral;
            timebutton.Background = Brushes.Gainsboro;
            scorebutton.Background = Brushes.Gainsboro;
            targetbutton.Background = Brushes.Gainsboro;

            titlechart.Content = "落下ゲーム";


        }


        /// <summary>
        /// タイムアタックグラフ
        /// </summary>
        private void showColumnChart(int a)
        {
            switch (a)
            {
                case 1:



                    DBConnect.Connect("kasiihara.db");

                    line = int.Parse(percent.Text);
                    settei = int.Parse(setting.Text);


                    string SQL2;
                    SQL2 = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + " and clear_line = " + line + "  and setting = " + settei + "  and traintype = 'スコアアタック' ";

                    DBConnect.ExecuteReader(SQL2);
                    Console.WriteLine(SQL2);

                    List<KeyValuePair<string, int>> scorevalueList = new List<KeyValuePair<string, int>>();



                    while (DBConnect.Reader.Read())
                    {
                        int clear_record = int.Parse(DBConnect.Reader[3].ToString());
                        //DBConnect.Reader[7].ToString().Substring(0, 15);


                        scorevalueList.Add(new KeyValuePair<string, int>(DBConnect.Reader[7].ToString(), clear_record));

                        Console.WriteLine(DBConnect.Reader[7].ToString() + "," + clear_record);



                    }
                    //Setting data for line chart


                    lineChart.DataContext = scorevalueList;

                    DBConnect.Dispose();

                    break;


                case 2:
                    
           
                    DBConnect.Connect("kasiihara.db");
                    line = int.Parse(percent.Text);
                    settei = int.Parse(setting.Text);

                    string SQLa;
                    SQLa = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and clear_line = " + line + " and setting = " + settei + "  and traintype = 'タイムアタック' ";
                    //SQLa = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = 10 and clear_line = 50 and traintype = 'タイムアタック' ";
                    DBConnect.ExecuteReader(SQLa);


                    List<KeyValuePair<string, int>> timevalueList = new List<KeyValuePair<string, int>>();


                    
                    while (DBConnect.Reader.Read())
                    {

                        int clear_record = int.Parse(DBConnect.Reader[3].ToString());
                       

                        timevalueList.Add(new KeyValuePair<string, int>(DBConnect.Reader[7].ToString(), clear_record));

                        Console.WriteLine(DBConnect.Reader[7].ToString() + "," + clear_record);

                    }
                    //Setting data for line chart
                    lineChart.DataContext = timevalueList;

                 
                    DBConnect.Dispose();


                    break;

                

                case 3:

                    DBConnect.Connect("kasiihara.db");

                 
                    string SQL3;
                    SQL3 = "SELECT * FROM t_targetgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + " ";

                    DBConnect.ExecuteReader(SQL3);
                    Console.WriteLine(SQL3);

                    List<KeyValuePair<string, int>> targetvalueList = new List<KeyValuePair<string, int>>();


                    while (DBConnect.Reader.Read())
                    {
                        int result_time = int.Parse(DBConnect.Reader[2].ToString());

                        targetvalueList.Add(new KeyValuePair<string, int>(DBConnect.Reader[5].ToString(), result_time));

                        Console.WriteLine(DBConnect.Reader[5].ToString() + "," + result_time);

                    }
                    //Setting data for line chart


                    lineChart.DataContext = targetvalueList;


                    DBConnect.Dispose();

                    break;

                case 4:

                    DBConnect.Connect("kasiihara.db");

                    fallse = int.Parse(setting_fall.Text);

                    string SQL4;
                    SQL4 = "SELECT * FROM t_fallgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + "  and set_time = " + fallse + "  ";

                    DBConnect.ExecuteReader(SQL4);
                    Console.WriteLine(SQL4);

                    List<KeyValuePair<string, int>> fallvalueList = new List<KeyValuePair<string, int>>();


                    while (DBConnect.Reader.Read())
                    {
                        int result_score = int.Parse(DBConnect.Reader[3].ToString());

                        fallvalueList.Add(new KeyValuePair<string, int>(DBConnect.Reader[6].ToString(), result_score));

                        Console.WriteLine(DBConnect.Reader[6].ToString() + "," + result_score);

                    }
                    //Setting data for line chart


                    lineChart.DataContext = fallvalueList;


                    DBConnect.Dispose();

                    break;

            }
            
 
        }

       
        void bout(object sender, EventArgs e)
        {
            t_id = -1;
            Application.Current.Properties["t_id"] = null;
            if (bac_b != null)
            {
                grid33.Children.Remove(bac_b);
            }
            var nextPage = new MainTitle();
            NavigationService.Navigate(nextPage);
        }

        void bmodo(object sender, EventArgs e)
        {
            if (t_id == -1)
            {
                t_id = -1;
                Application.Current.Properties["t_id"] = null;
                if (bac_b != null)
                {
                    grid33.Children.Remove(bac_b);
                }
                var nextPage = new k_Top();
                NavigationService.Navigate(nextPage);
            }
            else
            {
                t_id = -1;
                Application.Current.Properties["t_id"] = null;
                if (bac_b != null)
                {
                    grid33.Children.Remove(bac_b);
                }
                var nextPage = new t_top();
                NavigationService.Navigate(nextPage);
            }
        }

        void bac(object sender, EventArgs e)
        {
            t_id = -1;
            Application.Current.Properties["u_id"] = t_id;
            Application.Current.Properties["t_id"] = null;
            if (bac_b != null)
            {
                grid33.Children.Remove(bac_b);
            }
            var nextPage = new UserRecordCheck();
            NavigationService.Navigate(nextPage);
        }

         
        void timeok_Click(object sender, RoutedEventArgs e)
        {
            //表メソッド呼び出し
            hyou(gamejudge);
            //グラフメソッド呼び出し
            showColumnChart(gamejudge);
        }

        void hyou(int a)
        {
            switch (a)
            {
                case 1:

                   

                    line = int.Parse(percent.Text);
                    settei = int.Parse(setting.Text);

                    DBConnect.Connect("kasiihara.db");

                    String SQL2;


                    ///////////////////////1位スコアアタック表出力
                    SQL2 = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + " and clear_line = " + line + "  and setting = " + settei + "  and traintype = 'スコアアタック' ORDER BY clear_record DESC limit 1 offset 0 ";
                    DBConnect.ExecuteReader(SQL2);
                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[7].ToString().Substring(15, 1).Equals(":"))
                        {
                            date1.Content = DBConnect.Reader[7].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date1.Content = DBConnect.Reader[7].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line1.Content = DBConnect.Reader[3].ToString() + "回";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date1.Content = "データなし";
                        ///////クリア記録
                        line1.Content = "データなし";

                        DBConnect.Dispose();
                    }




                    ///////////////////////2位スコアアタック表出力
                    DBConnect.Connect("kasiihara.db");
                    SQL2 = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and clear_line = " + line + "  and setting = " + settei + "  and traintype = 'スコアアタック' ORDER BY clear_record DESC limit 1 offset 1 ";
                    DBConnect.ExecuteReader(SQL2);

                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[7].ToString().Substring(15, 1).Equals(":"))
                        {
                            date2.Content = DBConnect.Reader[7].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date2.Content = DBConnect.Reader[7].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line2.Content = DBConnect.Reader[3].ToString() + "回";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date2.Content = "データなし";
                        ///////クリア記録
                        line2.Content = "データなし";
                        DBConnect.Dispose();
                    }



                    ///////////////////////3位スコアアタック表出力
                    DBConnect.Connect("kasiihara.db");
                    SQL2 = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + " and clear_line = " + line + "  and setting = " + settei + "   and traintype = 'スコアアタック' ORDER BY clear_record DESC limit 1 offset 2 ";

                    DBConnect.ExecuteReader(SQL2);

                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[7].ToString().Substring(15, 1).Equals(":"))
                        {
                            date3.Content = DBConnect.Reader[7].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date3.Content = DBConnect.Reader[7].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line3.Content = DBConnect.Reader[3].ToString() + "回";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date3.Content = "データなし";
                        ///////クリア記録
                        line3.Content = "データなし";

                        DBConnect.Dispose();
                    }

                    break;



                case 2:

                  

                    DBConnect.Connect("kasiihara.db");
                    line = int.Parse(percent.Text);
                    settei = int.Parse(setting.Text);

                    String SQL;

                    ///////////////////////1位タイムアタック表出力
                    SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + " and clear_line = " + line + "  and setting = " + settei + "  and traintype = 'タイムアタック' ORDER BY clear_record ASC limit 1 offset 0 ";
                    DBConnect.ExecuteReader(SQL);
                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[7].ToString().Substring(15, 1).Equals(":"))
                        {
                            date1.Content = DBConnect.Reader[7].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date1.Content = DBConnect.Reader[7].ToString().Substring(0, 16);
                        }

                        ///////クリア記録
                        line1.Content = DBConnect.Reader[3].ToString() + "秒";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date1.Content = "データなし";
                        ///////クリア記録
                        line1.Content = "データなし";

                        DBConnect.Dispose();
                    }




                    ///////////////////////2位タイムアタック表出力
                    DBConnect.Connect("kasiihara.db");
                    SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and clear_line = " + line + "  and setting = " + settei + "   and traintype = 'タイムアタック' ORDER BY clear_record ASC limit 1 offset 1 ";
                    DBConnect.ExecuteReader(SQL);

                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[7].ToString().Substring(15, 1).Equals(":"))
                        {
                            date2.Content = DBConnect.Reader[7].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date2.Content = DBConnect.Reader[7].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line2.Content = DBConnect.Reader[3].ToString() + "秒";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date2.Content = "データなし";
                        ///////クリア記録
                        line2.Content = "データなし";
                        DBConnect.Dispose();
                    }



                    ///////////////////////3位タイムアタック表出力
                    DBConnect.Connect("kasiihara.db");
                    SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + " and clear_line = " + line + "  and setting = " + settei + "  and traintype = 'タイムアタック' ORDER BY clear_record ASC limit 1 offset 2 ";

                    DBConnect.ExecuteReader(SQL);

                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[7].ToString().Substring(15, 1).Equals(":"))
                        {
                           date3.Content = DBConnect.Reader[7].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date3.Content = DBConnect.Reader[7].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line3.Content = DBConnect.Reader[3].ToString() + "秒";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date3.Content = "データなし";
                        ///////クリア記録
                        line3.Content = "データなし";

                        DBConnect.Dispose();
                    }

                    break;

               
                case 3:


                    line = int.Parse(percent.Text);

                    DBConnect.Connect("kasiihara.db");

                    String SQL3;


                    ///////////////////////1位スコアアタック表出力
                    SQL3 = "SELECT * FROM t_targetgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + "  ORDER BY result_time ASC limit 1 offset 0 ";
                    DBConnect.ExecuteReader(SQL3);
                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[5].ToString().Substring(15, 1).Equals(":"))
                        {
                            date1.Content = DBConnect.Reader[5].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date1.Content = DBConnect.Reader[5].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line1.Content = DBConnect.Reader[2].ToString() + "秒";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date1.Content = "データなし";
                        ///////クリア記録
                        line1.Content = "データなし";

                        DBConnect.Dispose();
                    }




                    ///////////////////////2位スコアアタック表出力
                    DBConnect.Connect("kasiihara.db");
                    SQL3 = "SELECT * FROM t_targetgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " ORDER BY result_time ASC limit 1 offset 1 ";
                    DBConnect.ExecuteReader(SQL3);

                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[5].ToString().Substring(15, 1).Equals(":"))
                        {
                            date2.Content = DBConnect.Reader[5].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date2.Content = DBConnect.Reader[5].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line2.Content = DBConnect.Reader[2].ToString() + "秒";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date2.Content = "データなし";
                        ///////クリア記録
                        line2.Content = "データなし";
                        DBConnect.Dispose();
                    }



                    ///////////////////////3位スコアアタック表出力
                    DBConnect.Connect("kasiihara.db");
                    SQL3 = "SELECT * FROM t_targetgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + " ORDER BY result_time ASC limit 1 offset 2 ";

                    DBConnect.ExecuteReader(SQL3);

                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[5].ToString().Substring(15, 1).Equals(":"))
                        {
                            date3.Content = DBConnect.Reader[5].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date3.Content = DBConnect.Reader[5].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line3.Content = DBConnect.Reader[2].ToString() + "秒";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date3.Content = "データなし";
                        ///////クリア記録
                        line3.Content = "データなし";

                        DBConnect.Dispose();
                    }
                    break;


                case 4:


                    fallse = int.Parse(setting_fall.Text);

                    DBConnect.Connect("kasiihara.db");

                    String SQL4;


                    ///////////////////////1位スコアアタック表出力
                    SQL4 = "SELECT * FROM t_fallgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + "  and set_time = " + fallse + "  ORDER BY result_score DESC limit 1 offset 0 ";
                    DBConnect.ExecuteReader(SQL4);
                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[6].ToString().Substring(15, 1).Equals(":"))
                        {
                            date1.Content = DBConnect.Reader[6].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date1.Content = DBConnect.Reader[6].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line1.Content = DBConnect.Reader[3].ToString() + "点";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date1.Content = "データなし";
                        ///////クリア記録
                        line1.Content = "データなし";

                        DBConnect.Dispose();
                    }




                    ///////////////////////2位スコアアタック表出力
                    DBConnect.Connect("kasiihara.db");
                    SQL4 = "SELECT * FROM t_fallgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + "  and set_time = " + fallse + "  ORDER BY result_score DESC limit 1 offset 1 ";
                    DBConnect.ExecuteReader(SQL4);

                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[6].ToString().Substring(15, 1).Equals(":"))
                        {
                            date2.Content = DBConnect.Reader[6].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date2.Content = DBConnect.Reader[6].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line2.Content = DBConnect.Reader[3].ToString() + "点";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date2.Content = "データなし";
                        ///////クリア記録
                        line2.Content = "データなし";
                        DBConnect.Dispose();
                    }



                    ///////////////////////3位スコアアタック表出力
                    DBConnect.Connect("kasiihara.db");
                    SQL4 = "SELECT * FROM t_fallgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id =  " + u_id + "  and set_time = " + fallse + "  ORDER BY result_score DESC limit 1 offset 2 ";

                    DBConnect.ExecuteReader(SQL4);

                    if (DBConnect.Reader.Read())
                    {
                        if (DBConnect.Reader[6].ToString().Substring(15, 1).Equals(":"))
                        {
                            date3.Content = DBConnect.Reader[6].ToString().Substring(0, 15);
                        }
                        else
                        {
                            ////////日付
                            date3.Content = DBConnect.Reader[6].ToString().Substring(0, 16);
                        }
                        ///////クリア記録
                        line3.Content = DBConnect.Reader[3].ToString() + "点";
                        //Console.WriteLine(SQL);
                        DBConnect.Dispose();

                    }
                    else
                    if (DBConnect.Reader.Read() == false)
                    {
                        ////////日付
                        date3.Content = "データなし";
                        ///////クリア記録
                        line3.Content = "データなし";

                        DBConnect.Dispose();
                    }
                    break;

            }


        }

        void combo(int a)
        {
            switch (a)
            {

                case 1:
                    //スコア設定の表示
                    kuriarain.Content = "クリアライン";
                    percent.Visibility = Visibility.Visible;
                    paasento.Content = "%";
                    setting_fall.Visibility = Visibility.Collapsed;
                    setting.Visibility = Visibility.Visible;
                    okcombo.Visibility = Visibility.Visible;
                    byou_kai.Content = "秒数";
                    kazu.Content = "秒";
                    //setteiname.Content = "設定";

                    break;

                case 2:
                    //タイム設定の表示
                    kuriarain.Content = "クリアライン";
                    percent.Visibility = Visibility.Visible;
                    paasento.Content = "%";
                    setting_fall.Visibility = Visibility.Collapsed;
                    setting.Visibility = Visibility.Visible;
                    okcombo.Visibility = Visibility.Visible;
                    byou_kai.Content = "回数";
                    kazu.Content = "回";
                    //setteiname.Content = "設定";


                    break;

                case 3:
                    //ターゲットゲームは設定なし
                    percent.Visibility = Visibility.Collapsed;
                    setting_fall.Visibility = Visibility.Collapsed;
                    kuriarain.Content = "";
                    paasento.Content = "";
                    byou_kai.Content = "";
                    setting.Visibility = Visibility.Collapsed;
                    okcombo.Visibility = Visibility.Collapsed;
                    kazu.Content = "";
                    //setteiname.Content = " ";
                    break;

                case 4:
                    //落下ゲーム設定の表示
                    kuriarain.Content = "";
                    percent.Visibility = Visibility.Collapsed;
                    paasento.Content = "";
                    setting_fall.Visibility = Visibility.Visible;
                    setting.Visibility = Visibility.Collapsed;
                    okcombo.Visibility = Visibility.Visible;
                    byou_kai.Content = "秒数";
                    kazu.Content = "秒";
                    //setteiname.Content = "設定";

                    break;
                
                

            }
        }

        private void taggame_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

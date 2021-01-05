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
    /// k_RecordConfirmation.xaml の相互作用ロジック
    /// </summary>
    public partial class k_RecordConfirmation : Page
    {
        

        int t_id = -1;

        int heightm = 0;
        private Button bac_b = null;

        int u_id;

        int gamejudge;


        int labelNum = 1000;
        int Lnum = 0; //多分、列

        Label[] label;　　　///生成列の配列

        String SQL;

        int sorthi = 0;//何もしない、１、昇順　2:降順　日付
        int sortnum = 0;//何もしない、1:昇順 2:降順　ゲームモード
        public k_RecordConfirmation()
        {
            InitializeComponent();

            

            

            //////////////昇順、降順方法
            try 
            {
                sorthi = int.Parse(Application.Current.Properties["sorthii"].ToString());
            }
            catch
            {
                sorthi = 1;
            }
            try
            {
                sortnum = int.Parse(Application.Current.Properties["sortnumm"].ToString());
            }
            catch
            {
                sortnum = 0;
            }


             //ゲームモードの並び変え
            /*if (!(sortnum == 0))
            {
                gamesan.Background = Brushes.RoyalBlue;
                if (sortnum == 1)
                {
                    gamesan.Content = "▲";
                }
                else if (sortnum == 2)
                {
                    gamesan.Content = "▼";
                }
            }
            else
            {
                gamesan.Background = Brushes.Gainsboro;
                gamesan.Content = "▲";
            }
            */
            

            if (!(sorthi == 0))
            {
                //hidukesa.Background = Brushes.RoyalBlue;
                if (sorthi == 1)
                {
                    hidukesa.Content = "▲";
                }
                else if (sorthi == 2)
                {
                    hidukesa.Content = "▼";
                }
            }
            else
            {
                hidukesa.Background = Brushes.Gainsboro;
                hidukesa.Content = "▲";
            }
           

            if (bac_b != null)
            {
                grid3.Children.Remove(bac_b);
                bac_b = null;
            }

            //Label[] label = new Label[labelNum];　　　///生成列の配列
            WrapPanel backbuttons = new WrapPanel();

            Button top_b = new Button() { Content = "トップ画面", FontSize = 30, Margin = new Thickness(80, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            top_b.Click += bmodo;
            backbuttons.Children.Add(top_b);

            Button logout_b = new Button() { Content = "ログアウト", FontSize = 30, Margin = new Thickness(80, 50, 0, 0), Width = 250, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press };
            logout_b.Click += bout;
            backbuttons.Children.Add(logout_b);

            grid3.Children.Add(backbuttons);

            if (Application.Current.Properties["t_id"] != null)
            {
                t_id = int.Parse(Application.Current.Properties["t_id"].ToString());
            }

            if (t_id != -1)
            {
                bac_b = new Button() { Content = "戻る", FontSize = 50, HorizontalAlignment = HorizontalAlignment.Left, Height = 99.2, Margin = new Thickness(1550, 50, 0, 0), VerticalAlignment = VerticalAlignment.Top, Width = 250 };
                bac_b.Click += bac;

                grid3.Children.Add(bac_b);
            }

            

                u_id = int.Parse(Application.Current.Properties["u_id"].ToString());

                DBConnect.Connect("kasiihara.db");

                DBConnect.ExecuteReader("SELECT COUNT(*) as num FROM t_userrecord WHERE user_id = '" + u_id + "'");    ///ユーザー数カウント
                DBConnect.Reader.Read();
                if (int.Parse(DBConnect.Reader[0].ToString()) > 3)
                {
                    grid3.Height =1080 + (int.Parse(DBConnect.Reader[0].ToString()) - 3) * 124;
                    //                scroll1.Height = grid3.Height;
                }
                DBConnect.Dispose();




            
            SQL = "SELECT * FROM t_user WHERE user_id = " + u_id;
                DBConnect.Connect("kasiihara.db");

                    DBConnect.ExecuteReader(SQL);
                    DBConnect.Reader.Read();
                    name.Content = DBConnect.Reader[1].ToString();


             /*  最終ログイン表示
                    if (t_id == -1)
                    {
                        if (!DBConnect.Reader[5].ToString().Equals(""))
                        {
                            datetimedb.Text = DBConnect.Reader[5].ToString();　　//最終ログイン時間
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

                    */



                SQL = "SELECT * FROM t_maintaintrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id; //バランス訓練データ
                DBConnect.ExecuteReader(SQL);
                if (DBConnect.Reader.Read() == true)
                {
                    kaisu.Content = DBConnect.Reader[2].ToString() + "回";　//回数表示
                }
                else
                {
                    kaisu.Content = "0回";
                }

            DBConnect.Dispose();

            
            
            try
            {
                gamejudge  = int.Parse(Application.Current.Properties["judge"].ToString());
            }
            catch
            {
                gamejudge = 1;

            }

            //gamejudge = 1;

            //hyou(gamejudge);


            if (gamejudge == 1)
            {
                scorebutton.Background = Brushes.Coral;

                label = new Label[labelNum];

                DBConnect.Connect("kasiihara.db");
                //SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'スコアアタック' ";

                if (sorthi == 1)
                {
                    SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'スコアアタック' ORDER BY trainclear_date ASC";
                }
                else if (sorthi == 2)
                {
                    SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'スコアアタック' ORDER BY trainclear_date DESC";
                }

                DBConnect.ExecuteReader(SQL);

                while (DBConnect.Reader.Read())
                //列の生成  gametrain_id,userrecord_id,setting,clear_record,clear_line,user_id,traintype,trainclear_date,user_name,login_id,password,usertype,final_logindata,user_name_kana

                {

                    //////////日付
                    label[Lnum] = new Label() { Content = DBConnect.Reader[7].ToString(), FontSize = 45, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(113.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 427, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////ゲームモード
                    label[Lnum] = new Label() { Content = DBConnect.Reader[6].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(540.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////設定
                    label[Lnum] = new Label() { Content = DBConnect.Reader[2].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(972.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    if (DBConnect.Reader[6].ToString().Equals("スコアアタック"))
                    {
                        label[Lnum].Content = DBConnect.Reader[4]+"%   "+DBConnect.Reader[2].ToString() + "秒";
                    }
                    else
                    {
                        label[Lnum].Content = DBConnect.Reader[4] + "%   " + DBConnect.Reader[2].ToString() + "回";
                    }
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////クリア記録
                    label[Lnum] = new Label() { Content = DBConnect.Reader[3].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(1404.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    if (DBConnect.Reader[6].ToString().Equals("スコアアタック"))
                    {
                        label[Lnum].Content = DBConnect.Reader[3].ToString() + "回";
                    }
                    else
                    {
                        label[Lnum].Content = DBConnect.Reader[3].ToString() + "秒";
                    }
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;

                    heightm += 124;
                }

                DBConnect.Dispose();
            }
            else if (gamejudge == 2)
            {
                

                /* if (sorthi == 0)
                 {
                     if (sortnum == 0)
                     {
                         SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ";
                     }
                     else if (sortnum == 1)
                     {
                         SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + "and traintype = 'タイムアタック' ORDER BY traintype";
                     }
                     else if (sortnum == 2)
                     {
                         SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ORDER BY traintype DESC";
                     }
                 }
                 else if (sorthi == 1)
                 {
                     if (sortnum == 0)
                     {
                         SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック'  ORDER BY trainclear_date";
                     }
                     else if (sortnum == 1)
                     {
                         SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ORDER BY trainclear_date ,traintype";
                     }
                     else if (sortnum == 2)
                     {
                         SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ORDER BY trainclear_date ,traintype DESC";
                     }

                 }
                 else if (sorthi == 2)
                 {
                     if (sortnum == 0)
                     {
                         SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ORDER BY trainclear_date DESC";
                     }
                     else if (sortnum == 1)
                     {
                         SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ORDER BY trainclear_date DESC ,traintype";
                     }
                     else if (sortnum == 2)
                     {
                         SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ORDER BY trainclear_date DESC ,traintype DESC";
                     }

                 }*/


                timebutton.Background = Brushes.Coral;

                label = new Label[labelNum];   ///生成列の配列




                DBConnect.Connect("kasiihara.db");
                //SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ";

               
                if (sorthi == 1)
                {
                    SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ORDER BY trainclear_date ASC";
                }
                else if (sorthi == 2)
                {
                    SQL = "SELECT * FROM t_gametrain NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " and traintype = 'タイムアタック' ORDER BY trainclear_date DESC";
                }

                DBConnect.ExecuteReader(SQL);

                while (DBConnect.Reader.Read())
                //列の生成  gametrain_id,userrecord_id,setting,clear_record,clear_line,user_id,traintype,trainclear_date,user_name,login_id,password,usertype,final_logindata,user_name_kana

                {

                    //////////日付
                    label[Lnum] = new Label() { Content = DBConnect.Reader[7].ToString(), FontSize = 45, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(113.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 427, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////ゲームモード
                    label[Lnum] = new Label() { Content = DBConnect.Reader[6].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(540.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////設定
                    label[Lnum] = new Label() { Content = DBConnect.Reader[2].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(972.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    if (DBConnect.Reader[6].ToString().Equals("スコアアタック"))
                    {
                        label[Lnum].Content = DBConnect.Reader[4] + "%   " + DBConnect.Reader[2].ToString() + "秒";
                    }
                    else
                    {
                        label[Lnum].Content = DBConnect.Reader[4] + "%   " + DBConnect.Reader[2].ToString() + "回";
                    }
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////クリア記録
                    label[Lnum] = new Label() { Content = DBConnect.Reader[3].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(1404.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    if (DBConnect.Reader[6].ToString().Equals("スコアアタック"))
                    {
                        label[Lnum].Content = DBConnect.Reader[3].ToString() + "回";
                    }
                    else
                    {
                        label[Lnum].Content = DBConnect.Reader[3].ToString() + "秒";
                    }
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;

                    heightm += 124;
                }

                DBConnect.Dispose();
            }
            else if (gamejudge == 3)
            {

                targetbutton.Background = Brushes.Coral;

                label = new Label[labelNum];

                DBConnect.Connect("kasiihara.db");
                //SQL = "SELECT * FROM t_targetgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id ;

                if (sorthi == 1)
                {
                    SQL = "SELECT * FROM t_targetgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " ORDER BY trainclear_date ASC";
                }
                else if (sorthi == 2)
                {
                    SQL = "SELECT * FROM t_targetgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " ORDER BY trainclear_date DESC";
                }

                DBConnect.ExecuteReader(SQL);

                while (DBConnect.Reader.Read())
                //列の生成  gametrain_id,userrecord_id,setting,clear_record,clear_line,user_id,traintype,trainclear_date,user_name,login_id,password,usertype,final_logindata,user_name_kana

                {

                    //////////日付
                    label[Lnum] = new Label() { Content = DBConnect.Reader[5].ToString(), FontSize = 45, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(113.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 427, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////ゲームモード
                    label[Lnum] = new Label() { Content = DBConnect.Reader[4].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(540.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////設定
                    label[Lnum] = new Label() { Content = DBConnect.Reader[2].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(972.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    label[Lnum].Content = "なし";
                    
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    
                    //////////クリア記録
                    label[Lnum] = new Label() { Content = DBConnect.Reader[2].ToString() + "秒", FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(1404.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;

                    heightm += 124;
                }

                DBConnect.Dispose();
            }
            else if (gamejudge == 4)
            {
                fallbutton.Background = Brushes.Coral;

                label = new Label[labelNum];

                DBConnect.Connect("kasiihara.db");
                //SQL = "SELECT * FROM t_fallgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id ;

                if (sorthi == 1)
                {
                    SQL = "SELECT * FROM t_fallgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " ORDER BY trainclear_date ASC";
                }
                else if (sorthi == 2)
                {
                    SQL = "SELECT * FROM t_fallgame NATURAL JOIN t_userrecord NATURAL JOIN t_user WHERE user_id = " + u_id + " ORDER BY trainclear_date DESC";
                }

                DBConnect.ExecuteReader(SQL);

                while (DBConnect.Reader.Read())
                //列の生成  gametrain_id,userrecord_id,setting,clear_record,clear_line,user_id,traintype,trainclear_date,user_name,login_id,password,usertype,final_logindata,user_name_kana

                {

                    //////////日付
                    label[Lnum] = new Label() { Content = DBConnect.Reader[6].ToString(), FontSize = 45, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(113.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 427, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////ゲームモード
                    label[Lnum] = new Label() { Content = DBConnect.Reader[5].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(540.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////設定
                    label[Lnum] = new Label() { Content = DBConnect.Reader[2].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(972.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                   
                    label[Lnum].Content = DBConnect.Reader[2].ToString() + "秒";
                    
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;
                    //////////クリア記録
                    label[Lnum] = new Label() { Content = DBConnect.Reader[3].ToString(), FontSize = 60, HorizontalAlignment = HorizontalAlignment.Left, HorizontalContentAlignment = HorizontalAlignment.Center, Height = 124, Margin = new Thickness(1404.4, 611.053 + heightm, 0, 0), VerticalAlignment = VerticalAlignment.Top, VerticalContentAlignment = VerticalAlignment.Center, Width = 432, BorderBrush = Brushes.Black, BorderThickness = new Thickness(3) };
                   
                    label[Lnum].Content = DBConnect.Reader[3].ToString() + "点";
                    
                    grid3.Children.Add(label[Lnum]);
                    Lnum++;

                    heightm += 124;
                }

                DBConnect.Dispose();
            }
            else if(gamejudge == 5)
            {

            }
        }


        private void scorebutton_Click(object sender, RoutedEventArgs e)
        {
            //gamejudge = 1;

            //hyou(gamejudge);

            Application.Current.Properties["judge"] = 1;
            Console.WriteLine(Application.Current.Properties["judge"]);

            var nextPage = new k_RecordConfirmation();
            NavigationService.Navigate(nextPage);
        }

        private void timebutton_Click(object sender, RoutedEventArgs e)
        {
            

            //gamejudge = 2;

            //hyou(gamejudge);

            Application.Current.Properties["judge"] = 2; 

            //System.Windows.Controls.Label[]　
            Console.WriteLine(label);
            Console.WriteLine(Application.Current.Properties["judge"]);

            var nextPage = new k_RecordConfirmation();
            NavigationService.Navigate(nextPage);

        }

        private void targetbutton_Click(object sender, RoutedEventArgs e)
        {

            //gamejudge = 3;

            //hyou(gamejudge);

            Application.Current.Properties["judge"] = 3;

            var nextPage = new k_RecordConfirmation();
            NavigationService.Navigate(nextPage);
        }

        private void fallbutton_Click(object sender, RoutedEventArgs e)
        {
            //gamejudge = 4;

            //hyou(gamejudge);

            Application.Current.Properties["judge"] = 4;

            var nextPage = new k_RecordConfirmation();
            NavigationService.Navigate(nextPage);
        }


        void bout(object sender, EventArgs e)　/////ログアウト
        {
            t_id = -1;
            Application.Current.Properties["t_id"] = null;
            if (bac_b != null)
            {
                grid3.Children.Remove(bac_b);
            }
            var nextPage = new MainTitle();
            NavigationService.Navigate(nextPage);

            Application.Current.Properties["judge"] = null;

        }

        void bmodo(object sender, EventArgs e)　//////トップ画面
        {
            if (t_id == -1)
            {
                t_id = -1;
                Application.Current.Properties["t_id"] = null;
                if (bac_b != null)
                {
                    grid3.Children.Remove(bac_b);
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
                    grid3.Children.Remove(bac_b);
                }
                var nextPage = new t_top();
                NavigationService.Navigate(nextPage);
            }
        }

        void bac(object sender, EventArgs e)  /////戻る？
        {
            t_id = -1;
            Application.Current.Properties["u_id"] = t_id;
            Application.Current.Properties["t_id"] = null;
            if (bac_b != null)
            {
                grid3.Children.Remove(bac_b);
            }
            var nextPage = new UserRecordCheck();
            NavigationService.Navigate(nextPage);
        }

        void sort(object sender, EventArgs e)　////ソート
        {
            if ((sender as Button).Name.Equals("hidukesa"))
            {
                if(sorthi == 1)
                {
                    Application.Current.Properties["sorthii"] = 2;
                }
                else
                {
                    Application.Current.Properties["sorthii"] = 1;
                }
            }
            else
            {
                if (sortnum == 0)
                {
                    Application.Current.Properties["sortnumm"] = 1;
                }
                else if (sortnum == 1)
                {
                    Application.Current.Properties["sortnumm"] = 2;
                }
                else
                {
                    Application.Current.Properties["sortnumm"] = 0;
                }
            }

            var nextPage = new k_RecordConfirmation();
            NavigationService.Navigate(nextPage);
        }

        private void Tagbutton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["judge"] = 5;

            var nextPage = new k_RecordConfirmation();
            NavigationService.Navigate(nextPage);
        }
    }
}

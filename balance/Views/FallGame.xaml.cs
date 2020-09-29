using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using WiimoteLib;
using balance.DataBase;


namespace balance.Views
{
    /// <summary>
    /// FallGame.xaml の相互作用ロジック
    /// </summary>
    public partial class FallGame : Page
    {

        public delegate void Refresh_fa(object sender, RoutedEventArgs e);

        public delegate void Refresh_fb(object sender, RoutedEventArgs e);

        Wiimote wiimote = new Wiimote();

        double xza = 0; //重心のX座標
        double yza = 0; //重心のY座標

        private Rectangle drawingBalance = null;  //重心のマーク

        private Ellipse drawingEllipse = null;

        private Ellipse drawingEllipse1 = null;

        private Label drawingLabel = null;

        private Label drawingLabel1 = null;

        double leftsize = 0;  //左足の加重量
        double rightsize = 0; //右足の加重量

        delegate void UpdateWiimoteStateDelegate(object sender, WiimoteChangedEventArgs args);

        //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); //ストップウォッチsw生成
        DispatcherTimer dispatcharTimer; //ゲームの秒数を保持する

        int time_t; //計測タイム

        String SQL = "";　//SQL文
        int user_id = -1; //ログインのu_id

        Boolean game = false;
        int timese = 30;//選択タイム保持

        Random cRandom = new System.Random(); // 玉ランダム
        Random cRandom1 = new System.Random(); // 玉1ランダム
        int x, y, x1, y1;

        int target = 0; //得点

        DispatcherTimer dispatcharTimer11; //カウントダウンの秒数を保持する
        int cdtime;

        public FallGame()
        {
            InitializeComponent();
            if (!Application.Current.Properties["u_id"].ToString().Equals("guest")) //user,guest確認
            {
                user_id = int.Parse(Application.Current.Properties["u_id"].ToString());
            }

            wiimote.WiimoteChanged += OnWiimoteChanged;

            dispatcharTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcharTimer.Interval = new TimeSpan(0, 0, 0, 1, 1);
            dispatcharTimer.Tick += new EventHandler(dispatcharTimer_Tick);

            x = 300;
            y = 380;
            x1 = 1000;
            y1 = 10;

            dispatcharTimer11 = new DispatcherTimer();
            dispatcharTimer11.Interval = new TimeSpan(0, 0, 1);
            dispatcharTimer11.Tick += dispatcharTimer11_Tick;

            for (int i = 30; i <= 90; i += 30)
            {
                if (i == timese)
                {
                    timepercent.Items.Add(new ComboBoxItem() { IsSelected = true, Content = i });
                }
                else
                {
                    timepercent.Items.Add(i);
                }
            }



        }



        void OnWiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            if (game == true)
            {

                BalanceBoardState bbs = e.WiimoteState.BalanceBoardState;
                if (bbs.WeightKg < 5)
                {
                    xza = 600 - 35;  //(Canvas.Width/2) - (ballWidth /2)
                    yza = 337 - 35;  //(Canvas.Height/2) - (ballHeight /2)
                    leftsize = 0;
                }
                else
                {
                    xza = bbs.CenterOfGravity.X * 17 * 2 + 600 - 35;//*(Canvas.Width/ballWidth)*倍率 + (Canvas.Width/2) - (ballWidth /2)
                    yza = bbs.CenterOfGravity.Y * 9.6 * 3 + 337 - 35;//*(Canvas.Height/ballHeight)*倍率 + (Canvas.Height/2) - (ballHeight /2)
                    //                  leftsize = 700 - (350 + 14 * (bbs.CenterOfGravity.X));
                    leftsize = (1 - (xza / 790)) * 700;
                    if (xza > 978) //枠内に収まるように
                    {
                        xza = 978;// 1200 - 222 bdraw.Width - ballSize
                    }
                    else if (xza < 0)
                    {
                        xza = 0;
                    }

                    if (yza > 605)
                    {
                        yza = 605;// 675 - 70 bdraw.Height - ballSize
                    }
                    else if (yza < 0)
                    {
                        yza = 0;
                    }
                }
                //                * (pictureBox1.Width / ballWidth)) + (pictureBox1.Width / 2) - (ballWidth / 2)




                Dispatcher.Invoke(new Action(() =>
                {
                    //////////////重心の表示
                    if (this.drawingBalance != null)
                    {
                        this.beback.Children.Remove(this.drawingBalance);
                    }
                    this.drawingBalance = new Rectangle() { Fill = System.Windows.Media.Brushes.LimeGreen, Width = 222, Height = 45, Margin = new Thickness(xza, 630, 0, 0), };
                    this.beback.Children.Add(this.drawingBalance);

                    /////////////玉
                    int randtama = cRandom.Next(12);
                    //int randtama1 = cRandom1.Next(10);
                    y += 10;        //落下する玉の速さ
                    if (this.drawingEllipse != null)
                    {
                        this.beback.Children.Remove(this.drawingEllipse);
                    }

                    if (y > 675)
                    {
                        x = randtama * 100;
                        y = 0;
                    }

                    this.drawingEllipse = new Ellipse() { Fill = System.Windows.Media.Brushes.Red, Width = 70, Height = 70, Margin = new Thickness(x, y, 0, 0) };
                    this.beback.Children.Add(this.drawingEllipse);

                    if (this.drawingLabel != null)
                    {
                        this.beback.Children.Remove(this.drawingLabel);
                    }

                    this.drawingLabel = new Label() { Content = "十", Width = 70, Height = 75, Margin = new Thickness(x, y - 10, 0, 0), FontSize = 60, Foreground = System.Windows.Media.Brushes.White, FontWeight = FontWeights.Bold, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                    this.beback.Children.Add(this.drawingLabel);


                    /////////////玉2
                    y1 += 2;
                    if (this.drawingEllipse1 != null)
                    {
                        this.beback.Children.Remove(this.drawingEllipse1);
                    }

                    if (y1 > 675)
                    {
                        x1 = randtama * 100;
                        y1 = 0;
                    }

                    this.drawingEllipse1 = new Ellipse() { Fill = System.Windows.Media.Brushes.Blue, Width = 70, Height = 70, Margin = new Thickness(x1, y1, 0, 0) };
                    this.beback.Children.Add(this.drawingEllipse1);

                    if (this.drawingLabel1 != null)
                    {
                        this.beback.Children.Remove(this.drawingLabel1);
                    }

                    this.drawingLabel1 = new Label() { Content = "一", Width = 70, Height = 70, Margin = new Thickness(x1, y1 - 10, 0, 0), FontSize = 60, Foreground = System.Windows.Media.Brushes.White, FontWeight = FontWeights.Bold, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                    this.beback.Children.Add(this.drawingLabel1);


                    ///////////////Ellipse,Ellipse1当たり判定 y1は7の倍数の速さで動いているから585の数値にならないy1=588になる
                    ///585%y=0ならよい　585%y1=1なら-1する
                    if (xza < x + 35 && xza + 222 > x + 35 && 585 == y - 1)
                    {
                        target++;
                        PlaySound("fall_up.wav");
                    }
                    else if (xza < x1 + 35 && xza + 222 > x1 + 35 && 585 == y1 - 1)
                    {
                        target--;
                        PlaySound("fall_down.wav");
                    }
                    else if (xza < x + 35 && xza + 222 > x + 35 && 585 <= y)
                    {


                        drawingEllipse.Fill = System.Windows.Media.Brushes.LimeGreen;
                        get.Foreground = System.Windows.Media.Brushes.Red;
                        get.Content = "+1";
                    }
                    else if (xza < x1 + 35 && xza + 222 > x1 + 35 && 585 <= y1)
                    {



                        drawingEllipse1.Fill = System.Windows.Media.Brushes.LimeGreen;
                        get.Foreground = System.Windows.Media.Brushes.Blue;
                        get.Content = "-1";
                    }
                    else
                    {
                        get.Content = "";
                    }


                    count.Content = "獲得点数    " + target + "点";
                    //con.Content = x1 +"\n"+ y1;
                    //count.Content = "獲得個数　7個";

                    /*
                    A の左端 X 座標<B の右端 X 座標
                    A の右端 X 座標> B の左端 X 座標
                    A の上端 Y 座標 < B の下端 Y 座標
                    A の下端 Y 座標 > B の上端 Y 座標

                    if (xza<x+35 && xza+222>x+35 && 634 < y + 35 && 679 > y + 35)
                    {
                        drawingEllipse.Fill = System.Windows.Media.Brushes.Black;

                        target++;
                    }

                    xza          < x+35
                    xza+222      > x+35
                    yza          < y+35
                    yza+45       > y+35
                    */

                    if (time_t == 0)
                    {
                        game = false;
                        PlaySound("clear.wav");
                        //countdown.Content = "クリア";
                        //countdown.Foreground = System.Windows.Media.Brushes.Black;
                        dispatcharTimer.Stop();


                        drawingLabel.Content = "";
                        drawingLabel1.Content = "";







                        if (!Application.Current.Properties["u_id"].ToString().Equals("guest"))
                        {
                            DBConnect.Connect("kasiihara.db");
                            SQL = "INSERT INTO t_userrecord (user_id,traintype,trainclear_date)VALUES('" + user_id + "','落下ゲーム','" + DateTime.Now.ToString() + "')";
                            DBConnect.ExecuteReader(SQL);
                            SQL = "SELECT * FROM t_userrecord ORDER BY userrecord_id DESC";
                            DBConnect.ExecuteReader(SQL);
                            DBConnect.Reader.Read();
                            SQL = "INSERT INTO t_fallgame (userrecord_id,set_time,result_score)VALUES('" + DBConnect.Reader[0] + "', '" + timese + "','" + target + "')";
                            DBConnect.ExecuteReader(SQL);
                            DBConnect.Dispose();




                        }

                        dispatcharTimer = new DispatcherTimer(DispatcherPriority.Normal);
                        dispatcharTimer.Interval = new TimeSpan(0, 0, 1);
                        dispatcharTimer.Tick += new EventHandler(dispatcharTimer_Tick);


                        Application.Current.Properties["ftgamemodename"] = "落下ゲーム(簡単)";

                        Application.Current.Properties["ftresult"] = target;

                        Application.Current.Properties["ftresulttime"] = timese;

                        startbutton.Content = "スタート";

                        FTGameResult s = new FTGameResult(this.start, this.back, 1);
                        s.Title = "FTGameResult";
                        s.ShowDialog();

                    }

                }));



            }

        }




        void dispatcharTimer_Tick(object sender, EventArgs e)
        {

            time_t--;

            time.Content = "残り時間     " + time_t + "秒";
        }

        private void timeok_Click(object sender, RoutedEventArgs e)
        {
            timese = int.Parse(timepercent.Text);
            time.Content = "残り時間     " + timese + "秒";
        }

        void dispatcharTimer11_Tick(object sender, EventArgs e)
        {

            cdtime++;
            countdown.Content = (3 - cdtime).ToString();

            if (cdtime == 3) //スタート押されてると
            {

                countdown.Content = "";
                dispatcharTimer11.Stop();

                wiimote.Connect();
                startbutton.Content = "ストップ";
                game = true;
                dispatcharTimer.Start();



            }

        }

        private void start(object sender, RoutedEventArgs e)
        {
            if (startbutton.Content.Equals("スタート"))
            {
                time_t = timese;
                target = 0;
                countdown.Foreground = System.Windows.Media.Brushes.Red;
                countdown.Content = "3";
                cdtime = 0;

                count.Content = "獲得点数    0点";
                wiimote.Connect();
                dispatcharTimer11.Start();
            }
            else if (startbutton.Content.Equals("リスタート"))
            {
                countdown.Foreground = System.Windows.Media.Brushes.Red;
                countdown.Content = "3";
                cdtime = 0;
                wiimote.Connect();
                dispatcharTimer11.Start();
            }
            else
            {
                //ストップの状態だと
                wiimote.Disconnect();
                startbutton.Content = "リスタート";
                game = false;
                dispatcharTimer.Stop();
            }


        }

        private void back(object sender, RoutedEventArgs e)
        {

            wiimote.Disconnect();

            var nextPage = new GameSelect();
            NavigationService.Navigate(nextPage);

        }

        /// 音を出すプログラム
        private System.Media.SoundPlayer player = null;

        private void PlaySound(String waveFile)
        {
            if (player != null)
            {
                StopSound();
            }
            player = new System.Media.SoundPlayer(waveFile);
            player.Play();
        }

        private void StopSound()
        {
            if (player != null)
            {
                player.Stop();
                player = null;
            }
        }


    }
}

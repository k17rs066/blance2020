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
using System.Windows.Threading;
using System.IO;
using WiimoteLib;
using balance.DataBase;

namespace balance.Views
{
    /// <summary>
    /// Page1.xaml の相互作用ロジック
    /// </summary>
    public partial class GameOfTag : Page
    {
        Wiimote wiimote = new Wiimote();

        delegate void UpdateWiimoteStateDelegate(object sender, WiimoteChangedEventArgs args);

        DispatcherTimer dispatcharTimer; //ゲームの秒数を保持する

        double leftsize = 0;  //左足の加重量
        double rightsize = 0; //右足の加重量


        double xza = 0; //重心のX座標
        double yza = 0; //重心のY座標        int time;

        private Ellipse drawingBalance = null;  //重心のマーク
        private Label drawingLabel = null; //カウントダウン表示
        private Label drawingLabel1 = null; //END表示

        private Ellipse Enemy = null;
        private Label EnemyLabel = null;



        double xe, ye;
        double spe;
        

        String SQL = "";
        int user_id = -1;

        bool GameOver_flg = false;  //ゲームオーバーフラグ

        Boolean game = false;
        int timekeeper = 30;    //選択タイムの保持
        int time_t; //計測タイム
        DispatcherTimer dispatcharTimer11; //カウントダウンの秒数を保持する
        int cdtime;
 

        public GameOfTag()
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


            dispatcharTimer11 = new DispatcherTimer();
            dispatcharTimer11.Interval = new TimeSpan(0, 0, 1);
            dispatcharTimer11.Tick += dispatcharTimer11_Tick;


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
                    if (xza > 1173) //枠内に収まるように
                    {
                        xza = 1173;// 1200 - 222 bdraw.Width - ballSize
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

                Dispatcher.Invoke(new Action(() =>
                {
                    //////////////重心の表示
                    if (this.drawingBalance != null)
                    {
                        this.field.Children.Remove(this.drawingBalance);
                    }
                    this.drawingBalance = new Ellipse() { Fill = System.Windows.Media.Brushes.LimeGreen, Width = 70, Height = 70, Margin = new Thickness(xza, yza, 0, 0) }; //重心のマーク
                    this.field.Children.Add(this.drawingBalance);

                    //                * (pictureBox1.Width / ballWidth)) + (pictureBox1.Width / 2) - (ballWidth / 2)

                    //敵
                    //分岐：敵が自分を追尾
                    if (xza > xe)
                    {
                        xe += 0.5;

                    }

                    if(xza<xe)
                    {
                        xe -= 0.5;
                    }
                    if (yza > ye)
                    {
                        ye+=0.5;
                    }
                    if (yza < ye)
                    {
                        ye-=0.5;
                    }

                    if(this.Enemy!=null)
                    {
                        this.field.Children.Remove(this.Enemy);
                    }

                    ImageBrush enemy = new ImageBrush();
                    string abspath = System.IO.Path.GetFullPath("Image/akaoni.png");    //絶対パスを取得
                    enemy.ImageSource = new BitmapImage(new Uri(abspath));  //
                    this.Enemy = new Ellipse() { Fill = enemy, Width =250, Height=250 , Margin = new Thickness(xe,ye,0,0)};
                    this.field.Children.Add(this.Enemy);

                    double r = 35 + 125; //半径の和
                    double x = xza - xe;    //2つの円の中心のx座標の距離
                    double y = yza - ye;    //2つの円の中心のy座標の距離

                    if (r * r>= x*x + y*y) {//当たり判定、　 ---三平方の定理を利用---
                        GameOver_flg = true;

                    }

                    //枠内に収まるようにする 
                    if (xe > 945)   //1200-255  (枠からはみ出さないように5px余分にとる)
                    {
                        xe = 945;

                    }else if(xe < 5){

                        xe = 5;
                    }else if(ye > 420) //675-255
                    {
                        ye = 420;

                    }else if(ye < 5)
                    {
                        ye = 5;
                    }

                    dispatcharTimer = new DispatcherTimer(DispatcherPriority.Normal);
                    dispatcharTimer.Interval = new TimeSpan(0, 0, 1);
                    dispatcharTimer.Tick += new EventHandler(dispatcharTimer_Tick);

                    if (time_t == 0)    //制限時間まで逃げきれたら
                    {
                        game = false;
                        PlaySound("clear.wav");
                        dispatcharTimer.Stop();
                        Application.Current.Properties["GameResult"] = "クリア！";
                        Application.Current.Properties["GameResultTime"]=""

                    }



                    Application.Current.Properties["GameTitle"] = "鬼ごっこ";


                    startbutton.Content = "スタート";

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
            timekeeper = int.Parse(timepercent.Text);
            time.Content = "残り時間     " + timekeeper + "秒";
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
                time_t = timekeeper;
//                 target = 0;
                countdown.Foreground = System.Windows.Media.Brushes.Red;
                countdown.Content = "3";
                cdtime = 0;

//                 count.Content = "獲得点数    0点";
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

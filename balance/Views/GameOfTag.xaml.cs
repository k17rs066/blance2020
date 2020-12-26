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
using System.Threading;
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


        //public delegate void Refresh();




        TagSetting set; //設定画面のダイアログをセット

        TagResult result;   //結果画面のダイアログをセット

        public delegate void TagRestart(object sender, RoutedEventArgs e);  //デリゲート型の宣言
        public delegate void TagEnd(object sender, RoutedEventArgs e);  

        DispatcherTimer dispatcharTimer; //ゲームの秒数を保持する




        double xza = 0; //重心のX座標
        double yza = 0; //重心のY座標        int time;

        private Ellipse drawingBalance = null;  //重心のマーク
        private Label drawingLabel = null; //カウントダウン表示
        private Label drawingLabel1 = null; //END表示

        private Ellipse Enemy = null;
        private Label EnemyLabel = null;


        double xe, ye;
        public double spe=0.75;


        String SQL = "";
        int user_id = -1;

        bool GameOver_flg = false;  //ゲームオーバーフラグ

        Boolean game = false;
        public int timekeeper = 30;    //選択タイムの保持
        int time_t; //計測タイム ()
        DispatcherTimer dispatcharTimer11; //カウントダウンの秒数を保持する

        int cdtime;
        int cnttime=0;
        String speed_state = "";
        String size_state = "";

        int size = 200;
        Random random = new Random();

        public GameOfTag()
        {
            
            InitializeComponent();
            if (!Application.Current.Properties["u_id"].ToString().Equals("guest")) //user,guest確認
            {
                user_id = int.Parse(Application.Current.Properties["u_id"].ToString());
            }

            Application.Current.Properties["TagSpeed"] = "普通";
            Application.Current.Properties["TagSize"] = "普通";
            Application.Current.Properties["GameResultTime"] = timekeeper;
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
                speed_state = Application.Current.Properties["TagSpeed"].ToString();
                size_state = Application.Current.Properties["TagSize"].ToString();

                BalanceBoardState bbs = e.WiimoteState.BalanceBoardState;

                if (bbs.WeightKg < 5)
                {
                    xza = 1654/2 - 35;  //(Canvas.Width/2) - (ballWidth /2)
                    yza = 700/2 - 35;  //(Canvas.Height/2) - (ballHeight /2)

                }
                else
                {
                    xza = bbs.CenterOfGravity.X * 1654/70*2.2 + (1654 / 2) - 35;//*(Canvas.Width/ballWidth)*倍率 + (Canvas.Width/2) - (ballWidth /2)
                    yza = bbs.CenterOfGravity.Y * 700/70*2.5 + 350 - 35;//*(Canvas.Height/ballHeight)*倍率 + (Canvas.Height/2) - (ballHeight /2)


                    if (xza > 1644) //枠内に収まるように
                    {
                        xza = 1644;//  bdraw.Width - ballSize
                    }
                    else if (xza < 10)
                    {
                        xza = 10;
                    }

                    if (yza > 690)
                    {
                        yza = 690;//bdraw.Height - ballSize
                    }
                    else if (yza < 10)
                    {
                        yza = 10;
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



                    if(this.Enemy!=null)
                    {
                        this.field.Children.Remove(this.Enemy);
                    }

                    //敵（鬼）を表示
                    ImageBrush enemy = new ImageBrush();
                    string abspath = System.IO.Path.GetFullPath("Image/akaoni.png");    //絶対パスを取得
                    enemy.ImageSource = new BitmapImage(new Uri(abspath));  //イメージソースに代入

                    this.Enemy = new Ellipse() { Fill = enemy, Width =size, Height=size , Margin = new Thickness(xe,ye,0,0)};
                    this.field.Children.Add(this.Enemy);

                    //敵
                    //分岐：敵が自分を追尾
                    if (xza > xe)
                    {

                        xe += spe;

                    }

                    if (xza <= xe)
                    {
                        xe -= spe;

                    }
                    if (yza > ye)
                    {
                        ye += spe;
                    }
                    if (yza <= ye)
                    {
                        ye -= spe;
                    }

                    double r = 35 + size/2 -30; //半径の和
                    double x = xza - (xe);    //2つの円の中心のx座標の差
                    double y = yza - (ye);    //2つの円の中心のy座標の差

                    if ((r * r)> ((x*x) + (y*y))){//当たり判定、　 ---三平方の定理を利用---
                        Task.Run(async () =>
                        {
                            await Task.Delay(500);
                            if ((r * r) > ((x * x) + (y * y)))
                            {
                                GameOver_flg = true;

                            }
                            else
                            {
                                GameOver_flg = false;
                            }

                        });
                    }

                    //枠内に収まるようにする 
                    if (xe > 1349)   
                    {
                        xe = 1349;

                    }else if(xe < 5){

                        xe = 5;
                    }else if(ye > 695) 
                    {
                        ye = 695;

                    }else if(ye < 5)
                    {
                        ye = 5;
                    }

                    if (GameOver_flg == true )
                    {
                        game = false;
                        dispatcharTimer.Stop();

                        if (!Application.Current.Properties["u_id"].ToString().Equals("guest"))
                        {
                            DBConnect.Connect("kasiihara.db");
                            SQL = "INSERT INTO t_userrecord (user_id,traintype,trainclear_date)VALUES('" + user_id + "','鬼ごっこゲーム','" + DateTime.Now.ToString() + "')";
                            DBConnect.ExecuteReader(SQL);
                            SQL = "SELECT * FROM t_userrecord ORDER BY userrecord_id DESC";
                            DBConnect.ExecuteReader(SQL);
                            DBConnect.Reader.Read();
                            SQL = "INSERT INTO t_taggame (user_record_id,time,tag_speed,judge,tag_size)VALUES('" + DBConnect.Reader[0] + "', '" + cnttime + "','" + speed_state + "','ゲームオーバー','" + size_state+"')";
                            DBConnect.ExecuteReader(SQL);
                            DBConnect.Dispose();


                        }

                        Application.Current.Properties["GameResult"] = "ゲームオーバー!";
                        Application.Current.Properties["GameResultTime"] = cnttime;

                        result = new TagResult(this.start,this.back);
                        result.ShowDialog();

                        startbutton.Content = "スタート";
                    }

                    if (time_t == 0)    //制限時間まで逃げきれたら
                    {
                        game = false;
                        PlaySound("clear.wav");
                        dispatcharTimer.Stop();

                        if (!Application.Current.Properties["u_id"].ToString().Equals("guest"))
                        {
                            DBConnect.Connect("kasiihara.db");
                            SQL = "INSERT INTO t_userrecord (user_id,traintype,trainclear_date)VALUES('" + user_id + "','鬼ごっこゲーム','" + DateTime.Now.ToString() + "')";
                            DBConnect.ExecuteReader(SQL);
                            SQL = "SELECT * FROM t_userrecord ORDER BY userrecord_id DESC";
                            DBConnect.ExecuteReader(SQL);
                            DBConnect.Reader.Read();
                            SQL = "INSERT INTO t_taggame (user_record_id,time,tag_speed,judge,tag_size)VALUES('" + DBConnect.Reader[0] + "', '" + cnttime + "','" + speed_state + "','クリア','" + size_state +"')";
                            DBConnect.ExecuteReader(SQL);
                            DBConnect.Dispose();


                        }

                        dispatcharTimer = new DispatcherTimer(DispatcherPriority.Normal);
                        dispatcharTimer.Interval = new TimeSpan(0, 0, 1);
                        dispatcharTimer.Tick += new EventHandler(dispatcharTimer_Tick);

                        Application.Current.Properties["GameResult"] = "クリア！";
                        Application.Current.Properties["GameResultTime"] = cnttime;

                        result = new TagResult(this.start,this.back);
                        result.ShowDialog();

                        startbutton.Content = "スタート";
                    }



                }));
            }
                
        }



        void dispatcharTimer_Tick(object sender, EventArgs e)
        {

            time_t--;
            cnttime++;
            if (time_t > 60)
            {
                time.Content = "残り時間：     " + (time_t / 60) + "分" + (time_t % 60) + "秒";
            }
            else
            {
                time.Content = "残り時間：     " + (time_t) + "秒";
            }
        }

        void dispatcharTimer11_Tick(object sender, EventArgs e)
        {

            cdtime++;
            countdown.Content = (3 - cdtime).ToString();

            if (cdtime == 3) //スタートが押されてると
            {
                cnttime = 0;
                time_t = timekeeper;
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
                GameOver_flg = false;
                cdtime = 0;

                //鬼のオブジェクト・座標を初期化
                ye = 0; xe = 0;
                this.field.Children.Remove(this.Enemy);

                //重心のオブジェクト・座標を初期化
                xza = 0;yza = 0;
                this.field.Children.Remove(this.drawingBalance);

                cnttime = 0;
                wiimote.Connect();
                dispatcharTimer11.Start();
            }
            else if (startbutton.Content.Equals("リスタート")) //リスタートの状態だと
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

        private void TagSetting_Click(object sender,RoutedEventArgs e)  //設定ボタンを押す
        {
            startbutton.Content = "ストップ";
            set = new TagSetting();
            game = false; dispatcharTimer.Stop();
            set.ShowDialog();


            spe = set.ReciveSpeed;
            size = set.ReciveSize;
            timekeeper = set.ReciveTime;
            time_t = timekeeper;
            speed_state = Application.Current.Properties["TagSpeed"].ToString();
            size_state = Application.Current.Properties["TagSize"].ToString();
            tag_speed.Content = "鬼の速さ：    "+ speed_state ;
            tag_size.Content = "鬼の大きさ：    " + size_state;

            if (time_t > 60)
            {
                time.Content = "残り時間：     " + (time_t / 60) + "分" + time_t % 60 + "秒";
            }
            else
            {
                time.Content = "残り時間：     " + (time_t) + "秒";
            }
            startbutton.Content = "スタート";
        }

        private void rule_Click(object sender, RoutedEventArgs e)
        {

            TagGameRule s = new TagGameRule();
            s.ShowDialog();


        }

        private void PracMode(object sender,RoutedEventArgs e)
        {
            wiimote.Disconnect();
            var nextPage = new GameOfTag_Prac();
            NavigationService.Navigate(nextPage);
        }
    }
}

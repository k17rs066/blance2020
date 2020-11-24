using balance.DataBase;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WiimoteLib;



namespace balance.Views
{
    /// <summary>
    /// GameMode.xaml の相互作用ロジック  314行ダイアログ解除
    /// </summary>
    public partial class GameMode : Page
    {
        public delegate void Refresh(int a);

        public delegate void Refresh1(object sender, EventArgs e);

        public delegate void Refresh2(object sender, EventArgs e);

        Wiimote wiimote = new Wiimote();
        String SoundFile = "count.wav";　//音のファイル
        Boolean game; //game true起動、false終了の合図

        double xza = 0; //重心のX座標
        double yza = 0; //重心のY座標

        private Ellipse DrawingBalance = null;    //重心のマーク
        private Rectangle DrawingLeftLeg = null;　//左の％
        private Rectangle DrawingRightLeg = null;　//右の％

        private Rectangle DrawingLeftLine = null;  //左の設定％ライン
        private Rectangle DrawingRightLine = null;　//右の設定％ライン

        private Rectangle DrawingNextLeft = null; //％の青の■マーク

        Boolean DrawingNextRight = false;//drawingNextの右のマーク
        Boolean GameState = false;//現在gameをしているか
        Boolean GameType = false;//ゲーム種類　scoreatack、trueでtimatack

        double LeftSize = 0;  //左足の加重量
        double RightSize = 0; //右足の加重量

        String st = ""; //settin画面文字表示
        String lin = ""; //クリア設定％
        String set = "";　
        String se = "";

        Boolean ScoreFlag = false; //回数の設定されてるか
        Boolean TimeFlag = false; //秒数の設定されてるか


        int count = 0;//重心移動成功時にカウントする、インクリメント



        int nextx = 0;//次に重心移動を行う青い■のｘ座標

        int time_t = 0;//時間を表示する際に扱う変数、インクリメント
        int line = 0;//クリアラインの設定を引き継ぐ0～100のいずれかの整数をもつ

        int ClearCount = -1;//クリア回数が代入されている　変数は変わらない
        int tCount = 0;//テキストカウント（テキスト表示用のカウント変数）デクリメント
        int tCounth = 0;//テキストカウント保持（テキスト表示用のカウント変数に入る値を保持する）;

        /// <summary>
        /// scoreattack用の変数
        /// </summary>

        int time_s = -1;//GameSttingから設定秒数を引き継ぎ
        int htimes_s = 0; //


        System.Diagnostics.Stopwatch StopWatch = new System.Diagnostics.Stopwatch(); //ストップウォッチsw生成
        DispatcherTimer dispatcharTimer; //ゲームの秒数を保持する

        delegate void UpdateWiimoteStateDelegate(object sender, WiimoteChangedEventArgs args);

        String SQL = "";　//SQL文
        int user_id = -1; //ログインのu_id



        DispatcherTimer dispatcharTimer11; //カウントダウンの秒数を保持する
        int cdtime;



        public GameMode()
        {
            InitializeComponent();
            if (!Application.Current.Properties["u_id"].ToString().Equals("guest")) //user,guest確認
            {
                user_id = int.Parse(Application.Current.Properties["u_id"].ToString());
            }

            wiimote.WiimoteChanged += OnWiimoteChanged;

            dispatcharTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcharTimer.Interval = new TimeSpan(0, 0, 1);
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
                    xza = 430 - 35;
                    yza = 230 - 35;
                    LeftSize = 0;
                }
                else
                {
                    xza = bbs.CenterOfGravity.X * 12 * 2 + 430 - 35;//*(Canvas.Width/ballWidth)*倍率 + (Canvas.Width/2) - (ballWidth /2)
                    yza = bbs.CenterOfGravity.Y * 6.6 * 3 + 230 - 35;
                    //                    leftsize = 700 - (350 + 14 * (bbs.CenterOfGravity.X));
                    LeftSize = (1 - (xza / 790)) * 700;
                    if (xza > 790)　//枠内に収まるように
                    {
                        xza = 790;// 860 - 70 bdraw.Width - ballSize
                    }
                    else if (xza < 0)
                    {
                        xza = 0;
                    }

                    if (yza > 395)
                    {
                        yza = 395;// 465 - 70 bdraw.Height - ballSize
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
                    if (this.DrawingBalance != null)
                    {
                        this.balancedraw.Children.Remove(this.DrawingBalance);
                    }
                    this.DrawingBalance = new Ellipse() { Fill = System.Windows.Media.Brushes.GreenYellow, Width = 70, Height = 70, Margin = new Thickness(xza, yza, 0, 0) };　//重心のマーク
                    this.balancedraw.Children.Add(this.DrawingBalance);

                    //////////////左足の荷重量表示
                    if (this.DrawingLeftLeg != null)
                    {
                        this.leftleg.Children.Remove(this.DrawingLeftLeg);
                    }
                    this.DrawingLeftLeg = new Rectangle() { Fill = System.Windows.Media.Brushes.GreenYellow, Width = leftleg.Width, MaxHeight = leftleg.Height, MinHeight = 0 };
                    if (LeftSize < 0)
                    {
                        this.DrawingLeftLeg.Height = 0;
                    }
                    else if (LeftSize > leftleg.Height)
                    {
                        this.DrawingLeftLeg.Height = leftleg.Height;
                    }
                    else
                    {
                        this.DrawingLeftLeg.Height = LeftSize;
                    }
                    this.DrawingLeftLeg.Margin = new Thickness(0, leftleg.Height - LeftSize, 0, 0);
                    this.leftleg.Children.Add(this.DrawingLeftLeg);

                    /////////////////右足の荷重量表示
                    if (this.DrawingRightLeg != null)
                    {
                        this.RightLeg.Children.Remove(this.DrawingRightLeg);
                    }
                    this.DrawingRightLeg = new Rectangle() { Fill = System.Windows.Media.Brushes.GreenYellow, Width = RightLeg.Width, MaxHeight = RightLeg.Height, MinHeight = 0 };
                    if (RightLeg.Height - LeftSize < 0)
                    {
                        this.DrawingRightLeg.Height = 0;
                    }
                    else if (RightLeg.Height - LeftSize > RightLeg.Height)
                    {
                        this.DrawingRightLeg.Height = RightLeg.Height;
                    }
                    else if (LeftSize == 0)
                    {
                        this.DrawingRightLeg.Height = 0;
                    }
                    else
                    {
                        this.DrawingRightLeg.Height = RightLeg.Height - LeftSize;
                    }
                    this.DrawingRightLeg.Margin = new Thickness(0, RightLeg.Height - (RightLeg.Height - LeftSize), 0, 0);
                    this.RightLeg.Children.Add(this.DrawingRightLeg);

                    //////////////////////////////////////////////

                    leftparcent.Text = ((int)((LeftSize / leftleg.Height) * 100)).ToString() + "%"; //左足荷重％
                    if ((int)((LeftSize / leftleg.Height) * 100) == 0)
                    {
                        rightparcent.Text = "0%";
                    }
                    else
                    {
                        rightparcent.Text = (100 - ((int)((LeftSize / leftleg.Height) * 100))).ToString() + "%";
                    }

                    /////////////////青い■を表示
                    if (GameState == true)
                    {
                        if (DrawingNextRight == false) //左の％表示
                        {
                            if (DrawingNextLeft != null)
                            {
                                this.next.Children.Remove(this.DrawingNextLeft);
                            }

                            this.DrawingNextLeft = new Rectangle() { Fill = System.Windows.Media.Brushes.Blue, Width = 50, Height = next.Height };

                            this.DrawingNextLeft.Margin = new Thickness(nextx, 0, 0, 0);

                            this.next.Children.Add(this.DrawingNextLeft);
                            RightFlag.Content = ""; //黒い画面の矢印

                            LeftFlag.Content = "⬅︎"; //黒い画面の矢印

                            if ((int)((LeftSize / leftleg.Height) * 100) > (100 - line))　//赤い線を超えた場合
                            {
                                PlaySound(SoundFile);
                                DrawingNextRight = true;
                                count++;
                                tCount--;
                                if (GameType == false)　//回数表示
                                {
                                    tim.Text = count + "回";
                                }
                                else
                                {
                                    cc.Text = "残り" + tCount + "回";
                                }
                            }
                        }
                        else if (DrawingNextRight == true)　//右の％表示
                        {
                            if (DrawingNextLeft != null)
                            {
                                this.next.Children.Remove(this.DrawingNextLeft);
                            }

                            this.DrawingNextLeft = new Rectangle() { Fill = System.Windows.Media.Brushes.Blue, Width = 50, Height = next.Height };

                            this.DrawingNextLeft.Margin = new Thickness(next.Width - DrawingNextLeft.Width, 0, 0, 0);

                            this.next.Children.Add(this.DrawingNextLeft);

                            LeftFlag.Content = ""; //黒い画面の矢印


                            RightFlag.Content = "➡︎"; //黒い画面の矢印

                            if ((int)(((RightLeg.Height - LeftSize) / RightLeg.Height) * 100) > (100 - line))
                            {
                                PlaySound(SoundFile);
                                DrawingNextRight = false;
                                count++;
                                tCount--;
                                if (GameType == true)
                                {
                                    cc.Text = "残り" + tCount + "回";
                                }
                                else
                                {
                                    tim.Text = count + "回";
                                }
                            }
                        }

                        if ((GameType == true && ClearCount == count) || (GameType == false && time_s == 0))　//ゲームの終了条件
                        {
                            GameState = false;
                            PlaySound("clear.wav");
                            StopWatch.Stop();
                            dispatcharTimer.Stop();



                            if (GameType == true)　//タイムアタック時のデータベース
                            {
                                if (!Application.Current.Properties["u_id"].ToString().Equals("guest"))
                                {
                                    DBConnect.Connect("kasiihara.db");
                                    SQL = "INSERT INTO t_userrecord (user_id,traintype,trainclear_date)VALUES('" + user_id + "','タイムアタック','" + DateTime.Now.ToString() + "')";
                                    DBConnect.ExecuteReader(SQL);
                                    SQL = "SELECT * FROM t_userrecord ORDER BY userrecord_id DESC";
                                    DBConnect.ExecuteReader(SQL);
                                    DBConnect.Reader.Read();
                                    SQL = "INSERT INTO t_gametrain(userrecord_id,setting,clear_record,clear_line)VALUES('" + DBConnect.Reader[0] + "','" + ClearCount + "','" + time_t + "','" + lin + "')";
                                    DBConnect.ExecuteReader(SQL);
                                    DBConnect.Dispose();

                                    

                                }

                                //MessageBox.Show("終了です" + sw.Elapsed.Seconds + "秒");
                                //Application.Current.Properties["sw.Elapsed.Second"] = sw.Elapsed.Seconds;
                                Application.Current.Properties["time_t"] = time_t;
                                //Refresh1 rf1 = new Refresh1(this.bac);


                                tim.Text = "0秒";
                                StopWatch = new System.Diagnostics.Stopwatch();
                                dispatcharTimer = new DispatcherTimer(DispatcherPriority.Normal);
                                dispatcharTimer.Interval = new TimeSpan(0, 0, 1);
                                dispatcharTimer.Tick += new EventHandler(dispatcharTimer_Tick);
                                count = 0;
                                tCount = tCounth;
                                nextx = 0;
                                time_t = 0;

                                game = false;
                                StartButton.Content = "スタート";

                                GameResult w = new GameResult(this.Button_Color, this.BackButton_Click, this.StartButton_Click);
                                w.Title = "GameResult";
                                w.ShowDialog();



                            }
                            else //スコアタック時
                            {
                                if (!Application.Current.Properties["u_id"].ToString().Equals("guest"))
                                {
                                    DBConnect.Connect("kasiihara.db");
                                    SQL = "INSERT INTO t_userrecord (user_id,traintype,trainclear_date)VALUES('" + user_id + "','スコアアタック','" + DateTime.Now.ToString() + "')";
                                    DBConnect.ExecuteReader(SQL);
                                    SQL = "SELECT * FROM t_userrecord ORDER BY userrecord_id DESC";
                                    DBConnect.ExecuteReader(SQL);
                                    DBConnect.Reader.Read();
                                    SQL = "INSERT INTO t_gametrain(userrecord_id,setting,clear_record,clear_line)VALUES('" + DBConnect.Reader[0] + "','" + htimes_s + "','" + count + "','" + lin + "')";
                                    DBConnect.ExecuteReader(SQL);
                                    DBConnect.Dispose();


                                }
                                //MessageBox.Show("終了です" + Count + "回");
                                Application.Current.Properties["Count"] = count;

                                
                            
                                dispatcharTimer = new DispatcherTimer(DispatcherPriority.Normal);
                                dispatcharTimer.Interval = new TimeSpan(0, 0, 1);
                                dispatcharTimer.Tick += new EventHandler(dispatcharTimer_Tick);
                                time_s = htimes_s;
                                count = 0;

                                game = false;
                                StartButton.Content = "スタート";
                               
                                
                                GameResult w = new GameResult(this.Button_Color,this.BackButton_Click,this.StartButton_Click);
                                w.Title = "GameResult";
                                w.ShowDialog();
                                

                            }

                        }

                    }

                }));

            }

        }

        private void GameSetting()
        {
            throw new NotImplementedException();
        }

        void Page_Closing(object sender, System.ComponentModel.CancelEventArgs e)　//アプリ閉じたら
        {
            //if (game == true)
            //{
                wiimote.Disconnect();
            //}
        }



        void BackButton_Click(object sender, EventArgs e) //戻るボタン
        {
           // if (game == true)
           // {
                wiimote.Disconnect();
           // }
            var nextPage = new GameSelect();
            NavigationService.Navigate(nextPage);
        }

        public void StartButton_Click(object sender, EventArgs e)　//スタートボタン
        {
             
             if (StartButton.Content.Equals("スタート"))
             {
                 countdown.Content = "3";
                 cdtime = 0;
                 dispatcharTimer11.Start();
                 Console.WriteLine("スタート");

                if (GameType == true)
                {
                    StopWatch.Start();
                    cc.Text = "残り" + tCount + "回";
                    tim.Text = "0" + "秒";

                }
                else
                {
                    cc.Text = "残り" + time_s + "秒";
                    tim.Text = count + "回";
                }
             }
            else if (StartButton.Content.Equals("リスタート"))
            {
                countdown.Content = "3";
                cdtime = 0;
                dispatcharTimer11.Start();
                cdtime = 0;
                wiimote.Connect();
                dispatcharTimer11.Start();
            }
            else 
             {
                 Console.WriteLine("ストップ");

                 //ストップの状態だと
                 wiimote.Disconnect();
                 StartButton.Content = "リスタート";
                 game = false;
                 GameState = false;
                 dispatcharTimer.Stop();
                 if (GameType == true)
                 {
                     StopWatch.Stop();
                 }
                 else
                 {
                 }

             }

        }



        void SetDialog_Click(object sender, RoutedEventArgs e) 　//設定のダイアログ
        {
            ScoreAttackButton.Background = Brushes.Gainsboro;　//ボタン押したときの色変更
            TimeAttackButton.Background = Brushes.Gainsboro;
            ScoreFlag = false;
            TimeFlag = false;
            Refresh rf = new Refresh(this.Button_Color);


            Application.Current.Properties["setuser_id"] = user_id;

            Application.Current.Properties["gamemodename"] = "スコアアタック";

            Application.Current.Properties["hantei"] = "計測時間";
            Application.Current.Properties["tani"] = "秒";


            Window setting_window = new GameSetting(rf);
            setting_window.Title = "GameSetting";
            setting_window.ShowDialog();
        }

        //スコアタックを選択
        private void Set_ScoreAttack(object sender, RoutedEventArgs e)
        {
            ScoreAttackButton.Background = Brushes.Coral;　//ボタン押したときの色変更
            TimeAttackButton.Background = Brushes.Gainsboro;
            
            Refresh rf = new Refresh(this.Button_Color);

            Application.Current.Properties["setuser_id"] = user_id;

            Application.Current.Properties["gamemodename"] = (sender as Button).Content;


            DBConnect.Connect("kasiihara.db");
            SQL = "SELECT * FROM t_userrecord NATURAL JOIN t_gametrain WHERE user_id ='" + user_id + "' AND traintype ='スコアアタック' ORDER BY userrecord_id DESC";
            DBConnect.ExecuteReader(SQL);
            if (DBConnect.Reader.Read())
            {
                Application.Current.Properties["line"] = int.Parse(DBConnect.Reader[7].ToString());
                Application.Current.Properties["sette"] = int.Parse(DBConnect.Reader[5].ToString());
              
                Application.Current.Properties["settei"] = "計測時間" + Application.Current.Properties["sette"] + "秒";
                rf(0);
            }
            else
            {
                Application.Current.Properties["line"] = 60;
                Application.Current.Properties["sette"] = 10;

                Application.Current.Properties["settei"] = "計測時間" + Application.Current.Properties["sette"] + "秒";
                rf(0);
            }
            DBConnect.Dispose();
        }

        //タイムアタックを選択
        private void Set_TimeAttack(object sender, RoutedEventArgs e)
        {
            ScoreAttackButton.Background = Brushes.Gainsboro;　//ボタン押したときの色変更
            TimeAttackButton.Background = Brushes.Coral;

            Refresh rf = new Refresh(this.Button_Color);

            Application.Current.Properties["setuser_id"] = user_id;

            Application.Current.Properties["gamemodename"] = (sender as Button).Content;

            DBConnect.Connect("kasiihara.db");
            SQL = "SELECT * FROM t_userrecord NATURAL JOIN t_gametrain WHERE user_id ='" + user_id + "' AND traintype ='タイムアタック' ORDER BY userrecord_id DESC";
            DBConnect.ExecuteReader(SQL);
            if (DBConnect.Reader.Read())
            {
                Application.Current.Properties["line"] = int.Parse(DBConnect.Reader[7].ToString());
                Application.Current.Properties["sette"] = int.Parse(DBConnect.Reader[5].ToString());

                Application.Current.Properties["settei"] = "計測回数" + Application.Current.Properties["sette"] + "回";
                rf(1);
            }
            else
            {
                Application.Current.Properties["line"] = 60;
                Application.Current.Properties["sette"] = 10;

                Application.Current.Properties["settei"] = "計測回数" + Application.Current.Properties["sette"] + "回";
                rf(1);
            }
            DBConnect.Dispose();
        }



        void Button_Color(int t)
        {
            lin = Application.Current.Properties["line"].ToString();
            set = Application.Current.Properties["settei"].ToString();
            se = Application.Current.Properties["sette"].ToString(); //設定画面での秒数、回数
            if (t == 0) //scoreattackモード
            {
                ScoreAttackButton.Background = Brushes.Coral;
                GameType = false;
                time_s = int.Parse(se);
                htimes_s = time_s;
                ScoreFlag = true;
                TimeFlag = false;
            }
            else
            {
                TimeAttackButton.Background = Brushes.Coral;
                GameType = true;
                ScoreFlag = false;
                TimeFlag = true;
            }

            st = "クリアライン:" + lin + "%　" + set;
            settin.Text = st;

            line = 100 - int.Parse(lin);
            ClearCount = int.Parse(se);
            tCount = int.Parse(se);
            tCounth = int.Parse(se);
            if (DrawingLeftLine != null)
            {
                this.leftleg.Children.Remove(this.DrawingLeftLine);
            }
            this.DrawingLeftLine = new Rectangle() { Fill = System.Windows.Media.Brushes.Red, Width = leftleg.Width, Height = 10, MaxHeight = leftleg.Height, MinHeight = 0 };
            this.DrawingLeftLine.Margin = new Thickness(0, leftleg.Height * (((double)line / 100)), 0, 0);

            this.leftleg.Children.Add(this.DrawingLeftLine);
            //            this.rightleg.Children.Add(this.drawingLineL);
            if (DrawingRightLine != null)
            {
                this.RightLeg.Children.Remove(this.DrawingRightLine);
            }
            this.DrawingRightLine = new Rectangle() { Fill = System.Windows.Media.Brushes.Red, Width = RightLeg.Width, Height = 10, MaxHeight = RightLeg.Height, MinHeight = 0 };
            this.DrawingRightLine.Margin = new Thickness(0, RightLeg.Height * (((double)line / 100)), 0, 0);

            this.RightLeg.Children.Add(this.DrawingRightLine);
        }

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

        void dispatcharTimer_Tick(object sender, EventArgs e)
        {
            if (GameType == false)
            {
                time_s--;
                cc.Text = "残り" + time_s + "秒";
            }
            else
            {
                time_t++;
                tim.Text = time_t + "秒";
            }
        }

        
   

        void dispatcharTimer11_Tick(object sender, EventArgs e)
        {
           
            cdtime++;
            countdown.Content = (3 - cdtime).ToString();



            if (cdtime == 3)
            {
                countdown.Content = "";
                dispatcharTimer11.Stop();
                dispatcharTimer11.IsEnabled = false;

                if (ScoreFlag == true || TimeFlag == true) //秒・回数設定されると
                {
                    if (game == false) //スタート押されてると
                    {
                        if (dispatcharTimer11.IsEnabled == false)
                        {
                            
                            wiimote.Connect();
                            StartButton.Content = "ストップ";
                            game = true;
                            GameState = true;
                            dispatcharTimer.Start();
                            
                        }
                    }
                    /*coneの方のストップに行く
                     * else
                    { 
                        //ストップの状態だと
                        wiimote.Disconnect();
                        con.Content = "スタート";
                        game = false;
                        gameState = false;
                        dispatcharTimer.Stop();
                        if (gameM == true)
                        {
                            sw.Stop();
                        }
                        else
                        {
                        }
                    }
                    */
                }
                else
                {
                    MessageBox.Show("設定を行ってください。");
                }
                
            }
            
            

        }
        

    }
}

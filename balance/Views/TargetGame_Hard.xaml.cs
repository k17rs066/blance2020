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
    /// </summary>
{
    /// <summary>
    /// TargetGame.xaml の相互作用ロジック
    /// 


    public partial class TargetGame_Hard : Page
    {

        public delegate void Refresh_a(object sender, RoutedEventArgs e);

        public delegate void Refresh_b(object sender, RoutedEventArgs e);


        Wiimote wiimote = new Wiimote();

        double xza = 0; //重心のX座標
        double yza = 0; //重心のY座標

        private Ellipse drawingBalance = null;  //重心のマーク
        private Label drawingLabel = null; //カウントダウン表示
        private Label drawingLabel1 = null; //END表示

        double leftsize = 0;  //左足の加重量
        double rightsize = 0; //右足の加重量

        delegate void UpdateWiimoteStateDelegate(object sender, WiimoteChangedEventArgs args);

        //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch(); //ストップウォッチsw生成
        DispatcherTimer dispatcharTimer; //ゲームの秒数を保持する
        int time_t = 0; //計測タイム

        String SQL = "";　//SQL文
        int user_id = -1; //ログインのu_id

        Boolean game = false;

        int ctarget = 10; //ターゲット個数カウントダウン

        DispatcherTimer dispatcharTimer11; //カウントダウンの秒数を保持する
        int cdtime;


        double[,] ta = new double[,]
        {
            {840,92},
            {357,552},
            {1071,552},
            {122,92},
            {1071,92},
            {590,92},
            {122,552},
            {590,552},
            {357,92},
            {840,552},
        };

        public TargetGame_Hard()
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
                    xza = 621 - 35;  //(Canvas.Width/2) - (ballWidth /2)
                    yza = 337 - 35;  //(Canvas.Height/2) - (ballHeight /2)
                    leftsize = 0;
                }
                else
                {
                    xza = bbs.CenterOfGravity.X * 18 * 2 + 621 - 35;//*(Canvas.Width/ballWidth)*倍率 + (Canvas.Width/2) - (ballWidth /2)
                    yza = bbs.CenterOfGravity.Y * 9.6 * 3 + 337 - 35;//*(Canvas.Height/ballHeight)*倍率 + (Canvas.Height/2) - (ballHeight /2)
                    //                  leftsize = 700 - (350 + 14 * (bbs.CenterOfGravity.X));
                    leftsize = (1 - (xza / 790)) * 700;
                    if (xza > 1173) //枠内に収まるように
                    {
                        xza = 1173;// 1243 - 70 bdraw.Width - ballSize
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
                    this.drawingBalance = new Ellipse() { Fill = System.Windows.Media.Brushes.LimeGreen, Width = 70, Height = 70, Margin = new Thickness(xza, yza, 0, 0) }; //重心のマーク
                    this.beback.Children.Add(this.drawingBalance);

                    //tar1.Content = ("1ターゲット" + ta[0,0] + "," + ta[0,1]);
                    //zyuu.Content = ("2ターゲット" + ta[1, 0] + "," + ta[1, 1]);
                    atari(ta[0, 0], ta[0, 1], 1);
                    atari(ta[1, 0], ta[1, 1], 2);
                    atari(ta[2, 0], ta[2, 1], 3);
                    atari(ta[3, 0], ta[3, 1], 4);
                    atari(ta[4, 0], ta[4, 1], 5);
                    atari(ta[5, 0], ta[5, 1], 6);
                    atari(ta[6, 0], ta[6, 1], 7);
                    atari(ta[7, 0], ta[7, 1], 8);
                    atari(ta[8, 0], ta[8, 1], 9);
                    atari(ta[9, 0], ta[9, 1], 10);



                    if (target1 != null)
                    {
                        this.beback.Children.Remove(this.target1);
                    }

                    this.beback.Children.Add(this.target1);
                    /*        double hlength = 35 + 44; //重心半径　-　標的半径
                            double xlength = xza - ta[0, 0]; //重心X座標　-　標的X座標
                            double ylength = yza - ta[0, 1]; //重心Y座標　-　標的Y座標

                            if (hlength * hlength >= xlength * xlength + ylength * ylength && target1.IsEnabled == true)
                            {
                                target1.IsEnabled = false;
                                target1.Background = Brushes.Yellow;
                                ctarget--;

                            }
        */
                    //count.Content = "残り個数　　　" + ctarget + "個";
                    //count.Content = "残り個数　4個";

                    //////////////////////////終了条件
                    if (ctarget == 0 && target5.IsEnabled == false)
                    {
                        //this.drawingLabel1 = new Label() { Content = "", Width = 841, Height = 445, Margin = new Thickness(210, 176, 0, 0), FontSize = 350, Foreground = System.Windows.Media.Brushes.Red, FontWeight = FontWeights.Bold, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                        //this.beback.Children.Add(this.drawingLabel1);
                        game = false;

                        PlaySound("clear.wav");
                        //drawingLabel1.Content = "クリア";
                        //drawingLabel1.Foreground = System.Windows.Media.Brushes.Black;
                        dispatcharTimer.Stop();
                        startbutton.Content = "ストップ";



                        if (!Application.Current.Properties["u_id"].ToString().Equals("guest"))
                        {
                            DBConnect.Connect("kasiihara.db");
                            SQL = "INSERT INTO t_userrecord (user_id,traintype,trainclear_date)VALUES('" + user_id + "','ターゲットゲーム','" + DateTime.Now.ToString() + "')";
                            DBConnect.ExecuteReader(SQL);
                            SQL = "SELECT * FROM t_userrecord ORDER BY userrecord_id DESC";
                            DBConnect.ExecuteReader(SQL);
                            DBConnect.Reader.Read();
                            SQL = "INSERT INTO t_targetgame (userrecord_id,result_time)VALUES('" + DBConnect.Reader[0] + "', '" + time_t + "')";
                            DBConnect.ExecuteReader(SQL);
                            DBConnect.Dispose();


                            
                            

                        }

                        dispatcharTimer = new DispatcherTimer(DispatcherPriority.Normal);
                        dispatcharTimer.Interval = new TimeSpan(0, 0, 1);
                        dispatcharTimer.Tick += new EventHandler(dispatcharTimer_Tick);


                       

                        target1.IsEnabled = true;
                        target2.IsEnabled = true;
                        target3.IsEnabled = true;
                        target4.IsEnabled = true;
                        target5.IsEnabled = true;
                        target6.IsEnabled = true;
                        target7.IsEnabled = true;
                        target8.IsEnabled = true;
                        target9.IsEnabled = true;
                        target10.IsEnabled = true;







                        Application.Current.Properties["ftgamemodename"] = "ターゲットゲーム";

                        Application.Current.Properties["ftresult"] = time_t;

                        startbutton.Content = "スタート";

                        FTGameResult s = new FTGameResult(this.start, this.back);
                        s.Title = "FTGameResult";
                        s.ShowDialog();


                    }

                }));


            }

        }

        

        void atari(double a, double b, int han)
        {
            double hlength = 35 + 44; //重心半径　-　標的半径
            double xlength = xza - a; //重心X座標　-　標的X座標
            double ylength = yza - b; //重心Y座標　-　標的Y座標

            if (hlength * hlength >= xlength * xlength + ylength * ylength)
            {
                switch (han)
                {
                    case 1:
                        if (target1.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target1.IsEnabled = false;
                            target1.Background = Brushes.LimeGreen;
                            ctarget--;

                        }

                        break;

                    case 2:
                        if (target1.IsEnabled == false && target2.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target2.IsEnabled = false;
                            target2.Background = Brushes.LimeGreen;
                            ctarget--;
                        }
                        break;

                    case 3:
                        if (target1.IsEnabled == false && target2.IsEnabled == false && target3.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target3.IsEnabled = false;
                            target3.Background = Brushes.LimeGreen;
                            ctarget--;
                        }
                        break;

                    case 4:
                        if (target1.IsEnabled == false && target2.IsEnabled == false && target3.IsEnabled == false && target4.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target4.IsEnabled = false;
                            target4.Background = Brushes.LimeGreen;
                            ctarget--;
                        }
                        break;

                    case 5:
                        if (target1.IsEnabled == false && target2.IsEnabled == false && target3.IsEnabled == false && target4.IsEnabled == false && target5.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target5.IsEnabled = false;
                            target5.Background = Brushes.LimeGreen;
                            ctarget--;
                        }
                        break;

                    case 6:
                        if (target1.IsEnabled == false && target2.IsEnabled == false && target3.IsEnabled == false && target4.IsEnabled == false && target5.IsEnabled == false && target6.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target6.IsEnabled = false;
                            target6.Background = Brushes.LimeGreen;
                            ctarget--;
                        }
                        break;
                    case 7:
                        if (target1.IsEnabled == false && target2.IsEnabled == false && target3.IsEnabled == false && target4.IsEnabled == false && target5.IsEnabled == false && target6.IsEnabled == false && target7.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target7.IsEnabled = false;
                            target7.Background = Brushes.LimeGreen;
                            ctarget--;
                        }
                        break;
                    case 8:
                        if (target1.IsEnabled == false && target2.IsEnabled == false && target3.IsEnabled == false && target4.IsEnabled == false && target5.IsEnabled == false && target6.IsEnabled == false && target7.IsEnabled == false && target8.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target8.IsEnabled = false;
                            target8.Background = Brushes.LimeGreen;
                            ctarget--;
                        }
                        break;
                    case 9:
                        if (target1.IsEnabled == false && target2.IsEnabled == false && target3.IsEnabled == false && target4.IsEnabled == false && target5.IsEnabled == false && target6.IsEnabled == false && target7.IsEnabled == false && target8.IsEnabled == false && target9.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target9.IsEnabled = false;
                            target9.Background = Brushes.LimeGreen;
                            ctarget--;
                        }
                        break;
                    case 10:
                        if (target1.IsEnabled == false && target2.IsEnabled == false && target3.IsEnabled == false && target4.IsEnabled == false && target5.IsEnabled == false && target6.IsEnabled == false && target7.IsEnabled == false && target8.IsEnabled == false && target9.IsEnabled == false && target10.IsEnabled == true)
                        {
                            PlaySound("fall_up.wav");
                            target10.IsEnabled = false;
                            target10.Background = Brushes.LimeGreen;
                            ctarget--;
                        }
                        break;

                }


            }

            count.Content = "残り個数     " + ctarget + "個";
        }


        private void Thumb1_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[0, 0] = x;
                ta[0, 1] = y;

            }
        }

        private void Thumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[1, 0] = x;
                ta[1, 1] = y;

                //Console.WriteLine(x+","+y);
            }
        }

        private void Thumb3_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[2, 0] = x;
                ta[2, 1] = y;

            }
        }

        private void Thumb4_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[3, 0] = x;
                ta[3, 1] = y;

            }
        }

        private void Thumb5_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[4, 0] = x;
                ta[4, 1] = y;

            }
        }
        private void Thumb6_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[5, 0] = x;
                ta[5, 1] = y;

            }
        }

        private void Thumb7_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[6, 0] = x;
                ta[6, 1] = y;

            }
        }
        private void Thumb8_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[7, 0] = x;
                ta[7, 1] = y;

            }
        }
        private void Thumb9_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[8, 0] = x;
                ta[8, 1] = y;

            }
        }
        private void Thumb10_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (null != thumb)
            {
                var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
                var y = Canvas.GetTop(thumb) + e.VerticalChange;

                var canvas = thumb.Parent as Canvas;
                if (null != canvas)
                {
                    x = Math.Max(x, 0);
                    y = Math.Max(y, 0);
                    x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
                    y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);
                }

                Canvas.SetLeft(thumb, x);
                Canvas.SetTop(thumb, y);

                ta[9, 0] = x;
                ta[9, 1] = y;

            }
        }
        void dispatcharTimer_Tick(object sender, EventArgs e)
        {

            time_t++;
            time.Content = "計測時間    " + time_t + "秒";

        }

        void dispatcharTimer11_Tick(object sender, EventArgs e)
        {
            cdtime++;
            drawingLabel.Content = (3 - cdtime).ToString();

            if (cdtime == 3) //スタート押されてると
            {
                drawingLabel.Content = "";
                dispatcharTimer11.Stop();


                startbutton.Content = "ストップ";
                game = true;
                dispatcharTimer.Start();

            }

        }


        public void start(object sender, RoutedEventArgs e)
        {

            
            if (startbutton.Content.Equals("スタート"))
            {

                ctarget = 5;
                time_t = 0;

                
                //drawingLabel1.Content = " ";
                this.drawingLabel = new Label() { Content = "", Width = 841, Height = 445, Margin = new Thickness(210, 176, 0, 0), FontSize = 350, Foreground = System.Windows.Media.Brushes.Red, FontWeight = FontWeights.Bold, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                this.beback.Children.Add(this.drawingLabel);
                drawingLabel.Content = "3";
                cdtime = 0;
                dispatcharTimer11.Start();
                time.Content = "計測時間    " + time_t + "秒";
                count.Content = "残り個数     " + ctarget + "個";

                wiimote.Connect();

                // イメージブラシの作成　画像のパスを間違うと実行できない
                ImageBrush imageBrush1 = new ImageBrush();
                imageBrush1.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/1.png"));

                ImageBrush imageBrush2 = new ImageBrush();
                imageBrush2.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/2.png"));

                ImageBrush imageBrush3 = new ImageBrush();
                imageBrush3.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/3.png"));

                ImageBrush imageBrush4 = new ImageBrush();
                imageBrush4.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/4.png"));

                ImageBrush imageBrush5 = new ImageBrush();
                imageBrush5.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/5.png"));

                ImageBrush imageBrush6 = new ImageBrush();
                imageBrush6.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/6.png"));

                ImageBrush imageBrush7 = new ImageBrush();
                imageBrush7.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/7.png"));

                ImageBrush imageBrush8 = new ImageBrush();
                imageBrush8.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/8.png"));

                ImageBrush imageBrush9 = new ImageBrush();
                imageBrush9.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/9.png"));

                ImageBrush imageBrush10 = new ImageBrush();
                imageBrush10.ImageSource = new BitmapImage(new Uri("C:/Users/smkwlab02/Documents/GitHub/blanceNewGame/balance/Image/10.png"));

                target1.Background = imageBrush1;
                target2.Background = imageBrush2;
                target3.Background = imageBrush3;
                target4.Background = imageBrush4;
                target5.Background = imageBrush5;
                target6.Background = imageBrush6;
                target7.Background = imageBrush7;
                target8.Background = imageBrush8;
                target9.Background = imageBrush9;
                target10.Background = imageBrush10;

            }
            else if (startbutton.Content.Equals("リスタート"))
            {
                this.drawingLabel = new Label() { Content = "", Width = 841, Height = 445, Margin = new Thickness(210, 176, 0, 0), FontSize = 350, Foreground = System.Windows.Media.Brushes.Red, FontWeight = FontWeights.Bold, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                this.beback.Children.Add(this.drawingLabel);
                drawingLabel.Content = "3";
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


         void back(object sender, RoutedEventArgs e)
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

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
using WiimoteLib;
using System.Windows.Threading;

namespace balance.Views
{
    /// <summary>
    /// LoadTraining.xaml の相互作用ロジック
    /// </summary>
    /// 


    public partial class LoadTraining : Page
    {

        private Boolean asi = true;//両足
        Boolean train = false;
        Boolean trainstate = false;

        Wiimote wiimote = new Wiimote();

        double xza = 0;
        double yza = 0;

        private Ellipse drawingBalance = null;
        private Rectangle drawingLeftleg = null;
        private Rectangle drawingRightleg = null;

        private Rectangle drawingLineL = null;
        private Rectangle drawingLineR = null;

        double leftsize = 0;
        double leftsizeh = 0;
        int line = 50;

        bool oto = true;//音出してok;
        bool dline = true;//両足モードで70超えたときの音出しok

        int han = 0;
        int than = 0;
        int trhan = 0;

        Thickness jyougai = new Thickness(-1000, -1000, 0, 0);
        Thickness threem = new Thickness(822,169.857,0,0);
        Thickness twothreem = new Thickness(1441.2, 169.857, 0, 0);
        Thickness rightlegm = new Thickness(445.2, 133.2, 0, 0);
        Thickness leftlegm = new Thickness(126, 133.2, 0, 0);
        Thickness midlem = new Thickness(285.2, 133.2, 0, 0);

        Thickness rightparm = new Thickness(445.2, 877.91, 0, 0);
        Thickness leftparm = new Thickness(110.149, 877.91, 0, 0);
        Thickness rightmidlem = new Thickness(270.872, 877.91, 0, 0);
        DispatcherTimer timer;

        Boolean trb = false;
        Boolean twb = false;
        Boolean trwb = false;

        double Weight = 60;
        double weighpar = 0;

        String SQL = "";
        int user_id = -1;
        int pnum = -1;

        Boolean wiimo = false;


        Thickness hihyou = new Thickness(717, 41, 0, 0);

        int cali = 0;
        public LoadTraining()
        {
            InitializeComponent();
            
            if (!Application.Current.Properties["u_id"].ToString().Equals("guest"))
            {
                user_id = int.Parse(Application.Current.Properties["u_id"].ToString());
            }
 //           wiimote.Connect();
            wiimote.WiimoteChanged += OnWiimoteChanged;
            swei.TextChanged += ch;

            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(250);

            DBConnect.Connect("kasiihara.db");
            SQL = "SELECT * FROM t_Calibration ORDER BY calibration_id DESC";
            DBConnect.ExecuteReader(SQL);
            if (DBConnect.Reader.Read() == true)
            {
                cali = int.Parse(DBConnect.Reader[1].ToString());
            }
            DBConnect.Dispose();

        }

        void bac(object sender, EventArgs e)
        {

            var nextPage = new k_Top();
            NavigationService.Navigate(nextPage);

            if (train == true)
            {
                wiimote.Disconnect();
                wiimo = false;
                timer.Stop();
            }
        }


        void bac_clo(object sender, EventArgs e)
        {
            if ((sender as Button).Name.Equals("three"))
            {
                trb = true; twb = false; trwb = false;
                three.Background = Brushes.Coral;
                two.Background = Brushes.Gainsboro;
                twothree.Background = Brushes.Gainsboro;
                settin.Text = three.Content+"バランスモード";
                if (drawingLineR != null)
                {
                    this.rightleg.Children.Remove(this.drawingLineR);
                }
                this.drawingLineR = new Rectangle() { Fill = System.Windows.Media.Brushes.Red, Width = rightleg.Width, Height = 10, MaxHeight = rightleg.Height, MinHeight = 0 };
                this.drawingLineR.Margin = new Thickness(0, rightleg.Height * 0.66, 0, 0);

                this.rightleg.Children.Add(this.drawingLineR);
            }
            else if ((sender as Button).Name.Equals("two"))
            {
                trb = false; twb = true; trwb = false;
                three.Background = Brushes.Gainsboro;
                two.Background = Brushes.Coral;
                twothree.Background = Brushes.Gainsboro;
                settin.Text = two.Content + "バランスモード";
                if (asi == true)
                {
                    if (drawingLineL != null)
                    {
                        this.leftleg.Children.Remove(this.drawingLineL);
                    }
                    this.drawingLineL = new Rectangle() { Fill = System.Windows.Media.Brushes.Red, Width = leftleg.Width, Height = 10, MaxHeight = leftleg.Height, MinHeight = 0 };
                    this.drawingLineL.Margin = new Thickness(0, leftleg.Height * 0.3, 0, 0);

                    this.leftleg.Children.Add(this.drawingLineL);

                    if (drawingLineR != null)
                    {
                        this.rightleg.Children.Remove(this.drawingLineR);
                    }
                    this.drawingLineR = new Rectangle() { Fill = System.Windows.Media.Brushes.Red, Width = rightleg.Width, Height = 10, MaxHeight = rightleg.Height, MinHeight = 0 };
                    this.drawingLineR.Margin = new Thickness(0, rightleg.Height * 0.3, 0, 0);

                    this.rightleg.Children.Add(this.drawingLineR);
                }
                else
                {
                    if (drawingLineR != null)
                    {
                        this.rightleg.Children.Remove(this.drawingLineR);
                    }
                    this.drawingLineR = new Rectangle() { Fill = System.Windows.Media.Brushes.Red, Width = rightleg.Width, Height = 10, MaxHeight = rightleg.Height, MinHeight = 0 };
                    this.drawingLineR.Margin = new Thickness(0, rightleg.Height * 0.5, 0, 0);

                    this.rightleg.Children.Add(this.drawingLineR);
                }
            }
            else if ((sender as Button).Name.Equals("twothree"))
            {
                trb = false; twb = false; trwb = true;
                three.Background = Brushes.Gainsboro;
                two.Background = Brushes.Gainsboro;
                twothree.Background = Brushes.Coral;
                settin.Text = twothree.Content + "バランスモード";
                if (drawingLineR != null)
                {
                    this.rightleg.Children.Remove(this.drawingLineR);
                }
                this.drawingLineR = new Rectangle() { Fill = System.Windows.Media.Brushes.Red, Width = rightleg.Width, Height = 10, MaxHeight = rightleg.Height, MinHeight = 0 };
                this.drawingLineR.Margin = new Thickness(0, rightleg.Height * 0.33, 0, 0);

                this.rightleg.Children.Add(this.drawingLineR);
            }
        }

        void kiri(object sender, EventArgs e)
        {
            if (asi == true)
            {
                doubl.Content = "両足";
//                doubl.Background = Brushes.Cyan;
                asi = false;
                three.Margin = threem;
                twothree.Margin = twothreem;
                leftleg.Margin = jyougai;
                leftpar.Margin = jyougai;
                rightleg.Margin = midlem;
                rightpar.Margin = rightmidlem;
                asiname.Content = "片足";
                kijyun.Text = Weight + "kgが100%です"; ;
                if (drawingLineL != null)
                {
                    leftleg.Children.Remove(drawingLineL);
                }
                if (drawingLineR != null)
                {
                    rightleg.Children.Remove(drawingLineR);
                }
            }
            else if (asi == false)
            {
                doubl.Content = "片足";
//                doubl.Background = Brushes.DarkBlue;
                asi = true;
                three.Margin = jyougai;
                twothree.Margin = jyougai;
                leftleg.Margin = leftlegm;
                leftpar.Margin = leftparm;
                rightleg.Margin = rightlegm;
                rightpar.Margin = rightparm;
                asiname.Content = "両足";
                kijyun.Text ="";
                if (drawingLineL != null)
                {
                    leftleg.Children.Remove(drawingLineL);
                }
                if (drawingLineR != null)
                {
                    rightleg.Children.Remove(drawingLineR);
                }
            }
        }

        void cone(object sender, EventArgs e)
        {
            if ((train == false) && ((trb == true) || (twb == true) || (trwb == true)))
            {
                if (!Application.Current.Properties["u_id"].ToString().Equals("guest"))
                {

                    DBConnect.Connect("kasiihara.db");
                    SQL = "SELECT * FROM t_userrecord WHERE user_id = '"+user_id+"' AND traintype = '部分荷重訓練'";
                    DBConnect.ExecuteReader(SQL);
                    if (DBConnect.Reader.Read())
                    {
                        SQL = "SELECT * FROM t_maintaintrain WHERE userrecord_id = '" + DBConnect.Reader[0] + "'";
                        DBConnect.ExecuteReader(SQL);
                        DBConnect.Reader.Read();
                        pnum = int.Parse(DBConnect.Reader[2].ToString()) + 1;
                        SQL = "UPDATE t_maintaintrain SET play_number = '" + pnum + "' WHERE userrecord_id = '" + int.Parse(DBConnect.Reader[1].ToString()) + "'";
                        DBConnect.ExecuteReader(SQL);
                    }
                    else
                    {
                        SQL = "INSERT INTO t_userrecord (user_id,traintype,trainclear_date)VALUES('" + user_id + "','部分荷重訓練','"+DateTime.Now.ToString()+"')";
                        DBConnect.ExecuteReader(SQL);
                        SQL = "SELECT * FROM t_userrecord ORDER BY userrecord_id DESC";
                        DBConnect.ExecuteReader(SQL);
                        DBConnect.Reader.Read();
                        SQL = "INSERT INTO t_maintaintrain(userrecord_id,play_number)VALUES('" + DBConnect.Reader[0] + "','1')";
                        DBConnect.ExecuteReader(SQL);
                    }
                    
                    DBConnect.Dispose();

                }

                wiimote.Connect();
                (sender as Button).Content = "ストップ";
                train = true;
                trainstate = true;
                timer.Start();

            }
            else
            {
                    wiimote.Disconnect();
//                    kijyun.Text = "";
                    (sender as Button).Content = "スタート";
                    train = false;
                    trainstate = false;
                    timer.Stop();
            }
        }

        void OnWiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
/*            BalanceBoardState bbs = e.WiimoteState.BalanceBoardState;

            Dispatcher.Invoke(new Action(() =>
            {
                wei.Content = (int)double.Parse(bbs.WeightKg.ToString()) + cali;
            }));
 */           
            if (train == true)
            {
               BalanceBoardState bbs = e.WiimoteState.BalanceBoardState;
                if (bbs.WeightKg < 5)
                {
                    xza = 430 - 35;
                    yza = 230 - 35;
                    leftsize = 0;
                }
                else
                {
                    xza = bbs.CenterOfGravity.X * 12 * 2 + 430 - 35;//*(Canvas.Width/ballWidth)*倍率 + (Canvas.Width/2) - (ballWidth /2)
                    yza = bbs.CenterOfGravity.Y * 6.6 * 3 + 230 - 35;
//                    leftsize = 700 - (350 + 14 * (bbs.CenterOfGravity.X));

                        leftsize = (1-(xza/790)) * 700;

                    if (xza > 790)
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
                    wei.Content = (int)double.Parse(bbs.WeightKg.ToString()) + cali;
                    if (asi == true)
                    {
                        //////////////重心の表示
                        if (this.drawingBalance != null)
                        {
                            this.bdraw.Children.Remove(this.drawingBalance);
                        }
                        this.drawingBalance = new Ellipse() { Fill = System.Windows.Media.Brushes.GreenYellow, Width = 70, Height = 70, Margin = new Thickness(xza, yza, 0, 0) };
                        this.bdraw.Children.Add(this.drawingBalance);

                        //////////////左足の荷重量表示
                        if (this.drawingLeftleg != null)
                        {
                            this.leftleg.Children.Remove(this.drawingLeftleg);
                        }
                        this.drawingLeftleg = new Rectangle() { Fill = System.Windows.Media.Brushes.GreenYellow, Width = leftleg.Width, MaxHeight = leftleg.Height, MinHeight = 0 };
                        if (leftsize < 0)
                        {
                            this.drawingLeftleg.Height = 0;
                        }
                        else if (leftsize > leftleg.Height)
                        {
                            this.drawingLeftleg.Height = leftleg.Height;
                        }
                        else
                        {
                            this.drawingLeftleg.Height = leftsize;
                        }
                        if ((((leftsize / leftleg.Height) * 100) > 70) || (((leftsize / leftleg.Height) * 100) < 30))
                        {
                            this.drawingLeftleg.Fill = System.Windows.Media.Brushes.Red;
                        }
                        this.drawingLeftleg.Margin = new Thickness(0, leftleg.Height - leftsize, 0, 0);
                        this.leftleg.Children.Add(this.drawingLeftleg);

                        /////////////////右足の荷重量表示

                        if (this.drawingRightleg != null)
                        {
                            this.rightleg.Children.Remove(this.drawingRightleg);
                        }
                        this.drawingRightleg = new Rectangle() { Fill = System.Windows.Media.Brushes.GreenYellow, Width = rightleg.Width, MaxHeight = rightleg.Height, MinHeight = 0 };
                        if (rightleg.Height - leftsize < 0)
                        {
                            this.drawingRightleg.Height = 0;
                        }
                        else if (rightleg.Height - leftsize > rightleg.Height)
                        {
                            this.drawingRightleg.Height = rightleg.Height;
                        }
                        else if (leftsize == 0)
                        {
                            this.drawingRightleg.Height = 0;
                        }
                        else
                        {
                            this.drawingRightleg.Height = rightleg.Height - leftsize;
                        }
                        if ((((leftsize / leftleg.Height) * 100) > 70) || (((leftsize / leftleg.Height) * 100) < 30))
                        {
                            this.drawingRightleg.Fill = System.Windows.Media.Brushes.Red;
                        }
                        this.drawingRightleg.Margin = new Thickness(0, rightleg.Height - (rightleg.Height - leftsize), 0, 0);
                        this.rightleg.Children.Add(this.drawingRightleg);

                        

                        leftpar.Text = ((int)((leftsize / leftleg.Height) * 100)).ToString() + "%";
                        if ((int)((leftsize / leftleg.Height) * 100) == 0)
                        {
                            rightpar.Text = "0%";
                        }
                        else
                        {
                            rightpar.Text = (100 - ((int)((leftsize / leftleg.Height) * 100))).ToString() + "%";
                        }



                        if (trainstate == true)
                        {
                            if (asi == true)
                            {
                                if ((((leftsize / leftleg.Height) * 100) > 45) && ((leftsize / leftleg.Height) * 100) < 55)
                                {
                                    if (oto == true && han == 6)
                                    {
                                        PlaySound("100.wav");
                                        oto = false;
                                        han = 0;
                                        if (dline == false)
                                        {
                                            dline = true;
                                        }
                                    }
                                }
                                else if (((((leftsize / leftleg.Height) * 100) > 55) && ((leftsize / leftleg.Height) * 100) < 65) || ((((leftsize / leftleg.Height) * 100) > 35) && ((leftsize / leftleg.Height) * 100) < 45))
                                {
                                    if (oto == true && than == 2)
                                    {
                                        PlaySound("200.wav");
                                        oto = false;
                                        than = 0;
                                        if (dline == false)
                                        {
                                            dline = true;
                                        }
                                    }
                                }
                                else if (((((leftsize / leftleg.Height) * 100) > 65) && ((leftsize / leftleg.Height) * 100) < 70) || ((((leftsize / leftleg.Height) * 100) > 30) && ((leftsize / leftleg.Height) * 100) < 35))
                                {
                                    if (oto == true && trhan == 1)
                                    {
                                        PlaySound("300.wav");
                                        oto = false;
                                        trhan = 0;
                                        if (dline == false)
                                        {
                                            dline = true;
                                        }
                                    }
                                }
                                else if ((((leftsize / leftleg.Height) * 100) > 70) || (((leftsize / leftleg.Height) * 100) < 30))
                                {
                                    if (oto == true && dline == true)
                                    {
                                        PlaySound("bubbu.wav");
                                        dline = false;
                                        oto = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (asi == false)
                    {
                        weighpar = ((((double)bbs.WeightKg) / Weight) * 100);
                        /////////////////右足の荷重量表示
                        if (this.drawingRightleg != null)
                        {
                            this.rightleg.Children.Remove(this.drawingRightleg);
                        }
                        this.drawingRightleg = new Rectangle() { Fill = System.Windows.Media.Brushes.GreenYellow, Width = rightleg.Width, MaxHeight = rightleg.Height, MinHeight = 0 };

                            if (weighpar < 0)
                            {
                                this.drawingRightleg.Height = 0;
                            }
                            else if (weighpar > rightleg.Height)
                            {
                                this.drawingRightleg.Height = rightleg.Height;
                            }
                            else
                            {
                                this.drawingRightleg.Height = rightleg.Height*(weighpar/100);
                            }
                            this.drawingRightleg.Margin = new Thickness(0,rightleg.Height *((100 - weighpar)/100), 0, 0);

                            if ((trb == true && weighpar > 33) || (twb == true && weighpar > 50) || (trwb == true && weighpar > 66))
                            {
                                this.drawingRightleg.Fill = System.Windows.Media.Brushes.Red;
                            }

                        this.rightleg.Children.Add(this.drawingRightleg);


                        if ((int)((leftsize / leftleg.Height) * 100) == 0)
                        {
                            rightpar.Text = "0%";
                        }
                        else
                        {
                            rightpar.Text = (int)weighpar+ "%";
                        }


                        if (trainstate == true)
                        {
                            if (trb == true || twb == true || trwb == true)
                            {
                                if (((trb == true) && (weighpar < 23)) || ((twb == true) && (weighpar < 35)) || ((trwb == true) && (weighpar < 46)))
                                {
                                    if (oto == true && han == 6)
                                    {
                                        PlaySound("100.wav");
                                        oto = false;
                                        han = 0;
                                        if (dline == false)
                                        {
                                            dline = true;
                                        }
                                    }
                                }
                                else if (((trb == true) && ((weighpar > 23) && (weighpar < 30))) || ((twb == true) && ((weighpar > 35) && (weighpar < 45))) || ((trwb == true) && ((weighpar > 46) && (weighpar < 60))))
                                {
                                    if (oto == true && than == 2)
                                    {
                                        PlaySound("200.wav");
                                        oto = false;
                                        than = 0;
                                        if (dline == false)
                                        {
                                            dline = true;
                                        }
                                    }
                                }
                                else if (((trb == true) && ((weighpar > 30) && (weighpar < 33))) || ((twb == true) && ((weighpar > 45) && (weighpar < 50))) || ((trwb == true) && ((weighpar > 60) && (weighpar < 66))))
                                {
                                    if (oto == true && trhan == 1)
                                    {
                                        PlaySound("300.wav");
                                        oto = false;
                                        trhan = 0;
                                        if (dline == false)
                                        {
                                            dline = true;
                                        }
                                    }
                                }
                                else if (((trb == true) && (weighpar > 33)) || ((twb == true) && (weighpar > 50)) || ((trwb == true) && (weighpar > 66)))
                                {
                                    if (oto == true && dline == true)
                                    {
                                        PlaySound("bubbu.wav");
                                        dline = false;
                                        oto = false;
                                    }
                                }
                            }
                        }
                    }

                }));

            }

        }
        void Page_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
                wiimote.Disconnect();
                wiimo = false;
                timer.Stop();
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (oto == false)
            {
                oto = true;
            }
            if (han != 6)
            {
                han++;
            }
            if (than != 2)
            {
                than++;
            }
            if (trhan != 1)
            {
                trhan++;
            }
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

        private void migi(object sender, EventArgs e)
        {
            try
            {
                swei.Text = wei.Content.ToString();
            }
            catch
            {

            }
        }

        private void ch(object sender, TextChangedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Weight = int.Parse(swei.Text);
                    if (asi == false)
                    {
                        kijyun.Text = Weight + "kgが100%です";
                    }
                }));
            }
            catch
            {

            }
        }

        private void taihi(object sender, EventArgs e)
        {
            if ((sender as Button).Tag.Equals("true"))
            {
                kakusub.Content = "体重非表示";
                (sender as Button).Tag = "false";
                kakusu.Margin = jyougai;
            }
            else
            {
                kakusub.Content = "体重表示";
                (sender as Button).Tag = "true";
                kakusu.Margin = hihyou;
            }
        }
    }
}

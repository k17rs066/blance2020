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
using balance.Views;

namespace balance.Views
{
    /// <summary>
    /// Top.xaml の相互作用ロジック
    /// </summary>
    public partial class Certification : Page
    {
        public delegate void Refresh();

        int ButtonNum = -1; //患者ボタン生成数を表す変数
        int ButtonC = 0; //生成したボタン数のカウント
        int PageNum = 1; //現在のpage位置を把握。。。患者は15人ずつ表示する
        int UnderNum = 0; //page生成数の数
        int UnderButtonCount = 0; //page生成ボタンの数を数える
        int UnderButtonLeft = 150; //page生成ボタンの左側のマージンを計算する際に用いる

        int LeftMargin = 50;
        int TopMargin = 50;
        int ButtonNu = 0;
        String ButtonContent = "";
        String ButtonName = "";
        String LabelContent = "";

        int labelin = 40;
        int labelcount = 0;


        String SQL = "";

        WrapPanel wrapPanel = new WrapPanel();
        WrapPanel ChargeButton = new WrapPanel();
        WrapPanel wrapLabel = new WrapPanel();
        WrapPanel HomeButton = new WrapPanel();


        int buttonlin = 0;

        Button begin = null;
        Button fin = null;


        public Certification()
        {
            InitializeComponent();
            
            if (Application.Current.Properties["PageNum"] != null)
            {
                PageNum = int.Parse(Application.Current.Properties["PageNum"].ToString());
            }


            ////ユーザ数をカウントし、buttonNumに代入
            DBConnect.Connect("kasiihara.db");
            DBConnect.ExecuteReader("SELECT COUNT(*) as num FROM t_user WHERE usertype = '患者' ORDER BY user_id");
            DBConnect.Reader.Read();
            ButtonNum = int.Parse(DBConnect.Reader[0].ToString());
            if (ButtonNum == 0)
            {
                ButtonNum = 1;
            }
            DBConnect.Dispose();
            Button[] button = new Button[ButtonNum + 1];
            Label[] label = new Label[ButtonNum + 1];
            ////
            
            ////自身のページへ遷移するためのボタン作成準備
            UnderNum = (ButtonNum / 14); //ユーザ数をボタン表示数で割る
            if (ButtonNum % 14 != 0) //余りが出る場合はもう一ページ追加する
            {
                UnderNum++;
            }

            Button[] UnderButton = new Button[UnderNum];
            ////

            Thickness margin = new Thickness(LeftMargin, TopMargin, 0, 0);//患者のボタン用のマージン
            Thickness marginl = new Thickness(LeftMargin, TopMargin + labelin, 0, 0);//患者のボタンlabel用のマージン
            Thickness tantou = new Thickness(1530, 850, 0, 0);//担当者のボタン用のマージン

            Thickness home = new Thickness(LeftMargin, 850, 0, 0);//ホーム戻るのボタン用のマージン

            if (UnderNum > 3)
            {
                begin = new Button() { Content = "最初", FontSize = 50, Margin = new Thickness(650 + (UnderButtonLeft * -1), 900, 0, 0), Width = 200, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.Azure };
                fin = new Button() { Content = "最後", FontSize = 50, Margin = new Thickness(750 + (UnderButtonLeft * 3), 900, 0, 0), Width = 200, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.Azure };
                begin.Click += CurrentPage;
                begin.Tag = "begin";
                fin.Click += CurrentPage;
                fin.Tag = "fin";
                
                grid1.Children.Add(begin);
                grid1.Children.Add(fin);
            }

            ////患者のボタンの作成
            DBConnect.Connect("kasiihara.db");
//            DBConnect.ExecuteReader("SELECT * FROM t_user WHERE usertype = '患者' ORDER BY user_id Limit " +(14*(PageNum-1))+",14");
            DBConnect.ExecuteReader("SELECT * FROM t_user WHERE usertype = '患者' ORDER BY user_name_kana Limit " + (14 * (PageNum - 1)) + ",14");

            while (DBConnect.Reader.Read())
            {
                    ButtonContent = DBConnect.Reader[1].ToString();
                    ButtonName = DBConnect.Reader[0].ToString();
                    button[ButtonNu] = new Button() { Content = ButtonContent, FontSize = 50, Margin = margin, Width = 320, Height = 210,HorizontalContentAlignment = HorizontalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top,VerticalContentAlignment=VerticalAlignment.Center, ClickMode = ClickMode.Press, Background = Brushes.Coral };

                    button[ButtonNu].Click += patientTop;
                    button[ButtonNu].Tag = ButtonName;

                LabelContent = DBConnect.Reader[6].ToString();
                label[ButtonNu] = new Label() { Content = LabelContent, FontSize = 25, Margin = marginl, Width = 320, Height = 40, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, HorizontalContentAlignment = HorizontalAlignment.Center };

                labelcount++;
                    if (labelcount == 5)
                    {
                        labelin += 130;
                        marginl = new Thickness(LeftMargin, TopMargin + labelin, 0, 0);//患者のボタンlabel用のマージン
                        labelcount = -10;
                    }
                    
                    wrapPanel.Children.Add(button[ButtonNu]);
                    wrapLabel.Children.Add(label[ButtonNu]);

                    ButtonNu++;
                    ButtonC++;
            }
            DBConnect.Dispose();
            
            ////ゲストのボタンの作成
            button[ButtonNu] = new Button() { Content = "Guest", FontSize = 50,Margin = margin, Width = 320, Height = 210, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.DeepPink };
            button[ButtonNu].Click += patientTop;
            button[ButtonNu].Tag = "guest";

            wrapPanel.Children.Add(button[ButtonNu]);
            grid1.Children.Add(wrapPanel);
            grid1.Children.Add(wrapLabel);
            ////
            ////

            ////担当者のボタンの作成
            Button Charge = new Button() { Content = "担当者", FontSize = 50,Margin = tantou, Width = 320, Height = 150, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top , ClickMode = ClickMode.Press, Background = Brushes.Orange };
            Charge.Click += ShowDialogClicked;

            ChargeButton.Children.Add(Charge);
            grid1.Children.Add(ChargeButton);
            ////

            ////ホームのボタンの作成
            Button Home = new Button() { Content = "ホーム", FontSize = 50, Margin = home, Width = 320, Height = 150, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.Orchid};
            Home.Click += HomeClicked;

            HomeButton.Children.Add(Home);
            grid1.Children.Add(HomeButton);
            ////

            ////ページ遷移用のボタンの作成
            while (UnderButtonCount != UnderNum)
            {
                if (PageNum == 1)
                {
                    if (UnderButtonCount == PageNum - 1 || UnderButtonCount == PageNum || UnderButtonCount == PageNum + 1)
                    {
                        UnderButton[UnderButtonCount] = new Button() { Content = UnderButtonCount + 1, FontSize = 50, Margin = new Thickness(750 + (UnderButtonLeft * buttonlin), 900, 0, 0), Width = 100, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.CadetBlue };
                        UnderButton[UnderButtonCount].Click += CurrentPage;
                        UnderButton[UnderButtonCount].Tag = UnderButtonCount + 1;

                        buttonlin++;

                        grid1.Children.Add(UnderButton[UnderButtonCount]);
                    }
                }
                else if (UnderButtonCount == PageNum-2 || UnderButtonCount == PageNum-1 || UnderButtonCount == PageNum)
                {
                    UnderButton[UnderButtonCount] = new Button() { Content = UnderButtonCount + 1, FontSize = 50, Margin = new Thickness(750 + (UnderButtonLeft * buttonlin), 900, 0, 0), Width = 100, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.CadetBlue};
                    UnderButton[UnderButtonCount].Click += CurrentPage;
                    UnderButton[UnderButtonCount].Tag = UnderButtonCount + 1;

                    buttonlin++;

                    grid1.Children.Add(UnderButton[UnderButtonCount]);
                }
                else if (PageNum == UnderNum)
                {
                    if (UnderButtonCount == PageNum - 3 || UnderButtonCount == PageNum -2 || UnderButtonCount == PageNum -1)
                    {
                        UnderButton[UnderButtonCount] = new Button() { Content = UnderButtonCount + 1, FontSize = 50, Margin = new Thickness(750 + (UnderButtonLeft * buttonlin), 900, 0, 0), Width = 100, Height = 100, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, ClickMode = ClickMode.Press, Background = Brushes.Azure };
                        UnderButton[UnderButtonCount].Click += CurrentPage;
                        UnderButton[UnderButtonCount].Tag = UnderButtonCount + 1;

                        buttonlin++;

                        grid1.Children.Add(UnderButton[UnderButtonCount]);
                    }
                }

                UnderButtonCount++;
            }
            ////

            UnderButton[PageNum-1].Background = Brushes.RoyalBlue;
        }

        /// <summary>
        /// 患者が自分の名前のボタンをクリックすると患者用のトップ画面に遷移する
        /// </summary>
        void patientTop(object sender, EventArgs e)
        {
            Application.Current.Properties["UserName"] = (sender as Button).Content;
            Application.Current.Properties["User_id"] = (sender as Button).Tag;
            var nextPage = new k_Top();
            NavigationService.Navigate(nextPage);
        }

        /// <summary>
        /// 担当者がログインに成功した際に画面を担当者用のトップ画面に遷移する
        /// </summary>
        void t_Click()
        {
            Application.Current.Properties["kname"] = Application.Current.Properties["t_na"].ToString();
            Application.Current.Properties["k_id"] = Application.Current.Properties["t_id"].ToString();
            var nextPage = new t_top();
            NavigationService.Navigate(nextPage);
        }

        /// <summary>
        /// 担当者用のログインフォームを表示する(デリゲートによってt_Clickメソッドが使えるようになっている)
        /// </summary>
        public void ShowDialogClicked(object sender, RoutedEventArgs e)
        {
            Refresh rf = new Refresh(this.t_Click);
            Window w = new LoginForm(rf);
            w.Title = "LoginForm";
            w.ShowDialog();
        }

        /// <summary>
        /// homeのログインフォームを表示する(デリゲートによってt_Clickメソッドが使えるようになっている)
        /// </summary>
        public void HomeClicked(object sender, RoutedEventArgs e)
        {
            var nextPage = new MainTitle();
            NavigationService.Navigate(nextPage);
        }

        /// <summary>
        /// Certification(認証ページ)への遷移する
        /// </summary>
        void CurrentPage(object sender, EventArgs e)
        {
            if ((sender as Button).Tag.Equals("begin"))
            {
                Application.Current.Properties["PageNum"] = 1;
            }
            else if ((sender as Button).Tag.Equals("fin"))
            {
                Application.Current.Properties["PageNum"] = UnderNum;
            }
            else
            {
                Application.Current.Properties["PageNum"] = (sender as Button).Tag;
            }
            var nextPage = new Certification();
            NavigationService.Navigate(nextPage);
        }


    }
}

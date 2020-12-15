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
//using System.Windows.Navigation;

namespace balance.Views
{
    /// <summary>
    /// FTGameResult.xaml の相互作用ロジック
    /// Fall/targetGameの結果を表示
    /// </summary>
    public partial class FTGameResult : Window
    {
        public TargetGame.Refresh_a target_r = null;
        public TargetGame.Refresh_b target_e = null;




        public balance.Views.FallGame.Refresh_fa fall_r = null;
        public balance.Views.FallGame.Refresh_fb fall_e = null;

        public balance.Views.FallGame_Prac.Refresh_fPrac1 fall_rp = null;
        public balance.Views.FallGame_Prac.Refresh_fPrac2 fall_ep = null;

        int cleartime;



        public FTGameResult(balance.Views.TargetGame.Refresh_a pRefresh_a, balance.Views.TargetGame.Refresh_b pRefresh_b)
        {
            InitializeComponent();

            ftgamemode.Content = Application.Current.Properties["ftgamemodename"].ToString();

            target_r = pRefresh_a;
            target_e = pRefresh_b;

            ftresult.Content = Application.Current.Properties["ftresult"].ToString() + " 秒";

            fttokikai.Content = "ターゲット";
            ftresulttime.Content = "5 個";

        }



        public FTGameResult(balance.Views.FallGame_Prac.Refresh_fPrac1 pRefresh_fPrac1, balance.Views.FallGame_Prac.Refresh_fPrac2 pRefresh_fPrac2,double a)
        {

            InitializeComponent();

            ftgamemode.Content = Application.Current.Properties["ftgamemodename"].ToString();

            fall_rp = pRefresh_fPrac1;
            fall_ep = pRefresh_fPrac2;

            ftresult.Content = Application.Current.Properties["ftresult"].ToString() + " 点";

            //練習モードでは表示しない
            //             fttokikai.Content = "計測時間";
            //             ftresulttime.Content = Application.Current.Properties["ftresulttime"].ToString() + " 秒";
        }


        public FTGameResult(balance.Views.FallGame.Refresh_fa pRefresh_fa, balance.Views.FallGame.Refresh_fb pRefresh_fb, int a)
        {

            InitializeComponent();

            ftgamemode.Content = Application.Current.Properties["ftgamemodename"].ToString();

            fall_r = pRefresh_fa;
            fall_e = pRefresh_fb;

            ftresult.Content = Application.Current.Properties["ftresult"].ToString() + " 点";

            cleartime = (int)Application.Current.Properties["ftresulttime"];
            fttokikai.Content = "計測時間";
            ftresulttime.Content = cleartime/60 +"分" + cleartime%60+ " 秒";
        }



        private void end_Click(object sender, RoutedEventArgs e)
        {

            if (Application.Current.Properties["ftgamemodename"].ToString().Equals("ターゲットゲーム"))
            {
                target_e(sender, e);
                this.Close();
            }
            else if (Application.Current.Properties["ftgamemodename"].ToString().Equals("落下ゲーム"))
            {
                fall_e(sender, e);
                this.Close();
            }
            else if (Application.Current.Properties["ftgamemodename"].ToString().Equals("落下ゲーム(練習モード)"))
            {
                fall_ep (sender, e);
                this.Close();
            }
            

                
        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["ftgamemodename"].ToString().Equals("ターゲットゲーム"))
            {
                target_r(sender, e);
                this.Close();
            }
            else if (Application.Current.Properties["ftgamemodename"].ToString().Equals("落下ゲーム"))
            {
                fall_r(sender, e);
                this.Close();
            }
            else if (Application.Current.Properties["ftgamemodename"].ToString().Equals("落下ゲーム(練習モード)"))
            {
                fall_rp(sender, e);
                this.Close();
            }

        }
        
    }
}

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
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace balance.Views
{
    /// <summary>
    /// GameResult.xaml の相互作用ロジック
    /// </summary>
    public partial class GameResult : Window
    {
        public balance.Views.GameMode.Refresh rf = null;
        public balance.Views.GameMode.Refresh1 rf1 = null;
        public balance.Views.GameMode.Refresh2 rf2 = null;

        public GameResult(balance.Views.GameMode.Refresh pRefresh, balance.Views.GameMode.Refresh1 pRefresh1, balance.Views.GameMode.Refresh2 pRefresh2)
        {
            InitializeComponent();
            gamemode.Content = Application.Current.Properties["gamemodename"].ToString();
            resultline.Content = Application.Current.Properties["line"].ToString() + " ％";

            rf = pRefresh;
            rf1 = pRefresh1;
            rf2 = pRefresh2;


            //gamemode.Content = "タイムアタック";
             //resultline.Content = "60％";
            if (Application.Current.Properties["gamemodename"] == null)
            {
                     tokikai.Content = "計測時間";
                     resulttime.Content = "10秒";
                     resultscore.Content = "10回";
            }
            else if (Application.Current.Properties["gamemodename"].ToString().Equals("スコアアタック"))
            {
                tokikai.Content = "計測時間";
                resulttime.Content = Application.Current.Properties["sette"].ToString() + " 秒";
                resultscore.Content = Application.Current.Properties["Count"].ToString() + " 回";
            }
            else if (Application.Current.Properties["gamemodename"].ToString().Equals("タイムアタック"))
            {
                tokikai.Content = "計測回数";
                resulttime.Content = Application.Current.Properties["sette"].ToString() + " 回";
                //resultscore.Content = Application.Current.Properties["sw.Elapsed.Second"].ToString() + "秒";
                resultscore.Content = Application.Current.Properties["time_t"].ToString() + " 秒";
            }

        }


        private void end_Click(object sender, RoutedEventArgs e)
        {
            rf1(sender,e);

            this.Close();

        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            rf2(sender,e);

            if (Application.Current.Properties["gamemodename"].ToString().Equals("スコアアタック"))
            {
                rf(0);
            }
            else if(Application.Current.Properties["gamemodename"].ToString().Equals("タイムアタック"))
            {
                rf(1);
            }
            

            this.Close();


        }

    }

}

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



namespace balance.Views
{
    /// <summary>
    /// FTGameModeSelect.xaml の相互作用ロジック
    /// </summary>
    public partial class FallGameModeSelect: Page
    {

        int ballsize = 0;
        double ballspeed = 0;
        int time = 0;
        public FallGameModeSelect()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0,0,1);
            timer.Start();

        }

        private void Precmode_Click(object sender,EventArgs e)
        {
            var nextPage = new FallGame_Prac();
            NavigationService.Navigate(nextPage);

        }

         void timer_Tick(object sender, EventArgs e)
        {
            datetimeb.Text = DateTime.Now.ToString();
        }

        private void FallGame_Easy(object sender, RoutedEventArgs e)
        {
            var nextPage = new FallGame_Easy();
            NavigationService.Navigate(nextPage);
        }

        private void FallGame_Normal(object sender, RoutedEventArgs e)
        {
            var nextPage = new FallGame_Normal();
            NavigationService.Navigate(nextPage);
        }
        private void FallGame_Hard(object sender, RoutedEventArgs e)
        {
            var nextPage = new FallGame_Hard();
            NavigationService.Navigate(nextPage);
        }
        private void back(object sender, RoutedEventArgs e)
        {
            var nextPage = new GameSelect();
            NavigationService.Navigate(nextPage);
        }

        private void FallGameMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (FallGameMode.Text=="通常モード")
            {
                TimeSet_Label.Visibility = Visibility.Hidden;
                MinSet.Visibility = Visibility.Hidden;
                Min_Label.Visibility = Visibility.Hidden;
                SecSet.Visibility = Visibility.Hidden;
                Sec_Label.Visibility = Visibility.Hidden;
            }
            else if(FallGameMode.Text=="練習モード")
            {
                TimeSet_Label.Visibility = Visibility.Visible;
                MinSet.Visibility = Visibility.Visible;
                Min_Label.Visibility = Visibility.Visible;
                SecSet.Visibility = Visibility.Visible;
                Sec_Label.Visibility = Visibility.Visible;


            }
            else
            {

            }


        }

        private void BallSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BallSpeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MinSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SecSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Deside_Click(object sender, RoutedEventArgs e)
        {
            if (FallGameMode.Text.Equals("通常モード"))
            {
                int min = int.Parse(MinSet.Text);
                int sec = int.Parse(SecSet.Text);
                time =( min * 60) + sec;

                Application.Current.Properties["ballsize"] = ballsize;
                Application.Current.Properties["ballspeed"] = ballspeed;
                Application.Current.Properties["timeset"] = time;

                var nextPage = new FallGame();
                NavigationService.Navigate(nextPage);
            }
            else
            {
                Application.Current.Properties["ballsize"] = ballsize;
                Application.Current.Properties["ballspeed"] = ballspeed;
                Application.Current.Properties["timeset"] = 0;

                var nextPage = new FallGame_Prac();
                NavigationService.Navigate(nextPage);
            }
        }
    }
}

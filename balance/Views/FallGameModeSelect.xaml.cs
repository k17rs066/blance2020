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
    }
}

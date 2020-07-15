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
    public partial class FTGameModeSelect: Page
    {
        public FTGameModeSelect()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0,0,1);
            timer.Start();

        }

         void timer_Tick(object sender, EventArgs e)
        {
            datetimeb.Text = DateTime.Now.ToString();
        }

        private void FTGame_Easy(object sender, RoutedEventArgs e)
        {
            var nextPage = new FTGame_Easy();
            NavigationService.Navigate(nextPage);
        }

        private void FTGame_Normal(object sender, RoutedEventArgs e)
        {
            var nextPage = new FTGame_Normal();
            NavigationService.Navigate(nextPage);
        }
        private void FTGame_Hard(object sender, RoutedEventArgs e)
        {
            var nextPage = new FTGame_Hard();
            NavigationService.Navigate(nextPage);
        }
        private void back(object sender, RoutedEventArgs e)
        {
            var nextPage = new GameSelect();
            NavigationService.Navigate(nextPage);
        }
    }
}

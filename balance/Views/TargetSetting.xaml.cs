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

namespace balance.Views
{
    /// <summary>
    /// TargetSetting.xaml の相互作用ロジック
    /// </summary>
    public partial class TargetSetting : Page
    {
        public TargetSetting()
        {
            InitializeComponent();

            /*Mode.Items.Add("通常モード");
            Mode.Items.Add("練習モード");

            Mode.SelectedIndex = 0;*/

            Placement.Items.Add("パターン1");
            Placement.Items.Add("パターン2");
            Placement.Items.Add("パターン3");

            Placement.SelectedIndex = 0;
        }

        private void back(object sender,RoutedEventArgs e)
        {
            var nextPage = new GameSelect();
            NavigationService.Navigate(nextPage);
        }

        private void decision(object sender, RoutedEventArgs e)
        {
                if (Placement.Text == "パターン1")
                {
                    var nextPage = new TargetGame();
                    NavigationService.Navigate(nextPage);
                }
                else if (Placement.Text == "パターン2")
                {
                    var nextPage = new TargetGame_p2();
                    NavigationService.Navigate(nextPage);
                }
                else if (Placement.Text == "パターン3")
                {
                    var nextPage = new TargetGame_p3();
                    NavigationService.Navigate(nextPage);
                }
                else
                {
                    var nextPage = new TargetGame();
                    NavigationService.Navigate(nextPage);
                }
         }

    }
}

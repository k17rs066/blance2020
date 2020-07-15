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
    /// MainTitle.xaml の相互作用ロジック
    /// </summary>
    public partial class MainTitle : Page
    {

        public MainTitle()
        {
            InitializeComponent();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var nextPage = new Certification();
            NavigationService.Navigate(nextPage);
        }
    }
}

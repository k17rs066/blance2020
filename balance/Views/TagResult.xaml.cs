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

namespace balance.Views
{
    /// <summary>
    /// TagResult.xaml の相互作用ロジック
    /// </summary>
    /// 


    public partial class TagResult : Window
    {
        public GameOfTag.TagRestart RePlay_Tag = null;
        public GameOfTag.TagEnd End_Tag = null;
        public TagResult(GameOfTag.TagRestart tagRestart,GameOfTag.TagEnd tagEnd)
        {
            InitializeComponent();

            RePlay_Tag = tagRestart;
            End_Tag = tagEnd;

            Result.Content = Application.Current.Properties["GameResult"].ToString();

            TagSpeed2.Content = Application.Current.Properties["TagSpeed"].ToString();
            TimeResult2.Content = Application.Current.Properties["GameResultTime"].ToString()+"秒"; 
        }

        private void RePlayGame_Click(object sender, RoutedEventArgs e)
        {
            RePlay_Tag(sender, e);
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            End_Tag(sender, e);
            this.Close();
        }
    }
}

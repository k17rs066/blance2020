
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

using balance.DataBase;

namespace balance.Views
{
    /// <summary>
    /// User_Delete.xaml の相互作用ロジック
    /// </summary>
    public partial class User_Delete : Window
    {
        public balance.Views.UserManagiment.Refresh5 seni = null;

        public User_Delete(balance.Views.UserManagiment.Refresh5 pRefresh5)
        {
            InitializeComponent();

            deletename.Content = Application.Current.Properties["deletename"].ToString();
            deletetkananame.Content = Application.Current.Properties["deletekananame"].ToString();
            deletetype.Content = Application.Current.Properties["deletetype"].ToString();

            seni = pRefresh5;

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            String SQL;
            DBConnect.Connect("kasiihara.db");
            SQL = "DELETE FROM t_user WHERE user_id = '" + int.Parse(Application.Current.Properties["deleteid"].ToString()) + "'";
            DBConnect.ExecuteReader(SQL);
            DBConnect.Dispose();

            //送信するデータ格納
            Application.Current.Properties["henshuu"] = "3";
            Application.Current.Properties["rename"] = deletename.Content;
            Application.Current.Properties["reid"] = Application.Current.Properties["deleteid"].ToString();
            Application.Current.Properties["rename_kana"] = deletetkananame.Content;
            Application.Current.Properties["rename_type"] = deletetype.Content;

            seni(sender, e);

            this.Close();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

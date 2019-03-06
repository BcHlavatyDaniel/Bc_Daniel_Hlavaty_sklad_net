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
using System.Data;

namespace Bakalaris
{
    /// <summary>
    /// Interaction logic for AWindow.xaml
    /// </summary>
    public partial class AWindow : Window
    {
        DatabaseActions mDatabaseAction;

        public AWindow()
        {
            InitializeComponent();
            mDatabaseAction = new DatabaseActions();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string name = FirstName.GetLineText(0); //TO DO checks, if not kewl dont close windown, just reset
            string sName = SecondName.GetLineText(0);
            string stat = Stat.GetLineText(0);
            char type = '0';
            if (Typ1.IsChecked == true) type = '1';
            else if (Typ2.IsChecked == true) type = '2';
            else if (Typ3.IsChecked == true) type = '3';
            bool skladom;
            if (Skladom.IsChecked == true) skladom = true;
            else skladom = false;
            DateTime arrivalStr = DateTime.Parse(DateArrival.ToString());
            DateTime leaveStr = DateTime.Parse(DateLeave.ToString());
            mDatabaseAction.AddData(name, sName, stat, skladom, type, arrivalStr, leaveStr);

            MainWindow mMainWindow = new MainWindow();
            mMainWindow.Show();
            this.Close();
        }
    }

}

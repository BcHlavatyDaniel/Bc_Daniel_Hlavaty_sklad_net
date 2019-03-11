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
            Init();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mMainWindow = new MainWindow();
            mMainWindow.Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string name = FirstNameCmb.Text.ToString();
            string sName = SecondNameCmb.Text.ToString();

            bool checker = true;

            if (name.Trim() == "")
            {
                setWarningToBlock(FirstNameCmbBorder, FirstNameWarnBlock, true);
                checker = false;
            }
            else setWarningToBlock(FirstNameCmbBorder, FirstNameWarnBlock, false);


            if (sName.Trim() == "")
            {
                setWarningToBlock(SecondNameCmbBorder, SecondNameWarnBlock, true);
                checker = false;
            }
            else setWarningToBlock(SecondNameCmbBorder, SecondNameWarnBlock, false);

            string stat = Stat.GetLineText(0);

            if (stat.Trim() == "" || stat.Trim() == "0" || int.TryParse(stat.Trim(), out int n))
            {
                setWarningToBlock(StatBorder, StatWarnBlock, true);
                checker = false;
            }
            else setWarningToBlock(StatBorder, StatWarnBlock, false);

            char type = '0';
            if (Typ1.IsChecked == true) type = '1';
            else if (Typ2.IsChecked == true) type = '2';
            else if (Typ3.IsChecked == true) type = '3';

            if (type == '0')
            {
                setWarningToBlock(TypeBorder1, TypeWarnBlock, true);
                setWarningToBlock(TypeBorder2, TypeWarnBlock, true);
                setWarningToBlock(TypeBorder3, TypeWarnBlock, true);
                checker = false;
            }
            else
            {
                setWarningToBlock(TypeBorder1, TypeWarnBlock, false);
                setWarningToBlock(TypeBorder2, TypeWarnBlock, false);
                setWarningToBlock(TypeBorder3, TypeWarnBlock, false);
            }

            bool skladom;
            if (Skladom.IsChecked == true) skladom = true;
            else skladom = false;

            DateTime arrivalStr;
            if (!DateTime.TryParse(DateArrival.ToString(), out arrivalStr))
            {
                setWarningToBlock(DateArrivalBorder, DateArrivalWarnBlock, true);
                checker = false;
            }
            else
            {
                setWarningToBlock(DateArrivalBorder, DateArrivalWarnBlock, false);
            }

            DateTime leaveStr;
            if (!DateTime.TryParse(DateLeave.ToString(), out leaveStr))
            {
                setWarningToBlock(DateLeaveBorder, DateLeaveWarnBlock, true);
                checker = false;
            }
            else
            {
                setWarningToBlock(DateLeaveBorder, DateLeaveWarnBlock, false);
            }

            if (!checker)
                return;

            mDatabaseAction.AddData(name, sName, stat, skladom, type, arrivalStr, leaveStr);

            MainWindow mMainWindow = new MainWindow();
            mMainWindow.Show();
            this.Close();
        }

        private void setWarningToBlock(Border border, TextBlock textBlock, bool isSet)
        {
            if (isSet)
            {
                textBlock.Text = "Doplnte prosim policko";
                border.BorderBrush = Brushes.Red;
            } else
            {
                textBlock.Text = "";
                border.ClearValue(Border.BorderBrushProperty);
            }
            
        }

        private void Init()
        {
            mDatabaseAction = new DatabaseActions();
            FirstNameCmb.IsEditable = true;
            SecondNameCmb.IsEditable = true;

            DataSet data = mDatabaseAction.LoadData();
            DataTable dataTable = data.Tables[0];
            
            foreach(DataRow row in dataTable.Rows)
            {
                if (!FirstNameCmb.Items.Contains(row["FirstName"].ToString()))
                    FirstNameCmb.Items.Add(row["FirstName"].ToString());
                if (!SecondNameCmb.Items.Contains(row["SecondName"].ToString()))
                    SecondNameCmb.Items.Add(row["SecondName"].ToString());
            }

        }
    }

}

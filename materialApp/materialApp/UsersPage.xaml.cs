//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Graphics;
using System;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.IO;
using System.Net;
using DatabaseProj;

namespace materialApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class UsersPage : Page
    {
        private DbActions mDbActions;

        public UsersPage(DbActions mDbActions)
        {
            InitializeComponent();
			this.mDbActions = mDbActions;
        }
        private void Datagrid_Cmb_Update(object sender, RoutedEventArgs e)
		{ }
        public void ModalUserAddInit(object sender, RoutedEventArgs e)
		{ }
        private void Profile_Open(object sender, RoutedEventArgs e)
		{ }
        private void Search(object sender, SelectionChangedEventArgs e)
		{ }
    }
}



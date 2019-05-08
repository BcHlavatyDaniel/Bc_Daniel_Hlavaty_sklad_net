//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Graphics;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Data;
using Emgu.CV;
using Emgu.CV.UI;
using DatabaseProj;

namespace materialApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 

	public partial class MainWindow : Window
    {
        bool hamClosed = true;
		private DbActions mDbActions;
        VideoCapture mCapture = new VideoCapture();
        ImageViewer mViewer = new ImageViewer();

		enum mState { Einit_state, Esold_card, Esold_cash, Ereturned, Epaid_card, Epaid_cash, Earchived };

        public MainWindow()
        {
            InitializeComponent();
        }
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
            mDbActions = new DbActions();
		}
        private void Open_Hamburger(object sender, RoutedEventArgs e)
        {
            if (hamClosed)
            {
                Storyboard board = this.FindResource("OpenMenu") as Storyboard;
                board.Begin();
            }
            else
            {
                Storyboard board = this.FindResource("CloseMenu") as Storyboard;
                board.Begin();
            }
            hamClosed = !hamClosed;
        }
        private void Users_Open(object sender, RoutedEventArgs e)
        {
            Content.Content = new UsersPage(mDbActions, mViewer, mCapture);
        }

        private void Items_Open(object sender, RoutedEventArgs e)
        {
            Content.Content = new ItemsPage(mDbActions, mViewer, mCapture);
        }

        private void Log_Open(object sender, RoutedEventArgs e)
        {
            Content.Content = new LogPage(mDbActions, mViewer, mCapture);
        }
    }
}
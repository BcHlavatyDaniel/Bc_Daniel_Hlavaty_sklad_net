//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Graphics;
using DatabaseProj;
using System.Windows;
using System.Windows.Controls;

namespace materialApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 

	public partial class ItemsPage : Page
	{
		
        private DbActions mDbActions;

        public ItemsPage(DbActions mDbActions)
        {
            InitializeComponent();
			this.mDbActions = mDbActions;
		}

		private void Item_Open(object sender, RoutedEventArgs e)
		{
		}
		private void Profile_Item_Open(object sender, RoutedEventArgs e)
		{

		}

		private void SearchItems(object sender, RoutedEventArgs e)
		{
		}
	}
}


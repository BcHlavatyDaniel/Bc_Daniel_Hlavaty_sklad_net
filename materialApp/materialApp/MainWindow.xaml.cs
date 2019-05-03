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

/*      private void Open_Description(object sender, RoutedEventArgs e) //DUPLICATE daff aside
      {

          int index = itemsDataGrid.SelectedIndex;
          DataGridRow gridRow = (DataGridRow)itemsDataGrid.ItemContainerGenerator.ContainerFromItem(itemsDataGrid.SelectedItem);
          DataRowView rowView = (DataRowView)itemsDataGrid.SelectedItem;

          string id = rowView.Row.ItemArray[3].ToString();
          if (mVisibleList.Contains(index))
          {
              gridRow.DetailsVisibility = Visibility.Collapsed;
              DataGridDetailsPresenter presenter = CommonActions.FindVisualChild<DataGridDetailsPresenter>(gridRow);
              presenter.ApplyTemplate();
              var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
              mDbActions.LoadSaveSpecificItemDescription(id, true, textbox.Text);
              mVisibleList.Remove(index);
          }
          else
          {
              string desc = mDbActions.LoadSaveSpecificItemDescription(id, false, "");
              mVisibleList.Add(index);
              DataGridDetailsPresenter presenter = CommonActions.FindVisualChild<DataGridDetailsPresenter>(gridRow);// FindVisualChild<DataGridDetailsPresenter>(gridRow);
              presenter.ApplyTemplate();
              var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
              textbox.Text = desc;
              gridRow.DetailsVisibility = Visibility.Visible;
          }
      }*/

/*     private void Save_Description(object sender, RoutedEventArgs e)
     {
         int index = itemsDataGrid.SelectedIndex;
         DataGridRow gridRow = (DataGridRow)itemsDataGrid.ItemContainerGenerator.ContainerFromItem(itemsDataGrid.SelectedItem);
         DataRowView rowView = (DataRowView)itemsDataGrid.SelectedItem;

         string id = rowView.Row.ItemArray[3].ToString();
         DataGridDetailsPresenter presenter = CommonActions.FindVisualChild<DataGridDetailsPresenter>(gridRow);// FindVisualChild<DataGridDetailsPresenter>(gridRow);
         presenter.ApplyTemplate();
         var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
         mDbActions.LoadSaveSpecificItemDescription(id, true, textbox.Text);
     }
*/

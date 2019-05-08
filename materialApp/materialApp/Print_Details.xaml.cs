using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Spire.Pdf;
using System.Drawing.Printing;
using System.Management;

namespace materialApp
{
    /// <summary>
    /// Interaction logic for Print_Details.xaml
    /// </summary>
    public partial class Print_Details : Window
    {
        PdfDocument mPdf;
        public Print_Details(PdfDocument pdf)
        {
            InitializeComponent();
            mPdf = pdf;
            string path = System.IO.Directory.GetCurrentDirectory();
          //  path = path.Substring(path.Length - 11);
            path += "\\doesitwork.pdf";
            //  pdfShow.Source = new Uri(path, UriKind.Absolute);
            pdfShow.Navigate(new Uri(path, UriKind.Absolute));
        }

        private void Ok_print(object sender, RoutedEventArgs e)
        {
            PrinterSettings settings = new PrinterSettings();
            string printerName = settings.PrinterName;
            if (printerName == null || printerName == "")
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
                foreach (ManagementObject printer in searcher.Get())
                {
                    printerName = printer["Name"].ToString();
                    string availability = printer["Availability"].ToString();
                    //ak je available zober ho not sure ako to bude vyzerat tho :D :D
                }
            }
            else
            {
                mPdf.PrintSettings.PrinterName = printerName;
                //if virtual -> pdf.PrintSettings.PrintToFile("PrintToXps.xps");
                mPdf.Print();
            }
            AsyncClose();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void AsyncClose()
        {
            await Task.Delay(1000);
            this.Close();
        }
    }
}

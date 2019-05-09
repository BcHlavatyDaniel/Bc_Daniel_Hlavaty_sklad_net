using System;
using System.Threading.Tasks;
using System.Windows;
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
            path += "\\doesitwork.pdf";
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
                }
            }
            else
            {
                mPdf.PrintSettings.PrinterName = printerName;
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

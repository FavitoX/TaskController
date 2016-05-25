using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskController
{
    /// <summary>
    /// Interaction logic for wExportDialog.xaml
    /// </summary>
    public partial class wExportDialog : Window
    {
        public wExportDialog()
        {
            InitializeComponent();
        }

        public string FileName { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog sdialog = new System.Windows.Forms.SaveFileDialog();
            sdialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            sdialog.Title = " .: Exportar datos :. ";
            sdialog.ShowDialog();

            if (!sdialog.FileName.Equals(string.Empty))
            {
                if (!this.dtStartDate.SelectedDate.HasValue)
                {
                    MessageBox.Show("Seleccione la fecha inicial", " .: Task Controller :. ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!this.dtEndDate.SelectedDate.HasValue)
                {
                    MessageBox.Show("Seleccione la fecha final", " .: Task Controller :. ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                this.EndDate = this.dtEndDate.SelectedDate.Value;
                this.StartDate = this.dtStartDate.SelectedDate.Value;
                this.FileName = sdialog.FileName;
                this.DialogResult = true;
                this.Close();
            }
            else
                MessageBox.Show("Seleccione la ubicación", " .: Task Controller :. ", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

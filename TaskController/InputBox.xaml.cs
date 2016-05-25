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

namespace TaskController
{
    /// <summary>
    /// Lógica de interacción para InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public string TextInput { get; private set; }

        public InputBox()
        {
            InitializeComponent();
            TextInput = string.Empty;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            TextInput = string.Empty;
            this.DialogResult = false;
            this.Close();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            TextInput = this.txtText.Text.Trim();
            this.DialogResult = true;

            this.Close();
        }
    }
}

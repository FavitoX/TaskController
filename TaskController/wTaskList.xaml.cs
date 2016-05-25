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
    /// Lógica de interacción para wTaskList.xaml
    /// </summary>
    public partial class wTaskList : Window
    {
        public UTask SelectedTask { get; private set; }
        public UTask[] RunningTask { private get; set; }

        public wTaskList()
        {
            InitializeComponent();
        }


        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.lstTaskView.SelectedItem != null)
            {
                this.SelectedTask = (this.lstTaskView.SelectedItem as UTask);
                this.DialogResult = true;
            }

            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.lstTaskView.ItemsSource = this.RunningTask;
        }
    }
}

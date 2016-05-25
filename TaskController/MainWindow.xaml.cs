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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskController
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon = null;
        private TaskManager _tskManager = new TaskManager();

        public MainWindow()
        {
            InitializeComponent();

            double opacity;

            if (!double.TryParse(System.Configuration.ConfigurationManager.AppSettings["opacity"], out opacity))
                opacity = 1;

            this.Opacity = opacity;

            this._notifyIcon = new System.Windows.Forms.NotifyIcon();
            this._notifyIcon.Click += new EventHandler(notifyIcon_Click);
            this._notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
            this._notifyIcon.Icon = new System.Drawing.Icon(@"notification_icon.ico");
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea cerrar la aplicación?", " .: Task Controller :. ", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this._tskManager.SavePendingTasks();

                this._notifyIcon.Visible = false;
                this._notifyIcon.Dispose();

                this.Close();
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            if (this.IsVisible)
                this.Hide();
            else
                this.Show();
            // this.Topmost = false;
        }

        private void btnStartTask_Click(object sender, RoutedEventArgs e)
        {
            btnStartTask.IsEnabled = false;

            /* Mostramos la ventana para cargar los datos */
            InputBox ib = new InputBox();
            bool? retValue = ib.ShowDialog();

            /* Verificamos si hay que cargar la tarea */
            if ((retValue.HasValue) && (retValue.Value))
                this._tskManager.AddTask(new UTask(ib.TextInput));

            ib = null;
            btnStartTask.IsEnabled = true;
        }

        private void btnEndTask_Click(object sender, RoutedEventArgs e)
        {
            btnEndTask.IsEnabled = false;

            UTask[] tasks = this._tskManager.RunnigTasks;
            if (tasks.Length > 0)
            {
                wTaskList tskList = new wTaskList();

                tskList.RunningTask = tasks;
                bool? retValue = tskList.ShowDialog();

                /* Verificamos si hay que cargar la tarea */
                if ((retValue.HasValue) && (retValue.Value))
                    this._tskManager.EndTask(tskList.SelectedTask);

                tskList = null;
            }
            else
                MessageBox.Show("No tasks..");

            btnEndTask.IsEnabled = true;
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea exportar datos?", " .: Task Controller :. ", MessageBoxButton.YesNo, MessageBoxImage.Hand);

            if (result == MessageBoxResult.Yes)
            {
                wExportDialog wed = new wExportDialog();
                bool? wresult = wed.ShowDialog();

                if ((wresult.HasValue) && (wresult.Value == true))
                {
                    if(this._tskManager.Export(wed.FileName, wed.StartDate, wed.EndDate))
                        MessageBox.Show("Se guardaron los datos.", " .: Task Controller :. ", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("No fue posible exportar los datos.", " .: Task Controller :. ", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._notifyIcon.Visible = true;
        }
    }
}

using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskController
{
    class TaskManager
    {
        private Dictionary<long, UTask> _tasks = new Dictionary<long, UTask>();
        private DbManager _dbmanager = new DbManager();

        public UTask[] RunnigTasks
        {
            get
            {
                return (from r in this._tasks.Values
                        where r.End.HasValue == false
                        select r).ToArray<UTask>();
            }
        }

        public static long IdFactory()
        {
            return DateTime.Now.ToBinary();
        }

        public void AddTask(UTask task)
        {
            /* Iniciamos la tarea y la agregamos a la lista de tareas pendientes */
            task.StartTask();
            this._tasks.Add(task.ID, task);
        }

        public void EndTask(UTask task)
        {
            /* Finalizamos la tarea */
            task.EndTask();

            /* Guardamos la tarea finalizada */
            this._dbmanager.SaveTask(task);

            /* Quitamos la tarea finalizada de memoria */
            this._tasks.Remove(task.ID);
        }

        public bool Export(string fileName, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataTable tasks = this._dbmanager.ReadTasks(startDate, endDate);

                using (XLWorkbook wb = new XLWorkbook())
                {
                    IXLWorksheet ws = wb.Worksheets.Add(tasks);
                    ws.Name = string.Format("Tasks {0} - {1}", startDate.ToString("mmDDyy"), endDate.ToString("mmDDyy"));
                    wb.SaveAs(fileName);
                }
                return true;
            }
            catch { }

            return false;
        }

        internal void SavePendingTasks()
        {
            foreach (UTask ut in this._tasks.Values)
            {
                if(!ut.End.HasValue)
                    this.EndTask(ut);
            }
        }
    }
}

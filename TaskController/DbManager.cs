using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskController
{
    class DbManager
    {
        private string _ConnectionString = ConfigurationManager.ConnectionStrings["taskrepo"].ConnectionString;

        private const string SQL_SELECT = "SELECT [TaskId],[Description],[Start],[End] FROM [History]";
        private const string SQL_SELECT_BY_DATE = "SELECT [Description],[Start],[End] FROM [History] WHERE Start BETWEEN @startDate AND @endDate";
        private const string SQL_INSERT = "INSERT INTO [History] ([TaskId],[Description],[Start],[End]) VALUES (@TaskId, @description, @start, @end)";

        public void SaveTask(UTask task)
        {
            if (task == null)
                return;

            using(SqlCeConnection conn = new SqlCeConnection(this._ConnectionString))
            {
                using (SqlCeCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = SQL_INSERT;
                    cmd.Parameters.AddWithValue("@TaskId", task.ID);
                    cmd.Parameters.AddWithValue("@description", task.Description);
                    cmd.Parameters.AddWithValue("@start", task.Start);
                    cmd.Parameters.AddWithValue("@end", task.End);
                    conn.Open();

                    if (conn.State == ConnectionState.Open)
                    {
                        if (cmd.ExecuteNonQuery() != 1)
                            throw new InvalidOperationException("Cannot save task.");
                    }
                    else
                        throw new InvalidOperationException("Cannot save task, disconnected.");

                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        public DataTable ReadTasks(DateTime startDate, DateTime endDate)
        {
            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(SQL_SELECT_BY_DATE, this._ConnectionString))
            {
                DataSet ds = new DataSet();
                adapter.SelectCommand.Parameters.AddWithValue("@startDate", startDate);
                adapter.SelectCommand.Parameters.AddWithValue("@endDate", endDate);
                adapter.Fill(ds);

                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
            }

            return new DataTable();
        }

        public UTask[] ReadTasks()
        {
            List<UTask> tasks = new List<UTask>();
            using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(SQL_SELECT, this._ConnectionString))
            {
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count > 0)
                    foreach (DataRow r in ds.Tables[0].Rows)
                        tasks.Add(new UTask(r));

                return tasks.ToArray<UTask>();
            }
        }
    }
}

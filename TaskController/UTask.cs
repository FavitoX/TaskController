using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskController
{
    public class UTask
    {
        public long ID { get; private set; }
        public string Description { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime? End { get; private set; }

        internal UTask(DataRow row)
        {
            this.ID = row.Field<long>("TaskId");
            this.Description = row.Field<string>("Description");
            this.Start = row.Field<DateTime>("Start");

            if (row["End"] != DBNull.Value)
                this.End = row.Field<DateTime>("End");
            else
                this.End = null;
        }

        public UTask(string description)
        {
            this.ID = TaskManager.IdFactory();
            this.Description = description;
        }

        public void StartTask()
        {
            this.Start = DateTime.Now;
        }

        public void EndTask()
        {
            if(!this.End.HasValue)
                this.End = DateTime.Now;
        }
    }
}

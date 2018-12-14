using System;

namespace TaskManager.Model
{
    public class TaskDetails
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

       public bool EndTask { get; set; }

        public int? ParentId { get; set; }
    }
}

using System;

namespace TaskManager.Model
{
    public class TaskDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

       public bool EndTask { get; set; }

        public int? ParentId { get; set; }
    }
}

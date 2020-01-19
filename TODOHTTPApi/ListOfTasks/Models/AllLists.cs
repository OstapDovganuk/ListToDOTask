using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListOfTasks.Models
{
    public static class AllLists
    {
        public static List<ToDoList> _AllLists = new List<ToDoList>();
        public static string _currentList;
        public static ToDoList all_tasks = new ToDoList();
        public static ToDoList planned_tasks = new ToDoList();
        public static ToDoList important_tasks = new ToDoList();
        public static ToDoList today_tasks = new ToDoList();
        public static bool first = true;
    }
}

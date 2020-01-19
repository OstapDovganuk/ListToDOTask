using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi;

namespace ListOfTasks.Models
{
    public class ToDoList
    {
        [Required(ErrorMessage ="Name must be set")]
        public string Name { get; set; }
        public List<ToDoTask> _ToDoList { get; set; }
        public ToDoList()
        {
            _ToDoList = new List<ToDoTask>();
        }
    }
}

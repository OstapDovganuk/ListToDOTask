using System.ComponentModel.DataAnnotations;
namespace TODOHTTPApi.Models
{
    public class ToDoTask
    {
        [Key]
        public int ToDoTaskId { get; set; }
        [Required(ErrorMessage ="Title must be set")]
        public string title { get; set; }
        [Required(ErrorMessage = "Description must be set")]
        public string description { get; set; }
        public Importance taskimportance { get; set; } = Importance.normal;
        [DataType(DataType.Date, ErrorMessage ="It must be date")]
        public string date { get; set; }
        public bool isactive { get; set; } = false;
        [Required]
        public string TaskListId { get; set; }
        public string CreateDate { get; set; }
        public bool IsMultipleTask { get; set; } = false;
        public bool IsDeleted { get; set; }
    }
}

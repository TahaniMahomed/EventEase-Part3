using System.ComponentModel.DataAnnotations;

namespace Event_Ease2.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeID { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string EventTypeName { get; set; } // e.g., "Conference", "Wedding", "Concert"
    }
}
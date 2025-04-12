using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; } 

        [Required]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

    }
}

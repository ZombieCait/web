using System.ComponentModel.DataAnnotations;


namespace lab_01.Models
{
    public class MessageViewModel
    {
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        [Required]
        [StringLength(2000)]
        public string Text { get; set; }
    }
}

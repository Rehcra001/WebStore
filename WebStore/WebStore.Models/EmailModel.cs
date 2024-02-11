using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        public string To { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;
    }
}

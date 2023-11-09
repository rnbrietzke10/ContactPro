using System.ComponentModel.DataAnnotations;

namespace ContactPro.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? AppUserId { get; set; }

        [Required]
        [Display(Name ="Category Name")]
        public string? Name { get; set;}

        // Virtual property used to set up foreign key
        public virtual AppUser? AppUser { get; set; }
        public virtual ICollection<Contact> Contacts { get; } = new HashSet<Contact>();


    }
}

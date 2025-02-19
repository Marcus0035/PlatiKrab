using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlatiKrab.Data.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public bool Active { get; set; } = true;

        public List<Training> Trainings { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
    }
}

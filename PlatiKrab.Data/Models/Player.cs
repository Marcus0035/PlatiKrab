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

        // Vazební tabulky
        public List<PlayerTraining> PlayerTrainings { get; set; } = new();
        public List<PaymentPlayer> PaymentPlayers { get; set; } = new();
    }
}

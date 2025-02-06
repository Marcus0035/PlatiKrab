using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlatiKrab.Data.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public bool Paid { get; set; }

        // Trénink, za který je platba
        public int TrainingId { get; set; }
        public Training Training { get; set; } = null!;

        // Vazební tabulka pro hráče
        public Player PlayerWhoPays { get; set; } = new();
    }
}

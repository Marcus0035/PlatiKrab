using System.ComponentModel.DataAnnotations;

namespace PlatiKrab.Data.Models
{
    public class PaymentPlayer
    {
        [Key]
        public int PaymentPlayerId { get; set; }

        // FK na platbu
        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;

        // FK na hráče (platbu může provádět více hráčů)
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlatiKrab.Data.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public bool Paid { get; set; }

        public int TrainingId { get; set; }
        public Training Training { get; set; } = new();

        public int? PlayerId { get; set; }
        public Player? Player { get; set; } = new();

        public Payment(bool paid, int trainingId, Training training, int playerId, Player player)
        {
            Paid = paid;
            TrainingId = trainingId;
            Training = training;
            PlayerId = playerId;
            Player = player;
        }


        public Payment() { }
    }
}

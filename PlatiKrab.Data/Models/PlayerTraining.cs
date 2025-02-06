using System.ComponentModel.DataAnnotations;

namespace PlatiKrab.Data.Models
{
    public class PlayerTraining
    {
        [Key]
        public int PlayerTrainingId { get; set; }

        // FK na hráče
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        // FK na trénink
        public int TrainingId { get; set; }
        public Training Training { get; set; } = null!;
    }
}

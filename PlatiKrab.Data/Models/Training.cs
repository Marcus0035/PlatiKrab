using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlatiKrab.Data.Models
{
    public class Training
    {
        [Key]
        public int TrainingId { get; set; }
        public DateTime Date { get; set; }

        public Payment? Payment { get; set; }
        public List<Player> Players { get; set; } = new();
    }
}

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

        // Vazební tabulka
        public List<PlayerTraining> PlayerTrainings { get; set; } = new();

        // Může mít platbu, ale nemusí
        public Payment? Payment { get; set; }
    }
}

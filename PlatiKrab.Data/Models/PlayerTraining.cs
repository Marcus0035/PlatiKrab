using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatiKrab.Data.Models
{
    public class PlayerTraining
    {
        [Key]
        public int Id { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; } = new Player();

        public int TrainingId { get; set; }
        public Training Training { get; set; } = new Training();
    }
}

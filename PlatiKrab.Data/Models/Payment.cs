using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatiKrab.Data.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public bool IsPaid { get; set; }

        public int TrainingId { get; set; }
        public Training Training { get; set; } = new Training();
        public Player PlayerWhoPays { get; set; } = new Player();
    }
}

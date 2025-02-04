using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatiKrab.Data.Models
{
    public class Training
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; }

        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = new Payment();
        public List<PlayerTraining> Players { get; set; } = new List<PlayerTraining>();

    }
}

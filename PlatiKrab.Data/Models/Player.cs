using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatiKrab.Data.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public List<PlayerTraining> Trainings { get; set; } = new List<PlayerTraining>();
    }
}

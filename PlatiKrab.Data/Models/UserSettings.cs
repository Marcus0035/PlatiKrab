using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatiKrab.Data.Models
{
    public class UserSettings
    {
        [Key]
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public bool DarkMode { get; set; } = true;

    }
}

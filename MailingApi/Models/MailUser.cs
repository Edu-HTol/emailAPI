using MailingApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Models
{
    [Table("MailUser")]
    public class MailUser : IUser, IDentifier
    {
        [Key]
        [Column("usr_id")]
        public int Id { get; set; }
        [Column("usr_name")]
        public string Username { get; set; }
        [Column("usr_password")]
        public string Password { get; set; }
    }
}

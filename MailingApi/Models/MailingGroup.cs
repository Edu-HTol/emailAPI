using MailingApi.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Models
{
    [Table("MailingGroup")]
    public class MailingGroup : IDentifier
    {
        [Key]
        [Column("grp_id")]
        public int Id { get; set; }
        [Column("grp_name")]
        public string Name { get; set; }
        [Column("grp_ownerId")]
        public int GroupOwnerId { get; set; }
    }
}

using MailingApi.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Models
{
    [Table("MailConsumer")]
    public class MailConsumer : IDentifier 
    {
        [Key]
        [Column("con_id")]
        public int Id { get; set; }
        [Column("con_address")]
        public string ConsumerAddress { get; set; }
        [Column("con_groupId")]
        [ForeignKey("MailingGroup")]
        public int GroupId { get; set; }
    }

}

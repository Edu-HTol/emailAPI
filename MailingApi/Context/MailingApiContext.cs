using MailingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MailingApi.Layers
{
    public class MailingApiContext : DbContext
    {
        public DbSet<MailConsumer> Consumers { get; set; }
        public DbSet<MailingGroup> Groups { get; set; }
        public DbSet<MailUser> GroupOwners { get; set; }
        public MailingApiContext(DbContextOptions options) : base(options)
        {

        }

        public MailingApiContext(DbContextOptions options, DbSet<MailConsumer> consumers, DbSet<MailingGroup> groups, DbSet<MailUser> groupOwners) : base(options)
        {
            Consumers = consumers;
            Groups = groups;
            GroupOwners = groupOwners;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MailUser>()
                .HasIndex(x => x.Username)
                .IsUnique();
        }
    }
}

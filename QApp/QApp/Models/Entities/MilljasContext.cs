﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QApp.Models.Entities
{
    public partial class MilljasContext : DbContext
    {
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<Counter> Counter { get; set; }
        public virtual DbSet<Queue> Queue { get; set; }
        public virtual DbSet<User> User { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    optionsBuilder.UseSqlServer(@"Server=tcp:qapp.database.windows.net,1433;Initial Catalog=Milljas;Persist Security Info=False;User ID=milljas;Password=KronanWhite90;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("Card", "q");

                entity.HasOne(d => d.Queue)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.QueueId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Card__QueueId__6B24EA82");

                entity.HasOne(d => d.Teller)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.TellerId)
                    .HasConstraintName("FK_Card_User");
            });

            modelBuilder.Entity<Counter>(entity =>
            {
                entity.ToTable("Counter", "q");

                entity.Property(e => e.CounterName)
                    .IsRequired()
                    .HasColumnType("varchar(max)");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.Counter)
                    .HasForeignKey(d => d.CardId)
                    .HasConstraintName("FK__Counter__CardId__68487DD7");

                entity.HasOne(d => d.Queue)
                    .WithMany(p => p.Counter)
                    .HasForeignKey(d => d.QueueId)
                    .HasConstraintName("FK__Counter__QueueId__6477ECF3");
            });

            modelBuilder.Entity<Queue>(entity =>
            {
                entity.ToTable("Queue", "q");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(max)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "q");

                entity.Property(e => e.AspNetUserId).HasMaxLength(450);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });
        }
    }
}
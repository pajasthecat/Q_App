using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QApp.Models.Entities
{
    public partial class MilljasContext : DbContext
    {
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<Queue> Queue { get; set; }
        public virtual DbSet<Teller> Teller { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("Card", "q");

                entity.Property(e => e.CardCreateTime).HasColumnType("datetime");

                entity.Property(e => e.Qid).HasColumnName("QId");

                entity.Property(e => e.TellerEndTime).HasColumnType("datetime");

                entity.Property(e => e.TellerStartTime).HasColumnType("datetime");

                entity.Property(e => e.Tid).HasColumnName("TId");

                entity.HasOne(d => d.Q)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.Qid)
                    .HasConstraintName("FK__Card__QId__5AEE82B9");

                entity.HasOne(d => d.T)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.Tid)
                    .HasConstraintName("FK__Card__TId__5EBF139D");
            });

            modelBuilder.Entity<Queue>(entity =>
            {
                entity.ToTable("Queue", "q");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(max)");
            });

            modelBuilder.Entity<Teller>(entity =>
            {
                entity.ToTable("Teller", "q");

                entity.Property(e => e.Cid).HasColumnName("CId");

                entity.Property(e => e.Qid).HasColumnName("QId");

                entity.Property(e => e.TellerNumber)
                    .IsRequired()
                    .HasColumnType("varchar(max)");

                entity.HasOne(d => d.C)
                    .WithMany(p => p.Teller)
                    .HasForeignKey(d => d.Cid)
                    .HasConstraintName("FK__Teller__CId__5FB337D6");

                entity.HasOne(d => d.Q)
                    .WithMany(p => p.Teller)
                    .HasForeignKey(d => d.Qid)
                    .HasConstraintName("FK__Teller__QId__60A75C0F");
            });
        }
    }
}
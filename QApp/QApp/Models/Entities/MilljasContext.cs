using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QApp.Models.Entities
{
    public partial class MilljasContext : DbContext
    {
        public virtual DbSet<Queue> Queue { get; set; }
        public virtual DbSet<QueueTeller> QueueTeller { get; set; }
        public virtual DbSet<QueueUser> QueueUser { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    optionsBuilder.UseSqlServer(@"Server=tcp:qapp.database.windows.net,1433;Initial Catalog=Milljas;Persist Security Info=False;User ID=milljas;Password=KronanWhite90;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //}

        public MilljasContext(DbContextOptions<MilljasContext> options):base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Queue>(entity =>
            {
                entity.ToTable("Queue", "q");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(max)");
            });

            modelBuilder.Entity<QueueTeller>(entity =>
            {
                entity.ToTable("QueueTeller", "q");

                entity.Property(e => e.Qid).HasColumnName("QId");

                entity.HasOne(d => d.Q)
                    .WithMany(p => p.QueueTeller)
                    .HasForeignKey(d => d.Qid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__QueueTeller__QId__4D94879B");
            });

            modelBuilder.Entity<QueueUser>(entity =>
            {
                entity.ToTable("QueueUser", "q");

                entity.Property(e => e.Qid).HasColumnName("QId");

                entity.HasOne(d => d.Q)
                    .WithMany(p => p.QueueUser)
                    .HasForeignKey(d => d.Qid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__QueueUser__QId__4AB81AF0");
            });
        }
    }
}
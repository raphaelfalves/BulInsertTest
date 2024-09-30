using BulInsertTest.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulInsertTest.Context
{
    public class AppDbContext() : DbContext()
    {
        public DbSet<Produto> Produtos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(AppConfig.CONNECTION_STRING);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Produto>(entity =>
                {
                    entity.HasKey(p => p.Id);

                    entity.Property(p => p.Nome)
                    .HasMaxLength(100)
                    .IsRequired();

                    entity.Property(p => p.Descricao)
                    .HasMaxLength(200)
                    .IsRequired();

                    entity.Property(p => p.Preco)
                    .HasColumnType("decimal(14,2)");
                });
        }
    }
}

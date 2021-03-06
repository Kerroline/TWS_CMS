using MangaCMS.Models;
using MangaCMS.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.DAL
{
    public class MangaCMSContext : IdentityDbContext<CustomUser>
    {
        public DbSet<MangaModel> Mangas { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<StatusModel> Statuses { get; set; }
        public DbSet<ChapterModel> Chapters { get; set; }
        public DbSet<ProgressModel> Progresses { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<PageModel> Pages { get; set; }
        public DbSet<PosterModel> Posters { get; set; }

        public MangaCMSContext(DbContextOptions<MangaCMSContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatusModel>().HasData(
                new StatusModel { Id = 1, StatusName = "Настройка проекта" }
                );

            modelBuilder
                .Entity<ChapterModel>()
                .HasOne(ch => ch.Progress)
                .WithOne(pr => pr.Chapter)
                .HasForeignKey<ProgressModel>(pr => pr.ChapterId);

            modelBuilder.Entity<MangaGenreModel>()
                .HasKey(k => new { k.MangaId, k.GenreId });

            modelBuilder.Entity<MangaGenreModel>()
                .HasOne(mg => mg.Manga)
                .WithMany(t => t.listOfGenres)
                .HasForeignKey(mg => mg.MangaId);

            modelBuilder.Entity<MangaGenreModel>()
                .HasOne(mg => mg.Genre)
                .WithMany(t => t.MangasGenres)
                .HasForeignKey(mg => mg.GenreId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

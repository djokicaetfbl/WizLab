using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizLib_DataAccess.FluentConfig;
using WizLib_Model.Models;

namespace WizLib_DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) // paste optins to Base (osnovu) klasu
        {
        }
        //DbContext je odgovoran za kreiranje bindinga izmedju nase aplikacije i SQL servera.

        public DbSet<BookDetailsFromView> BookDetailsFromViews { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookDetail> BookDetails { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }

        public DbSet<Fluent_BookDetail> Fluent_BookDetails { get; set; }
        public DbSet<Fluent_Book> Fluent_Book { get; set; }
        public DbSet<Fluent_Author> Fluent_Author { get; set; }
        public DbSet<Fluent_Publisher> Fluent_Publisher { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) // override za konfiguraciju modelskih klasa kroz FluentAPI
        {
            // we configure Fluent API

            //category table name and column name
            modelBuilder.Entity<Category>().ToTable("tbl_category");
            modelBuilder.Entity<Category>().Property(c => c.Name).HasColumnName("CategoryName");

            modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.Author_Id, ba.Book_Id }); // kroz FluentAPI definisemo kompozitni kljuc!

            modelBuilder.ApplyConfiguration(new FluentBookConfig());
            modelBuilder.ApplyConfiguration(new FluentAuthorConfig());
            modelBuilder.ApplyConfiguration(new FluentBookAuthorConfig());
            modelBuilder.ApplyConfiguration(new FluentBookDetailConfig());
            modelBuilder.ApplyConfiguration(new FluentPublisherConfig());

            modelBuilder.Entity<BookDetailsFromView>().HasNoKey().ToView("GetOnlyBookDetails"); // kada nema primarnog kljuca Tracker nije ukljucen

        }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PersonalDiaryApp.Models
{
    public class DiaryDBContext : DbContext
    {

        public DiaryDBContext(DbContextOptions<DiaryDBContext> options) : base(options)
        {


        }

        public virtual DbSet<DiaryReport> DiaryReport { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //      //  optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyPesonalDiaryDb;Trusted_Connection=True;");
        //    }
        //}
    }
}
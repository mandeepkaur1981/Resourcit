using Microsoft.EntityFrameworkCore;
using ResourcitModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourcitAPI.Data
{
    public class ResourcitDbContext:DbContext
    {
        public ResourcitDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().Property(i=>i.Budget).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Resource>().HasData(
                new Resource() { ResourceId = 1, Firstname = "Mandeep", Lastname = "Kaur", Hiredate = Convert.ToDateTime("2017-05-30"), Middlename=""},
                new Resource() { ResourceId = 2, Firstname = "Harpreet", Lastname = "Singh", Hiredate = Convert.ToDateTime("2018-05-30"), Middlename = "", Termdate=Convert.ToDateTime("2019-12-31") },
                new Resource() { ResourceId = 3, Firstname = "Manpreet", Lastname = "Singh", Hiredate = Convert.ToDateTime("2020-05-30"), Middlename = "Pal" },
                new Resource() { ResourceId = 4, Firstname = "Neharika", Lastname = "Rambhatla", Hiredate = Convert.ToDateTime("2021-05-30"), Middlename = "" }
            );
            modelBuilder.Entity<Project>().HasData(
                new Project() { ProjectId = 1, Description="C# Project", ShortDescription="C#", Budget=45000, Startdate=Convert.ToDateTime("2016-01-01"), Enddate=Convert.ToDateTime("2021-12-31")},
                new Project() { ProjectId = 2, Description = "ASP.NET Web Project", ShortDescription = "ASP.NET Web", Budget = 900000, Startdate = Convert.ToDateTime("2019-01-01"), Enddate = Convert.ToDateTime("2021-12-31") },
                new Project() { ProjectId = 3, Description = "React Project", ShortDescription = "React", Budget = 145000, Startdate = Convert.ToDateTime("2018-01-01"), Enddate = Convert.ToDateTime("2022-01-31") }
                );
        }
    }
}

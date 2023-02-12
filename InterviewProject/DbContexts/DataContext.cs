using InterviewProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace InterviewProject.DbContexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    FirstName = "Seda",
                    LastName = "KELEŞ",
                    BirthDate = new DateTime(1996, 08,02)
                },
               new Student
               {
                   Id = 2,
                   FirstName = "Batuhan",
                   LastName = "GÜN",
                   BirthDate = new DateTime(1992, 03, 11)
               },
               new Student
               {
                   Id = 3,
                   FirstName = "Can",
                   LastName = "KARABULUT",
                   BirthDate = new DateTime(1996, 08, 01)
               },
               new Student
               {
                   Id = 4,
                   FirstName = "Serhat",
                   LastName = "ÜNAL",
                   BirthDate = new DateTime(1996, 10, 29)
               },
               new Student
               {
                   Id = 5,
                   FirstName = "Selma",
                   LastName = "YALÇIN",
                   BirthDate = new DateTime(1996, 06, 02)
               }
            );
            
            builder.Entity<Course>().HasData(
               new Course
               {
                   Id = 1,
                   CourseName = ".Net 6 Eğitimi"
               },
                new Course
                {
                    Id = 2,
                    CourseName = "Mikroservis Mimarisi"
                },
                 new Course
                 {
                     Id = 3,
                     CourseName = "İş Analisti"
                 },
                  new Course
                  {
                      Id = 4,
                      CourseName = "Python"
                  },
                  new Course
                  {
                      Id = 5,
                      CourseName = "SAP Danışmanlığı"
                  }
             ); 
            builder.Entity<CourseStudent>().HasData(
                new CourseStudent
                {
                    Id = 1,
                    StudentId = 1,
                    CourseId = 1
                },
                 new CourseStudent
                 {
                     Id = 2,
                     StudentId = 1,
                     CourseId = 2
                 },
                  new CourseStudent
                  {
                      Id = 3,
                      StudentId = 2,
                      CourseId = 4
                  },
                   new CourseStudent
                   {
                       Id = 4,
                       StudentId = 3,
                       CourseId = 3
                   },
                   new CourseStudent
                   {
                       Id = 5,
                       StudentId = 4,
                       CourseId = 3
                   },
                    new CourseStudent
                    {
                        Id = 6,
                        StudentId = 5,
                        CourseId = 5
                    }
              );

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
    }
}
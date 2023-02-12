using InterviewProject.Entities;

namespace InterviewProject.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<Student> Students { get; }
        IGenericRepository<Course> Courses { get; }
        IGenericRepository<CourseStudent> CourseStudents { get; }
        Task Save();
    }
}

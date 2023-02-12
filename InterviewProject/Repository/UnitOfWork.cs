using InterviewProject.DbContexts;
using InterviewProject.Entities;
using InterviewProject.IRepository;

namespace InterviewProject.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private IGenericRepository<Student> _students;
        private IGenericRepository<Course> _courses;
        private IGenericRepository<CourseStudent> _courseStudents;
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext=dataContext;
        }


        public IGenericRepository<Student> Students => _students ??= new GenericRepository<Student>(_dataContext);

        public IGenericRepository<Course> Courses => _courses ??= new GenericRepository<Course>(_dataContext);

        public IGenericRepository<CourseStudent> CourseStudents => _courseStudents ??= new GenericRepository<CourseStudent>(_dataContext);

        public void Dispose()
        {
            _dataContext.Dispose();
            GC.SuppressFinalize(this);
        }


        public async Task Save()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}

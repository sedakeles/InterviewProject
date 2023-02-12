using AutoMapper;
using InterviewProject.DbContexts;
using InterviewProject.Dtos;
using InterviewProject.Entities;
using InterviewProject.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseStudentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _context;
        private readonly ILogger<CourseStudentController> _logger;
        private readonly IMapper _mapper;
        public CourseStudentController(IUnitOfWork unitOfWork, DataContext context,ILogger<CourseStudentController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _context = context;

        }
        [HttpGet(Name = "GetCourseStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCourseStudents()
        {
            try
            {
                var result = await (from cs in _context.CourseStudents
                             join s in _context.Students on cs.StudentId equals s.Id
                             join c in _context.Courses on cs.CourseId equals c.Id
                             select new
                             {
                                 CourseName = c.CourseName,
                                 StudentName = s.FirstName + " " +s.LastName
                             }).ToListAsync();
            
                return Ok(result);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Şurada sorun oluştu {nameof(GetCourseStudents)}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterCourse([FromBody] RegisterCourseDto registerCourseDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Geçersiz POST isteği {nameof(RegisterCourse)}");
                return BadRequest(ModelState);
            }
            try
            {
                var s = await _unitOfWork.CourseStudents.Get(x=>x.Id==registerCourseDto.CourseId && x.StudentId==registerCourseDto.StudentId);
                if (s!=null)
                {
                    return BadRequest("Bu öğrenci bu kursu daha önce almıştır.");
                }
                var result = _mapper.Map<CourseStudent>(registerCourseDto);
                await _unitOfWork.CourseStudents.Insert(result);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCourseStudents",  result);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Şurada sorun oluştu {nameof(RegisterCourse)}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

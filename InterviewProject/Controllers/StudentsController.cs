using AutoMapper;
using InterviewProject.Dtos;
using InterviewProject.Entities;
using InterviewProject.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace InterviewProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentsController> _logger;
        private readonly IMapper _mapper;
        public StudentsController(IUnitOfWork unitOfWork, ILogger<StudentsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _unitOfWork.Students.GetAll();
                var result = _mapper.Map<IList<StudentDto>>(students);
                return Ok(result);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Şurada sorun oluştu {nameof(GetStudents)}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("{id:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                var student = await _unitOfWork.Students.Get(q => q.Id == id);
                var result = _mapper.Map<StudentDto>(student);
                return Ok(result);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Şurada sorun oluştu {nameof(GetStudent)}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Geçersiz POST isteği {nameof(CreateStudent)}");
                return BadRequest(ModelState);
            }
            try
            {
                var student = _mapper.Map<Student>(studentDto);
                await _unitOfWork.Students.Insert(student);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetStudent", new { id = student.Id }, student);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Şurada sorun oluştu {nameof(CreateStudent)}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto studentDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Geçersiz UPDATE isteği {nameof(UpdateStudent)}");
                return BadRequest(ModelState);
            }
            try
            {
                var student = await _unitOfWork.Students.Get(x => x.Id == id);

                if (student == null)
                {
                    _logger.LogError($"Geçersiz UPDATE isteği {nameof(UpdateStudent)}");
                    return BadRequest("Geçersiz data");
                }

                _mapper.Map(studentDto, student);
                _unitOfWork.Students.Update(student);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Şurada sorun oluştu {nameof(UpdateStudent)}");
                return StatusCode(500, "Internal Server Error");
            }

        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Geçersiz DELETE isteği {nameof(DeleteStudent)}");
                return BadRequest();
            }
            try
            {
                var student = await _unitOfWork.Students.Get(x => x.Id == id);

                if (student == null)
                {
                    _logger.LogError($"Geçersiz UPDATE isteği {nameof(DeleteStudent)}");
                    return BadRequest("Geçersiz data");
                }


                await _unitOfWork.Students.Delete(id);
                var students = await _unitOfWork.CourseStudents.GetAll(x => x.StudentId == id);
                if (students != null)
                {
                    foreach (var item in students)
                    {
                        await _unitOfWork.CourseStudents.Delete(item.Id);
                    }

                }

                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Şurada sorun oluştu {nameof(DeleteStudent)}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
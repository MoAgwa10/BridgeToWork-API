using B2W.Dto;
using B2W.Models;
using B2W.Models.Authentication;
using B2W.Models.Dto;
using B2W.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {



        private readonly ApplicationDbContext _db;

        public EducationController(ApplicationDbContext db)
        {
            _db = db;
        }

        
        [HttpGet("GetEducations")]
        public async Task<ActionResult<IEnumerable<EducationDto>>> GetEducations() //  تجلب جميع سجلات التعليم لكل المستخدمين في النظام.
        {
            var educations = await _db.Educations
                .Select(e => new EducationDto
                {
                    University = e.University,
                    Faculty = e.Faculty,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    ApplicationUserId = e.ApplicationUserId,
                })
                .ToListAsync();

            return Ok(educations);
        }


        [HttpGet("{applicationUserId}")] //تجلب سجل التعليم لمستخدم معين باستخدام
        public async Task<ActionResult<EducationDto>> GetEducationByUser(string applicationUserId)
        {
            var education = await _db.Educations
                .Where(e => e.ApplicationUserId == applicationUserId) 
                .FirstOrDefaultAsync();

            if (education == null)
                return NotFound("No education record found for this user.");

            var educationDto = new EducationDto
            {

                University = education.University,
                Faculty = education.Faculty,
                Degree = education.Degree,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
               ApplicationUserId=education.ApplicationUserId,
            };

            return Ok(educationDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EducationDto>> GetEducation(int id)
        {
            var education = await _db.Educations
                                          .Where(e => e.Id == id)
                                          .Select(e => new EducationDto
                                          {
                                              Id = e.Id,
                                              University = e.University,
                                              Faculty = e.Faculty,
                                              Degree = e.Degree,
                                              StartDate = e.StartDate,
                                              EndDate = e.EndDate,
                                              ApplicationUserId = e.ApplicationUserId
                                          })
                                          .FirstOrDefaultAsync();

            if (education == null)
                return NotFound("Education record not found.");

            return Ok(education);
        }




        [HttpPost]
        public async Task<IActionResult> AddEducation(EducationDto educationDto)
        {
            var education = new Education
            {
                University = educationDto.University,
                Faculty = educationDto.Faculty,
                Degree = educationDto.Degree,
                StartDate = educationDto.StartDate,
                EndDate = educationDto.EndDate,
                ApplicationUserId = educationDto.ApplicationUserId,
            };

            _db.Educations.Add(education);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEducationByUser), new { applicationUserId = education.ApplicationUserId }, educationDto);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> EditEducation(int id, EducationDto educationDto)
        {
            var education = await _db.Educations.FindAsync(id);
            if (education == null)
                return NotFound("Education record not found.");

            education.University = educationDto.University;
            education.Faculty = educationDto.Faculty;
            education.Degree = educationDto.Degree;
            education.StartDate = educationDto.StartDate;
            education.EndDate = educationDto.EndDate;

            await _db.SaveChangesAsync();
            return Ok("Education record updated successfully.");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveEducation(int id)
        {
            var education = await _db.Educations.FindAsync(id);
            if (education == null)
                return NotFound("Education record not found.");

            _db.Educations.Remove(education);
            await _db.SaveChangesAsync();

            return Ok("Education record deleted successfully.");







        }
        }
 } 


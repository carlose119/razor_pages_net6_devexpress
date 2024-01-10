using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using razor_pages_net6.Data;
using razor_pages_net6.Models;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace razor_pages_net6.Controllers {

    [Route("api/[controller]/[action]")]
    public class CoursesApiController : Controller {
        DBContext _context;

        public CoursesApiController(DBContext context) {
            _context = context;
        }

        const string VALIDATION_ERROR = "The request failed due to a validation error";

        // Load Courses according to load options
        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var courses = _context.Courses.Select(i => new {
                i.CourseID,
                i.Title,
                i.Credits
            });

            return Json(await DataSourceLoader.LoadAsync(courses, loadOptions));
        }

        // Insert a new course
        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var newCourse = new Course();
            PopulateModel(newCourse, JsonConvert.DeserializeObject<IDictionary>(values));

            if(!TryValidateModel(newCourse))
                return BadRequest(VALIDATION_ERROR);

            var result = _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();
            return Json(new { result.Entity.CourseID });
        }

        // Update an Course
        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var course = await _context.Courses.FirstOrDefaultAsync(item => item.CourseID == key);
            PopulateModel(course, JsonConvert.DeserializeObject<IDictionary>(values));

            if(!TryValidateModel(course))
                return BadRequest(VALIDATION_ERROR);

            await _context.SaveChangesAsync();
            return Ok();
        }

        // Remove an Course
        [HttpDelete]
        public async Task Delete(int key) {
            var course = await _context.Courses.FirstOrDefaultAsync(item => item.CourseID == key);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        void PopulateModel(Course course, IDictionary values) {
            if(values.Contains("CourseID"))
                course.CourseID = Convert.ToInt32(values["OrderId"]);

            if(values.Contains("Title"))
                course.Title = values["Title"] != null ? Convert.ToString(values["Title"]) : null;

            if(values.Contains("Credits"))
                course.Credits = Convert.ToInt32(values["Credits"]);
        }
    }
}
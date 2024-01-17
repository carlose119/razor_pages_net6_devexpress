using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using razor_pages_net6.Data;
using razor_pages_net6.Models;

namespace razor_pages_net6.Pages.Student;

public class IndexModel : PageModel
{
    private readonly DBContext _context;
    public List<Models.Student>? ListStudents { get; set; }
    public Dictionary<int, string> ListCourses { get; set; }

    public IndexModel(DBContext context)
    {
        _context = context;

        ListCourses = EnrollmentCourse();
    }

    public void OnGet()
    {
        ListStudents = _context.Students.ToList();
    }

    public JsonResult OnGetListStudents()
    {
        return new JsonResult(_context.Students.ToList());
    }

    public Dictionary<int, string> EnrollmentCourse()
    {
        var courses = (
            from e in _context.Enrollments
            join s in _context.Students on e.StudentID equals s.ID
            select new { CourseID = e.Course.CourseID, Title = e.Course.Title }
        )
            .Distinct()
            .ToList();
        var data = new Dictionary<int, string>();
        foreach (var p in courses)
        {
            data.Add(p.CourseID, p.Title);
        }

        return data;
    }

    public JsonResult OnGetListStudentsDevExpress()
    {
        return new JsonResult(_context.Students.ToList());
    }

    // runs on the GET request
    public IActionResult OnGetGridData(DataSourceLoadOptions loadOptions)
    {
        var students = _context.Students.Select(i => new
        {
            i.ID,
            i.LastName,
            i.FirstMidName,
            i.EnrollmentDate
        });
        return new JsonResult(DataSourceLoader.Load(students, loadOptions));
    }

    // runs on the PUT request
    public IActionResult OnPutGridRow(int key, string values)
    {
        var model = _context.Students.FirstOrDefault(item => item.ID == key);

        var _values = JsonConvert.DeserializeObject<IDictionary>(values);
        PopulateModel(model, _values);

        /* if (!TryValidateModel(model))
            return BadRequest("Validation failed"); */

        _context.SaveChanges();
        return new OkResult();
    }

    void PopulateModel(Models.Student model, IDictionary values)
    {
        if (values.Contains("ID"))
        {
            model.ID = Convert.ToInt32(values["ID"]);
        }
        if (values.Contains("LastName"))
        {
            model.LastName = Convert.ToString(values["LastName"]);
        }
        if (values.Contains("FirstMidName"))
        {
            model.FirstMidName = Convert.ToString(values["FirstMidName"]);
        }
        if (values.Contains("EnrollmentDate"))
        {
            model.EnrollmentDate = Convert.ToDateTime(values["EnrollmentDate"]);
        }
    }
}

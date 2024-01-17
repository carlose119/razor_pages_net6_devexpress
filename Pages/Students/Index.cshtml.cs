using System;
using System.Collections.Generic;
using System.Linq;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor_pages_net6.Data;
using razor_pages_net6.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            select new
            {
                CourseID = e.Course.CourseID,
                Title = e.Course.Title
            }
        ).Distinct().ToList();
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
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DevExtreme.AspNet.Mvc;
using razor_pages_net6.Data;
using razor_pages_net6.Models;
using Microsoft.EntityFrameworkCore;
using DevExtreme.AspNet.Mvc.Builders;

namespace razor_pages_net6.Pages.Courses;

public class IndexModel : PageModel
{
    private readonly DBContext _context;
    public List<Models.Course>? ListCourses { get; set; }

    public IndexModel(DBContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        ListCourses = _context.Courses.ToList();
    }

    public JsonResult OnGetListCourses()
    {
        return new JsonResult(_context.Courses.ToList());
    }
}

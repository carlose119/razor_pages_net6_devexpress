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

namespace razor_pages_net6.Pages.Student;

public class IndexModel : PageModel
{
    private readonly DBContext _context;
    public List<Models.Student>? ListStudents { get; set; }

    public IndexModel(DBContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        ListStudents = _context.Students.ToList();
    }

    public JsonResult OnGetListStudents()
    {
        return new JsonResult(_context.Students.ToList());
    }
}

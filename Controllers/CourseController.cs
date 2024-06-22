using AnitaSMS.Infrastructure.IRepository;
using AnitaSMS.Models.Entity;
using AnitaSMS.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnitaSMS.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICrudServices<Courses> _course;
        private readonly ICrudServices<Students> _student;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICrudServices<Courses> course,
            ICrudServices<Students> student,
            UserManager<ApplicationUser> userManager)
        {
            _course = course;
            _student = student;
            _userManager = userManager;
        }


        [Authorize(Roles = "ADMIN, STUDENT")]

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var CourseList = await _course.GetAllAsync();
            return View(CourseList);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            Courses course = new Courses();
            if (id > 0)
            {
                course = await _course.GetAsync(id);
            }
            return View(course);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> AddEdit(Courses course)
        {
            if (ModelState.IsValid)   //server side validation
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    if (course.Id == 0)
                    {
                        course.CreatedDate = DateTime.Now;
                        course.CreatedBy = userId;
                        await _course.InsertAsync(course);

                        TempData["success"] = "Data Added Successfully";
                    }
                    else
                    {
                        var OrgCourse = await _course.GetAsync(course.Id);
                        OrgCourse.CourseName = course.CourseName;
                        OrgCourse.CourseDescription = course.CourseDescription;
                        OrgCourse.IsActive = course.IsActive;
                        OrgCourse.ModifiedDate = DateTime.Now;
                        OrgCourse.ModifiedBy = userId;
                        await _course.UpdateAsync(OrgCourse);

                        TempData["success"] = "Data Updated Successfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "Something went wrong, please try again later";
                    //TempData["error"] = ex.Message;
                    return RedirectToAction(nameof(AddEdit));
                }
            }
            TempData["error"] = "Please Input Valid Data";
            return RedirectToAction(nameof(AddEdit));
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _course.GetAsync(id);
            _course.Delete(course);
            TempData["error"] = "Data Deleted Sucessfully";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN, STUDENT")]

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var course = await _course.GetAsync(id);
            return View(course);
        }
    }
}

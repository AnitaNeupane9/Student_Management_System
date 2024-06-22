using AnitaSMS.Infrastructure.IRepository;
using AnitaSMS.Models.Entity;
using AnitaSMS.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AnitaSMS.Controllers
{

    public class StudentController : Controller
    {
        private readonly ICrudServices<Students> _student;
        private readonly ICrudServices<Courses> _course;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ICrudServices<Students> student,
            ICrudServices<Courses> course,
            UserManager<ApplicationUser> userManager)
        {
            _student = student;
            _course = course;
            _userManager = userManager;
        }
        [Authorize(Roles = "ADMIN, STUDENT")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var studentList = await _student.GetAllAsync();
            ViewBag.Course = await _course.GetAllAsync();
            return View(studentList);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            Students student = new Students();

            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Course = await _course.GetAllAsync();
            if (id > 0)
            {
                student = await _student.GetAsync(id);
            }
            return View(student);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> AddEdit(Students student)
        {

            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Course = await _course.GetAllAsync();
            if (ModelState.IsValid)   //server side validation
            {
                try
                {
                    if (student.ImageFile != null)
                    {
                        string fileDirectory = $"wwwroot/UserImage";

                        if (!Directory.Exists(fileDirectory))
                        {
                            Directory.CreateDirectory(fileDirectory);
                        }
                        string uniqueFileName = Guid.NewGuid() + "_" + student.ImageFile.FileName;
                        string filePath = Path.Combine(Path.GetFullPath($"wwwroot/UserImage"), uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await student.ImageFile.CopyToAsync(fileStream);
                            student.StudentProfileurl = $"UserImage/" + uniqueFileName;

                        }
                    }
                    if (student.Id == 0)
                    {
                        student.CreatedDate = DateTime.Now;
                        student.CreatedBy = userId;
                        await _student.InsertAsync(student);

                        TempData["success"] = "Data Added Successfully";
                    }
                    else
                    {
                        var OrgStudent = await _student.GetAsync(student.Id);
                        OrgStudent.FirstName = student.FirstName;
                        OrgStudent.MiddleName = student.MiddleName;
                        OrgStudent.LastName = student.LastName;
                        OrgStudent.PhoneNumber = student.PhoneNumber;
                        OrgStudent.Address = student.Address;
                        OrgStudent.Email = student.Email;
                        OrgStudent.Class = student.Class;
                        OrgStudent.Section = student.Section;
                        OrgStudent.CourseId = student.CourseId;
                        OrgStudent.IsActive = student.IsActive;
                        OrgStudent.ModifiedDate = DateTime.Now;
                        OrgStudent.ModifiedBy = userId;
                        if (student.ImageFile != null)
                        {
                            OrgStudent.StudentProfileurl = student.StudentProfileurl;
                        }
                        await _student.UpdateAsync(OrgStudent);

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
            var student = await _student.GetAsync(id);
            _student.Delete(student);
            TempData["error"] = "Data Deleted Sucessfully";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN, STUDENT")]

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var student = await _student.GetAsync(id);
            ViewBag.Course = await _course.GetAllAsync();
            return View(student);
        }
    }
}

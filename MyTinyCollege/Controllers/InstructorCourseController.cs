using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTinyCollege.DAL;
using MyTinyCollege.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyTinyCollege.ViewModels;

namespace MyTinyCollege.Controllers
{
    [Authorize(Roles = "instructor")]
    public class InstructorCourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: InstructorCourse
        public ActionResult Index(int? courseID)
        {
            string userId = User.Identity.GetUserId();

            if (!string.IsNullOrEmpty(userId))
            {
                //found the user that us currently logged in
                //but we need their email to associate back to our
                //Person instructor entity
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
                //the username is the email (user registers and logs in with email)
                var currentInstructor = manager.FindByEmail(User.Identity.GetUserName());

                var viewModel = new InstructorIndexData();
                viewModel.Instructors = db.Instructors
                                        .Include(i => i.Courses)
                                        .Where(i => i.Email == currentInstructor.Email);

                var instructor = viewModel.Instructors
                                 .Where(i => i.Email == currentInstructor.Email).Single();

                viewModel.Courses = viewModel.Instructors
                                    .Where(i => i.ID == instructor.ID).Single().Courses;

                //get all students enrolled in selected course (via courseID route parameter)
                if (courseID != null)
                {
                    var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                    //Explicit Loading
                    db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                    foreach (Enrollment enrollment in selectedCourse.Enrollments)
                    {
                        db.Entry(enrollment).Reference(x => x.student).Load();
                    }
                    viewModel.Enrollments = selectedCourse.Enrollments;
                }

                return View(viewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        
        
        // GET: InstructorCourse/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
            return View(instructor);
        }

        // POST: InstructorCourse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,Email,HireDate")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
            return View(instructor);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

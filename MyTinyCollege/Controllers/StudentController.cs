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
using PagedList;
using MyTinyCollege.ViewModels; //for student body stats report

namespace MyTinyCollege.Controllers
{
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Student
        //public ActionResult Index()
        //{
        //    return View(db.Students.ToList());
        //}

        //mwilliams:  adding sorting, filtering, and paging functionality
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {

            //Prepare sort order 
            ViewBag.CurrentSort = sortOrder;//get current sort from UI
            ViewBag.LNameSortParm = string.IsNullOrEmpty(sortOrder) ? "lname_desc" : "";
            ViewBag.FNameSortParm = sortOrder == "fname" ? "fname_desc" : "fname";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";

            //for filtering and paging
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //Let's get our student data
            var students = from s in db.Students select s;

            //check for filter (searchString)
            if (!string.IsNullOrEmpty(searchString))
            {
                //Apply filter on first and last name 
                students = students.Where(s => s.LastName.Contains(searchString) ||
                s.FirstName.Contains(searchString));
            }

            //Apply the sort order 
            switch (sortOrder)
            {

                //FirstName Asc
                case "fname":
                    students = students.OrderBy(s => s.FirstName);
                    break;

                //FirstName Desc
                case "fname_desc":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;

                //EnrollmentDate Asc
                case "date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;

                //EnrollmentDate Desc
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;

                //Email Asc
                case "email":
                    students = students.OrderBy(s => s.Email);
                    break;

                //Email Desc
                case "email_desc":
                    students = students.OrderByDescending(s => s.Email);
                    break;
                //LastName Desc
                case "lname_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;

                //Default LastName Asc
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            //return the students object as a enumerable (list)
            //return View(students.ToList());

            //Setup Pager
            int pageSize = 3;  //start with page size of 3 for paging (how many records per page)
            int pageNumber = (page ?? 1);
            /* The two question marks represent the null-coalescing operator. 
             * The null-coalescing operator defines a default value for a nullable type; 
             * the expression (page ?? 1) means return the value of page if it has a value, 
             * or return 1 if page is null
             */
            return View(students.ToPagedList(pageNumber, pageSize));

        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstName,Email,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.People.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception /*ex*/)
            {
                //We could log the error - uncomment the ex 
                ModelState.AddModelError("", "Unable to save changes. Try again later!");
            }


            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentToUpdate = db.Students.Find(id);
            if (TryUpdateModel(studentToUpdate, "",
                new string[] { "LastName", "FirstName", "EnrollmentDate", "Email" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again later!");
                }
            }
            //Irregardless of the outcome (success or fail) we return the student model
            //with edit view or Index view
            return View(studentToUpdate);
        }
        //public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,Email,EnrollmentDate")] Student student)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(student).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(student);
        //}

        // GET: Student/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //check for error
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed please try again.";
            }

            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.People.Remove(student);
                db.SaveChanges();
            }
            catch (Exception)
            {
                //redirect user back to get Delete with same item (id)
                //including a boolean flag parameter stating that an error has occured
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("Index");
        }

        //tclark: Stats
        public ActionResult Stats()
        {
            IQueryable<ViewModels.EnrollmentDateGroup> data =
                from student in db.Students
                group student by student.EnrollmentDate into dateGroup
                select new ViewModels.EnrollmentDateGroup()
                {
                    EnrollmentDate = dateGroup.Key,
                    StudentCount = dateGroup.Count()
                };
            //The LINQ statement above groups the student entities by enrollment date,
            //calculating the number of entities in each group, and storing the results
            //in a collection of EnrollmentDateGroup view model objects

            return View(data.ToList());
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
namespace MyTinyCollege.Migrations.CollegeMigrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyTinyCollege.DAL.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\CollegeMigrations";
        }

        protected override void Seed(MyTinyCollege.DAL.SchoolContext context)
        {
            var students = new List<Student>
            {
                new Student {FirstName="Carson",
                             LastName ="Alexander",
                             EnrollmentDate =DateTime.Parse("2015-02-01"),
                             Email = "calexander@oultoncollege.com"},

                new Student {FirstName="Alonso",
                             LastName ="Arturo",
                             EnrollmentDate =DateTime.Parse("2015-12-15"),
                             Email = "aarturo@oultoncollege.com"},

                new Student {FirstName="Timmy",
                             LastName ="Alexander",
                             EnrollmentDate =DateTime.Parse("2015-06-21"),
                             Email = "talexander@oultoncollege.com"},

                new Student {FirstName="John",
                             LastName ="Smith",
                             EnrollmentDate =DateTime.Parse("2015-07-04"),
                             Email = "jsmith@oultoncollege.com"},

                new Student {FirstName="Frank",
                             LastName ="Bekkering",
                             EnrollmentDate =DateTime.Parse("2015-01-13"),
                             Email = "fbekkering@oultoncollege.com"},

                new Student {FirstName="Laura",
                             LastName ="Norman",
                             EnrollmentDate =DateTime.Parse("2015-03-28"),
                             Email = "lnorman@oultoncollege.com"}
            };

            //Loop ech student and add to database (only if they are not already there)
            //using the AssOrUpdate Method
            //The first parameter of this method specifies the propery to check if a row already exists
            students.ForEach(s => context.Students.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();

            //2. Add some instructors
            var instructors = new List<Instructor>
            {
                new Instructor
                {
                    FirstName="Terrance",
                    LastName="Clark",
                    HireDate=DateTime.Parse("2014-05-21"),
                    Email = "tclark@faculty.tinycollege.com"
                },

                new Instructor
                {
                    FirstName="Marc",
                    LastName="Williams",
                    HireDate=DateTime.Parse("2014-05-21"),
                    Email = "mwilliams@faculty.tinycollege.com"
                }
            };
            instructors.ForEach(s => context.Instructors.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();

            //3. Add some departments
            var departments = new List<Department>
            {
                new Department
                {
                    Name="Engineering",
                    Budget=350000,
                    StartDate=DateTime.Parse("2010-09-01"),
                    InstructorID=1
                },

                new Department
                {
                    Name="English",
                    Budget=150000,
                    StartDate=DateTime.Parse("2010-09-01"),
                    InstructorID=2
                }
            };

            departments.ForEach(s => context.Departments.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();



            //4. Add some courses
            var courses = new List<Course>
            {

                new Course
                {

                    CourseID=1045,
                    Title="Chemistry",
                    Credits=3,
                    DepartmentID=1
                },

                new Course
                {
                    CourseID=1046,
                    Title="Physics",
                    Credits=3,
                    DepartmentID=1
                },

                new Course
                {
                    CourseID=1047,
                    Title="Calculus",
                    Credits=3,
                    DepartmentID=1
                },

                new Course
                {
                    CourseID=1048,
                    Title="Literature",
                    Credits=3,
                    DepartmentID=2
                }
            };

            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.CourseID, s));
            context.SaveChanges();

            //5. Add some enrollments
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    StudentID=1,
                    CourseID=1045,
                    Grade=Grade.A
                },

                new Enrollment
                {
                    StudentID=2,
                    CourseID=1046,
                    Grade=Grade.B
                },

                new Enrollment
                {
                    StudentID=3,
                    CourseID=1047,
                    Grade=Grade.C
                },

                new Enrollment
                {
                    StudentID=4,
                    CourseID=1048,
                    Grade=Grade.D
                },

                new Enrollment
                {
                    StudentID=5,
                    CourseID=1045,
                    Grade=Grade.F
                },

                new Enrollment
                {
                    StudentID=6,
                    CourseID=1046,
                    Grade=Grade.A
                }
            };

            foreach (Enrollment e in enrollments)
            {
                var enroolmentInDatabase = context.Enrollments.Where(
                        s =>
                        s.StudentID == e.StudentID &&
                        s.course.CourseID == e.CourseID).SingleOrDefault();

                //SingleOrDefault-> Returns a single, specific element of a sequence, 
                //                  or a default value if no such element is found.
                //Use when expecting 0 or 1 item
                //You get 0 when 0 or 1 was expected (ok)
                //You get 1 when 0 or 1 was expected (ok)
                //You get 2 or more when 0 or 1 was expected (error)

                //Single:  Returns a single, specific element of a sequence
                //Use 1 item expected (not 0 or 2 and more)
                //You get 0 when 0 or 1 was expected (ok)
                //You get 1 when 0 or 1 was expected (ok)
                //You get 2 or more when 0 or 1 was expected (error)

                if (enroolmentInDatabase == null)
                {
                    //enrollment was not found - add it to db context
                    context.Enrollments.Add(e);
                }
            }//end of foreach
            context.SaveChanges();
        }
    }
}

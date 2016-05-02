using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyTinyCollege.Models;

namespace MyTinyCollege.ViewModels
{
    public class InstructorIndexData
    {
        //For Reading Related Data: Instructors - Courses - Enrollment
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
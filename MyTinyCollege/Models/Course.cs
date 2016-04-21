using System.Collections.Generic;

namespace MyTinyCollege.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        public int DepartmentID { get; set; }//FK to Department

        //============================Navigation Properties============================

        //1 course to many enrollments
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        //1 course to many instructors
        public virtual ICollection<Instructor> Instructors { get; set; }

        //1 course to 1 department
        public virtual Department Department { get; set; }

        //Combine CourseID and Title in one property
        public string CourseIdTitle
        {
            get
            {
                return CourseID + ": " + Title;
            }            
        }

    }
}
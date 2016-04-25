using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTinyCollege.Models
{
    public class Course
    {

        /*
         * By default the ID property will become the Primary Key of the database table
         * that corresponds to this class.  By default the EF (Entity Framework)
         * interprets a property that's name dID of ClassNameID as the PK.
         * Also, this PK will have an INDETITY Property, you can override it usung
         * the DatabaseGeneratedOption enum:
         * - Computed: Database generates a value whne row is inserted or updated
         * - Identity: Database generates a value when row is inserted
         * - None:     Database does not generate values
         */

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Number")]
        public int CourseID { get; set; } //PK - Note: with no Identity Property

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]//Credits can be between 0 and 5
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
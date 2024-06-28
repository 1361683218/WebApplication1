using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public string? CourseCode { get; set; }

    public int? StudentId { get; set; }

    public virtual Course? CourseCodeNavigation { get; set; }

    public virtual Student? Student { get; set; }
}

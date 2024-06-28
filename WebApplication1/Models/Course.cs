using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Course
{
    /// <summary>
    /// 课程代码，主键
    /// </summary>
    public string CourseCode { get; set; } = null!;

    /// <summary>
    /// 课程名称
    /// </summary>
    public string? CourseName { get; set; }

    /// <summary>
    /// 学分
    /// </summary>
    public int? Credits { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

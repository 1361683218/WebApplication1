using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Student
{
    /// <summary>
    /// 学号，主键
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 班级
    /// </summary>
    public string? ClassName { get; set; }

    /// <summary>
    /// 初始密码，默认学号
    /// </summary>
    public string? InitialPassword { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
public partial class Student
{
    /// <summary>
    /// 学号，主键
    /// </summary>
        [DisplayName("学生编号")]
    public int Id { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
        [DisplayName("姓名")]
    public string? Name { get; set; }

    /// <summary>
    /// 班级
    /// </summary>
        [DisplayName("班级")]
    public string? ClassName { get; set; }

    /// <summary>
        /// 初始密码
    /// </summary>
        [DisplayName("初始密码")]
    public string? InitialPassword { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
}
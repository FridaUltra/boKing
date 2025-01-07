using System;
using System.Collections.Generic;

namespace Model;

public partial class Staff
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<CheckInOut> CheckInOutCheckInStaffs { get; set; } = new List<CheckInOut>();

    public virtual ICollection<CheckInOut> CheckInOutCheckOutStaffs { get; set; } = new List<CheckInOut>();
}

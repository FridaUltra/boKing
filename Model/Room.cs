using System;
using System.Collections.Generic;

namespace Model;

public partial class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? RoomType { get; set; }

    public int BedCount { get; set; }

    public int Price { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<RoomToBooking> RoomToBookings { get; set; } = new List<RoomToBooking>();
}

using System;
using System.Collections.Generic;

namespace Model;

public partial class RoomGuest
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public int RoomId { get; set; }

    public int BookingId { get; set; }

    public virtual RoomToBooking RoomToBooking { get; set; } = null!;
}

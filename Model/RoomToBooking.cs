using System;
using System.Collections.Generic;

namespace Model;

public partial class RoomToBooking
{
    public int RoomId { get; set; }

    public int BookingId { get; set; }

    public int? GuestsInRoom { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<RoomGuest> RoomGuests { get; set; } = new List<RoomGuest>();
}

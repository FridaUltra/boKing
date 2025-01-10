using System;
using System.Collections.Generic;

namespace Model;

public partial class Review
{
    public int Id { get; set; }

    public int RoomGuestId { get; set; }

    public int RoomId { get; set; }

    public DateOnly ReviewDate { get; set; }

    public string? Text { get; set; }

    public int Rating { get; set; }

    public virtual Room Room { get; set; } = null!;
    public virtual RoomGuest RoomGuest { get; set; } = null!;
}

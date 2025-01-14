namespace Model;

public partial class Booking
{
    public int Id { get; set; }

    public string BookingNumber { get; set; } = null!;

    public int GuestId { get; set; }

    public int NumberOfPeople { get; set; }

    public DateOnly ArrivalDate { get; set; }

    public DateOnly DepartureDate { get; set; }

    public int NumberOfNights => (DepartureDate.ToDateTime(TimeOnly.MinValue) - ArrivalDate.ToDateTime(TimeOnly.MinValue)).Days;

    public int? TotalPrice { get; set; }

    public virtual Guest Guest { get; set; } = null!;

    public virtual ICollection<RoomToBooking> RoomToBookings { get; set; } = new List<RoomToBooking>();
}

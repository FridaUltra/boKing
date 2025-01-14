namespace Model;

public partial class CheckInOut
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public DateOnly CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public int CheckInStaffId { get; set; }

    public int? CheckOutStaffId { get; set; }

    public virtual Staff CheckInStaff { get; set; } = null!;

    public virtual Staff? CheckOutStaff { get; set; }
}

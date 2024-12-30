using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;

namespace boKing;

public partial class HotelContext : DbContext
{
    public HotelContext()
    {
    }

    public HotelContext(DbContextOptions<HotelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomGuest> RoomGuests { get; set; }

    public virtual DbSet<RoomToBooking> RoomToBookings { get; set; }

    static void LogWithColor(string output)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(output);
        Console.ResetColor();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = File.ReadAllText("connectionString.txt");
        optionsBuilder.UseSqlServer(connectionString).EnableSensitiveDataLogging(true).LogTo(LogWithColor, LogLevel.Information);
    }
        

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC07414E1CAE");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingNumber)
                .HasMaxLength(12)
                .IsUnicode(false);

            entity.HasOne(d => d.Guest).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking__GuestId__4BAC3F29");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Guest__3214EC07D8F2351D");

            entity.ToTable("Guest");

            entity.Property(e => e.Address)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC07FF440A40");

            entity.ToTable("Room");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.RoomType)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoomGuest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomGues__3214EC07EFD59908");

            entity.ToTable("RoomGuest");

            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .IsUnicode(false);

            entity.HasOne(d => d.RoomToBooking).WithMany(p => p.RoomGuests)
                .HasForeignKey(d => new { d.RoomId, d.BookingId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomGuest__5441852A");
        });

        modelBuilder.Entity<RoomToBooking>(entity =>
        {
            entity.HasKey(e => new { e.RoomId, e.BookingId }).HasName("PK__RoomToBo__F5BF68976589A367");

            entity.ToTable("RoomToBooking");

            entity.HasOne(d => d.Booking).WithMany(p => p.RoomToBookings)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomToBoo__Booki__5165187F");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomToBookings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomToBoo__RoomI__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

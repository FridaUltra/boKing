using System.Net.Sockets;
using boKing;
using Model;

internal class Program
{
    private static void Main(string[] args)
    {

        Console.WriteLine("Välkommen till bokningssystemet!");

        while (true)
        {
            Console.WriteLine("\n\n---------------------Huvudmeny---------------------------------\n");
            Console.WriteLine("\nVälj ett alternativ:");
            Console.WriteLine("1. Skapa bokning");
            Console.WriteLine("2. Lista bokningar och visa tillgängliga rum");

            Console.WriteLine("4. Registrera incheckning");
            Console.WriteLine("5. Registrera utcheckning");
            Console.WriteLine("6. Lista rum");
            Console.WriteLine("7. Lägg till rum");
            Console.WriteLine("8. Uppdatera rum");
            Console.WriteLine("9. Ta bort rum");
            Console.WriteLine("10. Lägg till recension");

            Console.WriteLine("0. Avsluta");
            Console.Write("\nDitt val: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("\n\n---------------------Skapa en bokning---------------------------------\n\n");
                    CreateBooking();
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("\n\n---------------------Lista bokningar och visa tillgängliga rum för ett tidsintervall---------------------------------\n\n");
                    Console.Write("Ange startdatum för intervallet (yyyy-mm-dd): ");
                    var startDate = CHelp.ReadDate();
                    Console.Write("Ange slutdatum för intervallet (yyyy-mm-dd): ");
                    var endDate = CHelp.ReadDate();

                    var bookings = GetBookingsForInterval(startDate, endDate);
                    Console.WriteLine($"\n==================Bokningar för intervallet ({bookings.Count})==================\n");
                    PrintBookings(bookings);
                    
                    var availableRooms = GetAllAvailableRoomsForIntervall(startDate, endDate);
                    Console.WriteLine($"\n==================Tillgängliga rum för intervallet ({availableRooms.Count})==================\n");

                    if (!availableRooms.Any())
                    {
                        Console.WriteLine("Inga rum är tillgängliga för det angivna intervallet.");
                        break;
                    }
                    PrintAvailableRooms(availableRooms);

                    Console.WriteLine("\nAntal tillgängliga rum: " + availableRooms.Count);
                    Console.WriteLine("\n\nVill du se detaljer för ett specifikt rum? (ja/nej)");
                    string answer = CHelp.ReadNotEmptyString();
                    if (answer.ToLower() == "ja")
                    {
                        Console.WriteLine("Ange rum-NR:");
                        var roomId = CHelp.ReadInt();
                        var room = availableRooms.FirstOrDefault(r => r.Id == roomId);
                        if (room == null)
                        {
                            Console.WriteLine("Ogiltigt rum-ID.");
                            break;
                        }
                        Console.WriteLine("---------------------Rumdetaljer---------------------------------\n");
                        Console.WriteLine($"\n\nRum-NR: {room.Id}, {room.Name}, Typ: {room.RoomType}, Sängar: {room.BedCount}, Pris: {room.Price}\nBeskrivning: {room.Description}\n");

                        // Visa recensioner för rummet
                        ShowReviewsByRoomId(roomId);
                     
                    }
                    break;
                case "3":
                    AddReview();
                    break;
                case "4":
                    CheckIn();
                    break;
                case "5":
                    CheckOut();
                    break;
                case "6":
                    GetAllRooms();
                    break;
                case "7":
                    AddRoom();
                    break;
                case "8":
                    UpdateRoomInfo();
                    break;
                case "9":
                    DeleteRoom();
                    break;
                case "10":
                    AddReview();
                    break;    
                case "0":
                    return;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }
        }    

    }

    static void GetAllRooms()
    {
        // Hämta alla rum från databasen som inte är raderade
        using var context = new HotelContext();
        IEnumerable<Room> rooms = context.Rooms.Where(r => r.IsDeleted == false);

        foreach (var room in rooms)
        {
            Console.WriteLine(
                $"Id: {room.Id}\n" +
                $"Namn: {room.Name}\n " +
                $"Beskrivning: {room.Description}\n " +
                $"Rumstyp: {room.RoomType}\n " +
                $"Antal sängar: {room.BedCount}\n " +
                $"Pris: {room.Price} \n \n"
            );
        }
    }

    static void UpdateRoomInfo()
    {
        // Frågar användaren om studentens ID att uppdatera
        Console.WriteLine("Ange ID för det rum som du vill uppdatera:");
        int id = CHelp.ReadInt();

        using var context = new HotelContext();

        // Hämtar rummet från databasen
        Room roomToUpdate = context.Rooms.Find(id);
        if (roomToUpdate == null || roomToUpdate.IsDeleted == true)
        {
            Console.WriteLine("Inget rum med angivet ID hittades.");
            return;
        }
        Console.WriteLine($"Nuvarande namn: {roomToUpdate.Name}");
        Console.WriteLine("Ange nytt namn (lämna tomt för att behålla nuvarande):");

        string newName;
        if (!string.IsNullOrEmpty(newName = Console.ReadLine()))
        {
            roomToUpdate.Name = newName;
        }

        Console.WriteLine($"Nuvarande beskrivning: {roomToUpdate.Description}\n");
        Console.WriteLine("Ange ny beskrivning (lämna tomt för att behålla nuvarande):");

        string newDescription;
        if (!string.IsNullOrEmpty(newDescription = Console.ReadLine()))
        {
            roomToUpdate.Description = newDescription;
        }

        Console.WriteLine($"Nuvarande rumstyp: {roomToUpdate.RoomType}");
        Console.WriteLine("Ange ny rumstyp (lämna tomt för att behålla nuvarande):");

        string newRoomType;
        if (!string.IsNullOrEmpty( newRoomType = Console.ReadLine()))
        {
            roomToUpdate.RoomType = newRoomType;
        }

        Console.WriteLine($"Nuvarande antal sängplatser: {roomToUpdate.BedCount}");
        Console.WriteLine("Ange nytt antal sängplatser (lämna tomt för att behålla nuvarande):");

        string newBedCount;
        if (!string.IsNullOrEmpty( newBedCount = Console.ReadLine()))
        {
            //TODO: kanske göra en try parse här.
            int bedCountAsInt = int.Parse(newBedCount);
            roomToUpdate.BedCount = bedCountAsInt;
        }

        Console.WriteLine($"\nNuvarande pris: {roomToUpdate.Price} kr");
        Console.WriteLine("Ange nytt pris (lämna tomt för att behålla nuvarande):");
        string newPrice;
        if (!string.IsNullOrEmpty( newPrice = Console.ReadLine()))
        {
            //TODO: kanske göra en try parse här.
            int priceAsInt = int.Parse(newPrice);
            roomToUpdate.Price = priceAsInt;
        
        }

        context.SaveChanges(); 
        Console.WriteLine("Rummets informationen har uppdaterats.");
    }

    static void AddRoom()
    {
        Console.Write("Ange namn på rummet: ");
        string name = CHelp.ReadNotEmptyString();

        Console.Write("Ange en beskrivning för rummet: ");
        string description = CHelp.ReadNotEmptyString();

        Console.Write("Ange rumstypen: ");
        string roomtype = CHelp.ReadNotEmptyString();

        Console.Write($"Ange antal sängplatser: ");
        int bedCount = CHelp.ReadInt();

        Console.Write($"Ange priset per natt: ");
        int price = CHelp.ReadInt();

        var newRoom = new Room
        {
            Name = name,
            Description = description,
            RoomType = roomtype,
            BedCount = bedCount,
            Price = price
        };

        using var context = new HotelContext();
        context.Rooms.Add(newRoom);
        context.SaveChanges();
        Console.WriteLine($"Rummet har lagts till i databasen och fått id {newRoom.Id}");
    }

    static void DeleteRoom()
    {
        Console.Write("Ange ID för det rum som du vill ta bort: ");
        int id = CHelp.ReadInt();

        using var context = new HotelContext();

         Room roomToDelete = context.Rooms.Find(id);
        if (roomToDelete == null)
        {
            Console.WriteLine("Inget rum med angivet ID hittades.");
            return;
        }

        // Kollar om det finns bokningar kopplade till rummet innan borttagning från dagens datum och framåt

        var bookings = context.RoomToBookings
            .Include(rtb => rtb.Booking)
            .Where(rtb => rtb.RoomId == id && rtb.Booking.ArrivalDate >= DateOnly.FromDateTime(DateTime.Now));

        if (bookings.Any())
        {
            Console.WriteLine("Det finns bokningar kopplade till rummet. Rummet kan inte tas bort.");
            return;
        }    
        
        Console.WriteLine("Vill du ta bort rummet nedan? Klicka j/n");
        Console.WriteLine(
            $"Id: {roomToDelete.Id}\n" +
            $"Namn: {roomToDelete.Name}\n " +
            $"Beskrivning: {roomToDelete.Description}\n " +
            $"Rumstyp: {roomToDelete.RoomType}\n " +
            $"Antal sängar: {roomToDelete.BedCount}\n " +
            $"Pris: {roomToDelete.Price} \n \n"
        );

        string answer = CHelp.ReadNotEmptyString();
        if (answer.ToLower() == "j")
        {
            roomToDelete.IsDeleted = true;
        }
        context.SaveChanges();
        Console.WriteLine("Rummet är nu borttaget");
    }

    static Guest AddGuest()
    {
        Console.WriteLine("Kontrollerar om gästen redan finns...");
        var existingGuest = SearchGuest();

        if (existingGuest != null)
        {
            Console.WriteLine("Gäst hittades:");
            Console.WriteLine($"Id: {existingGuest.Id}, {existingGuest.Name}, {existingGuest.Email}");
            Console.WriteLine("Vill du använda denna gäst? (ja/nej): ");
            string useExisting = CHelp.ReadNotEmptyString();
            if (useExisting.ToLower() == "ja")
            {
                Console.WriteLine("Existerande gäst valdes.");
                return existingGuest;
            }
        }

        Console.WriteLine("Ange för och efternamn:");
        string name = CHelp.ReadNotEmptyString();

        Console.WriteLine("Ange gatuadress:");
        string address = CHelp.ReadNotEmptyString();

        Console.WriteLine("Ange Epost:");
        string email = CHelp.ReadNotEmptyString();

        Guest newGuest = new()
        {
            Name = name,
            Address = address,
            Email = email
        };

        using var context = new HotelContext();
        context.Guests.Add(newGuest);
        context.SaveChanges();
        Console.WriteLine($"Gästen har lagts till och fått id {newGuest.Id}");
        return newGuest;
    }

    static void CreateBooking()
    {
        using var context = new HotelContext();

        Console.WriteLine("=====Bokningsförmulär=====\n");
        Console.WriteLine("Ange... \n");
        Console.Write("Ankomstdatum (yyyy-mm-dd): ");
        var arrivalDate = CHelp.ReadDate();
        Console.Write("Avresedatum (yyyy-mm-dd): ");
        var departureDate = CHelp.ReadDate();
        Console.Write("Antal rum: ");
        var numberOfRooms = CHelp.ReadInt();
        Console.Write("Antal personer: ");
        var numberOfPeople = CHelp.ReadInt();

        // Nästa steg är att välja antalet rum som ska bokas.
        // Tillgängliga rum visas och användaren ska välja vilka rum som ska bokas.
        // Rummen ska kopplas till bokningen och antalet gäster i varje rum ska anges först vid incheckning.
        // Kunden ska sedan fylla i sina uppgifter och bokningen ska sparas i databasen.

        var availableRooms = GetAllAvailableRoomsForInterval(arrivalDate, departureDate);
        if (!availableRooms.Any())
        {
            Console.WriteLine("Inga rum är tillgängliga för det angivna datumintervallet.");
            Console.WriteLine("Bokningen avbryts.");
            Thread.Sleep(2000);
            return;
        }
        PrintAvailableRooms(availableRooms);

        // Lista som sparar objektet RoomToBooking
        var roomsToBook = new List<RoomToBooking>();
        // Räkna ut antal nätter
        int numberOfNights = (departureDate.ToDateTime(TimeOnly.MinValue) - arrivalDate.ToDateTime(TimeOnly.MinValue)).Days;
        //Array som sparar rumId som användaren valt att boka
        var arrayOfRoomIds = new int[numberOfRooms];
        // Loop som körs för varje rum som ska bokas
        for (int i = 0; i < numberOfRooms; i++)
        {
            //Översikt över bokningens val hittills
            Console.Clear();
            Console.WriteLine("=====Bokningsformulär=====\n");
            Console.WriteLine("Ankomstdatum: " + arrivalDate);
            Console.WriteLine("Avresedatum: " + departureDate);
            Console.WriteLine("Antal nätter: " + numberOfNights);
            Console.WriteLine("Antal rum: " + numberOfRooms);
            Console.WriteLine("Antal personer: " + numberOfPeople);
            Console.WriteLine("Antal rum valda: " + i);

            Console.WriteLine("\n=====Välj rum att boka=====\n");
            PrintAvailableRooms(availableRooms);

            Console.Write($"\nAnge rum {i + 1}: ");
            arrayOfRoomIds[i] = CHelp.ReadInt();

            // Kollar om användare valt ett rum som finns i listan över tillgängliga rum
            var chosenRoom = availableRooms.FirstOrDefault(r => r.Id == arrayOfRoomIds[i]);
            if (chosenRoom == null)
            {
                Console.WriteLine("Ogiltigt rumnummer.");
                Thread.Sleep(2000);
                arrayOfRoomIds[i] = 0;
                i--;
                continue;
            }
            roomsToBook.Add(new RoomToBooking
            {
                RoomId = chosenRoom.Id,
            });

            // tar bort rum från listan över tillgängliga rum
            availableRooms.RemoveAll(r => r.Id == arrayOfRoomIds[i]);
        }
        
        // Ny översikt över bokningens val hittills
        Console.Clear();
        Console.WriteLine("=====Bokningsformulär=====\n");
        Console.WriteLine("Ankomstdatum: " + arrivalDate);
        Console.WriteLine("Avresedatum: " + departureDate);
        Console.WriteLine("Antal nätter: " + numberOfNights);
        Console.WriteLine("Antal rum: " + numberOfRooms);
        Console.WriteLine("Antal personer: " + numberOfPeople);
        Console.WriteLine("\n=====Valda rum=====\n");
        for (int i = 0; i < numberOfRooms; i++) // Ritar ut alla valda rum
        {
            var room = context.Rooms.Find(arrayOfRoomIds[i]);
            Console.WriteLine($"Rum {i + 1}: {room.Name}, Typ: {room.RoomType}, Sängar: {room.BedCount}, Pris: {room.Price} kr");
        }

        Console.WriteLine("\n=====Kundformulär=====\n");
        Console.Write("Namn: ");
        var name = CHelp.ReadNotEmptyString();
        Console.Write("Adress: ");
        var address = CHelp.ReadNotEmptyString();
        Console.Write("E-post: ");
        var email = CHelp.ReadNotEmptyString();

        //Räkna ut pris för bokningen
        int totalPrice = CalculateTotalPrice(roomsToBook, numberOfNights);
        // Skapa ny bokning
        var newBooking = new Booking
        {
            ArrivalDate = arrivalDate,
            DepartureDate = departureDate,
            NumberOfPeople = numberOfPeople,
            TotalPrice = totalPrice,
            BookingNumber = Guid.NewGuid().ToString().Substring(0, 8),
            RoomToBookings = roomsToBook,
            Guest = new Guest
            {
                Name = name,
                Address = address,
                Email = email
            }
        };

        context.Bookings.Add(newBooking);
        context.SaveChanges();


        Console.Clear();
        Console.WriteLine("=====Bokningsbekräftelse=====\n");
        Console.WriteLine("Ankomstdatum: " + arrivalDate);
        Console.WriteLine("Avresedatum: " + departureDate);
        Console.WriteLine("Antal nätter: " + numberOfNights);
        Console.WriteLine("Antal rum: " + numberOfRooms);
        Console.WriteLine("Antal personer: " + numberOfPeople);
        Console.WriteLine("Totalpris: " + totalPrice + " kr");
        Console.WriteLine("\n=====Valda rum=====\n");
        for (int i = 0; i < numberOfRooms; i++) // Ritar ut alla valda rum
        {
            var room = context.Rooms.Find(arrayOfRoomIds[i]);
            Console.WriteLine($"Rum {i + 1}: {room.Name}, Typ: {room.RoomType}, Sängar: {room.BedCount}, Pris: {room.Price} kr");
        }

        Console.WriteLine("\n=====Faktureringsuppgifter=====\n");
        Console.WriteLine("Namn: " + name);
        Console.WriteLine("Adress: " + address);
        Console.WriteLine("E-post: " + email);
    }

    static Guest? SearchGuest()
    {
        Console.WriteLine("1. Sök med namn");
        Console.WriteLine("2. Sök med e-post");
        Console.WriteLine("3. Sök med bokningsnummer \n");
        Console.Write("Välj ett alternativ: ");
        string choice = Console.ReadLine()!;

        using var context = new HotelContext();
        IQueryable<Guest> query = context.Guests;
        

        switch (choice)
        {
            case "1":
                Console.Write("Ange namn: ");
                string name = CHelp.ReadNotEmptyString();
                query = query.Where(g => g.Name.Contains(name));
                break;

            case "2":
                Console.Write("Ange e-post: ");
                string email = CHelp.ReadNotEmptyString();
                query = query.Where(g => g.Email == email);
                break;

            case "3":
                Console.Write("Ange bokningsnummer: ");
                string bookingNr = CHelp.ReadNotEmptyString();
                
                var booking = context.Bookings.FirstOrDefault(b => b.BookingNumber == bookingNr);
                if (booking == null)
                {
                    Console.WriteLine("Ingen bokning hittades.");
                    return null;
                }
                query = query.Where(g => g.Id == booking.GuestId);
                break;

            default:
                Console.WriteLine("Ogiltigt val.");
                return null;
        }

        // Hämta matchande gäster
        var guests = query.ToList();

        if (guests.Count == 0)
        {
            Console.WriteLine("Ingen gäst hittades.");
            return null;
        }

        // Om flera gäster hittas, visa val
        if (guests.Count > 1)
        {
            Console.WriteLine("Flera gäster hittades. Välj en:");
            for (int i = 0; i < guests.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {guests[i].Name} - {guests[i].Email}");
            }

            Console.Write("Välj en gäst (ange nummer): ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= guests.Count)
            {
                return guests[index - 1];
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
                return null;
            }
        }

        // Om bara en gäst hittas, returnera den
        return guests[0];
    }

    static List<Room> GetAllAvailableRoomsForIntervall(DateOnly startDate, DateOnly endDate)
    {
        // Hämta alla rum som är tillgängliga under det angivna datumintervallet
        using var context = new HotelContext();
        var availableRooms = context.Rooms
            .Where(r => r.RoomToBookings.All(rtb => rtb.Booking.ArrivalDate > endDate || rtb.Booking.DepartureDate <= startDate))
            .ToList();

        availableRooms.RemoveAll(r => r.IsDeleted == true);    

        return availableRooms;
    }

    static void PrintAvailableRooms(List<Room> availableRooms)
    {
        foreach (var room in availableRooms)
        {
            Console.WriteLine(
            $"Rum NR: {room.Id}, {room.Name}, Typ: {room.RoomType}, " +
            $"Sängar: {room.BedCount}, Pris: {room.Price} kr");
        }
    }

    static int CalculateTotalPrice(ICollection<RoomToBooking> roomToBookings, int numberOfNights)
    {
        using var context = new HotelContext();
        int totalPrice = 0;
        foreach (var rtb in roomToBookings)
        {
            var room = context.Rooms.Find(rtb.RoomId);
            if (room != null)
            {
                totalPrice += room.Price * numberOfNights;
            }
        }
        return totalPrice;
    }
    
    static void CheckIn()
    {
        Console.WriteLine("Ange bokningsnummer:");
        string bookingNumber = CHelp.ReadNotEmptyString();

        using var context = new HotelContext();
        // hämtar bokning baserat på bokningsnummer och inkluderar gäst och rumkopplingsinformation
        var booking = context.Bookings.Include(b => b.Guest).Include(b => b.RoomToBookings).FirstOrDefault(b => b.BookingNumber == bookingNumber);
        if (booking == null)
        {
            Console.WriteLine("Ingen bokning hittades.");
            return;
        }
        Console.WriteLine($"Bokning hittades:\nBokningsnummer: {booking.BookingNumber}\n{booking.Guest.Name}\nAnkomstdatum: {booking.ArrivalDate}\nAvresedatum: {booking.DepartureDate}\nNamn på gäst: {booking.Guest.Name}");
        
        // Om arrivaldate INTE är samma som dagens datum, avbryt incheckning
        if (booking.ArrivalDate != DateOnly.FromDateTime(DateTime.Now))
        {
            Console.WriteLine("Incheckning kan endast registreras på ankomstdagen.");
            return;
        }
        
        // Hämtar alla rum kopplade till bokningen
        foreach (var rtb in booking.RoomToBookings)
        {
            var room = context.Rooms.Find(rtb.RoomId);
            if (room != null)
            {
                rtb.Room = room;
                Console.WriteLine($"Rum: {room.Name}, Antal gäster: {rtb.GuestsInRoom}");
            }
        }

        // loop som kör tills man angett personal som finns registrerad
        Staff staff = null;
        int staffId;
        while (true)
        {
            Console.WriteLine("Ange ID för personal som registrerar incheckning:");
            staffId = CHelp.ReadInt();
            staff = context.Staff.Find(staffId);
            if (staff == null)
            {
                Console.WriteLine("Ogiltigt personal-ID. Försök igen.");
                continue;
            }
            break;
        }

        CheckInOut checkIn = new()
        {
            BookingId = booking.Id,
            CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            CheckInStaffId = staffId
        };

        context.CheckInOuts.Add(checkIn);
        context.SaveChanges();

        if (booking.TotalPrice == null)
        {
            booking.TotalPrice = CalculateTotalPrice(booking.RoomToBookings, booking.NumberOfNights);
            context.SaveChanges();
        }

        Console.WriteLine("Incheckning registrerad.");
        Console.WriteLine($"Totalpris: {booking.TotalPrice}");
    }

    static void CheckOut()
    {
        Console.WriteLine("Ange bokningsnummer:");
        var bookingnumber = CHelp.ReadNotEmptyString();

        using var context = new HotelContext();
        var booking = context.Bookings
            .Include(b => b.Guest)
            .Include(b => b.RoomToBookings)
            .FirstOrDefault(b => b.BookingNumber == bookingnumber);

        if (booking == null)
        {
            Console.WriteLine("Ingen bokning hittades.");
            return;
        }    


        var checkOut = context.CheckInOuts
            .FirstOrDefault(c => c.BookingId == booking.Id);

        if (checkOut == null)
        {
            Console.WriteLine("Ingen Incheckning hittades.");
            return;
        }

        if (checkOut.CheckOutDate != null)
        {
            Console.WriteLine("Gästen har redan checkat ut.");
            return;
        }

        Console.WriteLine("Ange personal-ID för utcheckning:");
        var staffId = CHelp.ReadInt();
        
        checkOut.CheckOutDate = DateOnly.FromDateTime(DateTime.Now);
        checkOut.CheckOutStaffId = staffId;

        context.SaveChanges();

        Console.WriteLine("Utcheckning registrerad!");
    }

    static List<Booking> GetBookingsForInterval(DateOnly startDate, DateOnly endDate)
    {
        using var context = new HotelContext();
        var bookings = context.Bookings
            .Where(b => b.ArrivalDate <= endDate && b.DepartureDate >= startDate)
            .OrderBy(b => b.ArrivalDate)
            .Include(b => b.Guest)
            .Include(b => b.RoomToBookings)
            .ThenInclude(rtb => rtb.Room)
            .ToList();

        return bookings;
    }

    static void PrintBookings(List<Booking> bookings)
    {
        foreach (var booking in bookings)
        {
            Console.WriteLine($"Bokningsnummer: {booking.BookingNumber}");
            Console.WriteLine($"Gäst: {booking.Guest.Name}");
            Console.WriteLine($"Ankomstdatum: {booking.ArrivalDate}");
            Console.WriteLine($"Avresedatum: {booking.DepartureDate}");
            Console.WriteLine($"Antal personer: {booking.NumberOfPeople}");
            Console.WriteLine($"Totalpris: {booking.TotalPrice}");
            Console.WriteLine("Rum kopplade till bokningen:");
            foreach (var rtb in booking.RoomToBookings)
            {
                Console.WriteLine($"-NR {rtb.Room.Id}, {rtb.Room.Name}, Antal sängar: {rtb.Room.BedCount}");
            }
            Console.WriteLine();
        }
    }

    static void AddReview()
    {
        Console.WriteLine("Ange bokningsnummer:");
        var bookingNumber = CHelp.ReadNotEmptyString();

        using var context = new HotelContext();
        var booking = context.Bookings
            .Include(b => b.Guest)
            .Include(b => b.RoomToBookings)
            .ThenInclude(rtb => rtb.Room)
            .FirstOrDefault(b => b.BookingNumber == bookingNumber);
        
        if (booking == null)
        {
            Console.WriteLine("Ingen bokning hittades.");
            return;
        }

        // Visa alla rum kopplade till bokningen
        Console.WriteLine("Välj rum som ska recenseras:");
        foreach (var rtb in booking.RoomToBookings)
        {
            Console.WriteLine($"-NR {rtb.Room.Id}, {rtb.Room.Name}, Typ: {rtb.Room.RoomType}");
        }

        // Måste välja ett rum att recensera som faktiskt finns i bokningen
        var roomId = CHelp.ReadInt();
        var roomToReview = booking.RoomToBookings.FirstOrDefault(rtb => rtb.RoomId == roomId);
        if (roomToReview == null)
        {
            Console.WriteLine("Ogiltigt rum-ID.");
            return;
        }

        // Kontrollera antalet reviews lämnade på detta rummet för den bokningen
        var reviews = context.Reviews.Where(r => r.BookingId == booking.Id && r.RoomId == roomId);
        if (reviews.Count() == roomToReview.GuestsInRoom)
        {
            Console.WriteLine("Rummet har recenserats av alla gäster.");
            return;
        }

        // Hämta antalet gäster registrerade i det rummet.
        var reviewsLeftToGive = roomToReview.GuestsInRoom - reviews.Count();
        Console.WriteLine($"{roomToReview.GuestsInRoom} st gäster registrerade för detta rum.");

        // Lista för att lagra recensioner
        List<Review> reviewsList = new();

        while (true)
        {
            if(reviewsLeftToGive == 0)
            {
                Console.WriteLine("Alla recensioner har lämnats.");
                break;
            }
            else
            {
                Console.WriteLine("Recensioner kvar att lämna: " + reviewsLeftToGive);
            }


            Console.WriteLine("Vill du lämna en recension? (ja/nej)");
            string answer = CHelp.ReadNotEmptyString();
            if (answer.ToLower() == "nej")
            {
                break;
            }
            else if (answer.ToLower() != "ja")
            {
                Console.WriteLine("Ogiltigt svar.");
                continue;
            }

            // Måste ange ett betyg mellan 1-5
            int rating;
            do
            {
                Console.WriteLine("Ange betyg (1-5):");
                rating = CHelp.ReadInt();
                if (rating < 1 || rating > 5)
                {
                    Console.WriteLine("Ogiltigt betyg.");
                } 
                
            } while (rating < 1 || rating > 5);
            

            Console.WriteLine("Ange recension:");
            var text  = CHelp.ReadNotEmptyString();

            Console.WriteLine("Ange namn på person som recenserar:");
            var name = CHelp.ReadNotEmptyString();

            // Skapa recensionen
            var review = new Review
            {   
                BookingId = booking.Id,
                RoomId = roomId,
                Name = name,
                ReviewDate = DateOnly.FromDateTime(DateTime.Now),
                Text = text,
                Rating = rating
            };

            reviewsList.Add(review);
            reviewsLeftToGive--;
        }
       
        context.Reviews.AddRange(reviewsList);

        context.SaveChanges();
        if(reviewsList.Count == 1)
        {
            Console.WriteLine("Recensionen har lagts till.");
        }
        else
        {
            Console.WriteLine("Recensioner har lagts till.");
        }
    }

    static void ShowReviewsByRoomId(int roomId)
    {
        using var context = new HotelContext();
        var reviews = context.Reviews.Where(r => r.RoomId == roomId);
        if (reviews.Any())
        {
            Console.WriteLine("Recensioner:");
            foreach (var review in reviews)
            {
                Console.WriteLine($"- {review.Name}, Betyg: {review.Rating}, Datum: {review.ReviewDate}, {review.Text}");
            }
        }
        else
        {
            Console.WriteLine("Inga recensioner hittades.");
        }

    }

}


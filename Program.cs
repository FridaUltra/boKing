using boKing;
using Model;

internal class Program
{
    private static void Main(string[] args)
    {
        UpdateRoomInfo();
        // GetAllRooms();
        // AddRoom();
        // DeleteRoom();
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
}

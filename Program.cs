using boKing;
using Model;

internal class Program
{
    private static void Main(string[] args)
    {
        GetAllRooms();
    }

    static void GetAllRooms()
    {
        // Skapar en instans av HotelContext för att interagera med databasen
        // Hämta alla rum från databasen som en lista
        using var context = new HotelContext();
        IEnumerable<Room> rooms = context.Rooms;

        // Itererar över varje rum i listan (som nyss hämtades ur databasen) och skriver ut deras information
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
}
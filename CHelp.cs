static class CHelp
{
    public static int ReadInt()
    {
        int input;
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.WriteLine("Ogiltig inmatning, försök igen:");
        }
        return input;
    }

    public static DateOnly ReadDate()
    {
        DateOnly input;
        while (!DateOnly.TryParse(Console.ReadLine(), out input))
        {
            Console.WriteLine("Ogiltigt datumformat, försök igen (åååå-mm-dd):");
        }
        return input;
    }

    public static string ReadNotEmptyString()
    {
        string input;
        while (string.IsNullOrEmpty(input = Console.ReadLine()))
        {
            Console.WriteLine("Ogiltig inmatning, försök igen:");
        }
        return input;
    }

}
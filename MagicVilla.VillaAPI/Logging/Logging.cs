namespace MagicVilla.VillaAPI.Logging
{
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            if(type.ToLower() == "error")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: " + message);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                if(type.ToLower() == "warming")
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Warning: " + message);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.WriteLine(message);
                }
                
            }
        }
    }
}

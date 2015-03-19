using System;

namespace SeaChase
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SeaChaseGame game = new SeaChaseGame())
            {
                game.Run();
            }
        }
    }
#endif
}


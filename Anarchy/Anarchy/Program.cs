using System;

namespace Anarchy
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Anarchy game = new Anarchy())
            {
                game.Run();
            }
        }
    }
#endif
}


using System;

namespace MrGuyLevelEditor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		[STAThreadAttribute]
        static void Main(string[] args)
        {
            using (Editor game = new Editor())
            {
                game.Run();
            }
        }
    }
#endif
}


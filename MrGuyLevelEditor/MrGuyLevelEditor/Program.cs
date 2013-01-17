using System;

namespace MrGuyLevelEditor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (EditorGUI game = new EditorGUI())
            {
                game.Run();
            }
        }
    }
#endif
}


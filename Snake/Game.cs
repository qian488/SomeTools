using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    enum ESceneType
    {
        Begin,
        Game,
        End,
    }
    class Game
    {
        public const int width = 80;
        public const int height = 20;

        public static ISceneUpdate nowScene;

        public Game() 
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            DisableResizeAndMaximize(); // 禁用最大化和窗口大小调整

            ChangeScene(ESceneType.Begin);
        }

        public void Start()
        {
            while (true)
            {
                if (nowScene != null)
                {
                    nowScene.Update();
                }
            }
        }

        public static void ChangeScene(ESceneType type)
        {
            Console.Clear();

            switch(type)
            {
                case ESceneType.Begin:
                    nowScene = new BeginScene();
                    break;
                case ESceneType.End:
                    nowScene = new EndScene();
                    break;
                case ESceneType.Game:
                    nowScene = new GameScene();
                    break;
            }
        }

        // Windows API functions
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DrawMenuBar(IntPtr hWnd);

        // Constants for system menu commands
        const uint SC_SIZE = 0xF000;       // "Size" option
        const uint SC_MAXIMIZE = 0xF030;   // "Maximize" option
        const uint MF_BYCOMMAND = 0x00000000;

        // Disable resizing and maximizing the console window
        public static void DisableResizeAndMaximize()
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            // Remove the "Size" and "Maximize" options from the system menu
            RemoveMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);      // Remove "Size" option
            RemoveMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);  // Remove "Maximize" option

            // Redraw the menu bar to reflect the changes
            DrawMenuBar(handle);
        }

    }
}

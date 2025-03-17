using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class InputThread
    {
        private Thread inputThread;
        public event Action inputEvent;

        private static InputThread instance = new InputThread();

        public static InputThread Instance
        {
            get
            {
                return instance;
            }
        }

        private InputThread()
        {
            inputThread = new Thread(CheckIpnut);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private void CheckIpnut()
        {
            while (true)
            {
                inputEvent?.Invoke();
            }
        }


    }
}

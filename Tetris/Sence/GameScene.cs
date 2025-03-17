using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class GameScene : ISceneUpdate
    {
        BlockWorker blockWorker;
        Map map;

        public GameScene()
        {
            map = new Map(this);
            blockWorker = new BlockWorker(map);
            InputThread.Instance.inputEvent += CheckIpnut;
        }

        private void CheckIpnut()
        {
            if (Console.KeyAvailable)
            {
                lock (blockWorker)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (blockWorker.CanChange(E_Change_Type.Left)) blockWorker.Change(E_Change_Type.Left);
                            break;
                        case ConsoleKey.RightArrow:
                            if (blockWorker.CanChange(E_Change_Type.Right)) blockWorker.Change(E_Change_Type.Right);
                            break;
                        case ConsoleKey.A:
                            if (blockWorker.CanMoveLR(E_Change_Type.Left)) blockWorker.MoveLR(E_Change_Type.Left);
                            break;
                        case ConsoleKey.D:
                            if (blockWorker.CanMoveLR(E_Change_Type.Right)) blockWorker.MoveLR(E_Change_Type.Right);
                            break;
                        case ConsoleKey.S:
                            if (blockWorker.CanAutoMoveDown()) blockWorker.AutoMoveDown();
                            break;
                    }
                }

            }
        }

        public void StopInputThread()
        {
            InputThread.Instance.inputEvent -= CheckIpnut;
        }

        public void Update()
        {
            lock (blockWorker)
            {
                map.Draw();
                blockWorker.Draw();

                if (blockWorker.CanAutoMoveDown())
                {
                    blockWorker.AutoMoveDown();
                }

            }

            Thread.Sleep(200);

        }
    }
}

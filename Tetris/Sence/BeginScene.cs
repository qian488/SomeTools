using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class BeginScene : BeginOrEndBaseScene
    {
        public BeginScene()
        {
            strTitle = "Tetris";
            strOne = "Start Game";
        }
        public override void EnterJKeyDo()
        {
            if (nowSelIndex == 0)
            {
                Game.ChangeScene(ESceneType.Game);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}

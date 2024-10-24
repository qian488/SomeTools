using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class BeginScene : BeginOrEndBaseScene
    {
        public BeginScene()
        {
            strTitle = "Snake";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class EndScene : BeginOrEndBaseScene
    {
        public EndScene()
        {
            strTitle = "Snake";
            strOne = "Rebirth";
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

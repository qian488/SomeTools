using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    abstract internal class GameObject : IDraw
    {
        public Position pos;
        // 实现接口 但是将接口的行为交给子类实现
        public abstract void Draw();
    }
}

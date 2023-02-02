using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    abstract class Controller
    {
        protected int index;

        public Controller(int controllerIndex)
        {
            index = controllerIndex;
        }

        public abstract bool IsFirePressed();
        public abstract bool IsJumpPressed();
        public abstract float GetHorizontal();
        public abstract float GetVertical();
    }
}

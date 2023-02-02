using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class KeyboardController : Controller
    {
        protected KeysList keysConfig;
        public KeyboardController(int controllerIndex, KeysList keys) : base(controllerIndex)
        {
            keysConfig = keys;
        }

        public override float GetHorizontal()
        {
            float direction = 0;

            if (Game.Window.GetKey(keysConfig.GetKey(KeyName.Right)))
            {
                direction = 1;
            }
            else if (Game.Window.GetKey(keysConfig.GetKey(KeyName.Left)))
            {
                direction = -1;
            }

            return direction;
        }

        public override float GetVertical()
        {
            float direction = 0;

            if (Game.Window.GetKey(keysConfig.GetKey(KeyName.Up)))
            {
                direction = -1;
            }
            else if (Game.Window.GetKey(keysConfig.GetKey(KeyName.Down)))
            {
                direction = 1;
            }

            return direction;
        }

        public override bool IsFirePressed()
        {
            return Game.Window.GetKey(keysConfig.GetKey(KeyName.Fire));
        }

        public override bool IsJumpPressed()
        {
            return Game.Window.GetKey(keysConfig.GetKey(KeyName.Jump));
        }
    }
}

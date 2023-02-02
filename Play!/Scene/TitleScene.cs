using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class TitleScene : Scene
    {
        protected Sprite background;
        protected Texture backgroundTexture;
        protected string backgroundTexturePath;
        protected KeyCode exitKey;

        public TitleScene(string bgTexturePath, KeyCode exitKey=KeyCode.Return)
        {
            backgroundTexturePath = bgTexturePath;
            this.exitKey = exitKey;
        }

        public override void Start()
        {
            background = new Sprite(Game.Window.Width, Game.Window.Height);
            backgroundTexture = new Texture(backgroundTexturePath);

            base.Start();
        }

        public override void Draw()
        {
            background.DrawTexture(backgroundTexture);
        }

        public override void Input()
        {
            if (Game.Window.GetKey(exitKey))
            {
                IsPlaying = false;
            }
        }

        public override Scene OnExit()
        {
            background = null;
            backgroundTexture = null;

            return base.OnExit();
        }
    }
}

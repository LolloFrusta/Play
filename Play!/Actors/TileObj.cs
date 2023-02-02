using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using Play_;

namespace TiledPlugin
{
    class TileObj : GameObject
    {
        private int texOffX;
        private int texOffY;
        private int texWidth;
        private int texHeight;

        public TileObj(string texture,
           int tOffX, int tOffY,
           int tWidth, int tHeight,

           float posX, float posY,
           float width, float height) 
            : base(texture, DrawLayer.Background, width, height)
        {
            //sprite = new Sprite(width, height);
            sprite.position.X = posX;
            sprite.position.Y = posY;
            texOffX = tOffX;
            texOffY = tOffY;
            texWidth = tWidth;
            texHeight = tHeight;

            IsActive = true;
        }

        public override void Draw()
        {
            if (!IsActive) return;
            sprite.DrawTexture(texture, texOffX, texOffY, texWidth, texHeight);
        }
    }
}

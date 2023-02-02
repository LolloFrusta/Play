using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class GameObject: IUpdatable,IDrawable
    {
        protected Sprite sprite;
        protected Texture texture;

        public RigidBody RigidBody;

        public virtual Vector2 Position
        {
            get { return sprite.position; }
            set { sprite.position = value; }
        }

        public float X { get { return sprite.position.X; } set { sprite.position.X = value; } }
        public float Y { get { return sprite.position.Y; } set { sprite.position.Y = value; } }

        public Vector2 Forward
        {
            get
            {
                return new Vector2((float)Math.Cos(sprite.Rotation), (float)Math.Sin(sprite.Rotation));
            }
            set
            {
                sprite.Rotation = (float)Math.Atan2(value.Y, value.X);
            }
        }

        public float HalfWidth { get; protected set; }
        public float HalfHeight { get; protected set; }

        public DrawLayer Layer { get; protected set; }

        public bool IsActive;

        public GameObject(string textureName, DrawLayer layer = DrawLayer.Playground,  float w = 0, float h = 0)
        {
            texture = GfxMgr.GetTexture(textureName);
            sprite = new Sprite(w == 0 ? Game.PixelsToUnits(texture.Width) : w, h == 0 ? Game.PixelsToUnits(texture.Height) : h);
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);

            HalfWidth = sprite.Width * 0.5f;
            HalfHeight = sprite.Height * 0.5f;

            Layer = layer;

            UpdateMgr.AddItem(this);
            DrawMgr.AddItem(this);
        }

        public virtual void Update()
        {
            
        }

        public virtual void OnCollide(Collision collisionInfo)
        {

        }

        public virtual void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture);
            }
        }

        public virtual void Destroy()
        {
            sprite = null;
            texture = null;

            UpdateMgr.RemoveItem(this);
            DrawMgr.RemoveItem(this);

            //rigidbody
            if (RigidBody != null)
            {
                RigidBody.Destroy();
                RigidBody = null;
            }
        }

        //~GameObject()
        //{
        //    Console.WriteLine("Distruttore chiamato su oggetto"+this.GetType());
        //}

    }
}

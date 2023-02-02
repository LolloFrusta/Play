using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Play_
{
    abstract class Actor : GameObject
    {
        protected BulletType bulletType;
        protected int energy;

        public int Energy { get { return energy; } protected set { energy = value; energyBar.Scale((float)energy / (float)MaxEnergy); } }
        public int MaxEnergy { get; protected set; }

        protected float nextShoot;

        protected ProgressBar energyBar;


        protected Actor(string textureName, DrawLayer layer = DrawLayer.Playground, float w = 0, float h = 0)
            : base(textureName, layer, w, h)
        {
            float unitDist = Game.PixelsToUnits(4);
            energyBar = new ProgressBar("barFrame", "blueBar", new Vector2(unitDist));
            energyBar.IsActive = true;
            energy = MaxEnergy = 100;
            //energyBar.Position = new Vector2(Position.X - energyBar.HalfWidth, Position.Y - HalfHeight - energyBar.HalfHeight * 3);
        }

        public override void Update()
        {
            if (IsActive)
            {
                if (RigidBody.Velocity != Vector2.Zero)
                {
                    Forward = RigidBody.Velocity;
                }

                energyBar.Position = new Vector2(Position.X - energyBar.HalfWidth, Position.Y - HalfHeight - energyBar.HalfHeight * 3);

            }
        }

        protected virtual void Shoot(Vector2 direction)
        {
            Bullet b = BulletsMgr.GetBullet(bulletType);

            if (b != null)
            {
                b.IsActive = true;
                b.Shoot(Position, direction);
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            //Tile collision
            OnWallCollides(collisionInfo);
        }

        protected virtual void OnWallCollides(Collision collisionInfo)
        {
            if (collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                //horizontal collision
                if (X < collisionInfo.Collider.X)
                {
                    //collision from left
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }
                X += collisionInfo.Delta.X;
                RigidBody.Velocity.X = 0;

            }
            else
            {
                //vertical collision
                if (Y < collisionInfo.Collider.Y)
                {
                    //collision from top
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                    RigidBody.Velocity.Y = 0;
                }
                else
                {
                    //collision from bottom
                    RigidBody.Velocity.Y = -RigidBody.Velocity.Y * 0.8f;

                }
                Y += collisionInfo.Delta.Y;
            }
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;
            if (Energy <= 0)
            {
                Energy = 0;
                OnDie();
            }
        }

        public virtual void AddEnergy(int amount)
        {
            Energy = Math.Min(Energy + amount, MaxEnergy);
        }

        public abstract void OnDie();
    }
}

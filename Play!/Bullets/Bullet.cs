using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_

{
    enum BulletType { PlayerBullet, EnemyBullet, LAST }

    abstract class Bullet : GameObject
    {
        protected float speed = 10;
        protected int damage = 25;
        public BulletType Type { get; protected set; }

        public Bullet(string textureName, float w = 0, float h = 0) : base(textureName, DrawLayer.Middleground, w, h)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
        }

        public virtual void Shoot(Vector2 startPosition, Vector2 direction)
        {
            Position = startPosition;
            RigidBody.Velocity = direction * speed;
        }

        public override void Update()
        {
            if (IsActive)
            {
                Vector2 centerDist = Position - Game.ScreenCenter;
                if (centerDist.LengthSquared > Game.ScreenDiagonalSquared)
                {
                    BulletsMgr.RestoreBullet(this);
                }
            }
        }
    }
}

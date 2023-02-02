using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class PlayerBullet : Bullet
    {
        public PlayerBullet() : base("bullet")
        {
            Type = BulletType.PlayerBullet;
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.Tile);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if(collisionInfo.Collider is Enemy)
            {
                ((Enemy)collisionInfo.Collider).AddDamage(damage);
            }
            BulletsMgr.RestoreBullet(this);
        }
    }
}

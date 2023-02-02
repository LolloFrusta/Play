using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet() : base("bullet")
        {
            Type = BulletType.EnemyBullet;
            RigidBody.Type = RigidBodyType.EnemyBullet;
        }
    }
}

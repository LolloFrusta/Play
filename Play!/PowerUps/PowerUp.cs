using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class PowerUp : GameObject
    {
        public PowerUp() : base("powerUp", DrawLayer.Middleground)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.PowerUp;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.Enemy);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            ((Actor)collisionInfo.Collider).AddEnergy(25);
            IsActive = false;
        }
    }
}

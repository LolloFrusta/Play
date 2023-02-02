using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class Block : GameObject
    {
        private float blockUnitWidth;
        private float blockUnitHeight;

        public Block(float blockUnitWidth, float blockUnitHeight)
            : base("crate", DrawLayer.Background, blockUnitWidth, blockUnitHeight)
        {
            IsActive = true;
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Tile;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Player);
            RigidBody.AddCollisionType(RigidBodyType.Enemy);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}

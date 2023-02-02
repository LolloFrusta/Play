using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class BoxCollider : Collider
    {
        protected float halfWidth;
        protected float halfHeight;

        public float Width { get { return halfWidth * 2; } }
        public float Height { get { return halfHeight * 2; } }

        public BoxCollider(RigidBody owner, float w, float h) : base(owner)
        {
            halfWidth = w * 0.5f;
            halfHeight = h * 0.5f;
        }

        public override bool Collides(Collider collider, ref Collision collisionInfo)
        {
            return collider.Collides(this, ref collisionInfo);
        }

        public override bool Collides(CircleCollider circle, ref Collision collisionInfo)
        {
            float deltaX = circle.Position.X -
                Math.Max(Position.X - halfWidth,
                Math.Min(circle.Position.X, Position.X + halfWidth));

            float deltaY = circle.Position.Y -
                Math.Max(Position.Y - halfHeight,
                Math.Min(circle.Position.Y, Position.Y + halfHeight));

            return (deltaX * deltaX + deltaY * deltaY) < (circle.Radius * circle.Radius);
        }

        public override bool Contains(Vector2 point)
        {
            return
                point.X >= Position.X - halfWidth &&
                point.X <= Position.X + halfWidth &&
                point.Y >= Position.Y - halfHeight &&
                point.Y <= Position.Y + halfHeight;
        }

        public override bool Collides(BoxCollider other, ref Collision collisionInfo)
        {
            Vector2 distance = other.Position - Position;
            float deltaX = Math.Abs(distance.X) - (other.halfWidth + this.halfWidth);

            if(deltaX > 0)
            {
                return false;
            }

            float deltaY = Math.Abs(distance.Y) - (other.halfHeight + this.halfHeight);

            if (deltaY > 0)
            {
                return false;
            }

            collisionInfo.Type = CollisionType.RectsIntersection;
            collisionInfo.Delta = new Vector2(-deltaX, -deltaY);//in order to have positive values

            return true;
        }

        public override bool Collides(CompoundCollider compound, ref Collision collisionInfo)
        {
            return compound.Collides(this, ref collisionInfo);
        }
    }
}

using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class CompoundCollider : Collider
    {
        public Collider BoundingCollider;

        protected List<Collider> colliders;

        public CompoundCollider(RigidBody owner, Collider boundingCollider) : base(owner)
        {
            BoundingCollider = boundingCollider;
            colliders = new List<Collider>();
        }

        public virtual void AddCollider(Collider collider)
        {
            colliders.Add(collider);
        }

        public virtual bool InnerCollidersCollide(Collider collider, ref Collision collisionInfo)
        {
            //search for collision with inner colliders
            for (int i = 0; i < colliders.Count; i++)
            {
                if (collider.Collides(colliders[i], ref collisionInfo))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Collides(Collider collider, ref Collision collisionInfo)
        {
            return collider.Collides(this, ref collisionInfo);
        }

        public override bool Collides(BoxCollider box, ref Collision collisionInfo)
        {
            return (box.Collides(BoundingCollider, ref collisionInfo) && InnerCollidersCollide(box, ref collisionInfo));
        }

        public override bool Collides(CircleCollider circle, ref Collision collisionInfo)
        {
            return (circle.Collides(BoundingCollider, ref collisionInfo) && InnerCollidersCollide(circle, ref collisionInfo));
        }

        public override bool Collides(CompoundCollider other, ref Collision collisionInfo)
        {
            if (BoundingCollider.Collides(other.BoundingCollider, ref collisionInfo))
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    for (int j = 0; j < other.colliders.Count; j++)
                    {
                        if (colliders[i].Collides(other.colliders[j], ref collisionInfo))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override bool Contains(Vector2 point)
        {
            if (BoundingCollider.Contains(point))
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    if (colliders[i].Contains(point))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

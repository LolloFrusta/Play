using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    static class PhysicsMgr
    {
        static List<RigidBody> items;
        static Collision collisionInfo;

        public static float G = 9f;

        static PhysicsMgr()
        {
            items = new List<RigidBody>();
        }

        public static void AddItem(RigidBody rb)
        {
            items.Add(rb);
        }

        public static void RemoveItem(RigidBody rb)
        {
            items.Remove(rb);
        }

        public static void Update()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].IsActive)
                {
                    items[i].Update();
                }
            }
        }

        public static void CheckCollisions()
        {
            for (int i = 0; i < items.Count -1; i++)
            {
                if(items[i].IsActive && items[i].IsCollisionsAffected)
                {
                    //check collisions with next items
                    for (int j = i+1; j < items.Count; j++)
                    {
                        if(items[j].IsActive && items[j].IsCollisionsAffected)
                        {
                            //check if one of the RB is interested in collision check

                            bool firstCheck = items[i].CollisionTypeMatches(items[j].Type);
                            bool secondCheck = items[j].CollisionTypeMatches(items[i].Type);

                            if ((firstCheck || secondCheck) && items[i].Collides(items[j], ref collisionInfo))
                            {
                                if (firstCheck)
                                {
                                    collisionInfo.Collider = items[j].GameObject;
                                    items[i].GameObject.OnCollide(collisionInfo);
                                }

                                if (secondCheck)
                                {
                                    collisionInfo.Collider = items[i].GameObject;
                                    items[j].GameObject.OnCollide(collisionInfo);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ClearAll()
        {
            items.Clear();
        }
    }
}

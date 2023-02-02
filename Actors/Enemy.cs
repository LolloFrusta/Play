using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using PathFinding;

namespace Play_
{
    class Enemy : Actor
    {
        protected StateMachine fsm;
        //protected Vector2 PointToReach;
        protected float halfConeAngle = MathHelper.DegreesToRadians(40);
        protected float shootDistance;
        private GridPathfinder pathFinder;
        private PowerUpMgr pwrUpMgr;
        public float walkSpeed;
        public float followSpeed;
        public float VisionRadius { get; protected set; }

        public Player Rival;
        public GameObject Target;

        public Enemy(string textureName, GridPathfinder pf, PowerUpMgr aPwrUpMgr) : base(textureName, DrawLayer.Playground)
        {
            RigidBody = new RigidBody(this);
            //RigidBody.IsGravityAffected = true;
            RigidBody.Type = RigidBodyType.Enemy;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);

            bulletType = BulletType.EnemyBullet;

            VisionRadius = 4f;
            walkSpeed = 1.5f;
            followSpeed = walkSpeed * 2.0f;
            shootDistance = 3.0f;

            pathFinder = pf;
            pwrUpMgr = aPwrUpMgr;

            fsm = new StateMachine();
            fsm.AddState(StateEnum.WALK, new WalkState(this));
            fsm.AddState(StateEnum.FOLLOW, new FollowState(this));
            fsm.AddState(StateEnum.SHOOT, new ShootState(this));
            //fsm.AddState(StateEnum.RECHARGE, new RechargeState(this));
            //fsm.GoTo(StateEnum.WALK);
            fsm.SetFirstState(StateEnum.WALK);
        }

        public void ComputePlayerPoint()
        {
            pathFinder.SelectPathFromTo(Position, Rival.Position);
        }

        public void ComputeRandomPoint()
        {
            pathFinder.SelectRandomPathFrom(Position);
        }

        public bool CanAttackPlayer()
        {
            if (Rival == null || !Rival.IsAlive)
            {
                return false;
            }
            Vector2 distVect = Rival.Position - Position;
            return distVect.LengthSquared < shootDistance * shootDistance;
        }

        public void HeadToPlayer()
        {
            /*
            if (Rival != null)
            {
                Vector2 distVect = Rival.Position - Position;
                RigidBody.Velocity = distVect.Normalized() * followSpeed;
            }
            */
            Vector2 dir = pathFinder.NextPathDirection(Position);
            RigidBody.Velocity = followSpeed * dir;
        }

        public void HeadToPoint()
        {
            Vector2 dir = pathFinder.NextPathDirection(Position);
            //Improve with pathFinder.IsPathEnd()
            if (dir.LengthSquared == 0)
            {
                ComputeRandomPoint();
            }

            RigidBody.Velocity = walkSpeed * dir;
        }

        public bool IsPointVisible(Vector2 point,out Vector2 distanceVector)
        {
            distanceVector = point - Position;

            if (distanceVector.LengthSquared <= VisionRadius * VisionRadius)
            {
                float pointAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(Forward, distanceVector.Normalized()), -1f, 1f));
                if (pointAngle <= halfConeAngle)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual Player GetBestPlayerToFight()
        {

            Player bestPlayer = null;

            List<Player> visiblePlayers = GetVisiblePlayers();

            if (visiblePlayers.Count > 0)
            {
                if (visiblePlayers.Count > 1)
                {
                    float maxFuzzy = -1;

                    for (int i = 0; i < visiblePlayers.Count; i++)
                    {
                        //distance
                        Vector2 distanceFromPlayer = Position - visiblePlayers[i].Position;
                        float fuzzyDistance = 1 - distanceFromPlayer.LengthSquared / (VisionRadius * VisionRadius);

                        //energy
                        float fuzzyEnergy = 1 - visiblePlayers[i].Energy / visiblePlayers[i].MaxEnergy;

                        //angle
                        float playerAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(visiblePlayers[i].Forward, distanceFromPlayer.Normalized()), -1, 1));
                        float fuzzyAngle = 1 - (playerAngle / (float)Math.PI);

                        float fuzzySum = fuzzyDistance + fuzzyEnergy + fuzzyAngle;

                        if(fuzzySum > maxFuzzy)
                        {
                            maxFuzzy = fuzzySum;
                            bestPlayer = visiblePlayers[i];
                        }
                    }
                }
                else
                {
                    bestPlayer = visiblePlayers[0];
                }
            }

            return bestPlayer;
        }

        public List<Player> GetVisiblePlayers()
        {
            List<Player> players = ((PlayScene)Game.CurrentScene).Players;
            List<Player> visiblePlayers = new List<Player>();

            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].IsAlive)
                {
                    continue;
                }

                Vector2 distanceVector;

                if (IsPointVisible(players[i].Position,out distanceVector))
                {
                    visiblePlayers.Add(players[i]);
                }

                ////compute distance
                //Vector2 distVect = players[i].Position - Position;

                ////if player is inside vision radius check cone angle
                //if (distVect.LengthSquared <= visionRadius * visionRadius)
                //{
                //    float playerAngle = (float)Math.Acos(MathHelper.Clamp(Vector2.Dot(Forward, distVect.Normalized()), -1f, 1f));
                //    if (playerAngle <= halfConeAngle)
                //    {
                //        visiblePlayers.Add(players[i]);
                //    }
                //}
            }

            return visiblePlayers;
        }

        public virtual PowerUp GetNearestPowerUp()
        {
            PowerUp nearest = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < pwrUpMgr.PowerUps.Count; i++)
            {
                Vector2 distanceVector;

                if (IsPointVisible(pwrUpMgr.PowerUps[i].Position, out distanceVector))
                {
                    if (distanceVector.LengthSquared < minDistance)
                    {
                        nearest = pwrUpMgr.PowerUps[i];
                        minDistance = distanceVector.LengthSquared;
                    }
                }
            }

            return nearest;
        }

        public void ShootPlayer()
        {
            if (Rival != null)
            {
                Vector2 direction = Rival.Position - Position;
                Forward = direction;

                if (nextShoot <= 0)
                {
                    Shoot(Forward);
                    nextShoot = RandomGenerator.GetRandomFloat() * 2 + 1;
                }
            }
        }


        public override void Update()
        {
            if (IsActive)
            {
                if (nextShoot > 0)
                {
                    nextShoot -= Game.DeltaTime;
                }
                fsm.Update();
                base.Update();
            }
        }

        public virtual void Spawn()
        {
            IsActive = true;
            Energy = MaxEnergy;
            fsm.GoTo(StateEnum.WALK);
        }

        public override void OnDie()
        {
            //restore into SpawnMgr
            IsActive = false;
        }
    }
}

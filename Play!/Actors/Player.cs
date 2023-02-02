using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class Player : Actor
    {
        protected Controller controller;
        protected float speed = 6;
        protected float jumpSpeed = -8;

        protected bool isJumpPressed;
        protected bool isFirePressed;

        protected TextObject playerName;

        public bool IsGrounded { get { return RigidBody.Velocity.Y == 0; } }
        public int PlayerID { get; protected set; }

        public bool IsAlive;


        public Player(Controller ctrl, int playerID) : base("player", DrawLayer.Playground)
        {
            IsAlive = true;
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.AddCollisionType(RigidBodyType.Tile);
            RigidBody.Friction = 40;

            bulletType = BulletType.PlayerBullet;

            PlayerID = playerID;

            controller = ctrl;

            playerName = new TextObject(new Vector2(0, 0), "Player " + (PlayerID + 1));
        }

        public virtual void Input()
        {
            float directionX = controller.GetHorizontal();
            if (directionX != 0)
            {
                RigidBody.Velocity.X = directionX * speed;

            }

            float directionY = controller.GetVertical();
            if (directionY != 0)
            {
                RigidBody.Velocity.Y = directionY * speed;
            }

            if (controller.IsFirePressed())
            {
                if (!isFirePressed)
                {
                    isFirePressed = true;
                    Shoot();
                }
            }
            else
            {
                isFirePressed = false;
            }

        }

        protected virtual void Jump()
        {
            RigidBody.Velocity.Y = jumpSpeed;
        }

        protected virtual void Shoot()
        {
            Shoot(Forward);
        }

        public override void OnDie()
        {
            IsAlive = false;
            ((PlayScene)Game.CurrentScene).OnPlayerDies();
        }
    }
}

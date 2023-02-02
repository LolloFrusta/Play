using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Play_
{
    class ShootState : State
    {
        private Enemy enemy;

        private float checkForNewPlayer;

        public ShootState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void OnEnter()
        {
            this.enemy.RigidBody.Velocity = Vector2.Zero;
        }

        public override void Update()
        {
            checkForNewPlayer -= Game.DeltaTime;

            if (checkForNewPlayer<=0)
            {
                checkForNewPlayer = RandomGenerator.GetRandomFloat() + 0.2f;

                enemy.Rival = enemy.GetBestPlayerToFight();
            }

            if (enemy.Rival == null || !enemy.CanAttackPlayer())
            {
                stateMachine.GoTo(StateEnum.WALK);
                return;
            }
            else
            {
                //TODO: 
                // - Cool down. 1 Bullet at time.
                // - Sprite Animation (O)
                enemy.ShootPlayer();
            }

            //same energy => 1
            //Enemy MAX => 100 => 1
            //Player MAX => 0.01

            //float continueAttack = Math.Min(enemy.Energy / enemy.Rival.Energy, 1);
             

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class FollowState : State
    {
        private Enemy enemy;
        private RandomTimer checkForNewPlayer;
        private RandomTimer checkForPowerUp;

        public FollowState(Enemy enemy)
        {
            this.enemy = enemy;
            checkForNewPlayer = new RandomTimer(0.2f, 1.2f);
            checkForPowerUp = new RandomTimer(0.4f, 1.35f);
        }

        protected virtual bool ContinueFollow(PowerUp nearestPowerUp)
        {
            float rechargeDistFuzzy = 1 - (nearestPowerUp.Position - enemy.Position).LengthSquared / (enemy.VisionRadius * enemy.VisionRadius);
            float followDistFuzzy = 1 - (enemy.Rival.Position - enemy.Position).LengthSquared / (enemy.VisionRadius * enemy.VisionRadius);

            float rechargeNrgFuzzy = 1 - (float)enemy.Energy / (float)enemy.MaxEnergy;

            //Chase
            //same energy => 1
            //Enemy MAX => 100
            //Player MAX => 0.01
            float followNrgFuzzy = Math.Min((float)enemy.Energy / (float)enemy.Rival.Energy, 1);

            float rechargeSum = rechargeDistFuzzy + rechargeNrgFuzzy;
            float followSum = followDistFuzzy + followNrgFuzzy;

            return (followSum > rechargeSum);
        }

        public override void OnEnter()
        {
            enemy.ComputePlayerPoint();
        }

        public override void Update()
        {
            //enemy.HeadToPlayer();
            //return;

            checkForNewPlayer.Tick();

            if (checkForNewPlayer.IsOver())
            {
                enemy.Rival = enemy.GetBestPlayerToFight();

                checkForNewPlayer.Reset();
            }
/*
            checkForPowerUp.Tick();

            if (checkForPowerUp.IsOver())
            {
                PowerUp p = enemy.GetNearestPowerUp();
                if (p != null)
                {
                    if (enemy.Rival == null || !ContinueFollow(p))
                    {
                        enemy.Target = p;
                        stateMachine.GoTo(StateEnum.RECHARGE);
                        return;
                    }
                }
            }
*/
            if (enemy.Rival==null || !enemy.Rival.IsAlive)
            {
                stateMachine.GoTo(StateEnum.WALK);
            }
            else if (enemy.CanAttackPlayer())
            {
                stateMachine.GoTo(StateEnum.SHOOT);
            }
            else
            {
                enemy.HeadToPlayer();
            }
        }
    }
}

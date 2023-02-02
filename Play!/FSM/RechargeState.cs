using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class RechargeState : State
    {
        private Enemy enemy;

        public RechargeState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public override void OnEnter()
        {
            if(enemy.Target!=null && enemy.Target.IsActive)
            {
                Vector2 distance = enemy.Target.Position - enemy.Position;

                enemy.RigidBody.Velocity = distance.Normalized() * enemy.followSpeed * 1.3f;
            }
        }

        public override void Update()
        {
            if (enemy.Target == null || !enemy.Target.IsActive)
            {//target is no more available
                enemy.Target = null;

                if(enemy.Rival!=null && enemy.Rival.IsActive)
                {
                    stateMachine.GoTo(StateEnum.FOLLOW);
                }
                else
                {
                    stateMachine.GoTo(StateEnum.WALK);
                }
            }
        }
    }
}

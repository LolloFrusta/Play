using OpenTK;
using System;
using System.Collections.Generic;
using PathFinding;

namespace Play_
{
    class PowerUpMgr
    {
        public List<PowerUp> PowerUps { get; private set; }
        private int listSize;
        private float nextSpawn;
        private GridPathfinder pf;
        public PowerUpMgr(GridPathfinder pathfinder, int maxQueueSize)
        {
            pf = pathfinder;
            listSize = maxQueueSize;

            PowerUps = new List<PowerUp>(listSize);

            for (int i = 0; i < listSize; i++)
            {
                PowerUps.Add(new PowerUp());
            }

            nextSpawn = RandomGenerator.GetRandomFloat() * 2 + 3;
        }

        public void Update()
        {
            nextSpawn -= Game.DeltaTime;
            if (nextSpawn <= 0)
            {
                SpawnPowerUp();
                nextSpawn = RandomGenerator.GetRandomFloat() * 3 + 3;
            }
        }

        private void SpawnPowerUp()
        {
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if (!PowerUps[i].IsActive)
                {
                    PowerUps[i].Position = GetRandomPoint();
                    PowerUps[i].IsActive = true;
                    break;
                }
            }
        }

        private Vector2 GetRandomPoint()
        {
            /*
            float randX = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoWidth - 2) + 1;
            float randY = RandomGenerator.GetRandomFloat() * (Game.Window.OrthoHeight - 2) + 1;
            return new Vector2(randX, randY);
            */
            return pf.PickRandomPosition();
        }
    }
}

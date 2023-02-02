using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Play_;

namespace PathFinding
{
    class GridPathfinder
    {

        private GridGraph graph;
        private float unitWidth;
        private float unitHeight;
        private NodePath path;
        private int pathIndex;
        private Node nextNode;
        private List<Vector2> walkableCells;

        public GridPathfinder(int[,] grid, float unitWidth, float unitHeight)
        {
            this.graph = new GridGraph(grid);
            this.unitWidth = unitWidth;
            this.unitHeight = unitHeight;
            this.walkableCells = new List<Vector2>();

            int walkableValue = 1;

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == walkableValue)
                    {
                        walkableCells.Add(new Vector2(col, row));
                    }
                }
            }
        }

        public void SelectRandomPathFrom(Vector2 position)
        {
            Vector2 cell = ToCell(position);
            Node source = graph.NodeAt((int)cell.Y, (int)cell.X);

            Vector2 targetCell = PickRandomCell();
            Node destin = graph.NodeAt((int)targetCell.Y, (int)targetCell.X);
            
            path = WeightedGraphAlgo.AStar_ShortestPath(source, destin);
            if (path.Length < 2)
            {
                Console.WriteLine("Path Skipped");
                SelectRandomPathFrom(position);
                return;
            }
            pathIndex = 1;
            nextNode = path.At(pathIndex);
        }

        public Vector2 PickRandomPosition()
        {
            Vector2 targetCell = PickRandomCell();
            return ToPosition((int)targetCell.Y, (int)targetCell.X);
        }

        private Vector2 PickRandomCell()
        {
            int index = RandomGenerator.GetRandomInt(0, walkableCells.Count - 1); //-1 serve???
            return walkableCells[index];
        }

        public void SelectPathFromTo(Vector2 position, Vector2 target)
        {
            Vector2 currentCell = ToCell(position);
            Vector2 targetCell = ToCell(target);

            Node startNode = graph.NodeAt((int)currentCell.Y, (int)currentCell.X);
            Node endNode = graph.NodeAt((int)targetCell.Y, (int)targetCell.X);
            path = WeightedGraphAlgo.AStar_ShortestPath(startNode, endNode);
            if (path.Length < 2)
            {
                return;
            }

            pathIndex = 1;
            nextNode = path.At(pathIndex);
        }

        private Vector2 ToCell(Vector2 position)
        {
            Vector2 v = new Vector2();
            int row = (int)(position.Y / unitHeight);
            int col = (int)(position.X / unitWidth);
            v.Y = row;
            v.X = col;
            return v;
        }

        private Vector2 ToPosition(int row, int col)
        {
            Vector2 result = new Vector2();
            result.X = col * unitWidth + unitWidth * 0.5f;
            result.Y = row * unitHeight + unitHeight * 0.5f;
            return result;
        }

        public Vector2 NextPathDirection(Vector2 position)
        {
            Vector2 nextPos = ToPosition(nextNode.Row, nextNode.Col);
           
            Vector2 distVect = nextPos - position;
            if (distVect.LengthSquared <= 0.0001f)
            {
                pathIndex++;
                if (pathIndex < path.Length)
                {
                    nextNode = path.At(pathIndex);

                    Vector2 cell = ToCell(position);
                    int row = (int)cell.Y;
                    int col = (int)cell.X;
                    distVect.X = nextNode.Col - col;
                    distVect.Y = nextNode.Row - row;
                }
                else
                {
                    return new Vector2(0, 0);
                }
            }
            return distVect.Normalized();
        }
    }
}

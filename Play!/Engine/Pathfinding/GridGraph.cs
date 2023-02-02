using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class GridGraph
    {
        private int[,] data;

        public int Rows { get; }
        public int Cols { get; }

        private Node[] nodes;

        public GridGraph(int[,] data)
        {
            this.data = data;
            Rows = data.GetLength(0);
            Cols = data.GetLength(1);

            this.nodes = new Node[Rows * Cols];
            BuildGraph();
        }

        public Node NodeAt(int r, int c)
        {
            //Proteggere nota at e ritornare NULL
            if (r < 0 || r >= Rows) return null;
            if (c < 0 || c >= Cols) return null;
            return nodes[r * Cols + c];
        }

        private void BuildGraph()
        { 
            //1. Node creation
            for(int r=0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    string label = r + "x" + c;
                    Node n = new Node(label, r, c);
                    //int pos = r * Rows + c; BUGGONE
                    int pos = r * Cols + c;
                    nodes[pos] = n;
                }
            }

            //2. Link creation
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Node curr = NodeAt(r, c);

                    //LinkTo Up
                    if (r - 1 >= 0)
                    {
                        Node n = NodeAt(r - 1, c);
                        curr.LinkTo(n, data[r - 1, c]);
                    }
                    //LinkTo Down
                    if (r + 1 < Rows)
                    {
                        Node n = NodeAt(r + 1, c);
                        curr.LinkTo(n, data[r + 1, c]);
                    }
                    //LinkTo Left
                    if (c - 1 >= 0)
                    {
                        Node n = NodeAt(r, c - 1);
                        curr.LinkTo(n, data[r, c - 1]);
                    }
                    //LinkTo Right
                    if (c + 1 < Cols)
                    {
                        Node n = NodeAt(r, c + 1);
                        curr.LinkTo(n, data[r, c + 1]);
                    }
                }
            }
        }


        public void Print()
        {
            for(int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {

                    Node node = NodeAt(row, col);
                    Console.WriteLine(node.Label);
                    foreach (KeyValuePair<Node, int> Each in node.WeigthedEdges)
                    {
                        Node neigh = Each.Key;
                        int cost = Each.Value;
                        Console.WriteLine("\t" + neigh.Label + "(" + cost +")");
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class WeightedGraphAlgo
    {
        public static Dictionary<Node, Node> Dijkstra_Visit(Node root)
        {
            Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
            Dictionary<Node, int> dist = new Dictionary<Node, int>();
            List<Node> potentials = new List<Node>();

            dist[root] = 0;
            prev[root] = root;
            potentials.Add(root);

            while (potentials.Count > 0)  // N
            {
                Node selected = BestPotential(potentials, dist); // N
                potentials.Remove(selected);

                foreach (KeyValuePair<Node, int> Each in selected.WeigthedEdges) // V/N
                {
                    Node neigh = Each.Key;
                    int cost = Each.Value;
                    int potCost = dist[selected] + cost;
                    if (!dist.ContainsKey(neigh))
                    {
                        potentials.Add(neigh);
                        dist[neigh] = potCost;
                        prev[neigh] = selected;
                    }
                    else if (potCost < dist[neigh])
                    {
                        dist[neigh] = potCost;
                        prev[neigh] = selected;
                    }
                }
            }

            // N * (N + V/N) =>  N2 + V
            return prev;
        }

        public static NodePath Dijkstra_ShortestPath(Node source, Node dest)
        {
            Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
            Dictionary<Node, int> dist = new Dictionary<Node, int>();
            List<Node> potentials = new List<Node>();

            dist[source] = 0;
            prev[source] = source;
            potentials.Add(source);

            while (potentials.Count > 0)
            {
                Node selected = BestPotential(potentials, dist);
                if (selected.Equals(dest)) break;

                potentials.Remove(selected);

                foreach (KeyValuePair<Node, int> Each in selected.WeigthedEdges)
                {
                    Node neigh = Each.Key;
                    int cost = Each.Value;
                    int potCost = dist[selected] + cost;
                    if (!dist.ContainsKey(neigh))
                    {
                        potentials.Add(neigh);
                        dist[neigh] = potCost;
                        prev[neigh] = selected;
                    }
                    else if (potCost < dist[neigh])
                    {
                        dist[neigh] = potCost;
                        prev[neigh] = selected;
                    }
                }
            }

            return PathTo(dest, prev);
        }

        private static Node BestPotential(List<Node> potentials, Dictionary<Node, int> dist)
        {
            Node Selected = potentials[0];
            for (int i = 1; i < potentials.Count; i++)
            {
                Node each = potentials[i];

                if (dist[each] < dist[Selected])
                {
                    Selected = each;
                }
            }
            return Selected;
        }

        static NodePath PathTo(Node aNode, Dictionary<Node, Node> parents)
        {
            NodePath path = new NodePath();
            Node current = aNode;
            path.Add(current);
            while (true)
            {
                Node parent = parents[current];
                if (parent.Equals(current)) break;
                path.Add(parent, 0);
                current = parent;
            }
            return path;
        }



        public static NodePath AStar_ShortestPath(Node source, Node dest)
        {
            Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
            Dictionary<Node, int> dist = new Dictionary<Node, int>();
            Dictionary<Node, int> heur = new Dictionary<Node, int>();
            List<Node> potentials = new List<Node>();

            dist[source] = 0;
            heur[source] = 0;
            prev[source] = source;
            potentials.Add(source);

            while (potentials.Count > 0)
            {
                Node selected = BestPotential(potentials, heur);
                if (selected.Equals(dest)) break;

                potentials.Remove(selected);

                foreach (KeyValuePair<Node, int> Each in selected.WeigthedEdges)
                {
                    Node neigh = Each.Key;
                    int cost = Each.Value;
                    int potCost = dist[selected] + cost;
                    int heurCost = potCost + Heurstic(neigh, dest);
                    if (!dist.ContainsKey(neigh))
                    {
                        potentials.Add(neigh);
                        dist[neigh] = potCost;
                        prev[neigh] = selected;
                        heur[neigh] = heurCost;
                    }
                    else if (potCost < dist[neigh])
                    {
                        dist[neigh] = potCost;
                        prev[neigh] = selected;
                        heur[neigh] = heurCost;
                    }
                }
            }

            return PathTo(dest, prev);
        }

        private static int Heurstic(Node nodeA, Node nodeB)
        {
            return Math.Abs(nodeA.Row - nodeB.Row) + Math.Abs(nodeA.Col - nodeB.Col);
        }
    }

    
}

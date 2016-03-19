using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tibia_Path_Finder {
    public class AStarNode {
        public AStarNode previous;
        public Connection connection;
        public NormalNode node;
        public double cost;
        
        public Rectangle rect {  get { return node.rect;  } }

        public AStarNode(AStarNode previous, Connection connection, NormalNode node, double cost) {
            this.previous = previous;
            this.connection = connection;
            this.node = node;
            this.cost = cost;
        }
    }

    public class AStar {
        public static AStarNode GetMinimum(List<AStarNode> list, NormalNode goal) {
            double minValue = double.MaxValue;
            AStarNode min = null;
            foreach(var node in list) {
                double nodeValue = node.cost;
                if (nodeValue < minValue) {
                    min = node;
                    minValue = nodeValue;
                }
            }
            return min;
        }
        
        public static double Distance(NormalNode a, NormalNode b) {
            //return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
            return Math.Sqrt(Math.Pow(a.rect.X - b.rect.X, 2) + Math.Pow(a.rect.Y - b.rect.Y, 2));
        }

        public static AStarNode FindRoute(NormalNode start, NormalNode goal, List<Rectangle> rectangles = null) {
            List<AStarNode> openSet = new List<AStarNode> { new AStarNode(null, null, start, 0) };
            HashSet<NormalNode> closedSet = new HashSet<NormalNode>();

            while (openSet.Count > 0) {
                AStarNode current = GetMinimum(openSet, goal);
                if (current.node == goal) {
                    return current;
                }
                openSet.Remove(current);
                closedSet.Add(current.node);
                if (rectangles != null) {
                    bool found = false;
                    foreach (Rectangle rectangle in rectangles) {
                        if (current.rect.IntersectsWith(rectangle)) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) continue;
                }
                foreach (Connection conn in current.node.neighbors) {
                    if (closedSet.Contains(conn.neighbor)) continue;

                    double newCost = current.cost + (conn.settings != null ? conn.settings.cost : Distance(current.node, conn.neighbor));

                    AStarNode neighborAStar = openSet.Find(o => o.node == conn.neighbor);
                    if (neighborAStar == null) {
                        openSet.Add(new AStarNode(current, conn, conn.neighbor, newCost));
                    } else if (neighborAStar.cost < newCost) {
                        continue;
                    }
                }
            }
            return null;
        }

        
    }
}

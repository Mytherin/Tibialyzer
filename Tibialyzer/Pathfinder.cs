
// Copyright 2016 Mark Raasveldt
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SQLite;
using System.Drawing.Imaging;

namespace Tibialyzer {
    public class Node {
        public List<Node> neighbors;
        public Rectangle rect;
        public int z;
        public Node(int x, int y, int z, int width, int height) {
            this.rect = new Rectangle(x, y, width, height);
            this.z = z;
            this.neighbors = new List<Node>();
        }
        public Node(int x, int y, int z, int size) {
            this.rect = new Rectangle(x, y, size, size);
            this.z = z;
            this.neighbors = new List<Node>();
        }

        public Node(int z) {
            this.z = z;
            this.neighbors = new List<Node>();
        }

        public double x {
            get { return rect.X + rect.Width / 2.0; }
        }

        public double y {
            get { return rect.Y + rect.Height / 2.0; }
        }
    }

    public class DijkstraClass {
        public double cost;
    }

    public class DijkstraNode : DijkstraClass {
        public DijkstraNode previous;
        public Node node;

        public Rectangle rect { get { return node.rect; } }

        public DijkstraNode(DijkstraNode previous, Node node, double cost) {
            this.previous = previous;
            this.node = node;
            this.cost = cost;
        }
    }

    public class DijkstraPoint : DijkstraClass {
        public DijkstraPoint previous;
        public Point point;

        public DijkstraPoint(DijkstraPoint previous, Point point, double cost) {
            this.previous = previous;
            this.cost = cost;
            this.point = point;
        }
    }

    public class Dijkstra {
        public static DijkstraPoint GetMinimum(List<DijkstraPoint> list) {
            double minValue = double.MaxValue;
            DijkstraPoint min = null;
            foreach (var node in list) {
                double nodeValue = node.cost;
                if (nodeValue < minValue) {
                    min = node;
                    minValue = nodeValue;
                }
            }
            return min;
        }

        public static DijkstraNode GetMinimum(List<DijkstraNode> list) {
            double minValue = double.MaxValue;
            DijkstraNode min = null;
            foreach (var node in list) {
                double nodeValue = node.cost;
                if (nodeValue < minValue) {
                    min = node;
                    minValue = nodeValue;
                }
            }
            return min;
        }

        public static double Distance(Point a, Point b) {
            // Euclidean distance
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static double Distance(double ax, double ay, double bx, double by) {
            // Euclidean distance
            return Math.Sqrt(Math.Pow(ax - bx, 2) + Math.Pow(ay - by, 2));
        }

        public static double Distance(Node a, Node b) {
            // Euclidean distance
            return Math.Sqrt(Math.Pow(a.rect.X - b.rect.X, 2) + Math.Pow(a.rect.Y - b.rect.Y, 2));
        }

        private static List<Color> walkableColors = new List<Color> {
            Color.FromArgb(0, 204, 0),
            Color.FromArgb(153, 153, 153),
            Color.FromArgb(255, 204, 153),
            Color.FromArgb(153, 102, 51),
            Color.FromArgb(255, 255, 255),
            Color.FromArgb(153, 153, 153),
            Color.FromArgb(204, 255, 255),
            Color.FromArgb(255, 255, 0)
        };

        public static bool isWalkable(Color color, List<Color> additionalWalkableColors) {
            return walkableColors.Contains(color) || additionalWalkableColors.Contains(color);
        }

        public static Point[] getNeighbors(Point point) {
            if (point.X > 0 && point.Y > 0 && point.X < Coordinate.MaxWidth && point.Y < Coordinate.MaxHeight) {
                Point[] points = new Point[8];
                points[0] = new Point(point.X - 1, point.Y - 1);
                points[1] = new Point(point.X, point.Y - 1);
                points[2] = new Point(point.X - 1, point.Y);
                points[3] = new Point(point.X + 1, point.Y + 1);
                points[4] = new Point(point.X + 1, point.Y);
                points[5] = new Point(point.X, point.Y + 1);
                points[6] = new Point(point.X - 1, point.Y + 1);
                points[7] = new Point(point.X + 1, point.Y - 1);
                return points;
            }
            return null;
        }

        public static DijkstraPoint FindRoute(Bitmap mapImage, Point start, Point end, List<Rectangle> bounds, List<Color> additionalWalkableColors) {
            if (!isWalkable(mapImage.GetPixel(start.X, start.Y), additionalWalkableColors)) {
                throw new Exception(String.Format("Point ({0},{1}) is not walkable.", start.X, start.Y));
            }

            List<DijkstraPoint> openSet = new List<DijkstraPoint> { new DijkstraPoint(null, start, 0) };
            HashSet<Point> closedSet = new HashSet<Point>();
            DijkstraPoint closestNode = null;
            double closestDistance = double.MaxValue;


            while (openSet.Count > 0) {
                DijkstraPoint current = GetMinimum(openSet);
                if (current.point.Equals(end)) {
                    return current;
                }
                if (Distance(current.point, end) < closestDistance) {
                    closestDistance = Distance(current.point, end);
                    closestNode = current;
                }

                openSet.Remove(current);
                closedSet.Add(current.point);

                //check all the neighbors of the current point
                foreach (Point p in getNeighbors(current.point)) {
                    if (closedSet.Contains(p)) continue;
                    if (!isWalkable(mapImage.GetPixel(p.X, p.Y), additionalWalkableColors)) {
                        closedSet.Add(p);
                        continue;
                    }
                    if (bounds != null) {
                        bool found = false;
                        foreach (Rectangle bound in bounds) {
                            if (bound.Contains(p)) {
                                found = true;
                                break;
                            }
                        }
                        if (!found) continue;
                    }
                    double newCost = current.cost + Distance(current.point, p);

                    DijkstraPoint neighborPoint = openSet.Find(o => o.point == p);
                    if (neighborPoint == null) {
                        openSet.Add(new DijkstraPoint(current, p, newCost));
                    } else if (neighborPoint.cost < newCost) {
                        continue;
                    } else {
                        openSet.Remove(neighborPoint);
                        openSet.Add(new DijkstraPoint(current, p, newCost));
                    }
                }
            }
            if (closestDistance > 5 && bounds != null) {
                return FindRoute(mapImage, start, end, null, additionalWalkableColors);
            }
            return closestNode;
        }

        public static DijkstraNode FindRoute(Node start, Node goal) {
            // Dijkstra algorithm
            // We use Dijkstra instead of A* because there are random teleports in Tibia (e.g. boats)
            // And it is difficult to create a good A* heuristic that takes random teleports into account
            // and Dijkstra has more consistently good performance than A* with a bad heuristic
            List<DijkstraNode> openSet = new List<DijkstraNode> { new DijkstraNode(null, start, 0) };
            HashSet<Node> closedSet = new HashSet<Node>();

            while (openSet.Count > 0) {
                // Extract path with current minimal cost
                DijkstraNode current = GetMinimum(openSet);
                if (current.node == goal) {
                    return current;
                }
                // Add node to closed set
                openSet.Remove(current);
                closedSet.Add(current.node);

                // Iterate over all neighboring nodes
                foreach (Node neighbor in current.node.neighbors) {
                    if (closedSet.Contains(neighbor)) continue;

                    // If node is not in closed set, create a new path, we use conn.settings.cost for 'special' connections (e.g. teleports, stairs)
                    double newCost = current.cost + Distance(current.node, neighbor);

                    DijkstraNode neighborAStar = openSet.Find(o => o.node == neighbor);
                    if (neighborAStar == null) {
                        // The node is not in the open set; add it
                        openSet.Add(new DijkstraNode(current, neighbor, newCost));
                    } else if (neighborAStar.cost < newCost) {
                        // If the node is already in the open set and with a lower cost than this path, this path cannot be optimal, so skip it
                        continue;
                    } else {
                        // If the node is already in the open set but with a higher cost, remove it and add this node
                        openSet.Remove(neighborAStar);
                        openSet.Add(new DijkstraNode(current, neighbor, newCost));
                    }
                }
            }
            return null;
        }
    }

    public class Pathfinder {
        // The pathfinder class is responsible for managing the nodes
        // The nodes are stored as a separate list of nodes for each floor (z-level); there are 16 floors in total
        static List<List<Node>> nodes = new List<List<Node>>();
        public static void LoadFromDatabase(string databaseFile) {
            SQLiteConnection conn;
            SQLiteCommand command;
            SQLiteDataReader reader;

            conn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", databaseFile));
            conn.Open();

            // First add a list for each of the 16 floors
            for (int k = 0; k <= 15; k++) {
                nodes.Add(new List<Node>());
            }
            // we keep a temporary <id,Node> map for adding connections
            Dictionary<int, Node> hnodeids = new Dictionary<int, Node>();
            // Read the hierarchical nodes
            command = new SQLiteCommand("SELECT id,x,y,z,width,height FROM HierarchicalNode", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int id = reader.GetInt32(0);
                int x = reader.GetInt32(1);
                int y = reader.GetInt32(2);
                int z = reader.GetInt32(3);
                int width = reader.GetInt32(4);
                int height = reader.GetInt32(5);
                Node n = new Node(x, y, z, width, height);
                // add the node to the appropriate floor
                nodes[z].Add(n);
                // id-map
                hnodeids.Add(id, n);
            }
            // Now read the hierarchical connections from the database
            command = new SQLiteCommand("SELECT nodeid,nodeid2 FROM HierarchicalConnections", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int nodeid = reader.GetInt32(0);
                int nodeid2 = reader.GetInt32(1);
                // Add the connection to the nodes, using the id-map to extract the nodes
                hnodeids[nodeid].neighbors.Add(hnodeids[nodeid2]);
                hnodeids[nodeid2].neighbors.Add(hnodeids[nodeid]);
            }
        }

        public static Node GetNode(int x, int y, int z) {
            // For a specified coordinate (x,y,z), get the corresponding node
            // Note that we use "big" nodes; nodes are much bigger than a single square, thus we check which boundary rectangle contains the point
            // We could do this with a quadtree or spatial partitioning structure instead, but the low amount of nodes does not make that very worthwhile
            double mindist = int.MaxValue;
            Node closestNode = null;
            foreach (Node node in nodes[z]) {
                if (node.rect.Contains(x, y)) return node;
                else {
                    double distance = Dijkstra.Distance(x, y, node.x, node.y);
                    if (distance < mindist) {
                        mindist = distance;
                        closestNode = node;
                    }
                }
            }
            return closestNode;
        }
    }
}

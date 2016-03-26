
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SQLite;
using System.Drawing.Imaging;

namespace Tibialyzer {
    public class Node {
        public List<Connection> neighbors;
        public Rectangle rect;
        public int z;
        public Node(int x, int y, int z, int width, int height) {
            this.rect = new Rectangle(x, y, width, height);
            this.z = z;
            this.neighbors = new List<Connection>();
        }
        public Node(int x, int y, int z, int size) {
            this.rect = new Rectangle(x, y, size, size);
            this.z = z;
            this.neighbors = new List<Connection>();
        }

        public Node(int z) {
            this.z = z;
            this.neighbors = new List<Connection>();
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
        public Connection connection;

        public Node node { get { return connection.node; } }
        public Rectangle rect { get { return connection.node.rect; } }

        public DijkstraNode(DijkstraNode previous, Connection connection, double cost) {
            this.previous = previous;
            this.connection = connection;
            this.cost = cost;
        }
    }

    public class DijkstraPoint : DijkstraClass {
        public DijkstraPoint previous;
        public Point3D point;

        public DijkstraPoint(DijkstraPoint previous, Point3D point, double cost) {
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

        public static double Distance(Point3D a, Point3D b) {
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

        public static bool isWalkable(Point3D point, Color color) {
            return walkableColors.Contains(color) || Pathfinder.doors[point.Z].ContainsKey(new Tuple<int, int>(point.X, point.Y));
        }

        public static IEnumerable<Point3D> getNeighbors(Point3D point) {
            if (point.X > 0 && point.Y > 0 && point.X < Coordinate.MaxWidth && point.Y < Coordinate.MaxHeight) {
                yield return new Point3D(point.X - 1, point.Y - 1, point.Z);
                yield return new Point3D(point.X, point.Y - 1, point.Z);
                yield return new Point3D(point.X - 1, point.Y, point.Z);
                yield return new Point3D(point.X + 1, point.Y + 1, point.Z);
                yield return new Point3D(point.X + 1, point.Y, point.Z);
                yield return new Point3D(point.X, point.Y + 1, point.Z);
                yield return new Point3D(point.X - 1, point.Y + 1, point.Z);
                yield return new Point3D(point.X + 1, point.Y - 1, point.Z);
                if (Pathfinder.specialConnection.ContainsKey(point.Z)) {
                    var tpl = new Tuple<int, int>(point.X, point.Y);
                    if (Pathfinder.specialConnection[point.Z].ContainsKey(tpl)) {
                        foreach (SpecialConnection connection in Pathfinder.specialConnection[point.Z][tpl]) {
                            yield return new Point3D(connection.destination);
                        }
                    }
                }
            }
            yield break;
        }

        public static DijkstraPoint FindRoute(Bitmap mapImage, Point3D start, Point3D end, List<Rectangle3D> bounds, List<Color> additionalWalkableColors, SpecialConnection connection = null) {
            Point3D connectionPoint = connection == null ? new Point3D(-1,-1,0) : (connection.source.z == start.Z ? new Point3D(connection.source) : new Point3D(connection.destination));

            List<DijkstraPoint> openSet = new List<DijkstraPoint> { new DijkstraPoint(null, start, 0) };
            HashSet<Point3D> closedSet = new HashSet<Point3D>();
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
                foreach (Point3D p in getNeighbors(current.point)) {
                    double newCost = current.cost + Distance(current.point, p);

                    if (connection != null && p == connectionPoint) {
                        return new DijkstraPoint(current, p, newCost);
                    }

                    if (current.point.Z != start.Z) {
                        continue;
                    }

                    if (closedSet.Contains(p)) continue;
                    if (!isWalkable(p, mapImage.GetPixel(p.X, p.Y))) {
                        closedSet.Add(p);
                        continue;
                    }
                    if (bounds != null) {
                        bool found = false;
                        foreach (Rectangle3D bound in bounds) {
                            if (bound.Contains(p)) {
                                found = true;
                                break;
                            }
                        }
                        if (!found) continue;
                    }

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
            return closestNode;
        }


        public static double Distance(Node node, Point3D goal) {
            return Math.Abs(node.z - goal.Z) * 100 + Distance(node.x, node.y, goal.X, goal.Y);
        }

        public static DijkstraNode FindRoute(Node start, Node goal, Point3D exactGoal, DijkstraNode previousPath = null) {
            // Dijkstra algorithm
            // We use Dijkstra instead of A* because there are random teleports in Tibia (e.g. boats)
            // And it is difficult to create a good A* heuristic that takes random teleports into account
            // and Dijkstra has more consistently good performance than A* with a bad heuristic
            List<DijkstraNode> openSet = new List<DijkstraNode> { new DijkstraNode(null, new Connection(start), 0) };
            HashSet<Node> closedSet = new HashSet<Node>();

            double closestDistance = double.MaxValue;
            DijkstraNode closestNode = null;
            
            Dictionary<Node, DijkstraNode> nodes = new Dictionary<Node, DijkstraNode>(); 
            if (previousPath != null) {
                DijkstraNode it = previousPath;
                DijkstraNode prevNode = null;
                while(it != null) {
                    if (it.node == start) {
                        it.previous = null;
                        it.connection = new Connection(it.node);
                        return previousPath;
                    }
                    if (prevNode != null && !nodes.ContainsKey(prevNode.node)) {
                        nodes.Add(prevNode.node, it);
                    }
                    prevNode = it;
                    it = it.previous;
                }
            }

            
            while (openSet.Count > 0) {
                // Extract path with current minimal cost
                DijkstraNode current = GetMinimum(openSet);
                if (current.node == goal) {
                    return current;
                }
                if (nodes.ContainsKey(current.node)) {
                    DijkstraNode node = nodes[current.node];
                    node.previous = current;
                    return previousPath;
                }
                double distance = Distance(current.node, exactGoal);
                if (distance < closestDistance) {
                    closestNode = current;
                    closestDistance = distance;
                }
                // Add node to closed set
                openSet.Remove(current);
                closedSet.Add(current.node);

                // Iterate over all neighboring nodes
                foreach (Connection neighbor in current.node.neighbors) {
                    if (closedSet.Contains(neighbor.node)) continue;

                    // If node is not in closed set, create a new path, we use conn.settings.cost for 'special' connections (e.g. teleports, stairs)
                    double newCost = current.cost + (neighbor.connection == null ? Distance(current.node, neighbor.node) : 100);

                    DijkstraNode neighborAStar = openSet.Find(o => o.node == neighbor.node);
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
            return closestNode;
        }
    }

    public class Pathfinder {
        // The pathfinder class is responsible for managing the nodes
        // The nodes are stored as a separate list of nodes for each floor (z-level); there are 16 floors in total
        private static List<List<Node>> nodes = new List<List<Node>>();
        public static Dictionary<int, Dictionary<Tuple<int, int>, List<SpecialConnection>>> specialConnection = new Dictionary<int, Dictionary<Tuple<int, int>, List<SpecialConnection>>>();
        public static List<Dictionary<Tuple<int, int>, string>> doors = new List<Dictionary<Tuple<int, int>, string>>();
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
            Dictionary<int, SpecialConnection> connectionMap = new Dictionary<int, SpecialConnection>();
            command = new SQLiteCommand("SELECT id, x1,y1,z1,x2,y2,z2,name,cost FROM SpecialConnections", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int id = reader.GetInt32(0);
                Coordinate source = new Coordinate(reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));
                Coordinate dest = new Coordinate(reader.GetInt32(4), reader.GetInt32(5), reader.GetInt32(6));
                string name = reader[7].ToString();
                int cost = reader.GetInt32(8);
                if (!specialConnection.ContainsKey(source.z)) specialConnection.Add(source.z, new Dictionary<Tuple<int, int>, List<SpecialConnection>>());
                Tuple<int, int> tpl = new Tuple<int, int>(source.x, source.y);
                if (!specialConnection[source.z].ContainsKey(tpl)) specialConnection[source.z].Add(tpl, new List<SpecialConnection>());
                SpecialConnection connection = new SpecialConnection { source = source, destination = dest, name = name, cost = cost };
                specialConnection[source.z][tpl].Add(connection);
                connectionMap.Add(id, connection);
            }
            // Now read the hierarchical connections from the database
            command = new SQLiteCommand("SELECT nodeid,nodeid2, specialid FROM HierarchicalConnections", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int nodeid = reader.GetInt32(0);
                int nodeid2 = reader.GetInt32(1);
                int specialConnection = reader.GetInt32(2);
                SpecialConnection connection = null;
                if (specialConnection >= 0) {
                    connection = connectionMap[specialConnection];
                }
                // Add the connection to the nodes, using the id-map to extract the nodes
                hnodeids[nodeid].neighbors.Add(new Connection(hnodeids[nodeid2], connection));
            }
            for (int i = 0; i <= 15; i++) {
                doors.Add(new Dictionary<Tuple<int, int>, string>());
            }
            command = new SQLiteCommand("SELECT x,y,z,condition FROM Doors", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int x = reader.GetInt32(0);
                int y = reader.GetInt32(1);
                int z = reader.GetInt32(2);
                string condition = reader.IsDBNull(3) ? null : reader[3].ToString();
                doors[z].Add(new Tuple<int, int>(x, y), condition);
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

    public class Connection {
        public Node node;
        public SpecialConnection connection;
        public Connection(Node node, SpecialConnection connection = null) {
            this.node = node;
            this.connection = connection;
        }
    }

    public class SpecialConnection {
        public Coordinate source;
        public Coordinate destination;
        public string name;
        public int cost;
    }

    public struct Point3D {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public Point3D(int X, int Y, int Z) {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public Point3D(Coordinate c) {
            this.X = c.x;
            this.Y = c.y;
            this.Z = c.z;
        }

        public override int GetHashCode() {
            return (X + (Y + 1) * 3000 + (Z + 1) * 9000000).GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj is Point3D) {
                Point3D other = (Point3D)obj;
                return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
            }
            return false;
        }

        public static bool operator ==(Point3D a, Point3D b) {
            return a.Equals(b);
        }

        public static bool operator !=(Point3D a, Point3D b) {
            return !(a == b);
        }
    }

    public class Rectangle3D {
        public Rectangle Rect;
        public int Z;

        public Rectangle3D(Rectangle rect, int z) {
            this.Rect = rect;
            this.Z = z;
        }

        public bool Contains(Point3D point) {
            return point.Z == Z ? Rect.Contains(point.X, point.Y) : false;
        }
    }
}

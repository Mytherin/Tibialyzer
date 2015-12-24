using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SQLite;

namespace Tibialyzer {
    public class ConnectionSettings {
        public string name;
        public int cost;

        public ConnectionSettings(string name, int cost) {
            this.name = name;
            this.cost = cost;
        }
    }

    public class Connection {
        public Node neighbor;
        public ConnectionSettings settings;

        public Connection(Node neighbor, ConnectionSettings settings = null) {
            this.neighbor = neighbor;
            this.settings = settings;
        }
    }

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

    public class DijkstraNode {
        public DijkstraNode previous;
        public Connection connection;
        public Node node;
        public double cost;

        public Rectangle rect { get { return node.rect; } }

        public DijkstraNode(DijkstraNode previous, Connection connection, Node node, double cost) {
            this.previous = previous;
            this.connection = connection;
            this.node = node;
            this.cost = cost;
        }
    }

    public class Dijkstra {
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

        public static double Distance(Node a, Node b) {
            // Euclidean distance
            return Math.Sqrt(Math.Pow(a.rect.X - b.rect.X, 2) + Math.Pow(a.rect.Y - b.rect.Y, 2));
        }

        public static DijkstraNode FindRoute(Node start, Node goal) {
            // Dijkstra algorithm
            // We use Dijkstra instead of A* because there are random teleports in Tibia (e.g. boats)
            // And it is difficult to create a good A* heuristic that takes random teleports into account
            // and Dijkstra has more consistently good performance than A* with a bad heuristic
            List<DijkstraNode> openSet = new List<DijkstraNode> { new DijkstraNode(null, null, start, 0) };
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
                foreach (Connection conn in current.node.neighbors) {
                    if (closedSet.Contains(conn.neighbor)) continue;

                    // If node is not in closed set, create a new path, we use conn.settings.cost for 'special' connections (e.g. teleports, stairs)
                    double newCost = current.cost + (conn.settings != null ? conn.settings.cost : Distance(current.node, conn.neighbor));

                    DijkstraNode neighborAStar = openSet.Find(o => o.node == conn.neighbor);
                    if (neighborAStar == null) {
                        // The node is not in the open set; add it
                        openSet.Add(new DijkstraNode(current, conn, conn.neighbor, newCost));
                    } else if (neighborAStar.cost < newCost) {
                        // If the node is already in the open set and with a lower cost than this path, this path cannot be optimal, so skip it
                        continue;
                    } else {
                        // If the node is already in the open set but with a higher cost, remove it and add this node
                        openSet.Remove(neighborAStar);
                        openSet.Add(new DijkstraNode(current, conn, conn.neighbor, newCost));
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
            Dictionary<int, Node> nodeids = new Dictionary<int, Node>(); 
            // we keep a temporary <id,Connection> map for adding connection settings
            Dictionary<int, Tuple<Connection, Connection>> connectionids = new Dictionary<int, Tuple<Connection, Connection>>();
            // First read all the nodes from the database
            command = new SQLiteCommand("SELECT id,x,y,z,width,height FROM Node", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int id = reader.GetInt32(0);
                int x = reader.GetInt32(1);
                int y = reader.GetInt32(2);
                int z = reader.GetInt32(3);
                int width = reader.GetInt32(4);
                int height = reader.GetInt32(5);
                Node n = new Node(x, y, z, width, height);
                // Add the node to the appropriate floor
                nodes[z].Add(n);
                // id-map
                nodeids.Add(id, n);
            }
            // Now read the connections from the database
            command = new SQLiteCommand("SELECT id,nodeid,nodeid2 FROM Connections", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int id = reader.GetInt32(0);
                int nodeid = reader.GetInt32(1);
                int nodeid2 = reader.GetInt32(2);
                Connection c = new Connection(nodeids[nodeid]);
                Connection c2 = new Connection(nodeids[nodeid2]);
                // Add the connection to the nodes, using the id-map to extract the nodes
                nodeids[nodeid].neighbors.Add(c2);
                nodeids[nodeid2].neighbors.Add(c);
                // id-map
                connectionids.Add(id, new Tuple<Connection, Connection>(c, c2));
            }
            // Load connection settings from the database
            // These settings are only present for 'special' connections (teleports, stairs)
            command = new SQLiteCommand("SELECT connectionid,name,cost FROM ConnectionSettings", conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                int cost = reader.GetInt32(2);
                ConnectionSettings settings = new ConnectionSettings(name, cost);
                connectionids[id].Item1.settings = settings;
                connectionids[id].Item2.settings = settings;
            }
        }

        public static Node GetNode(int x, int y, int z) {
            // For a specified coordinate (x,y,z), get the corresponding node
            // Note that we use "big" nodes; nodes are much bigger than a single square, thus we check which boundary rectangle contains the point
            // We could do this with a quadtree or spatial partitioning structure instead, but the low amount of nodes does not make that very worthwhile
            foreach (Node node in nodes[z]) {
                if (node.rect.Contains(x, y)) return node;
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Data.SQLite;

using Tibialyzer;

namespace Tibia_Path_Finder {
    public partial class Form1 : Form {
        List<Color> walkableColors = new List<Color> { Color.FromArgb(0, 204, 0), Color.FromArgb(153, 153, 153), Color.FromArgb(255, 204, 153), Color.FromArgb(153, 102, 51), Color.FromArgb(255, 255, 255), Color.FromArgb(153, 153, 153), Color.FromArgb(204, 255, 255), Color.FromArgb(255, 255, 0) };
        Color stairsColor = Color.FromArgb(255, 255, 0);

        Image[] mapImages;

        public Form1() {
            InitializeComponent();

            mapImages = new Image[16];
            for (int i = 0; i <= 15; i++) {
                mapImages[i] = Image.FromFile(String.Format("MapImages/map{0}.png", i));
            }

            Main();
        }

        public unsafe Node[,] CreateNodes(Image image, int z, List<Dictionary<Tuple<int, int>, string>> doors) {
            Node[,] array = new Node[image.Width, image.Height];

            Bitmap b = new Bitmap(image);
            BitmapData bData = b.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, b.PixelFormat);

            int bitsPerPixel = Image.GetPixelFormatSize(bData.PixelFormat);

            var dict = doors[z];

            // First create the set of nodes out of walkable tiles
            byte* scan0 = (byte*)bData.Scan0.ToPointer();
            for (int x = 0; x < bData.Width; x++) {
                for (int y = 0; y < bData.Height; y++) {
                    byte* data = scan0 + y * bData.Stride + x * bitsPerPixel / 8;
                    Color color = Color.FromArgb(data[3], data[2], data[1], data[0]);
                    if (walkableColors.Contains(color) || dict.ContainsKey(new Tuple<int, int>(x, y))) {
                        array[x, y] = new Node(color, x, y, z);
                    } else {
                        array[x, y] = null;
                    }
                }
            }
            b.UnlockBits(bData);
            this.pictureBox1.Image = b;
            // Now create the neighboring nodes
            for (int x = 0; x < image.Width; x++) {
                for (int y = 0; y < image.Height; y++) {
                    if (array[x, y] != null) {
                        if (array[x - 1, y] != null) {
                            array[x, y].addNeighbor(array[x - 1, y]);
                        }
                        if (array[x + 1, y] != null) {
                            array[x, y].addNeighbor(array[x + 1, y]);
                        }
                        if (array[x, y - 1] != null) {
                            array[x, y].addNeighbor(array[x, y - 1]);
                        }
                        if (array[x, y + 1] != null) {
                            array[x, y].addNeighbor(array[x, y + 1]);
                        }
                        if (array[x + 1, y + 1] != null) {
                            array[x, y].addNeighbor(array[x + 1, y + 1]);
                        }
                        if (array[x - 1, y + 1] != null) {
                            array[x, y].addNeighbor(array[x - 1, y + 1]);
                        }
                        if (array[x + 1, y - 1] != null) {
                            array[x, y].addNeighbor(array[x + 1, y - 1]);
                        }
                        if (array[x - 1, y - 1] != null) {
                            array[x, y].addNeighbor(array[x - 1, y - 1]);
                        }
                    }
                }
            }


            return array;
        }

        public void inheritChildren(Node main, Node merge, Dictionary<Node, Node> originalList) {
            if (merge.children == null) {
                Node n = originalList[merge];
                n.parent = main;
                main.children.Add(n);
            } else {
                foreach (Node ch in merge.children) {
                    inheritChildren(main, ch, originalList);
                }
            }
        }

        public string ReverseName(string str) {
            if (str.ToLower().Contains(" from ") && str.ToLower().Contains(" to ")) {
                string[] splits = str.Split(' ');
                string final = "";
                string from = null;
                for(int i = 0; i < splits.Length; i++) {
                    if (splits[i].ToLower() == "to") {
                        final += "from ";
                        for(int j = i + 1; j < splits.Length; j++) {
                            final += splits[j] + " ";
                        }
                        final += "to " + from;
                        final = final.Substring(0, final.Length - 1) + ".";
                        return final;
                    } else if (from != null) {
                        from += splits[i] + " ";
                    } else if (splits[i].ToLower() != "from") {
                        final += splits[i] + " ";
                    } else {
                        from = "";
                    }
                }
            }
            return str;
        }

        public List<SpecialConnection> CreateSpecialConnections(List<Node[,]> nodeLists) {
            Stopwatch sw;

            List<SpecialConnection> specialConnections = new List<SpecialConnection>();
            
            sw = Stopwatch.StartNew();
            // Add connections between floors created by stairs/rope holes (these are added if both image (i) and image (i + 1) have the color yellow at a pixel)
            for (int x = 0; x < mapImages[7].Width; x++) {
                for (int y = 0; y < mapImages[7].Height; y++) {
                    for (int i = 0; i < 15; i++) {
                        if (nodeLists[i][x, y] != null && nodeLists[i][x, y].color == stairsColor) {
                            Node other = null;
                            if (nodeLists[i + 1][x, y] != null && nodeLists[i + 1][x, y].color == stairsColor) {
                                other = nodeLists[i + 1][x, y];
                            } else if (nodeLists[i + 1][x + 1, y] != null && nodeLists[i + 1][x + 1, y].color == stairsColor) {
                                other = nodeLists[i + 1][x + 1, y];
                            } else if (nodeLists[i + 1][x - 1, y] != null && nodeLists[i + 1][x - 1, y].color == stairsColor) {
                                other = nodeLists[i + 1][x - 1, y];
                            } else if (nodeLists[i + 1][x, y - 1] != null && nodeLists[i + 1][x, y - 1].color == stairsColor) {
                                other = nodeLists[i + 1][x, y - 1];
                            } else if (nodeLists[i + 1][x, y + 1] != null && nodeLists[i + 1][x, y + 1].color == stairsColor) {
                                other = nodeLists[i + 1][x, y + 1];
                            }
                            if (other != null) {
                                SpecialConnection connection = new SpecialConnection { source = nodeLists[i][x, y], destination = other, cost = 50, name = "Stairs" };
                                SpecialConnection connection2 = new SpecialConnection { source = other, destination = nodeLists[i][x, y], cost = 50, name = "Stairs" };
                                specialConnections.Add(connection);
                                specialConnections.Add(connection2);
                                nodeLists[i][x, y].addNeighbor(other, connection);
                                other.addNeighbor(nodeLists[i][x, y], connection2);
                            }
                        }
                    }
                }
            }

            // Add connections that are stored in the teleports.xml file
            // These are teleports that cannot be deduced from the image files themselves (i.e. actual teleporters, boats)
            using (StreamReader str = new StreamReader("teleports.xml")) {
                using (XmlReader reader = XmlReader.Create(str)) {
                    Node startNode = null, endNode = null;
                    string currentNode = null;
                    string startName = "";
                    Dictionary<string, string> attributes = new Dictionary<string, string>();
                    while (reader.Read()) {
                        switch (reader.NodeType) {
                            case XmlNodeType.Element:
                                currentNode = reader.Name;
                                attributes.Clear();
                                if (currentNode == "Destination") {
                                    attributes.Add("Name", reader.GetAttribute("Name"));
                                    attributes.Add("Cost", reader.GetAttribute("Cost"));
                                }
                                break;
                            case XmlNodeType.Text:
                                if (currentNode == "Name") {
                                    startName = reader.Value;
                                } else if (currentNode == "Start") {
                                    string[] split = reader.Value.Split(',');
                                    int x = int.Parse(split[0]);
                                    int y = int.Parse(split[1]);
                                    int z = int.Parse(split[2]);
                                    startNode = nodeLists[z][x, y];
                                } else if (currentNode == "Destination") {
                                    string name = startName + " to " + attributes["Name"];
                                    int cost = int.Parse(attributes["Cost"]);
                                    string[] split = reader.Value.Split(',');
                                    int x = int.Parse(split[0]);
                                    int y = int.Parse(split[1]);
                                    int z = int.Parse(split[2]);
                                    endNode = nodeLists[z][x, y];

                                    SpecialConnection connection = new SpecialConnection { source = startNode, destination = endNode, cost = cost, name = name };
                                    SpecialConnection connection2 = new SpecialConnection { source = endNode, destination = startNode, cost = cost, name = ReverseName(name) };
                                    specialConnections.Add(connection);
                                    specialConnections.Add(connection2);
                                    startNode.addNeighbor(endNode, connection);
                                    endNode.addNeighbor(startNode, connection2);
                                }
                                break;
                            case XmlNodeType.XmlDeclaration:
                            case XmlNodeType.ProcessingInstruction:
                            case XmlNodeType.Comment:
                                break;
                            case XmlNodeType.EndElement:
                                currentNode = null;
                                if (reader.Name == "Teleport") {
                                    startNode = null;
                                }
                                break;
                        }
                    }
                }
            }
            // Add connections that are stored in the 'teleports' file
            using (StreamReader reader = new StreamReader("teleports")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] split = line.Split('-');
                    if (split.Length != 3) continue;

                    string[] source = split[0].Split(',');
                    string[] dest = split[1].Split(',');
                    string name = split[2];
                    bool one_way = false;
                    if (name.Contains(" ONE WAY")) {
                        one_way = true;
                        name = name.Replace(" ONE WAY", "");
                    }
                    int x, y, z;
                    x = int.Parse(source[0]);
                    y = int.Parse(source[1]);
                    z = int.Parse(source[2]);
                    Node startNode = nodeLists[z][x, y];
                    x = int.Parse(dest[0]);
                    y = int.Parse(dest[1]);
                    z = int.Parse(dest[2]);
                    Node endNode = nodeLists[z][x, y];

                    if (startNode == null || endNode == null) {
                        Console.WriteLine(String.Format("Warning: {0}", line));
                        continue;
                    }

                    SpecialConnection connection;
                    if (!startNode.hasNeighbor(endNode)) {
                        connection = new SpecialConnection { source = startNode, destination = endNode, cost = 100, name = name };
                        specialConnections.Add(connection);
                        startNode.addNeighbor(endNode, connection);
                    }

                    if (!one_way) {
                        if (!endNode.hasNeighbor(startNode)) {
                            connection = new SpecialConnection { source = endNode, destination = startNode, cost = 100, name = ReverseName(name) };
                            specialConnections.Add(connection);
                            endNode.addNeighbor(startNode, connection);
                        }
                    }
                }
            }

            sw.Stop();
            Console.WriteLine("Create teleports: {0}ms", sw.Elapsed.TotalMilliseconds);

            return specialConnections;
        }

        public List<Dictionary<Tuple<int, int>, string>> CreateDoors() {
            var doors = new List<Dictionary<Tuple<int, int>, string>>();
            for (int i = 0; i <= 15; i++) {
                doors.Add(new Dictionary<Tuple<int, int>, string>());
            }

            using (StreamReader reader = new StreamReader("doors")) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] split = line.Split('-');
                    string condition = null;
                    if (split.Length > 1) {
                        condition = split[1];
                    }
                    string[] location = split[0].Split(',');

                    int x, y, z;
                    x = int.Parse(location[0]);
                    y = int.Parse(location[1]);
                    z = int.Parse(location[2]);
                    var tpl = new Tuple<int, int>(x, y);
                    if (doors[z].ContainsKey(tpl)) continue;
                    doors[z].Add(tpl, condition);
                }
            }
            return doors;
        }

        public void CreateDatabase() {
            Stopwatch sw;

            File.Delete("nodes.db");

            var doors = CreateDoors();

            sw = Stopwatch.StartNew();
            List<Node[,]> nodeLists = new List<Node[,]>();
            // Create the set of nodes for every map image
            for (int i = 0; i <= 15; i++) {
                nodeLists.Add(CreateNodes(mapImages[i], i, doors));
            }
            sw.Stop();
            Console.WriteLine("Create initial nodes {0}ms", sw.Elapsed.TotalMilliseconds);

            var specialConnections = CreateSpecialConnections(nodeLists);

            sw = Stopwatch.StartNew();
            // Now do some optimization stuff
            List<Node> allNodes = new List<Node>();
            List<Node> highLevelNodes = new List<Node>();
            for (int k = 0; k <= 15; k++) {
                HashSet<Node> hierarchicalNodes = new HashSet<Node>();
                Node[,] nodes = nodeLists[k];
                for (int x = 0; x < mapImages[7].Width; x++) {
                    for (int y = 0; y < mapImages[7].Height; y++) {
                        if (nodes[x, y] != null) {
                            hierarchicalNodes.Add(nodes[x, y]);
                            nodes[x, y].parent = null;
                        }
                    }
                }

                // we create a layer above the 'base' nodes
                // this layer is used to speed up pathfinding significantly
                // basically, we create a high-level node that is the parent to a certain amount of nodes
                // it connects to other high level nodes that are reachable from the child-nodes
                // this is basically a high level 'filter' step that allows us to immediately discard a large number of paths early on
                // by first computing the path on these high level nodes (which is cheap because there are few of them)
                // we only need to consider paths that go through the high level nodes, filtering a significant amount of possible paths
                int childCount = 100; // this is the maximum amount of children for the hierachical node
                while (hierarchicalNodes.Count > 0) {
                    Node current = null;
                    foreach (Node n in hierarchicalNodes) {
                        current = n;
                        break;
                    }
                    if (current == null) break;

                    Node hierarchicalNode = new Node(current);

                    hierarchicalNode.children = new List<Node>();
                    List<Node> openSet = new List<Node> { current };
                    HashSet<Node> closedSet = new HashSet<Node>() { current };
                    while (openSet.Count > 0 && hierarchicalNode.children.Count < childCount) {
                        Node n = openSet[0];
                        openSet.RemoveAt(0);
                        hierarchicalNode.children.Add(n);
                        hierarchicalNodes.Remove(n);
                        if (n.parent != null) throw new Exception("Already has a parent.");
                        n.parent = hierarchicalNode;
                        foreach (Connection conn in n.neighbors) {
                            if (conn.settings != null) continue;
                            Node neighbor = conn.neighbor as Node;
                            if (hierarchicalNodes.Contains(neighbor) && !closedSet.Contains(neighbor)) {
                                closedSet.Add(neighbor);
                                openSet.Add(neighbor);
                            }
                        }
                    }
                    // set up the bounding rectangles of the high level nodes
                    foreach (Node n in hierarchicalNode.children) {
                        hierarchicalNode.Merge(n.rect);
                    }
                    highLevelNodes.Add(hierarchicalNode);
                }
            }

            // now set up the neighbors of the high level nodes
            // we check the neighbors of the low-level nodes, and check the high-level node they are part of
            // if they are part of a different high-level node, that high-level node is reachable from this high-level node, thus it is a neighbor
            for (int k = 0; k <= 15; k++) {
                foreach (Node hierarchicalNode in highLevelNodes) {
                    foreach (Node n in hierarchicalNode.children) {
                        foreach (Connection conn in n.neighbors) {
                            Node neighbor = conn.neighbor as Node;
                            if (neighbor.parent == null) throw new Exception("Node without a parent.");
                            if (neighbor.parent != hierarchicalNode) {
                                Connection existingConnection = hierarchicalNode.getNeighbor(neighbor.parent);
                                if (existingConnection == null) {
                                    hierarchicalNode.addNeighbor(neighbor.parent, conn.settings);
                                }
                            }
                        }
                    }
                }
            }

            Node node = nodeLists[beginz][beginx, beginy];

            sw.Stop();
            Console.WriteLine("Create hierarchical stuff: {0}ms", sw.Elapsed.TotalMilliseconds);


            SQLiteConnection sqlconn = new SQLiteConnection(String.Format("Data Source={0};Version=3;", "nodes.db"));
            sqlconn.Open();
            SQLiteCommand command;
            command = new SQLiteCommand("CREATE TABLE Doors(x INTEGER, y INTEGER, z INTEGER, condition STRING)", sqlconn);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE TABLE SpecialConnections(id INTEGER PRIMARY KEY AUTOINCREMENT, x1 INTEGER, y1 INTEGER, z1 INTEGER, x2 INTEGER, y2 INTEGER, z2 INTEGER, name STRING, cost INTEGER)", sqlconn);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE TABLE HierarchicalNode(id INTEGER PRIMARY KEY AUTOINCREMENT, x INTEGER, y INTEGER, z INTEGER,  width INTEGER, height INTEGER)", sqlconn);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("CREATE TABLE HierarchicalConnections(nodeid INTEGER, nodeid2 INTEGER, specialid INTEGER)", sqlconn);
            command.ExecuteNonQuery();

            using (SQLiteTransaction transaction = sqlconn.BeginTransaction()) {
                for (int i = 0; i < doors.Count; i++) {
                    foreach (var value in doors[i]) {
                        command = new SQLiteCommand(String.Format("INSERT INTO Doors(x, y, z, condition) VALUES ({0},{1},{2},{3})", value.Key.Item1, value.Key.Item2, i, value.Value == null ? "NULL" : String.Format("'{0}'", value.Value)), sqlconn, transaction);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (Node n in highLevelNodes) {
                    command = new SQLiteCommand(String.Format("INSERT INTO HierarchicalNode(x, y, z, width, height) VALUES ({0},{1},{2},{3},{4})", n.rect.X, n.rect.Y, n.z, n.rect.Width, n.rect.Height), sqlconn, transaction);
                    command.ExecuteNonQuery();
                    command.CommandText = "select last_insert_rowid()";
                    int rowid = (int)(long)command.ExecuteScalar();
                    n.weight = rowid;
                }
                foreach (Node n in highLevelNodes) {
                    foreach (Connection c in n.neighbors) {
                        int special_id = -1;
                        if (c.settings != null) {
                            SpecialConnection connection = c.settings;
                            command = new SQLiteCommand(String.Format("INSERT INTO SpecialConnections(x1, y1, z1, x2, y2, z2, name, cost) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, '{6}', {7});", connection.source.rect.X, connection.source.rect.Y, connection.source.z, connection.destination.rect.X, connection.destination.rect.Y, connection.destination.z, connection.name.Replace("'", "''"), connection.cost), sqlconn, transaction);
                            command.ExecuteNonQuery();
                            command.CommandText = "select last_insert_rowid()";
                            special_id = (int)(long)command.ExecuteScalar();
                        }
                        command = new SQLiteCommand(String.Format("INSERT INTO HierarchicalConnections(nodeid, nodeid2, specialid) VALUES ({0},{1}, {2})", n.weight, (c.neighbor as Node).weight, special_id), sqlconn, transaction);
                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
        }

        public void LoadFromDatabase() {
            Pathfinder.LoadFromDatabase("nodes.db");
        }

        public static int beginx, beginy, beginz, endx, endy, endz;

        public void RedrawRoute() {
            Stopwatch sw;

            sw = Stopwatch.StartNew();
            Point3D start = new Point3D(beginx, beginy, beginz);
            Point3D target = new Point3D(endx, endy, endz);
            var res = Pathfinder.FindRoute(start, target, new Bitmap(mapImages[start.Z]));
            DijkstraPoint point = res.Item1;
            var rects = res.Item2;
            sw.Stop();
            Console.WriteLine("Find Route: {0}ms", sw.Elapsed.TotalMilliseconds);


            Bitmap bitmap = new Bitmap(mapImages[beginz]);

            DijkstraPoint tempPoint = point;
            while (point != null) {
                if (point.point.X < mapX) {
                    mapX = point.point.X - 20;
                }
                if (point.point.X > mapX + mapWidth) {
                    mapWidth = point.point.X - mapX + 20;
                }
                if (point.point.Y < mapY) {
                    mapY = point.point.Y - 20;
                }
                if (point.point.Y > mapY + mapHeight) {
                    mapHeight = point.point.Y - mapY + 20;
                }
                point = point.previous;
            }
            point = tempPoint;
            using (Graphics gr = Graphics.FromImage(bitmap)) {
                while (point.previous != null) {
                    gr.DrawLine(Pens.Black, new Point((int)point.point.X, (int)point.point.Y), new Point((int)point.previous.point.X, (int)point.previous.point.Y));
                    point = point.previous;
                }

                foreach (Rectangle3D rect in rects) {
                    gr.DrawRectangle(Pens.Yellow, rect.Rect);
                }
            }
            bigMap = bitmap;
            UpdateMap();
        }

        public void Main() {
            string beginstr = "126.57,125.189,7";
            string endstr = "126.55,125.172,6";

            int minx = 124 * 256;
            int miny = 121 * 256;
            beginx = int.Parse(beginstr.Split(',')[0].Split('.')[0]) * 256 + int.Parse(beginstr.Split(',')[0].Split('.')[1]) - minx;
            beginy = int.Parse(beginstr.Split(',')[1].Split('.')[0]) * 256 + int.Parse(beginstr.Split(',')[1].Split('.')[1]) - miny;
            beginz = int.Parse(beginstr.Split(',')[2]);
            endx = int.Parse(endstr.Split(',')[0].Split('.')[0]) * 256 + int.Parse(endstr.Split(',')[0].Split('.')[1]) - minx;
            endy = int.Parse(endstr.Split(',')[1].Split('.')[0]) * 256 + int.Parse(endstr.Split(',')[1].Split('.')[1]) - miny;
            endz = int.Parse(endstr.Split(',')[2]);

            CreateDatabase();
            LoadFromDatabase();

            RedrawRoute();
        }




        int mapX = int.MaxValue;
        int mapY = int.MaxValue;

        private void button1_Click(object sender, EventArgs e) {
            beginx--;
            RedrawRoute();
        }

        private void button2_Click(object sender, EventArgs e) {
            beginy--;
            RedrawRoute();
        }

        private void button3_Click(object sender, EventArgs e) {
            beginx++;
            RedrawRoute();
        }

        private void button4_Click(object sender, EventArgs e) {
            beginy++;
            RedrawRoute();
        }

        int mapWidth = 10;
        int mapHeight = 10;

        Bitmap bigMap;
        Bitmap mapImage;
        private void UpdateMap() {
            if (mapImage != null) mapImage.Dispose();
            mapImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics gr = Graphics.FromImage(mapImage)) {
                Image big_map = bigMap;
                Point point = new Point(mapX, mapY);
                Rectangle sourceRectangle = new Rectangle(
                    point.X,
                    point.Y,
                    mapWidth,
                    mapHeight);

                gr.DrawImage(big_map, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height), sourceRectangle, GraphicsUnit.Pixel);
            }

            pictureBox1.Image = mapImage;
        }
    }

    public class SpecialConnection {
        public Node source;
        public Node destination;
        public string name;
        public int cost;
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tibia_Path_Finder {

    public class ConnectionSettings {
        public string name;
        public int cost;

        public ConnectionSettings(string name, int cost) {
            this.name = name;
            this.cost = cost;
        }
    }

    public class Connection {
        public NormalNode neighbor;
        public SpecialConnection settings;

        public Connection(NormalNode neighbor, SpecialConnection settings = null) {
            this.neighbor = neighbor;
            this.settings = settings;
        }
    }

    public class NormalNode {
        public List<Connection> neighbors;
        public Rectangle rect;
        public int z;
        public NormalNode(int x, int y, int z, int width, int height) {
            this.rect = new Rectangle(x, y, width, height);
            this.z = z;
            this.neighbors = new List<Connection>();
        }
        public NormalNode(int x, int y, int z, int size) {
            this.rect = new Rectangle(x, y, size, size);
            this.z = z;
            this.neighbors = new List<Connection>();
        }

        public NormalNode(int z) {
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

    public class HierarchicalNode : NormalNode {
        public List<NormalNode> children;

        public HierarchicalNode(int x, int y, int z, int width, int height) : base(z) {
            this.rect = new Rectangle(x, y, width, height);
            this.children = new List<NormalNode>();
        }
    }


    public class Node : NormalNode {
        public int weight;
        public Node parent;
        public List<Node> children;
        public Color color;


        public Node(Node n) : base(n.z) {
            this.rect = n.rect;
            this.weight = n.z;
            this.parent = null;
            this.children = null;
            this.color = n.color;
        }

        public Node(Color color, int x, int y, int z) : base(z) {
            this.rect = new Rectangle();
            rect.X = x;
            rect.Y = y;
            rect.Width = 1;
            rect.Height = 1;
            this.weight = 1;
            this.parent = null;
            this.color = color;
        }

        public override string ToString() {
            return rect.ToString();
        }

        public void Merge(Rectangle other) {
            this.rect = Rectangle.Union(rect, other);
            /*
            rect.X = Math.Min(rect.X, other.X);
            rect.Y = Math.Min(rect.Y, other.Y);
            rect.Width = Math.Max(rect.X + rect.Width, other.X + other.Width) - rect.X;
            rect.Height = Math.Max(rect.Y + rect.Height, other.Y + other.Height) - rect.Y;*/
        }

        public void addNeighbor(Node node, SpecialConnection settings = null, bool possibleDuplicate = false) {
            if (!possibleDuplicate && this.neighbors.Find(o => o.neighbor == node) != null) {
                throw new Exception("");
            }
            this.neighbors.Add(new Connection(node, settings));
        }

        public void removeNeighbor(Node node) {
            this.neighbors.Remove(this.neighbors.Find(o => o.neighbor == node));
        }

        public bool hasNeighbor(Node node) {
            return this.neighbors.Find(o => o.neighbor == node) != null;
        }

        public Connection getNeighbor(Node node) {
            return this.neighbors.Find(o => o.neighbor == node);
        }
    }
}

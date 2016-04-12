
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
using System.Windows.Forms;

namespace Tibialyzer {
    public class Target {
        public Image image;
        public int size;
        public Coordinate coordinate;
    }

    public class TibiaPath {
        public DijkstraPoint path;
        public Coordinate begin;
        public Coordinate end;
    }

    public class MapPictureBox : PictureBox {
        public Image mapImage;
        public Coordinate mapCoordinate;
        public int sourceWidth;
        public int zCoordinate;
        public Coordinate beginCoordinate;
        public List<Target> targets;
        public List<TibiaPath> paths;
        public Coordinate targetCoordinate = null;
        private Point3D previousCoordinate = new Point3D(-1, -1, -1);
        private Point3D FakePlayerData = new Point3D(-1, -1, -1);
        public int beginWidth;
        public Map map;
        public Map otherMap;
        public const int minWidth = 40;
        public const int maxWidth = 400;
        public delegate void MapUpdatedHandler();
        public string nextTarget = null;
        public string nextImportantTarget = null;
        public Point3D nextConnectionPoint = new Point3D(-1, -1, -1);
        TibiaPath playerPath = null;
        public event MapUpdatedHandler MapUpdated;

        public MapPictureBox() {
            mapImage = null;
            mapCoordinate = null;
            beginCoordinate = null;
            sourceWidth = 0;
            beginWidth = 0;
            targets = new List<Target>();
            paths = new List<TibiaPath>();
            map = null;
            otherMap = null;
        }

        protected override void Dispose(bool disposing) {
            if (mapImage != null) {
                mapImage.Dispose();
            }
            if (map != null) {
                map.Dispose();
            }
            if (otherMap != null) {
                otherMap.Dispose();
            }
            base.Dispose(disposing);
        }

        private int convertx(int x) {
            return (int)((double)(x - (mapCoordinate.x - sourceWidth / 2)) / sourceWidth * this.Width);
        }

        private int converty(int y) {
            return (int)((double)(y - (mapCoordinate.y - sourceWidth / 2)) / sourceWidth * this.Height);
        }

        public void DrawPath(Graphics gr, TibiaPath path) {
            if (path.begin.z == mapCoordinate.z) {
                List<Point> points = new List<Point>();
                DijkstraPoint node = path.path;
                while (node != null) {
                    points.Add(new Point(convertx(node.point.X), converty(node.point.Y)));
                    node = node.previous;
                }
                if (points.Count <= 1) return;
                gr.DrawLines(UIManager.pathPen, points.ToArray());
            }
        }

        System.Timers.Timer refreshTimer = new System.Timers.Timer();
        public void SetTargetCoordinate(Coordinate coordinate) {
            this.targetCoordinate = coordinate;
            refreshTimer = new System.Timers.Timer(50);
            refreshTimer.Elapsed += RefreshMapTimer;
            refreshTimer.Enabled = true;
        }

        private object mapBoxLock = new object();
        private void RefreshMapTimer(object sender, System.Timers.ElapsedEventArgs e) {
            if (this.IsDisposed) return;
            try {
                refreshTimer.Dispose();
                refreshTimer = null;
                this.Invoke((MethodInvoker)delegate {
                    UpdateMap(true);
                });
                refreshTimer = new System.Timers.Timer(50);
                refreshTimer.Elapsed += RefreshMapTimer;
                refreshTimer.Enabled = true;
            } catch {

            }
        }

        private DijkstraNode previousResult = null;

        public void UpdateMap(bool periodicUpdate = false) {
            lock (mapBoxLock) {
                int PlayerX = 0, PlayerY = 0, PlayerZ = 0;
                bool recomputeRoute = true;

                if (targetCoordinate != null) {
                    MemoryReader.UpdateBattleList();
                    PlayerX = MemoryReader.X;
                    PlayerY = MemoryReader.Y;
                    PlayerZ = MemoryReader.Z;

                    Point3D playerCoordinate = new Point3D(PlayerX, PlayerY, PlayerZ);
                    if (previousCoordinate != playerCoordinate) {
                        previousCoordinate = playerCoordinate;
                        mapCoordinate = new Coordinate(PlayerX, PlayerY, PlayerZ);
                    } else {
                        if (FakePlayerData.X >= 0) {
                            PlayerX = FakePlayerData.X;
                            PlayerY = FakePlayerData.Y;
                            PlayerZ = FakePlayerData.Z;

                            mapCoordinate = new Coordinate(PlayerX, PlayerY, PlayerZ);

                            FakePlayerData = new Point3D(-1, -1, -1);
                        } else {
                            if (periodicUpdate) return;
                            recomputeRoute = false;
                        }
                    }
                }
                if (beginCoordinate == null) {
                    beginCoordinate = new Coordinate(mapCoordinate);
                    beginWidth = sourceWidth;
                }
                if (beginCoordinate.x == Coordinate.MaxWidth / 2 && beginCoordinate.y == Coordinate.MaxHeight / 2 && beginCoordinate.z == 7) {
                    if (this.Image != StyleManager.GetImage("nomapavailable.png") && this.Image != null) {
                        this.Image.Dispose();
                    }
                    this.Image = StyleManager.GetImage("nomapavailable.png");
                    this.SizeMode = PictureBoxSizeMode.Zoom;
                    return;
                }
                if (mapCoordinate.z < 0) {
                    mapCoordinate.z = 0;
                } else if (mapCoordinate.z >= StorageManager.mapFilesCount) {
                    mapCoordinate.z = StorageManager.mapFilesCount - 1;
                }
                if (mapCoordinate.x - sourceWidth / 2 < 0) {
                    mapCoordinate.x = sourceWidth / 2;
                }
                if (mapCoordinate.x + sourceWidth / 2 > Coordinate.MaxWidth) {
                    mapCoordinate.x = Coordinate.MaxWidth - sourceWidth / 2;
                }
                if (mapCoordinate.y - sourceWidth / 2 < 0) {
                    mapCoordinate.y = sourceWidth / 2;
                }
                if (mapCoordinate.y + sourceWidth / 2 > Coordinate.MaxHeight) {
                    mapCoordinate.y = Coordinate.MaxHeight - sourceWidth / 2;
                }

                Image image;
                if (mapCoordinate.z == zCoordinate) {
                    image = map != null ? map.GetImage() : mapImage;
                } else {
                    Map m = StorageManager.getMap(mapCoordinate.z);
                    if (otherMap != null && m != otherMap) {
                        otherMap.Dispose();
                    }
                    otherMap = m;
                    image = m.GetImage();
                }

                lock (image) {
                    sourceWidth = Math.Min(Math.Max(sourceWidth, minWidth), maxWidth);
                    Rectangle sourceRectangle = new Rectangle(mapCoordinate.x - sourceWidth / 2, mapCoordinate.y - sourceWidth / 2, sourceWidth, sourceWidth);
                    Bitmap bitmap = new Bitmap(this.Width, this.Height);
                    using (Graphics gr = Graphics.FromImage(bitmap)) {
                        gr.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), sourceRectangle, GraphicsUnit.Pixel);

                        if (targetCoordinate != null && recomputeRoute) {
                            Coordinate beginCoordinate = new Coordinate(PlayerX, PlayerY, PlayerZ);

                            Node beginNode = Pathfinder.GetNode(beginCoordinate.x, beginCoordinate.y, beginCoordinate.z);
                            Node endNode = Pathfinder.GetNode(targetCoordinate.x, targetCoordinate.y, targetCoordinate.z);

                            List<Rectangle3D> collisionBounds = null;
                            DijkstraNode highresult = Dijkstra.FindRoute(beginNode, endNode, new Point3D(targetCoordinate), previousResult);
                            previousResult = highresult;
                            SpecialConnection connection = null;

                            nextConnectionPoint = new Point3D(-1, -1, -1);
                            nextImportantTarget = null;
                            nextTarget = "Head to the destination.";
                            if (highresult != null) {
                                collisionBounds = new List<Rectangle3D>();
                                while (highresult != null) {
                                    highresult.rect.Inflate(5, 5);
                                    collisionBounds.Add(new Rectangle3D(highresult.rect, highresult.node.z));
                                    /*if (highresult.node.z == beginCoordinate.z) {
                                        Point tl = new Point(convertx(highresult.rect.X), converty(highresult.rect.Y));
                                        Point tr = new Point(convertx(highresult.rect.X + highresult.rect.Width), converty(highresult.rect.Y + highresult.rect.Height));
                                        gr.DrawRectangle(Pens.Yellow, new Rectangle(tl.X, tl.Y, (tr.X - tl.X), (tr.Y - tl.Y)));
                                    }*/
                                    if (highresult.connection.connection != null) {
                                        connection = highresult.connection.connection;
                                        if (connection.name.Equals("stairs", StringComparison.InvariantCultureIgnoreCase)) {
                                            nextTarget = connection.destination.z > connection.source.z ? "Go down the stairs." : "Go up the stairs.";
                                        } else if (connection.name.Equals("levitate", StringComparison.InvariantCultureIgnoreCase)) {
                                            nextTarget = connection.destination.z > connection.source.z ? "Levitate down." : "Levitate up.";
                                        } else {
                                            nextImportantTarget = String.Format("Take the {0}.", connection.name);
                                            nextTarget = null;
                                        }
                                        nextConnectionPoint = new Point3D(connection.destination.x, connection.destination.y, connection.destination.z);
                                    }
                                    highresult = highresult.previous;
                                }
                                if (collisionBounds.Count == 0) collisionBounds = null;
                            }

                            Map m = StorageManager.getMap(beginCoordinate.z);
                            DijkstraPoint result = Dijkstra.FindRoute(image as Bitmap, new Point3D(beginCoordinate), new Point3D(targetCoordinate), collisionBounds, null, connection);
                            if (result != null) {
                                playerPath = new TibiaPath();
                                playerPath.path = result;
                                playerPath.begin = beginCoordinate;
                                playerPath.end = targetCoordinate;
                                DrawPath(gr, playerPath);
                            }
                        } else if (!recomputeRoute && playerPath != null) {
                            DrawPath(gr, playerPath);
                        }

                        foreach (TibiaPath path in paths) {
                            DrawPath(gr, path);
                        }

                        foreach (Target target in targets) {
                            if (target.coordinate.z == mapCoordinate.z) {
                                int x = target.coordinate.x - (mapCoordinate.x - sourceWidth / 2);
                                int y = target.coordinate.y - (mapCoordinate.y - sourceWidth / 2);
                                if (x >= 0 && y >= 0 && x < sourceWidth && y < sourceWidth) {
                                    x = (int)((double)x / sourceWidth * bitmap.Width);
                                    y = (int)((double)y / sourceWidth * bitmap.Height);
                                    lock (target.image) {
                                        int targetWidth = (int)((double)target.size / target.image.Height * target.image.Width);
                                        gr.DrawImage(target.image, new Rectangle(x - targetWidth, y - target.size, targetWidth * 2, target.size * 2));
                                    }
                                }
                            }
                        }
                    }
                    if (this.Image != null) {
                        lock (this.Image) {
                            this.Image.Dispose();
                        }
                    }
                    this.Image = bitmap;
                }
                if (MapUpdated != null)
                    MapUpdated();
            }
        }

        bool drag_map = false;
        Point screen_center;
        Point center_point;
        Point initial_position;
        protected override void OnMouseUp(MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                if (drag_map) {
                    drag_map = false;
                    System.Windows.Forms.Cursor.Show();
                    System.Windows.Forms.Cursor.Position = initial_position;
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                this.Focus();
                drag_map = false;
                initial_position = System.Windows.Forms.Cursor.Position;
                Screen screen = Screen.FromControl(this);
                screen_center = new Point(screen.Bounds.X + screen.Bounds.Width / 2, screen.Bounds.Y + screen.Bounds.Height / 2);
                System.Windows.Forms.Cursor.Position = new Point(screen_center.X, screen_center.Y);
                center_point = new Point(screen_center.X, screen_center.Y);
                System.Windows.Forms.Cursor.Hide();
                drag_map = true;
            }
            base.OnMouseDown(e);
        }

        int counter = 0;
        bool skipMovement = false;
        protected override void OnMouseMove(MouseEventArgs e) {
            if (skipMovement) return;
            if (drag_map) {
                Point screenPoint = this.PointToScreen(new Point(e.X, e.Y));
                mapCoordinate.x = (int)Math.Max(Math.Min(mapCoordinate.x + (screenPoint.X - center_point.X), Coordinate.MaxWidth), 0);
                mapCoordinate.y = (int)Math.Max(Math.Min(mapCoordinate.y + (screenPoint.Y - center_point.Y), Coordinate.MaxHeight), 0);
                this.UpdateMap();
                center_point = screenPoint;
                if (counter++ > 10) {
                    // we reset the position of the cursor to the center of the screen every 10 ticks
                    // this is to prevent the cursor from hitting the edges of the monitor while moving the map
                    // hitting the edges of the monitor prevents the user from moving the map, but the cursor is invisible so it is weird for the user
                    // doing this every tick makes the map movement not work as smoothly, so we only do it every 10 ticks, that should be plenty anyway
                    skipMovement = true;
                    System.Windows.Forms.Cursor.Position = new Point(screen_center.X, screen_center.Y);
                    center_point = new Point(screen_center.X, screen_center.Y);
                    skipMovement = false;
                    counter = 0;
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            sourceWidth -= e.Delta / 3;
            sourceWidth = Math.Min(Math.Max(sourceWidth, minWidth), maxWidth);
            UpdateMap();
            base.OnMouseWheel(e);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.KeyData == Keys.Subtract || e.KeyData == Keys.OemMinus) {
                mapCoordinate.z--;
                UpdateMap();
            } else if (e.KeyData == Keys.Add || e.KeyData == Keys.Oemplus) {
                mapCoordinate.z++;
                UpdateMap();
            } else if (e.KeyData == Keys.Space) {
                mapCoordinate = new Coordinate(beginCoordinate);
                sourceWidth = beginWidth;
                UpdateMap();
            } else if (e.KeyData == Keys.Right) {
                FakePlayerData = nextConnectionPoint;
            } else {
                base.OnKeyDown(e);
            }
        }

        public void NextStep() {
            if (nextConnectionPoint.X >= 0) {
                FakePlayerData = nextConnectionPoint;
            }
        }
    }
}

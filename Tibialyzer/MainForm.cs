using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Numerics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;

namespace Tibialyzer {
    public partial class MainForm : Form {
        public static MainForm mainForm;
        public static ScriptEngine pyEngine = null;
        public static Color background_color = Color.FromArgb(0, 51, 102);
        public static double opacity = 0.8;
        public static bool transparent = true;
        public static Image[] image_numbers = new Image[10];
        private Form tooltipForm = null;
        private static Image tibia_image = null;
        public static Image back_image = null;
        public static Image prevpage_image = null;
        public static Image nextpage_image = null;
        public static Image item_background = null;
        public static Image cross_image = null;
        public static Image star_image = null;
        public static Image mapup_image = null;
        public static Image mapdown_image = null;
        private bool keep_working = true;
        private static string database_file = @"Database\Database.db";
        private static string settings_file = @"Database\settings.txt";
        private List<string> character_names = new List<string>();
        public static List<Map> map_files = new List<Map>();
        public static Color label_text_color = Color.FromArgb(191, 191, 191);
        public static int max_creatures = 50;
        public string priority_command = null;
        public List<string> new_names = null;
        private bool prevent_settings_update = false;
        private bool minimize_notification = true;
        public int notification_seconds = 20;
        public bool allow_extensions = true;
        public bool copy_advances = true;
        public bool debug_mode = false;
        public int notification_value = 2000;
        static List<string> cities = new List<string>() { "ab'dendriel", "carlin", "kazordoon", "venore", "thais", "ankrahmun", "farmine", "gray beach", "liberty bay", "port hope", "rathleton", "roshamuul", "yalahar", "svargrond", "edron", "darashia", "rookgaard", "dawnport", "gray beach" };
        public List<string> notification_items = new List<string>();
        private static List<string> extensions = new List<string>();

        private Stack<string> command_stack = new Stack<string>();

        public MainForm() {
            mainForm = this;
            InitializeComponent();
            InitializePython();
            back_image = Image.FromFile(@"Images\back.png");
            prevpage_image = Image.FromFile(@"Images\prevpage.png");
            nextpage_image = Image.FromFile(@"Images\nextpage.png");
            cross_image = Image.FromFile(@"Images\cross.png");
            star_image = Image.FromFile(@"Images\star.png");
            tibia_image = Image.FromFile(@"Images\tibia.png");
            mapup_image = Image.FromFile(@"Images\mapup.png");
            mapdown_image = Image.FromFile(@"Images\mapdown.png");
            item_background = System.Drawing.Image.FromFile(@"Images\item_background.png");
            for (int i = 0; i < 10; i++) {
                image_numbers[i] = System.Drawing.Image.FromFile(@"Images\" + i.ToString() + ".png");
            }
            Label fff = new Label();
            fff.Text = "X";
            fff.Location = new Point(this.Size.Width - 32, 8);
            this.Controls.Add(fff);

            if (Directory.Exists("Extensions")) {
                string[] files = Directory.GetFiles("Extensions");
                for (int i = 0; i < files.Length; i++) {
                    if (files[i].EndsWith(".py")) {
                        string[] split = files[i].Split('\\');
                        extensions.Add(split[split.Length - 1].Substring(0, split[split.Length - 1].Length - 3));
                    }
                }
            }
            NotificationForm.Initialize();
            CreatureStatsForm.InitializeCreatureStats();
            HuntListForm.Initialize();
            BackgroundWorker bw = new BackgroundWorker();
            makeDraggable(this.Controls);
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync();

            this.loadTimerImage.Image = new Bitmap(@"Images\load.gif");
        }

        void makeDraggable(Control.ControlCollection controls) {
            foreach (Control c in controls) {
                if (c == this.closeButton || c == this.minimizeButton) continue;
                if (c is Label || c is Panel) {
                    c.MouseDown += new System.Windows.Forms.MouseEventHandler(this.draggable_MouseDown);
                }
                if (c is Panel || c is TabPage || c is TabControl) {
                    makeDraggable(c.Controls);
                }
            }
        }

        System.Timers.Timer circleTimer = null;
        void bw_DoWork(object sender, DoWorkEventArgs e) {
            ScriptScope pyScope = CreateNewScope();
            while (keep_working) {
                if (circleTimer == null) {
                    circleTimer = new System.Timers.Timer(1000);
                    circleTimer.Elapsed += circleTimer_Elapsed;
                    circleTimer.Enabled = true;
                }
                if (ReadMem(pyScope)) {
                    circleTimer.Dispose();
                    circleTimer = null;
                    this.BeginInvoke((MethodInvoker)delegate {
                        this.loadTimerImage.Enabled = true;
                    });
                }
            }
        }

        void circleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            this.Invoke((MethodInvoker)delegate {
                this.loadTimerImage.Enabled = false;
            });
        }


        public static string ToTitle(string str) {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        public static void ExecuteFile(string filename, ScriptScope pyScope) {
            ScriptSource source = pyEngine.CreateScriptSourceFromFile(filename);
            CompiledCode compiled = source.Compile();
            // Executes in the scope of Python
            compiled.Execute(pyScope);
        }

        public static void CompileSourceAndExecute(string code, ScriptScope pyScope) {
            ScriptSource source = pyEngine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
            CompiledCode compiled = source.Compile();
            // Executes in the scope of Python
            compiled.Execute(pyScope);
        }

        public static List<string> FindTimestamps(byte[] array) {
            int index = 0;
            List<string> strings = new List<string>();
            //byte[] buffer_array = new byte[array.Length];
            int start = 0;
            for (int i = 0; i < array.Length; i++) {
                if (index == 5) {
                    if (array[i] == 0) {
                        strings.Add(System.Text.Encoding.UTF8.GetString(array, start, (i - start)));
                        index = 0;
                    }
                } else if (array[i] > 47 && array[i] < 59) {
                    if (index == 2) {
                        if (array[i] == 58) {
                            start = i - 2;
                            index++; 
                        } else {
                            index = 0;
                        }
                    } else if (array[i] < 58) {
                        index++;
                    } else {
                        index = 0;
                    }
                } else {
                    index = 0;
                }
            }
            return strings;
        }

        public static ScriptScope CreateNewScope(bool minimal = false) {
            ScriptScope pyScope = pyEngine.CreateScope();
            // add sqlite3 library to python
            CompileSourceAndExecute("import sys, nt, clr", pyScope);
            CompileSourceAndExecute("sys.path.append(nt.getcwd() + '\\Lib')", pyScope);
            CompileSourceAndExecute("sys.path.append(nt.getcwd() + '\\Ironpython')", pyScope);
            CompileSourceAndExecute("clr.AddReference('IronPython.dll')", pyScope);
            CompileSourceAndExecute("clr.AddReference('IronPython.SQLite.dll')", pyScope);
            CompileSourceAndExecute("clr.AddReference('IronPython.Modules.dll')", pyScope);
            CompileSourceAndExecute("clr.AddReference('IronPython.Wpf.dll')", pyScope);
            // setup database connection
            CompileSourceAndExecute("import _sqlite3", pyScope);
            CompileSourceAndExecute("conn = _sqlite3.connect('" + database_file + "')", pyScope);
            CompileSourceAndExecute("c = conn.cursor()", pyScope);
            if (!minimal) {
                ExecuteFile("initialization.py", pyScope);
                ExecuteFile("plural_map.py", pyScope);
                CompileSourceAndExecute("ignore_stamp = create_stamp()", pyScope);
                CompileSourceAndExecute("read_settings('" + settings_file + "')", pyScope);
                InitializeMaps(pyScope);

                IronPython.Runtime.PythonDictionary settings = pyScope.GetVariable("settings") as IronPython.Runtime.PythonDictionary;
                IronPython.Runtime.List names = settings["Names"] as IronPython.Runtime.List;
                string massive_string = "";
                foreach (object obj in names) {
                    massive_string += obj.ToString() + "\n";
                }

                MainForm.mainForm.Invoke((MethodInvoker)delegate {
                    MainForm.mainForm.prevent_settings_update = true;
                    MainForm.mainForm.nameTextBox.Text = massive_string;
                    MainForm.mainForm.prevent_settings_update = false;
                });
            }
            return pyScope;
        }

        public static void DestroyScope(ScriptScope scope) {
            CompileSourceAndExecute("conn.close()", scope);
        }

        public void InitializePython() {
            Dictionary<string, object> options = new Dictionary<string, object>();
            if (debug_mode) options["Debug"] = true;
            pyEngine = Python.CreateEngine(options);
        }

        //based on http://www.codeproject.com/Articles/716227/Csharp-How-to-Scan-a-Process-Memory
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;
        const int PROCESS_WM_READ = 0x0010;
        public struct MEMORY_BASIC_INFORMATION {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;   // size of the region allocated by the program
            public int State;   // check if allocated (MEM_COMMIT)
            public int Protect; // page protection (must be PAGE_READWRITE)
            public int lType;
        }
        public struct SYSTEM_INFO {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }
        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        private bool ReadMemory(ScriptScope pyScope) {
            CompileSourceAndExecute(
                            "item_drops = dict()\n" +
                            "exp = dict()\n" +
                            "damage_dealt = dict()\n" +
                            "commands = dict()\n" +
                            "seen_logs = set()\n" + 
                            "urls = dict()", pyScope);

            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;
            Process[] processes = Process.GetProcessesByName("Tibia");
            if (processes.Length == 0) {
                Thread.Sleep(1000);
                return false;
            }
            Process process = processes[0];
            IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            int bytesRead = 0;  // number of bytes read with ReadProcessMemory

            //DateTime x = DateTime.Now;
            while (proc_min_address_l < proc_max_address_l) {
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, 28);

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT) {
                    byte[] buffer = new byte[mem_basic_info.RegionSize];

                    // read everything in the buffer above
                    ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);
                    List<string> strings = FindTimestamps(buffer);
                    if (strings.Count > 0) {
                        pyScope.SetVariable("chunk", strings);
                        CompileSourceAndExecute("useful = search_chunk(chunk, item_drops, exp, damage_dealt, commands, urls)", pyScope);
                    }
                }

                // move to the next memory chunk
                proc_min_address_l += mem_basic_info.RegionSize;
                proc_min_address = new IntPtr(proc_min_address_l);
            }
            //Console.WriteLine((DateTime.Now - x).TotalMilliseconds);
            process.Dispose();
            return true;
        }

        public static int GetInteger(IronPython.Runtime.List list, int index) {
            if (list.__len__() < index) {
                return -127;
            }
            return list[index] == null || list[index].ToString() == "" ? -127 : int.Parse(list[index].ToString());
        }

        public static float GetFloat(IronPython.Runtime.List list, int index) {
            if (list.__len__() < index) {
                return -127;
            }
            return list[index] == null || list[index].ToString() == "" ? -127 : float.Parse(list[index].ToString());
        }

        public static bool GetBoolean(IronPython.Runtime.List list, int index) {
            if (list.__len__() < index) {
                return false;
            }
            return list[index] == null || list[index].ToString() == "" ? false : list[index].ToString() == "1";
        }

        public static Image GetImage(IronPython.Runtime.List list, int index, bool make_transparent = false) {
            Image image = null;
            if (list[index] != null) {
                var collection = list[index] as System.Collections.Generic.ICollection<System.Byte>;
                byte[] b = collection.ToArray<byte>();
                image = byteArrayToImage(b);

                if (make_transparent) {
                    if (image.RawFormat.Guid == ImageFormat.Gif.Guid) {
                        int frames = image.GetFrameCount(FrameDimension.Time);
                        if (frames == 1) {
                            Bitmap new_bitmap = new Bitmap(image);
                            new_bitmap.MakeTransparent();
                            image.Dispose();
                            image = new_bitmap;
                        }
                    }
                }
            }
            return image;
        }

        public static void InitializeMaps(ScriptScope pyScope) {
            CompileSourceAndExecute("c.execute('SELECT * FROM WorldMap')", pyScope);
            CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
            IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            foreach (object obj in result) {
                IronPython.Runtime.List list = obj as IronPython.Runtime.List;
                Map m = new Map();
                m.z = GetInteger(list, 0);
                m.image = GetImage(list, 1);
                map_files.Add(m);
            }
        }

        public static Image byteArrayToImage(byte[] byteArrayIn) {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private Creature CreatureFromList(IronPython.Runtime.List list) {
            if (list == null) return null;
            Creature c = new Creature();
            c.name = list[1].ToString();
            c.id = GetInteger(list, 0);
            c.health = GetInteger(list, 2);
            c.experience = GetInteger(list, 3);
            c.maxdamage = GetInteger(list, 4);
            c.summoncost = GetInteger(list, 5);
            c.illusionable = GetBoolean(list, 6);
            c.pushable = GetBoolean(list, 7);
            c.pushes = GetBoolean(list, 8);
            c.res_phys = GetInteger(list, 9);
            c.res_holy = GetInteger(list, 10);
            c.res_death = GetInteger(list, 11);
            c.res_fire = GetInteger(list, 12);
            c.res_energy = GetInteger(list, 13);
            c.res_ice = GetInteger(list, 14);
            c.res_earth = GetInteger(list, 15);
            c.res_drown = GetInteger(list, 16);
            c.res_lifedrain = GetInteger(list, 17);
            c.paralysable = GetBoolean(list, 18);
            c.senseinvis = GetBoolean(list, 19);
            c.image = GetImage(list, 20);
            if (c.image == null) return null;
            return c;
        }

        public Creature GetCreature(string name, ScriptScope pyScope, bool skin = false) {
            // get the creature from the database
            if (name == "random") {
                CompileSourceAndExecute("c.execute('SELECT * FROM Creatures ORDER BY RANDOM() LIMIT 1')", pyScope);
            } else {
                CompileSourceAndExecute("c.execute('SELECT * FROM Creatures WHERE LOWER(name)=?', ['" + name.Replace("'", "\\'") + "'])", pyScope);
            }
            CompileSourceAndExecute("res = c.fetchall()", pyScope);
            CompileSourceAndExecute("result = None if len(res) == 0 else list(res[0])", pyScope);

            IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            Creature c = CreatureFromList(result);
            if (c == null) return null;
            // now load the items
            CompileSourceAndExecute("c.execute('SELECT id, name, actual_value, vendor_value, stackable, capacity, category, image, discard, convert_to_gold, look_text, percentage FROM CreatureDrops INNER JOIN Items ON CreatureDrops.itemid=Items.id WHERE CreatureDrops.creatureid=" + c.id.ToString() + "')", pyScope);
            CompileSourceAndExecute("res = c.fetchall()", pyScope);
            CompileSourceAndExecute("result = None if len(res) == 0 else res", pyScope);
            CompileSourceAndExecute("result = [list(x) for x in result] if result != None else None", pyScope);
            result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            if (result == null) return c;
            for (int i = 0; i < result.__len__(); i++) {
                IronPython.Runtime.List itemList = result[i] as IronPython.Runtime.List;
                if (itemList == null) continue;
                Item item = ItemFromList(itemList);
                ItemDrop itemDrop = new ItemDrop();
                itemDrop.item = item;
                itemDrop.percentage = GetFloat(itemList, 11);
                c.itemdrops.Add(itemDrop);
            }
            if (skin) {
                CompileSourceAndExecute("c.execute('SELECT Skins.itemid, Skins.skinitemid, percentage FROM Skins WHERE creatureid=?', [" + c.id + "])", pyScope);
                CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
                result = pyScope.GetVariable("result") as IronPython.Runtime.List;
                if (result.Count > 0) {
                    result = result[0] as IronPython.Runtime.List;
                    c.skin = new Skin();
                    int drop_item = GetInteger(result, 0);
                    int skin_item = GetInteger(result, 1);
                    c.skin.percentage = GetFloat(result, 2);
                    c.skin.skin_item = GetItem(skin_item, pyScope);
                    c.skin.drop_item = GetItem(drop_item, pyScope);
                }

            }
            return c;
        }

        private NPC NPCFromList(IronPython.Runtime.List itemList) {
            if (itemList == null) return null;
            NPC npc = new NPC();
            npc.id = GetInteger(itemList, 0);
            npc.name = itemList[1].ToString();
            npc.city = itemList[2] != null ? itemList[2].ToString() : "Unknown";
            npc.pos.x = GetFloat(itemList, 3);
            npc.pos.y = GetFloat(itemList, 4);
            npc.pos.z = GetInteger(itemList, 5);
            npc.image = GetImage(itemList, 6);
            return npc;
        }

        public NPC GetNPC(string name, ScriptScope pyScope) {
            CompileSourceAndExecute("c.execute('SELECT id,name,city,x,y,z,image FROM NPCs WHERE LOWER(name)=?', ['" + name.Replace("'", "\\'") + "'])", pyScope);
            CompileSourceAndExecute("res = c.fetchall()", pyScope);
            CompileSourceAndExecute("result = None if len(res) == 0 else list(res[0])", pyScope);

            IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;

            NPC npc = NPCFromList(result);
            if (npc != null && npc.name == "Rashid") {
                CompileSourceAndExecute("import datetime", pyScope);
                CompileSourceAndExecute("day = datetime.date.today().strftime('%A')", pyScope);
                CompileSourceAndExecute("c.execute('SELECT city, x, y, z FROM RashidPositions WHERE day=?', [day])", pyScope);
                CompileSourceAndExecute("result = list(c.fetchall()[0])", pyScope);
                result = pyScope.GetVariable("result") as IronPython.Runtime.List;
                npc.city = result[0].ToString();
                npc.pos.x = GetFloat(result, 1);
                npc.pos.y = GetFloat(result, 2);
                npc.pos.z = GetInteger(result, 3);
            }
            return npc;
        }

        private HuntingPlace GetHuntingPlace(string name, ScriptScope pyScope) {
            CompileSourceAndExecute("c.execute('SELECT id FROM HuntingPlaces WHERE LOWER(name)=?', ['" + name + "'])", pyScope);
            CompileSourceAndExecute("res = [list(x) for x in c.fetchall()]", pyScope);
            CompileSourceAndExecute("result = res[0] if len(res) > 0 else None", pyScope);
            IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            if (result == null) return null;
            return GetHuntingPlace(GetInteger(result, 0), pyScope);
        }

        private HuntingPlace GetHuntingPlace(int id, ScriptScope pyScope) {
            CompileSourceAndExecute("c.execute('SELECT id, name, level, exprating, lootrating, image, city FROM HuntingPlaces WHERE id=?', [" + id + "])", pyScope);
            CompileSourceAndExecute("res = [list(x) for x in c.fetchall()]", pyScope);
            CompileSourceAndExecute("result = res[0] if len(res) > 0 else None", pyScope);
            IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            HuntingPlace h = HuntingPlaceFromList(result);
            if (h == null) return null;

            CompileSourceAndExecute("c.execute('SELECT x, y, z FROM HuntingPlaceCoordinates INNER JOIN HuntingPlaces ON HuntingPlaces.id=HuntingPlaceCoordinates.huntingplaceid WHERE id=?', [" + id + "])", pyScope);
            CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
            result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            foreach (object obj in result) {
                Coordinate c = CoordinateFromList(obj as IronPython.Runtime.List);
                if (c != null) h.coordinates.Add(c);
            }

            CompileSourceAndExecute("c.execute('SELECT id, name, health, experience, maxdamage, summon, illusionable, pushable, pushes, physical, holy, death, fire, energy, ice, earth, drown, lifedrain, paralysable, senseinvis, image FROM HuntingPlaceCreatures INNER JOIN Creatures ON HuntingPlaceCreatures.creatureid=Creatures.id WHERE HuntingPlaceCreatures.huntingplaceid=?', [" + id + "])", pyScope);
            CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
            result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            foreach (object obj in result) {
                Creature c = CreatureFromList(obj as IronPython.Runtime.List);
                if (c != null) h.creatures.Add(c);
            }

            CompileSourceAndExecute("c.execute('SELECT huntingplaceid, x, y, z, ordering, name, notes FROM HuntingPlaceDirections WHERE huntingplaceid=? ORDER BY ordering', [" + id + "])", pyScope);
            CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
            result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            foreach (object obj in result) {
                Directions c = DirectionsFromList(obj as IronPython.Runtime.List);
                if (c != null) h.directions.Add(c);
            }

            CompileSourceAndExecute("c.execute('SELECT huntingplaceid, questid, notes FROM HuntingPlaceRequirements WHERE huntingplaceid=?', [" + id + "])", pyScope);
            CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
            result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            foreach (object obj in result) {
                Requirements c = RequirementsFromList(obj as IronPython.Runtime.List);
                if (c != null) h.requirements.Add(c);
            }
            return h;
        }

        private Coordinate CoordinateFromList(IronPython.Runtime.List itemList) {
            if (itemList == null) return null;
            Coordinate c = new Coordinate();
            c.x = GetFloat(itemList, 0);
            c.y = GetFloat(itemList, 1);
            c.z = GetInteger(itemList, 2);
            return c;
        }

        private Directions DirectionsFromList(IronPython.Runtime.List itemList) {
            if (itemList == null) return null;
            Directions c = new Directions();
            c.huntingplaceid = GetInteger(itemList, 0);
            c.x = GetFloat(itemList, 1);
            c.y = GetFloat(itemList, 2);
            c.z = GetInteger(itemList, 3);
            c.ordering = GetInteger(itemList, 4);
            c.name = itemList[5] != null ? itemList[5].ToString() : "Unknown";
            c.notes = itemList[6] != null ? itemList[6].ToString() : "Unknown";
            return c;
        }

        private Requirements RequirementsFromList(IronPython.Runtime.List itemList) {
            if (itemList == null) return null;
            Requirements c = new Requirements();
            c.huntingplaceid = GetInteger(itemList, 0);
            c.questid = GetInteger(itemList, 1);
            c.notes = itemList[2] != null ? itemList[2].ToString() : "Unknown";
            return c;
        }

        private HuntingPlace HuntingPlaceFromList(IronPython.Runtime.List itemList) {
            if (itemList == null) return null;
            HuntingPlace h = new HuntingPlace();
            h.id = GetInteger(itemList, 0);
            h.name = itemList[1] != null ? itemList[1].ToString() : "Unknown";
            h.level = GetInteger(itemList, 2);
            h.exp_quality = GetInteger(itemList, 3);
            h.loot_quality = GetInteger(itemList, 4);
            h.image = GetImage(itemList, 5);
            h.city = itemList[6] != null ? itemList[6].ToString() : "Unknown";
            return h;
        }

        private Item ItemFromList(IronPython.Runtime.List itemList) {
            if (itemList == null) return null;
            Item item = new Item();
            item.id = GetInteger(itemList, 0);
            item.name = itemList[1].ToString();
            item.actual_value = GetInteger(itemList, 2);
            item.vendor_value = GetInteger(itemList, 3);
            item.stackable = GetBoolean(itemList, 4);
            item.capacity = GetFloat(itemList, 5);
            item.category = itemList[6] != null ? itemList[6].ToString() : "Unknown";
            item.image = GetImage(itemList, 7, true);
            item.discard = GetBoolean(itemList, 8);
            item.convert_to_gold = GetBoolean(itemList, 9);
            item.look_text = itemList[10] != null ? itemList[10].ToString() : "Unknown";
            return item;
        }

        public Item GetItem(int id, ScriptScope pyScope) {
            CompileSourceAndExecute("c.execute('SELECT id, name, actual_value, vendor_value, stackable, capacity, category, image, discard, convert_to_gold, look_text FROM Items WHERE id=?' , [" + id.ToString() + "])", pyScope);
            CompileSourceAndExecute("res = c.fetchall()", pyScope);
            CompileSourceAndExecute("result = None if len(res) == 0 else list(res[0])", pyScope);

            IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            Item item = ItemFromList(result);
            return item;
        }
        public Item GetItem(string name, ScriptScope pyScope) {
            CompileSourceAndExecute("c.execute('SELECT id, name, actual_value, vendor_value, stackable, capacity, category, image, discard, convert_to_gold, look_text FROM Items WHERE LOWER(name)=?' , ['" + name.Replace("'", "\\'") + "'])", pyScope);
            CompileSourceAndExecute("res = c.fetchall()", pyScope);
            CompileSourceAndExecute("result = None if len(res) == 0 else list(res[0])", pyScope);

            IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            Item item = ItemFromList(result);
            return item;
        }

        private void ShowSimpleNotification(string title, string text, Image image) {
            notifyIcon1.BalloonTipText = text;
            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.Icon = Icon.FromHandle(((Bitmap)image).GetHicon());
            notifyIcon1.ShowBalloonTip(5000);
        }

        public void CloseNotification() {
            this.Invoke((MethodInvoker)delegate {
                if (tooltipForm != null) {
                    tooltipForm.Close();
                }
            });
        }

        long last_notification = -1000;
        private void ShowNotification(NotificationForm f, string command, bool screenshot = false) {
            long current_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (current_time - last_notification < 1000) return;
            command_stack.Push(command);
            if (tooltipForm != null) {
                tooltipForm.Close();
            }
            last_notification = current_time;
            int position_x = 0, position_y = 0;
            Screen screen;
            Process[] tibia_process = Process.GetProcessesByName("Tibia");
            if (tibia_process.Length == 0) {
                screen = Screen.FromControl(this);
            } else {
                Process tibia = tibia_process[0];
                screen = Screen.FromHandle(tibia.MainWindowHandle);
            }
            //position_x = screen.WorkingArea.Right - 30 - f.Width;
            position_x = screen.WorkingArea.Left + 30;
            position_y = screen.WorkingArea.Top + 30;
            f.StartPosition = FormStartPosition.Manual;
            f.SetDesktopLocation(position_x, position_y);
            f.TopMost = true;
            f.Show();

            if (screenshot) {
                f.Close();
                tooltipForm = null;
            } else tooltipForm = f;
        }

        public void Back() {
            if (command_stack.Count <= 1) return;
            command_stack.Pop(); // remove the current command
            string command = command_stack.Pop();
            priority_command = command;
        }

        public bool HasBack() {
            return command_stack.Count > 1;
        }

        private void ShowCreatureDrops(Creature c, string comm) {
            if (c == null) return;
            CreatureDropsForm f = new CreatureDropsForm();
            f.creature = c;

            this.Invoke((MethodInvoker)delegate {
                ShowNotification(f, comm);
            });
        }

        private void ShowCreatureStats(Creature c, string comm) {
            if (c == null) return;
            CreatureStatsForm f = new CreatureStatsForm();
            f.creature = c;

            this.Invoke((MethodInvoker)delegate {
                ShowNotification(f, comm);
            });
        }

        private void ShowCreatureList(List<Creature> c, string comm) {
            if (c == null) return;
            CreatureList f = new CreatureList();
            f.creatures = c;

            this.Invoke((MethodInvoker)delegate {
                ShowNotification(f, comm);
            });
        }

        private void ShowItemView(Item i, List<NPC> BuyNPCs, List<NPC> SellNPCs, List<Creature> creatures, string comm) {
            if (i == null) return;
            ItemViewForm f = new ItemViewForm();
            f.item = i;
            f.BuyNPCs = BuyNPCs;
            f.SellNPCs = SellNPCs;
            f.creatures = creatures;

            this.Invoke((MethodInvoker)delegate {
                ShowNotification(f, comm);
            });
        }

        private void ShowNPCForm(NPC c, string comm) {
            if (c == null) return;
            NPCForm f = new NPCForm();
            f.npc = c;

            this.Invoke((MethodInvoker)delegate {
                ShowNotification(f, comm);
            });
        }

        private void ShowDamageMeter(Dictionary<string, int> dps, string comm, string filter = "", string screenshot_path = "") {
            DamageMeter f = new DamageMeter(screenshot_path);
            f.dps = dps;
            f.filter = filter;
            this.Invoke((MethodInvoker)delegate {
                ShowNotification(f, comm, screenshot_path != "");
            });
        }

        private void ShowLootDrops(List<Creature> creatures, List<Item> items, string comm, string screenshot_path) {
            LootDropForm ldf = new LootDropForm(screenshot_path);
            ldf.creatures = creatures;
            ldf.items = items;

            this.Invoke((MethodInvoker)delegate {
                ShowNotification(ldf, comm, screenshot_path != "");
            });
        }

        private void ShowHuntList(List<HuntingPlace> h, string header, string comm) {
            if (h != null) h = h.OrderBy(o => o.level).ToList();
            HuntListForm f = new HuntListForm();
            f.hunting_places = h;
            f.header = header;

            this.Invoke((MethodInvoker)delegate {
                ShowNotification(f, comm);
            });
        }

        private void ShowHuntingPlace(HuntingPlace h, string comm) {
            HuntingPlaceForm f = new HuntingPlaceForm();
            f.hunting_place = h;

            this.Invoke((MethodInvoker)delegate {
                ShowNotification(f, comm);
            });
        }

        public void ShowItemNotification(string name, ScriptScope pyScope, string comm) {
            Item i = GetItem(name, pyScope);
            if (i == null) return;

            List<NPC> sell_npcs = new List<NPC>();
            List<NPC> buy_npcs = new List<NPC>();
            IronPython.Runtime.List result;
            CompileSourceAndExecute("c.execute('SELECT NPCs.id, NPCs.name, NPCs.city, NPCs.x, NPCs.y, NPCs.z, NPCs.image, catalog.value  FROM (SELECT * FROM Items WHERE LOWER(name)=?) AS i INNER JOIN SellItems AS catalog ON i.id=catalog.itemid INNER JOIN NPCs ON catalog.vendorid=NPCs.id', ['" + i.name.Replace("'", "\\'") + "'])", pyScope);
            CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
            result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            foreach (object obj in result) {
                NPC npc = NPCFromList(obj as IronPython.Runtime.List);
                if (npc != null) {
                    npc.value = GetInteger(obj as IronPython.Runtime.List, 7);
                    sell_npcs.Add(npc);
                }
            }
            CompileSourceAndExecute("c.execute('SELECT NPCs.id, NPCs.name, NPCs.city, NPCs.x, NPCs.y, NPCs.z, NPCs.image, catalog.value  FROM (SELECT * FROM Items WHERE LOWER(name)=?) AS i INNER JOIN BuyItems AS catalog ON i.id=catalog.itemid INNER JOIN NPCs ON catalog.vendorid=NPCs.id', ['" + i.name.Replace("'", "\\'") + "'])", pyScope);
            CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
            result = pyScope.GetVariable("result") as IronPython.Runtime.List;
            foreach (object obj in result) {
                NPC npc = NPCFromList(obj as IronPython.Runtime.List);
                if (npc != null) {
                    npc.value = GetInteger(obj as IronPython.Runtime.List, 7);
                    buy_npcs.Add(npc);
                }
            }
            ShowItemView(i, buy_npcs, sell_npcs, null, comm);
        }

        private void ShowListNotification(List<Command> commands, string type, string comm) {
            ListNotification f = new ListNotification(commands);
            f.type = type;

            this.Invoke((MethodInvoker)delegate {
                ShowNotification(f, comm);
            });
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            notifyIcon1.Visible = false;
        }

        private bool ReadMem(ScriptScope pyScope) {
            if (!ReadMemory(pyScope)) return false;
            ExecuteFile("parse_logresults.py", pyScope);
            float exph = pyScope.GetVariable<float>("exph");
            IronPython.Runtime.PythonDictionary damage = pyScope.GetVariable("dps") as IronPython.Runtime.PythonDictionary;
            IronPython.Runtime.List new_advances = pyScope.GetVariable("new_advances") as IronPython.Runtime.List;
            if (copy_advances) {
                foreach (object obj in new_advances)
                    this.Invoke((MethodInvoker)delegate {
                        Clipboard.SetText(obj.ToString());
                    });
                new_advances.Clear();
            }
            IronPython.Runtime.List commands = pyScope.GetVariable("new_commands") as IronPython.Runtime.List;
            commands.reverse();
            if (priority_command != null) {
                commands.Add(priority_command);
                priority_command = null;
            }
            if (new_names != null) {
                IronPython.Runtime.PythonDictionary settings = pyScope.GetVariable("settings") as IronPython.Runtime.PythonDictionary;
                IronPython.Runtime.List names = settings["Names"] as IronPython.Runtime.List;
                names.Clear();
                foreach (string str in new_names) {
                    names.Add(str);
                }
                new_names = null;
                CompileSourceAndExecute("write_settings('" + settings_file + "')", pyScope);
            }

            foreach (object command in commands) {
                string c = command.ToString();
                string comp = c.Trim().ToLower();
                if (comp.StartsWith("creature@")) {
                    string parameter = c.Split('@')[1].Trim().ToLower();
                    if (parameter.Contains('%')) {
                        List<Creature> creatures = new List<Creature>();
                        CompileSourceAndExecute("c.execute('SELECT * FROM Creatures WHERE LOWER(name) LIKE ? LIMIT ?', ['" + parameter.Replace("'", "\\'") + "', " + max_creatures.ToString() + "])", pyScope);
                        CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);

                        IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
                        foreach (object obj in result) {
                            Creature cr = CreatureFromList(obj as IronPython.Runtime.List);
                            if (cr != null)
                                creatures.Add(cr);
                        }

                        ShowCreatureList(creatures, c);
                    } else {
                        Creature cr = GetCreature(parameter, pyScope, true);
                        ShowCreatureDrops(cr, c);
                    }
                } else if (comp.StartsWith("stats@")) {
                    Creature cr = GetCreature(c.Split('@')[1].Trim().ToLower(), pyScope);
                    ShowCreatureStats(cr, c);
                } else if (comp.StartsWith("delete@")) {
                    CompileSourceAndExecute("delete_logmessage('" + c.Split('@')[1].Trim().Replace("'", "\\'") + "')", pyScope);
                } else if (comp.StartsWith("skin@")) {
                    string parameter = c.Split('@')[1].Trim().ToLower();
                    Creature cr = GetCreature(parameter, pyScope, true);
                    if (cr == null) continue;
                    if (cr.skin == null) continue;
                    CompileSourceAndExecute("insert_skin('" + cr.skin.drop_item.name.Replace("'", "\\'") + "')", pyScope);
                } else if (comp.StartsWith("damage@")) {
                    string[] splits = c.Split('@');
                    string screenshot_path = "";
                    string parameter = splits[1].Trim().ToLower();
                    if (parameter == "screenshot" && splits.Length > 2) {
                        parameter = "";
                        screenshot_path = splits[2];
                    }
                    Dictionary<string, int> dps_dictionary = new Dictionary<string, int>();
                    foreach (KeyValuePair<object, object> kvp in damage) {
                        dps_dictionary.Add(kvp.Key.ToString(), (int)(float.Parse(kvp.Value.ToString())));
                    }
                    ShowDamageMeter(dps_dictionary, c, parameter, screenshot_path);
                } else if (comp.StartsWith("exp@")) {
                    ShowSimpleNotification("Experience", "Currently gaining " + ((int)exph).ToString() + " experience an hour.", tibia_image);
                } else if (comp.StartsWith("loot@") || comp.StartsWith("clipboard@")) {
                    string[] splits = c.Split('@');
                    bool clipboard = comp.StartsWith("clipboard@");
                    string screenshot_path = "";
                    string parameter = splits[1].Trim().ToLower().Replace("'", "\\'");
                    if (parameter == "screenshot" && splits.Length > 2) {
                        parameter = "";
                        screenshot_path = splits[2];
                    }
                    CompileSourceAndExecute("(creature_kills, creature_loot) =  get_recent_drops('" + parameter + "')", pyScope);
                    IronPython.Runtime.List creature_kills = pyScope.GetVariable("creature_kills") as IronPython.Runtime.List;
                    IronPython.Runtime.List creature_loot = pyScope.GetVariable("creature_loot") as IronPython.Runtime.List;
                    List<Creature> creatures = new List<Creature>();
                    List<Item> items = new List<Item>();
                    foreach (object obj in creature_kills) {
                        Creature cr = CreatureFromList((obj as IronPython.Runtime.PythonTuple)[0] as IronPython.Runtime.List);
                        if (cr == null) throw new Exception("Invalid creature object returned during loot@ query.");
                        cr.kills = int.Parse((obj as IronPython.Runtime.PythonTuple)[1].ToString());
                        creatures.Add(cr);
                    }
                    foreach (object obj in creature_loot) {
                        Item i = ItemFromList((obj as IronPython.Runtime.List)[0] as IronPython.Runtime.List);
                        if (i == null) throw new Exception("Invalid item object returned during loot@ query.");
                        i.drops = int.Parse((obj as IronPython.Runtime.List)[1].ToString());
                        items.Add(i);
                    }
                    
                    // Copy loot message to the clipboard
                    // clipboard@<creature> copies the loot of a specific creature to the clipboard
                    // clipboard@ copies all loot to the clipboard
                    if (clipboard) {
                        string loot_string;
                        if (creatures.Count == 1) {
                            loot_string = "Total Loot of " + creatures[0].kills + " " + creatures[0].name + (creatures[0].kills > 1 ? "s" : "") + ": ";
                        } else {
                            loot_string = "Total Loot of " + creatures.Sum(o => o.kills) + " Kills: ";
                        }
                        foreach (Item item in items) {
                            loot_string += item.drops + " " + item.name + (item.drops > 1 ? "s" : "") + ", ";
                        }
                        loot_string = loot_string.Substring(0, loot_string.Length - 2) + ".";
                        this.Invoke((MethodInvoker)delegate {
                            Clipboard.SetText(loot_string);
                        });
                    }
                    else ShowLootDrops(creatures, items, c, screenshot_path);
                } else if (comp.StartsWith("reset@")) {
                    CompileSourceAndExecute("reset_loot()", pyScope);
                } else if (comp.StartsWith("drop@")) {
                    Item i = GetItem(c.Split('@')[1].Trim().ToLower(), pyScope);
                    if (i == null) continue;
                    CompileSourceAndExecute("c.execute('SELECT c.id,c.name,c.health,c.experience,c.maxdamage,c.summon,c.illusionable,c.pushable,c.pushes,c.physical,c.holy,c.death,c.fire,c.energy,c.ice,c.earth,c.drown,c.lifedrain,c.paralysable,c.senseinvis,c.image FROM (SELECT * FROM Items WHERE name=?) AS i INNER JOIN CreatureDrops AS crd ON crd.itemid=i.id INNER JOIN Creatures AS c ON c.id=crd.creatureid LIMIT ?', ['" + i.name.Replace("'", "\\'") + "', " + max_creatures.ToString() + "])", pyScope);
                    CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);

                    List<Creature> creatures = new List<Creature>();
                    IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
                    foreach (object obj in result) {
                        Creature cr = CreatureFromList(obj as IronPython.Runtime.List);
                        if (cr != null) creatures.Add(cr);
                    }

                    ShowItemView(i, null, null, creatures, c);
                } else if (comp.StartsWith("item@")) {
                    ShowItemNotification(c.Split('@')[1].Trim().ToLower(), pyScope, c);
                } else if (comp.StartsWith("hunt@")) {
                    string parameter = c.Split('@')[1].Trim().ToLower();
                    HuntingPlace h = null;
                    if (cities.Contains(parameter)) {
                        List<HuntingPlace> hunting_places = new List<HuntingPlace>();
                        CompileSourceAndExecute("c.execute('SELECT id, name, level, exprating, lootrating, image, city FROM HuntingPlaces WHERE LOWER(city)=?', ['" + parameter.Replace("'", "\\'") + "'])", pyScope);
                        CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
                        IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
                        foreach (object obj in result) {
                            h = HuntingPlaceFromList(obj as IronPython.Runtime.List);
                            if (h != null) hunting_places.Add(h);
                        }
                        ShowHuntList(hunting_places, "Hunts in " + parameter, c);
                        continue;
                    }
                    h = GetHuntingPlace(parameter, pyScope);
                    if (h != null) {
                        ShowHuntingPlace(h, c);
                        continue;
                    }
                    Creature cr = GetCreature(c.Split('@')[1].Trim().ToLower(), pyScope);
                    if (cr != null) {
                        CompileSourceAndExecute("c.execute('SELECT HuntingPlaces.id FROM HuntingPlaceCreatures INNER JOIN HuntingPlaces ON HuntingPlaces.id=HuntingPlaceCreatures.huntingplaceid WHERE creatureid=?', [" + cr.id + "])", pyScope);
                        CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
                        List<HuntingPlace> hunting_places = new List<HuntingPlace>();
                        IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
                        foreach (object obj in result) {
                            int id = GetInteger(obj as IronPython.Runtime.List, 0);
                            h = GetHuntingPlace(id, pyScope);
                            if (h != null) hunting_places.Add(h);
                        }
                        cr.Dispose();
                        ShowHuntList(hunting_places, "Hunting Places of " + cr.name, c);
                        continue;
                    }
                    int minlevel = -1, maxlevel = -1;
                    int level;
                    if (int.TryParse(parameter, out level)) {
                        minlevel = (int)(level * 0.8);
                        maxlevel = (int)(level * 1.2);
                    } else if (parameter.Contains('-')) {
                        string[] split = parameter.Split('-');
                        int.TryParse(split[0].Trim(), out minlevel);
                        int.TryParse(split[1].Trim(), out maxlevel);
                    }
                    if (minlevel >= 0 && maxlevel >= 0) {
                        List<HuntingPlace> hunting_places = new List<HuntingPlace>();
                        CompileSourceAndExecute("c.execute('SELECT id, name, level, exprating, lootrating, image, city FROM HuntingPlaces WHERE level>=? AND level <=?', [" + minlevel.ToString() + ", " + maxlevel.ToString() + "])", pyScope);
                        CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", pyScope);
                        IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
                        foreach (object obj in result) {
                            h = HuntingPlaceFromList(obj as IronPython.Runtime.List);
                            if (h != null) hunting_places.Add(h);
                        }
                        ShowHuntList(hunting_places, "Hunts between levels " + minlevel.ToString() + "-" + maxlevel.ToString(), c);
                        continue;
                    }
                } else if (comp.StartsWith("npc@")) {
                    NPC npc = GetNPC(c.Split('@')[1].Trim().ToLower(), pyScope);
                    ShowNPCForm(npc, c);
                } else if (comp.StartsWith("run@")) {
                    if (!allow_extensions) continue;
                    try {
                        CompileSourceAndExecute("a = " + c.Split('@')[1], pyScope);
                        ShowSimpleNotification(pyScope.GetVariable("a").ToString(), "Run result of " + c.Split('@')[1], tibia_image);
                    } catch {
                        continue;
                    }
                } else if (comp.StartsWith("savelog@")) {
                    CompileSourceAndExecute("save_log('" + c.Split('@')[1].Trim().Replace("'", "\\'") + "')", pyScope);
                } else if (comp.StartsWith("loadlog@")) {
                    CompileSourceAndExecute("load_log('" + c.Split('@')[1].Trim().Replace("'", "\\'") + "')", pyScope);
                } else if (comp.StartsWith("setdiscardgoldratio@")) {
                    double val;
                    if (double.TryParse(c.Split('@')[1].Trim(), out val)) {
                        CompileSourceAndExecute("set_gold_ratio(" + val.ToString() + ")", pyScope);
                    }
                } else if (comp.StartsWith("wiki@")) { 
                    string parameter = c.Split('@')[1].Trim();
                    string response = "";
                    using (WebClient client = new WebClient()) {
                        response = client.DownloadString(String.Format("http://tibia.wikia.com/api/v1/Search/List?query={0}&limit=1&minArticleQuality=10&batch=1&namespaces=0", parameter));
                    }
                    Regex regex = new Regex("\"url\":\"([^\"]+)\"");
                    Match m = regex.Match(response);
                    var gr = m.Groups[1];
                    OpenUrl(gr.Value.Replace("\\/", "/"));
                } else if (comp.StartsWith("setconvertgoldratio@")) {
                    string parameter = c.Split('@')[1].Trim();
                    string[] split = parameter.Split('-');
                    if (split.Length < 2) continue;
                    int stackable = 0;
                    if (split[0] == "1") stackable = 1;
                    double val;
                    if (double.TryParse(split[1], out val)) {
                        CompileSourceAndExecute("set_convert_ratio(" + val.ToString() + ", " + stackable.ToString() + ")", pyScope);
                    }
                } else if (comp.StartsWith("recent@") || comp.StartsWith("url@")) {
                    bool url = comp.StartsWith("url@");
                    string type = (url ? "urls" : "commands");
                    // Show all recent commands, we show either the last 15 commands, or all commands in the last 5 minutes
                    CompileSourceAndExecute("recent_commands = get_recent_commands(type='" + type + "')", pyScope);

                    List<Command> command_list = new List<Command>();
                    IronPython.Runtime.List result = pyScope.GetVariable("recent_commands") as IronPython.Runtime.List;
                    foreach (object obj in result) {
                        Command comm = new Command();
                        comm.player = (obj as IronPython.Runtime.List)[0].ToString();
                        comm.command = (obj as IronPython.Runtime.List)[1].ToString();
                        command_list.Add(comm);
                    }
                    string parameter = c.Split('@')[1].Trim().ToLower();
                    int number;
                    //recent@<number> opens the last <number> command, so recent@1 opens the last command
                    if (int.TryParse(parameter, out number)) {
                        if (number > 0 && number <= command_list.Count) {
                            ListNotification.OpenCommand(command_list[number - 1].command, type);;
                            continue;
                        }
                    } else {
                        //recent@<player> opens the last 
                        bool found = false;
                        foreach (Command comm in command_list) {
                            if (comm.player.ToLower() == parameter) {
                                ListNotification.OpenCommand(command_list[number].command, type);
                                found = true;
                                break;
                            }
                        }
                        if (found) continue;
                    }
                    ShowListNotification(command_list, url ? "urls" : "commands", c);
                } else if (comp.StartsWith("pickup@")) {
                    CompileSourceAndExecute("c.execute('UPDATE Items SET discard=0 WHERE LOWER(name)=?', ['" + c.Split('@')[1].Trim().ToLower().Replace("'", "\\'") + "'])", pyScope);
                    CompileSourceAndExecute("conn.commit()", pyScope);
                    CompileSourceAndExecute("invalidate_item('" + c.Split('@')[1].Trim().ToLower().Replace("'", "\\'") + "')", pyScope);
                } else if (comp.StartsWith("nopickup@")) {
                    CompileSourceAndExecute("c.execute('UPDATE Items SET discard=1 WHERE LOWER(name)=?', ['" + c.Split('@')[1].Trim().ToLower().Replace("'", "\\'") + "'])", pyScope);
                    CompileSourceAndExecute("conn.commit()", pyScope);
                    CompileSourceAndExecute("invalidate_item('" + c.Split('@')[1].Trim().ToLower().Replace("'", "\\'") + "')", pyScope);
                } else if (comp.StartsWith("convert@")) {
                    CompileSourceAndExecute("c.execute('UPDATE Items SET convert_to_gold=1 WHERE LOWER(name)=?', ['" + c.Split('@')[1].Trim().ToLower().Replace("'", "\\'") + "'])", pyScope);
                    CompileSourceAndExecute("conn.commit()", pyScope);
                    CompileSourceAndExecute("invalidate_item('" + c.Split('@')[1].Trim().ToLower().Replace("'", "\\'") + "')", pyScope);
                } else if (comp.StartsWith("noconvert@")) {
                    CompileSourceAndExecute("c.execute('UPDATE Items SET convert_to_gold=0 WHERE LOWER(name)=?', ['" + c.Split('@')[1].Trim().ToLower().Replace("'", "\\'") + "'])", pyScope);
                    CompileSourceAndExecute("conn.commit()", pyScope);
                    CompileSourceAndExecute("invalidate_item('" + c.Split('@')[1].Trim().ToLower().Replace("'", "\\'") + "')", pyScope);
                } else if (comp.StartsWith("setval@")) {
                    string parameter = c.Split('@')[1].Trim();
                    if (!parameter.Contains('=')) continue;
                    string[] split = parameter.Split('=');
                    string item = split[0].Trim().ToLower().Replace("'", "\\'");
                    int value = 0;
                    try { value = int.Parse(split[1].Trim()); } catch { continue; }

                    CompileSourceAndExecute("c.execute('UPDATE Items SET actual_value=? WHERE LOWER(name)=?', [" + value.ToString() + ",'" + item + "'])", pyScope);
                    CompileSourceAndExecute("conn.commit()", pyScope);
                } else {
                    if (allow_extensions) {
                        bool found_extension = false;
                        foreach (string extension in extensions) {
                            if (comp.StartsWith(extension + '@')) {
                                found_extension = true;
                                try {
                                    CompileSourceAndExecute("_parameter = '" + c.Split('@')[1].Trim().Replace("'", "\\'") + "'", pyScope);
                                    ExecuteFile(@"Extensions\" + extension + ".py", pyScope);
                                } catch (Exception e) {
                                    ShowSimpleNotification("Error in command " + command, e.ToString(), tibia_image);
                                }
                                break;
                            }
                        }
                        if (found_extension) continue;
                    }
                    bool found = false;
                    foreach (string city in cities) {
                        if (comp.StartsWith(city + "@")) {
                            string item_name = c.Split('@')[1].Trim();
                            string[] tables = { "BuyItems", "SellItems" };
                            for (int i = 0; i < tables.Length; i++) {
                                CompileSourceAndExecute("c.execute('SELECT n.name FROM (SELECT * FROM NPCs WHERE LOWER(city)=?) AS n INNER JOIN " + tables[i] + " AS catalog ON n.id=catalog.vendorid INNER JOIN (SELECT * FROM Items WHERE LOWER(name)=?) AS i ON catalog.itemid=i.id', ['" + city + "', '" + item_name + "'])", pyScope);
                                CompileSourceAndExecute("result = c.fetchall()", pyScope);
                                IronPython.Runtime.List result = pyScope.GetVariable("result") as IronPython.Runtime.List;
                                if (result.Count > 0) {
                                    string name = (result[0] as IronPython.Runtime.PythonTuple)[0].ToString();
                                    NPC npc = GetNPC(name.ToLower(), pyScope);
                                    ShowNPCForm(npc, c);
                                    break;
                                }
                            }
                            found = true;
                        }
                    }
                    if (found) continue;
                    //if we get here we didn't find any command
                    ShowSimpleNotification("Unrecognized command", "Unrecognized command: " + command, tibia_image);
                }
            }
            IronPython.Runtime.List item_drops = pyScope.GetVariable("new_items") as IronPython.Runtime.List;
            foreach (object l in item_drops) {
                IronPython.Runtime.List list = l as IronPython.Runtime.List;
                string creature_name = list[0].ToString();
                string item_name = list[1].ToString();
                int item_value = GetInteger(list, 2);
                if (item_value >= notification_value) {
                    Creature c = GetCreature(creature_name.Trim().ToLower(), pyScope);
                    if (c != null) {
                        ShowSimpleNotification(creature_name, creature_name + " dropped a " + item_name + ".", c.image);
                    }
                }
            }
            return true;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void draggable_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public static int DisplayCreatureList(System.Windows.Forms.Control.ControlCollection controls, List<TibiaObject> l, int base_x, int base_y, int max_x, int spacing, bool transparent, Func<TibiaObject, string> tooltip_function = null, float magnification = 1.0f) {
            int x = 0, y = 0;
            int width = 0, height = 0;
            foreach (TibiaObject cr in l) {
                Image image = cr.GetImage();
                if (image.Width * magnification > width) width = (int)(image.Width * magnification);
                if (image.Height * magnification > height) height = (int)(image.Height * magnification);
            }

            // add a tooltip that displays the creature names
            ToolTip value_tooltip = new ToolTip();
            value_tooltip.AutoPopDelay = 60000;
            value_tooltip.InitialDelay = 500;
            value_tooltip.ReshowDelay = 0;
            value_tooltip.ShowAlways = true;
            value_tooltip.UseFading = true;
            foreach (TibiaObject cr in l) {
                Image image = cr.GetImage();
                string name = cr.GetName();
                if (max_x < (x + base_x + (int)(image.Width * magnification) + spacing)) {
                    x = 0;
                    y = y + spacing + height;
                }
                PictureBox image_box;
                if (transparent) image_box = new PictureBox();
                else image_box = new PictureBox();
                image_box.Image = image;
                image_box.BackColor = Color.Transparent;
                image_box.Size = new Size((int)(image.Width * magnification), height);
                image_box.Location = new Point(base_x + x, base_y + y);
                image_box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                image_box.Name = name;
                controls.Add(image_box);
                if (magnification != 1.0f) {
                    Bitmap bitmap = new Bitmap((int)(image.Width * magnification), (int)(image.Height * magnification));
                    Graphics gr = Graphics.FromImage(bitmap);
                    gr.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                    image_box.Image = bitmap;
                } else {
                    image_box.Image = image;
                }
                if (tooltip_function == null) {
                    value_tooltip.SetToolTip(image_box, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name));
                } else {
                    value_tooltip.SetToolTip(image_box, tooltip_function(cr));
                }

                x = x + (int)(image.Width * magnification) + spacing;
            }
            x = 0;
            y = y + height;
            return y;
        }

        private void creatureSearch_TextChanged(object sender, EventArgs e) {
            ScriptScope mainScope = CreateNewScope(true);
            string creature = (sender as TextBox).Text;
            CompileSourceAndExecute("c.execute('SELECT * FROM Creatures WHERE name LIKE ? LIMIT 30', ['%" + creature.Replace("'", "\\'") + "%'])", mainScope);
            CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", mainScope);
            IronPython.Runtime.List creatures = mainScope.GetVariable("result") as IronPython.Runtime.List;
            List<TibiaObject> l = new List<TibiaObject>();
            foreach (object obj in creatures) {
                Creature cr = CreatureFromList(obj as IronPython.Runtime.List);
                if (cr != null) l.Add(cr);
            }
            this.SuspendLayout();
            this.creaturePanel.Controls.Clear();
            DisplayCreatureList(this.creaturePanel.Controls, l, 10, 10, this.creaturePanel.Width - 20, 4, false);
            foreach (Control c in creaturePanel.Controls) {
                if (c is PictureBox) {
                    c.Click += ShowCreatureInformation;
                }
            }
            this.ResumeLayout(false);
        }
        private void itemSearchBox_TextChanged(object sender, EventArgs e) {
            ScriptScope mainScope = CreateNewScope(true);
            string creature = (sender as TextBox).Text;
            CompileSourceAndExecute("c.execute('SELECT id, name, actual_value, vendor_value, stackable, capacity, category, image, discard, convert_to_gold, look_text FROM Items WHERE name LIKE ? LIMIT 30', ['%" + creature.Replace("'", "\\'") + "%'])", mainScope);
            CompileSourceAndExecute("result = [list(x) for x in c.fetchall()]", mainScope);
            IronPython.Runtime.List creatures = mainScope.GetVariable("result") as IronPython.Runtime.List;
            List<TibiaObject> l = new List<TibiaObject>();
            foreach (object obj in creatures) {
                Item i = ItemFromList(obj as IronPython.Runtime.List);
                if (i != null) l.Add(i);
            }
            this.SuspendLayout();
            this.itemPanel.Controls.Clear();
            DisplayCreatureList(this.itemPanel.Controls, l, 10, 10, this.itemPanel.Width - 20, 4, false);
            foreach (Control c in itemPanel.Controls) {
                if (c is PictureBox) {
                    c.Click += ShowItemInformation;
                }
            }
            this.ResumeLayout(false);
        }

        void ShowCreatureInformation(object sender, EventArgs e) {
            string creature_name = (sender as Control).Name;
            this.priority_command = "creature@" + creature_name;
        }

        void ShowItemInformation(object sender, EventArgs e) {
            string item_name = (sender as Control).Name;
            this.priority_command = "item@" + item_name;
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e) {
            if (prevent_settings_update) return;
            List<string> names = new List<string>();

            string[] lines = (sender as RichTextBox).Text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
                names.Add(lines[i]);
            new_names = names;
        }

        private void exportLogButton_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Export Log File";
            if (File.Exists("exported_log")) {
                int i = 1;
                while (File.Exists("exported_log (" + i.ToString() + ")")) i++;
                dialog.FileName = "exported_log (" + i.ToString() + ")";
            } else {
                dialog.FileName = "exported_log";
            }
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                priority_command = "savelog@" + dialog.FileName.Replace("\\\\", "/").Replace("\\", "/");
            }
        }

        private void resetButton_Click(object sender, EventArgs e) {
            priority_command = "reset@";
        }

        private void importLogFile_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Import Log File";
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                priority_command = "loadlog@" + dialog.FileName;
            }
        }

        private void saveLootImage_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "png";
            dialog.Title = "Save Loot Image";
            if (File.Exists("loot_screenshot.png")) {
                int i = 1;
                while (File.Exists("loot_screenshot (" + i.ToString() + ").png")) i++;
                dialog.FileName = "loot_screenshot (" + i.ToString() + ").png";
            } else {
                dialog.FileName = "loot_screenshot.png";
            }
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                priority_command = "loot@screenshot@" + dialog.FileName.Replace("\\\\", "/").Replace("\\", "/");
            }

        }

        private void damageButton_Click(object sender, EventArgs e) {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "png";
            dialog.Title = "Save Damage Image";
            if (File.Exists("damage_screenshot.png")) {
                int i = 1;
                while (File.Exists("damage_screenshot (" + i.ToString() + ").png")) i++;
                dialog.FileName = "damage_screenshot (" + i.ToString() + ").png";
            } else {
                dialog.FileName = "damage_screenshot.png";
            }
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                priority_command = "damage@screenshot@" + dialog.FileName.Replace("\\\\", "/").Replace("\\", "/");
            }
        }

        private void applyRatioButton_Click(object sender, EventArgs e) {
            double val = 0;
            if (double.TryParse(goldRatioTextBox.Text, out val)) {
                priority_command = "setdiscardgoldratio@" + goldRatioTextBox.Text;
            }
        }

        private void stackableConvertApply_Click(object sender, EventArgs e) {
            double val = 0;
            if (double.TryParse(stackableConvertTextBox.Text, out val)) {
                priority_command = "setconvertgoldratio@1-" + stackableConvertTextBox.Text;
            }
        }

        private void unstackableConvertApply_Click(object sender, EventArgs e) {
            double val = 0;
            if (double.TryParse(unstackableConvertTextBox.Text, out val)) {
                priority_command = "setconvertgoldratio@0-" + unstackableConvertTextBox.Text;
            }
        }

        public static void OpenUrl(string str) {
            str = str.Trim().Replace(" ", "%20");
            if (!str.StartsWith("http://") && !str.StartsWith("https://")) {
                str = "http://" + str;
            }
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C start " + str + "");

            procStartInfo.UseShellExecute = true;

            // Do not create the black window.
            procStartInfo.CreateNoWindow = true;
            procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(procStartInfo);
        }

        private void closeButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void minimizeButton_Click(object sender, EventArgs e) {
            this.Hide();
            this.minimizeIcon.Visible = true;
            if (minimize_notification) {
                this.minimize_notification = false;
                this.minimizeIcon.ShowBalloonTip(3000);
            }
        }

        private static Color hoverColor = Color.FromArgb(200, 55, 55);
        private static Color normalColor = Color.FromArgb(172, 24, 24);
        private void closeButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = hoverColor;
        }

        private void closeButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = normalColor;
        }

        private static Color minimizeHoverColor = Color.FromArgb(191, 191, 191);
        private static Color minimizeNormalColor = Color.Transparent;
        private void minimizeButton_MouseEnter(object sender, EventArgs e) {
            (sender as Control).BackColor = minimizeHoverColor;
        }

        private void minimizeButton_MouseLeave(object sender, EventArgs e) {
            (sender as Control).BackColor = minimizeNormalColor;
        }

        private void minimizeIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
            this.minimizeIcon.Visible = false;
            this.Show();
        }
    }
}


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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Numerics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Data.SQLite;

namespace Tibialyzer {
    public partial class MainForm : Form {
        private static Object ItemLock = new Object();
        private static Dictionary<string, Item> _itemNameMap = new Dictionary<string, Item>();
        private static Dictionary<int, Item> _itemIdMap = new Dictionary<int, Item>();
        public static bool itemsLoaded = false;
        private static Object CreatureLock = new Object();
        private static Dictionary<string, Creature> _creatureNameMap = new Dictionary<string, Creature>();
        private static Dictionary<int, Creature> _creatureIdMap = new Dictionary<int, Creature>();
        public static bool creaturesLoaded = false;
        private static Object NPCLock = new Object();
        private static Dictionary<string, NPC> _npcNameMap = new Dictionary<string, NPC>();
        private static Dictionary<int, NPC> _npcIdMap = new Dictionary<int, NPC>();
        public static bool npcsLoaded = false;
        private static Object HuntLock = new Object();
        private static Dictionary<string, HuntingPlace> _huntingPlaceNameMap = new Dictionary<string, HuntingPlace>();
        private static Dictionary<int, HuntingPlace> _huntingPlaceIdMap = new Dictionary<int, HuntingPlace>();
        public static bool huntsLoaded = false;
        private static Object SpellLock = new Object();
        private static Dictionary<string, Spell> _spellNameMap = new Dictionary<string, Spell>();
        private static Dictionary<int, Spell> _spellIdMap = new Dictionary<int, Spell>();
        public static bool spellsLoaded = false;
        private static Dictionary<int, Mount> _mountIdMap = new Dictionary<int, Mount>();
        private static Dictionary<string, Mount> _mountNameMap = new Dictionary<string, Mount>();
        public static bool mountsLoaded = false;
        private static Dictionary<string, Outfit> _outfitNameMap = new Dictionary<string, Outfit>();
        private static Dictionary<int, Outfit> _outfitIdMap = new Dictionary<int, Outfit>();
        public static bool outfitsLoaded = false;
        private static Dictionary<string, WorldObject> _worldObjectNameMap = new Dictionary<string, WorldObject>();
        public static bool worldObjectsLoaded = false;

        public static Dictionary<string, List<Task>> taskList = new Dictionary<string, List<Task>>();
        public static Dictionary<int, string> taskGroups = new Dictionary<int, string>();

        public static Dictionary<int, City> cityIdMap = new Dictionary<int, City>();
        public static Dictionary<string, City> cityNameMap = new Dictionary<string, City>();
        private static Dictionary<int, Quest> questIdMap = new Dictionary<int, Quest>();
        private static Dictionary<string, Quest> questNameMap = new Dictionary<string, Quest>();
        public static Dictionary<int, Event> eventIdMap = new Dictionary<int, Event>();
        private static List<Map> mapFiles = new List<Map>();

        private static List<HelpCommand> helpCommands = new List<HelpCommand>();

        public static int mapFilesCount { get { return mapFiles.Count; } }

        public static Map getMap(int z) {
            if (mapFiles[z].references == 0) {
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT image FROM WorldMap WHERE z={0}", z), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();
                Image image = Image.FromStream(reader.GetStream(0));
                mapFiles[z].image = new Bitmap(image);
                image.Dispose();
            }
            mapFiles[z].references++;
            return mapFiles[z];
        }

        #region Item Handling

        public static Item getItem(string name) {
            name = name.ToLower().Trim();
            if (_itemNameMap.ContainsKey(name)) {
                return _itemNameMap[name];
            }
            if (itemsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Items WHERE LOWER(name)='{1}';", _itemProperties, name.Replace("\'", "\'\'")), mainForm.conn);
            Item item = createItem(command.ExecuteReader());
            return registerItem(item);
        }

        public static Item getItem(int id) {
            if (_itemIdMap.ContainsKey(id)) {
                return _itemIdMap[id];
            }
            if (itemsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Items WHERE id={1};", _itemProperties, id), mainForm.conn);
            Item item = createItem(command.ExecuteReader());
            return registerItem(item);
        }

        private static Item registerItem(Item item) {
            if (item == null) return null;
            lock (ItemLock) {
                if (_itemIdMap.ContainsKey(item.id)) {
                    item.image.Dispose();
                    return _itemIdMap[item.id];
                }
                _itemIdMap.Add(item.id, item);
                string name = item.GetName().ToLower();
                if (!_itemNameMap.ContainsKey(name)) {
                    _itemNameMap.Add(item.GetName().ToLower(), item);
                }
            }
            return item;
        }

        private static void UpdateItem(int itemid, bool discard, bool convert, long value, SQLiteTransaction transaction) {
            if (_itemIdMap.ContainsKey(itemid)) {
                _itemIdMap[itemid].discard = discard;
                _itemIdMap[itemid].convert_to_gold = convert;
                _itemIdMap[itemid].actual_value = value;
            }
            SQLiteCommand command = new SQLiteCommand(String.Format("UPDATE Items SET discard={1},convert_to_gold={2},actual_value={3} WHERE id={0}", itemid, discard ? 1 : 0, convert ? 1 : 0, value), mainForm.conn, transaction);
            command.ExecuteNonQuery();
        }

        private static string _itemPropertiesBase = "id, name, actual_value, vendor_value, stackable, capacity, category, discard, convert_to_gold, look_text, title, currency";
        private static string _itemProperties = _itemPropertiesBase + ", image";
        private static Item createItem(SQLiteDataReader reader) {
            SQLiteCommand command;

            if (!reader.Read()) {
                return null;
            }

            Item item = new Item();
            item.permanent = true;
            item.id = reader.GetInt32(0);
            item.displayname = reader.GetString(1);
            item.actual_value = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetInt64(2);
            item.vendor_value = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt64(3);
            item.stackable = reader.GetBoolean(4);
            item.capacity = reader.IsDBNull(5) ? DATABASE_NULL : reader.GetFloat(5);
            item.category = reader.IsDBNull(6) ? "Unknown" : reader.GetString(6);
            item.discard = reader.GetBoolean(7);
            item.convert_to_gold = reader.GetBoolean(8);
            item.look_text = reader.IsDBNull(9) ? String.Format("You see a {0}.", item.displayname) : reader.GetString(9);
            item.title = reader.GetString(10);
            item.currency = reader.IsDBNull(11) ? DATABASE_NULL : reader.GetInt32(11);
            try {
                item.image = reader.IsDBNull(12) ? StyleManager.GetImage("placeholder-item.png") : Image.FromStream(reader.GetStream(12));
            } catch {
                item.image = StyleManager.GetImage("placeholder-item.png");
            }

            if (item.image.RawFormat.Guid == ImageFormat.Gif.Guid) {
                int frames = item.image.GetFrameCount(FrameDimension.Time);
                if (frames == 1) {
                    Bitmap new_bitmap = new Bitmap(item.image);
                    new_bitmap.MakeTransparent();
                    item.image.Dispose();
                    item.image = new_bitmap;
                }
            }

            command = new SQLiteCommand(String.Format("SELECT vendorid, value FROM SellItems WHERE itemid={0}", item.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                ItemSold sellItem = new ItemSold();
                sellItem.itemid = item.id;
                sellItem.npcid = reader.GetInt32(0);
                sellItem.price = reader.GetInt32(1);
                item.sellItems.Add(sellItem);
            }
            command = new SQLiteCommand(String.Format("SELECT vendorid, value FROM BuyItems WHERE itemid={0}", item.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                ItemSold buyItem = new ItemSold();
                buyItem.itemid = item.id;
                buyItem.npcid = reader.GetInt32(0);
                buyItem.price = reader.GetInt32(1);
                item.buyItems.Add(buyItem);
            }
            command = new SQLiteCommand(String.Format("SELECT creatureid, percentage, min, max FROM CreatureDrops WHERE itemid={0}", item.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                ItemDrop itemDrop = new ItemDrop();
                itemDrop.itemid = item.id;
                itemDrop.creatureid = reader.GetInt32(0);
                itemDrop.percentage = reader.IsDBNull(1) ? DATABASE_NULL : reader.GetFloat(1);
                if (itemDrop.percentage > 100) {
                    itemDrop.min = 1;
                    itemDrop.max = (int)(itemDrop.percentage / 100.0 * 2.0);
                    itemDrop.percentage = 100;
                } else {
                    itemDrop.min = Math.Max(reader.GetInt32(2), 1);
                    itemDrop.max = Math.Max(reader.GetInt32(3), itemDrop.min);
                }

                item.itemdrops.Add(itemDrop);
            }
            command = new SQLiteCommand(String.Format("SELECT questid FROM QuestRewards WHERE itemid={0}", item.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                item.rewardedBy.Add(getQuest(reader.GetInt32(0)));
            }
            command = new SQLiteCommand(String.Format("SELECT property, value FROM ItemProperties WHERE itemid={0}", item.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                string property = reader.GetString(0);
                switch(property) {
                    case "Voc":
                        item.vocation = reader.GetString(1);
                        break;
                    case "Level":
                        item.level = reader.GetInt32(1);
                        break;
                    case "Def":
                        item.defensestr = reader["value"].ToString();
                        if (!int.TryParse(item.defensestr, out item.defense)) {
                            item.defense = int.Parse(item.defensestr.Split(' ')[0]);
                        }
                        break;
                    case "Attrib":
                        item.attrib = reader.GetString(1);
                        break;
                    case "Atk":
                        item.attack = reader.GetInt32(1);
                        break;
                    case "Atk+":
                        item.atkmod = reader.GetInt32(1);
                        break;
                    case "Hit+":
                        string str = reader["value"].ToString();
                        int.TryParse(str, out item.hitmod);
                        break;
                    case "Arm":
                        item.armor = reader.GetInt32(1);
                        break;
                    case "Range":
                        item.range = reader.GetInt32(1);
                        break;
                    case "Type":
                        item.type = reader.GetString(1);
                        break;
                }
            }

            return item;
        }
        public static void loadItemImage(int id) {
            Item it = getItem(id);
            if (it.image == null) {
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT image FROM Items WHERE id={0};", id), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    it.image = Image.FromStream(reader.GetStream(0));
                    if (it.image.RawFormat.Guid == ImageFormat.Gif.Guid) {
                        int frames = it.image.GetFrameCount(FrameDimension.Time);
                        if (frames == 1) {
                            Bitmap new_bitmap = new Bitmap(it.image);
                            new_bitmap.MakeTransparent();
                            it.image.Dispose();
                            it.image = new_bitmap;
                        }
                    }
                }
            }
        }
        #endregion

        #region Creature Handling
        public static Creature getCreature(string name) {
            name = name.ToLower().Trim();
            if (_creatureNameMap.ContainsKey(name)) {
                return _creatureNameMap[name];
            }
            if (creaturesLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Creatures WHERE LOWER(name)='{1}';", _creatureProperties, name.Replace("\'", "\'\'")), mainForm.conn);
            Creature cr = createCreature(command.ExecuteReader());
            return registerCreature(cr);
        }

        public static Creature getCreature(int id) {
            if (_creatureIdMap.ContainsKey(id)) {
                return _creatureIdMap[id];
            }
            if (creaturesLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Creatures WHERE id={1};", _creatureProperties, id), mainForm.conn);
            Creature cr = createCreature(command.ExecuteReader());
            return registerCreature(cr);
        }

        private static Creature registerCreature(Creature cr) {
            if (cr == null) return null;
            lock (CreatureLock) {
                if (_creatureIdMap.ContainsKey(cr.id)) {
                    cr.image.Dispose();
                    return _creatureIdMap[cr.id];
                }
                _creatureIdMap.Add(cr.id, cr);
                string name = cr.GetName().ToLower();
                if (!_creatureNameMap.ContainsKey(name)) {
                    _creatureNameMap.Add(cr.GetName().ToLower(), cr);
                }
            }
            return cr;
        }

        private static string _creaturePropertiesBase = "id, name, health, experience, maxdamage, summon, illusionable, pushable, pushes, physical, holy, death, fire, energy, ice, earth, drown, lifedrain, paralysable, senseinvis, abilities, title, speed, armor, boss";
        private static string _creatureProperties = _creaturePropertiesBase + ", image";
        private static Creature createCreature(SQLiteDataReader reader) {
            SQLiteCommand command;

            if (!reader.Read()) {
                return null;
            }

            Creature cr = new Creature();
            cr.permanent = true;
            cr.id = reader.GetInt32(0);
            cr.displayname = reader["name"].ToString();
            cr.health = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetInt32(2);
            cr.experience = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt32(3);
            cr.maxdamage = reader.IsDBNull(4) ? DATABASE_NULL : reader.GetInt32(4);
            cr.summoncost = reader.IsDBNull(5) ? DATABASE_NULL : reader.GetInt32(5);
            cr.illusionable = reader.GetBoolean(6);
            cr.pushable = reader.GetBoolean(7);
            cr.pushes = reader.GetBoolean(8);
            cr.res_phys = reader.IsDBNull(9) ? 100 : reader.GetInt32(9);
            cr.res_holy = reader.IsDBNull(10) ? 100 : reader.GetInt32(10);
            cr.res_death = reader.IsDBNull(11) ? 100 : reader.GetInt32(11);
            cr.res_fire = reader.IsDBNull(12) ? 100 : reader.GetInt32(12);
            cr.res_energy = reader.IsDBNull(13) ? 100 : reader.GetInt32(13);
            cr.res_ice = reader.IsDBNull(14) ? 100 : reader.GetInt32(14);
            cr.res_earth = reader.IsDBNull(15) ? 100 : reader.GetInt32(15);
            cr.res_drown = reader.IsDBNull(16) ? 100 : reader.GetInt32(16);
            cr.res_lifedrain = reader.IsDBNull(17) ? 100 : reader.GetInt32(17);
            cr.paralysable = reader.GetBoolean(18);
            cr.senseinvis = reader.GetBoolean(19);
            cr.abilities = reader.IsDBNull(20) ? DATABASE_STRING_NULL : reader["abilities"].ToString();
            cr.title = reader[21].ToString();
            cr.speed = reader.IsDBNull(22) ? DATABASE_NULL : reader.GetInt32(22);
            cr.armor = reader.IsDBNull(23) ? DATABASE_NULL : reader.GetInt32(23);
            cr.boss = reader.GetInt32(24) > 0;
            try {
                cr.image = reader.IsDBNull(25) ? StyleManager.GetImage("placeholder-creature.png") : Image.FromStream(reader.GetStream(25));
            } catch {
                cr.image = StyleManager.GetImage("placeholder-creature.png");
            }



            command = new SQLiteCommand(String.Format("SELECT skinitemid, knifeitemid, percentage FROM Skins WHERE creatureid={0}", cr.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Skin skin = new Skin();
                skin.dropitemid = reader.GetInt32(0);
                skin.skinitemid = reader.GetInt32(1);
                skin.percentage = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetFloat(2);
                cr.skin = skin;
            }

            command = new SQLiteCommand(String.Format("SELECT itemid, percentage, min, max FROM CreatureDrops WHERE creatureid={0}", cr.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                ItemDrop itemDrop = new ItemDrop();
                itemDrop.creatureid = cr.id;
                itemDrop.itemid = reader.GetInt32(0);
                itemDrop.percentage = reader.IsDBNull(1) ? DATABASE_NULL : reader.GetFloat(1);
                if (itemDrop.percentage > 100) {
                    itemDrop.min = 1;
                    itemDrop.max = (int)(itemDrop.percentage / 100.0 * 2.0);
                    itemDrop.percentage = 100;
                } else {
                    itemDrop.min = Math.Max(reader.GetInt32(2), 1);
                    itemDrop.max = Math.Max(reader.GetInt32(3), itemDrop.min);
                }
                cr.itemdrops.Add(itemDrop);
            }

            return cr;
        }

        public static void loadCreatureImage(int id) {
            Creature cr = getCreature(id);
            if (cr.image == null) {
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT image FROM Creatures WHERE id={0};", id), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    cr.image = Image.FromStream(reader.GetStream(0));
                }
            }
        }
        #endregion

        #region NPC Handling
        public static NPC getNPC(string name) {
            name = name.ToLower().Trim();
            if (_npcNameMap.ContainsKey(name)) {
                return _npcNameMap[name];
            }
            if (itemsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM NPCs WHERE LOWER(name)='{1}';", _npcProperties, name.Replace("\'", "\'\'")), mainForm.conn);
            NPC npc = createNPC(command.ExecuteReader());
            return registerNPC(npc);
        }

        public static NPC getNPC(int id) {
            if (_npcIdMap.ContainsKey(id)) {
                return _npcIdMap[id];
            }
            if (npcsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM NPCs WHERE id={1};", _npcProperties, id), mainForm.conn);
            NPC npc = createNPC(command.ExecuteReader());
            return registerNPC(npc);
        }

        private static NPC registerNPC(NPC npc) {
            if (npc == null) return null;
            lock (NPCLock) {
                if (_npcIdMap.ContainsKey(npc.id)) {
                    npc.image.Dispose();
                    return _npcIdMap[npc.id];
                }
                _npcIdMap.Add(npc.id, npc);
                string name = npc.name.ToLower();
                if (!_npcNameMap.ContainsKey(name)) {
                    _npcNameMap.Add(npc.name.ToLower(), npc);
                }
            }
            return npc;
        }

        private static string _npcProperties = "id,name,city,x,y,z,image,job";
        private static NPC createNPC(SQLiteDataReader reader) {
            SQLiteCommand command;

            if (!reader.Read()) {
                return null;
            }

            NPC npc = new NPC();
            npc.permanent = true;
            npc.id = reader.GetInt32(0);
            npc.name = reader["name"].ToString();
            npc.city = reader["city"].ToString();
            if (!reader.IsDBNull(3) && !reader.IsDBNull(4) && !reader.IsDBNull(5)) {
                npc.pos.x = reader.GetInt32(3);
                npc.pos.y = reader.GetInt32(4);
                npc.pos.z = reader.GetInt32(5);
            }
            npc.image = reader.IsDBNull(6) ? StyleManager.GetImage("placeholder-mount.png") : Image.FromStream(reader.GetStream(6));
            npc.job = reader.IsDBNull(7) ? "" : reader.GetString(7);
            if (npc.image.RawFormat.Guid == ImageFormat.Gif.Guid) {
                int frames = npc.image.GetFrameCount(FrameDimension.Time);
                if (frames == 1) {
                    Bitmap new_bitmap = new Bitmap(npc.image);
                    new_bitmap.MakeTransparent();
                    npc.image.Dispose();
                    npc.image = new_bitmap;
                }
            }

            // special case for rashid: change location based on day of the week
            if (npc != null && npc.name == "Rashid") {
                command = new SQLiteCommand(String.Format("SELECT city, x, y, z FROM RashidPositions WHERE day='{0}'", DateTime.Now.DayOfWeek.ToString()), mainForm.conn);
                reader = command.ExecuteReader();
                if (reader.Read()) {
                    npc.city = reader["city"].ToString();
                    npc.pos.x = reader.GetInt32(1);
                    npc.pos.y = reader.GetInt32(2);
                    npc.pos.z = reader.GetInt32(3);
                }
            }
            command = new SQLiteCommand(String.Format("SELECT itemid, value FROM SellItems WHERE vendorid={0}", npc.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                ItemSold sellItem = new ItemSold();
                sellItem.itemid = reader.GetInt32(0);
                sellItem.npcid = npc.id;
                sellItem.price = reader.GetInt32(1);
                npc.sellItems.Add(sellItem);
            }
            command = new SQLiteCommand(String.Format("SELECT itemid, value FROM BuyItems WHERE vendorid={0}", npc.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                ItemSold buyItem = new ItemSold();
                buyItem.itemid = reader.GetInt32(0);
                buyItem.npcid = npc.id;
                buyItem.price = reader.GetInt32(1);
                npc.buyItems.Add(buyItem);
            }
            command = new SQLiteCommand(String.Format("SELECT spellid,knight,druid,paladin,sorcerer FROM SpellNPCs WHERE npcid={0}", npc.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                SpellTaught t = new SpellTaught();
                t.npcid = npc.id;
                t.spellid = reader.GetInt32(0);
                t.knight = reader.GetBoolean(1);
                t.druid = reader.GetBoolean(2);
                t.paladin = reader.GetBoolean(3);
                t.sorcerer = reader.GetBoolean(4);
                npc.spellsTaught.Add(t);
            }

            command = new SQLiteCommand(String.Format("SELECT DISTINCT questid FROM QuestNPCs WHERE npcid={0}", npc.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Quest q = getQuest(reader.GetInt32(0));
                npc.involvedQuests.Add(q);
            }

            command = new SQLiteCommand(String.Format("SELECT destination,cost,notes FROM NPCDestinations WHERE npcid={0}", npc.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Transport t = new Transport();
                t.destination = reader.GetString(0);
                t.cost = reader.GetInt32(1);
                t.notes = reader.GetString(2);
                npc.transportOffered.Add(t);
            }
            return npc;
        }
        #endregion

        #region Hunt Handling
        public static HuntingPlace getHunt(string name) {
            name = name.ToLower().Trim();
            if (_huntingPlaceNameMap.ContainsKey(name)) {
                return _huntingPlaceNameMap[name];
            }
            if (huntsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM HuntingPlaces WHERE LOWER(name)='{1}';", _huntProperties, name.Replace("\'", "\'\'")), mainForm.conn);
            HuntingPlace hunt = createHunt(command.ExecuteReader());
            return registerHunt(hunt);
        }

        public static HuntingPlace getHunt(int id) {
            if (_huntingPlaceIdMap.ContainsKey(id)) {
                return _huntingPlaceIdMap[id];
            }
            if (huntsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM HuntingPlaces WHERE id={1};", _huntProperties, id), mainForm.conn);
            HuntingPlace hunt = createHunt(command.ExecuteReader());
            return registerHunt(hunt);
        }

        private static HuntingPlace registerHunt(HuntingPlace h) {
            if (h == null) return null;
            lock (HuntLock) {
                if (_huntingPlaceIdMap.ContainsKey(h.id)) {
                    h.image.Dispose();
                    return _huntingPlaceIdMap[h.id];
                }
                _huntingPlaceIdMap.Add(h.id, h);
                string name = h.name.ToLower();
                if (!_huntingPlaceNameMap.ContainsKey(name)) {
                    _huntingPlaceNameMap.Add(h.name.ToLower(), h);
                }
            }
            return h;
        }

        private static string _huntProperties = "id, name, level, exprating, lootrating, image, city";
        private static HuntingPlace createHunt(SQLiteDataReader reader) {
            SQLiteCommand command;

            if (!reader.Read()) {
                return null;
            }

            HuntingPlace huntingPlace = new HuntingPlace();
            huntingPlace.permanent = true;
            huntingPlace.id = reader.GetInt32(0);
            huntingPlace.name = reader["name"].ToString();
            huntingPlace.level = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetInt32(2);
            huntingPlace.exp_quality = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt32(3);
            huntingPlace.loot_quality = reader.IsDBNull(4) ? DATABASE_NULL : reader.GetInt32(4);
            string imageName = reader.GetString(5).ToLower();
            Creature cr = getCreature(imageName);
            if (cr != null) {
                huntingPlace.image = cr.GetImage();
            } else {
                NPC npc = getNPC(imageName);
                if (npc != null) {
                    huntingPlace.image = npc.GetImage();
                } else {
                    throw new Exception("Unrecognized npc or creature image.");
                }
            }
            huntingPlace.city = reader["city"].ToString();

            // Hunting place coordinates
            command = new SQLiteCommand(String.Format("SELECT x, y, z FROM HuntingPlaceCoordinates WHERE huntingplaceid={0}", huntingPlace.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Coordinate c = new Coordinate();
                c.x = reader.IsDBNull(0) ? DATABASE_NULL : reader.GetInt32(0);
                c.y = reader.IsDBNull(1) ? DATABASE_NULL : reader.GetInt32(1);
                c.z = reader.IsDBNull(2) ? DATABASE_NULL : reader.GetInt32(2);
                huntingPlace.coordinates.Add(c);
            }
            // Hunting place directions
            command = new SQLiteCommand(String.Format("SELECT beginx, beginy, beginz,endx, endy, endz, ordering, description, settings FROM HuntDirections WHERE huntingplaceid={0} ORDER BY ordering", huntingPlace.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Directions d = new Directions();
                d.huntingplaceid = huntingPlace.id;
                d.begin = new Coordinate(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
                d.end = new Coordinate(reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
                d.ordering = reader.GetInt32(6);
                d.description = reader["description"].ToString();
                d.settings = reader.GetString(8);
                huntingPlace.directions.Add(d);
            }

            // Hunting place creatures
            command = new SQLiteCommand(String.Format("SELECT creatureid FROM HuntingPlaceCreatures WHERE huntingplaceid={0}", huntingPlace.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                int creatureid = reader.GetInt32(0);
                huntingPlace.creatures.Add(creatureid);
            }
            // Hunting place requirements
            command = new SQLiteCommand(String.Format("SELECT questid, requirementtext FROM HuntRequirements WHERE huntingplaceid={0}", huntingPlace.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                Requirements r = new Requirements();
                r.huntingplaceid = huntingPlace.id;
                int questid = reader.IsDBNull(0) ? DATABASE_NULL : reader.GetInt32(0);
                r.quest = questIdMap[questid];
                r.notes = reader["requirementtext"].ToString();
                huntingPlace.requirements.Add(r);
            }
            return huntingPlace;
        }
        #endregion

        #region Spell Handling
        public static Spell getSpell(string name) {
            name = name.ToLower().Trim();
            if (_spellNameMap.ContainsKey(name)) {
                return _spellNameMap[name];
            }
            if (spellsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Spells WHERE LOWER(name)='{1}';", _spellProperties, name.Replace("\'", "\'\'")), mainForm.conn);
            Spell s = createSpell(command.ExecuteReader());
            return registerSpell(s);
        }

        public static Spell getSpell(int id) {
            if (_spellIdMap.ContainsKey(id)) {
                return _spellIdMap[id];
            }
            if (spellsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Spells WHERE id={1};", _spellProperties, id), mainForm.conn);
            Spell s = createSpell(command.ExecuteReader());
            return registerSpell(s);
        }

        private static Spell registerSpell(Spell h) {
            if (h == null) return null;
            lock (SpellLock) {
                if (_spellIdMap.ContainsKey(h.id)) {
                    h.image.Dispose();
                    return _spellIdMap[h.id];
                }
                _spellIdMap.Add(h.id, h);
                string name = h.name.ToLower();
                if (!_spellNameMap.ContainsKey(name)) {
                    _spellNameMap.Add(h.name.ToLower(), h);
                }
            }
            return h;
        }

        private static string _spellProperties = "id, name, words, element, cooldown, premium, promotion, levelrequired, goldcost, manacost, knight, paladin, sorcerer, druid, image";
        private static Spell createSpell(SQLiteDataReader reader) {
            SQLiteCommand command;

            if (!reader.Read()) {
                return null;
            }

            Spell spell = new Spell();
            spell.permanent = true;
            spell.id = reader.GetInt32(0);
            spell.name = reader["name"].ToString();
            spell.words = reader["words"].ToString();
            spell.element = reader.IsDBNull(3) ? "Unknown" : reader.GetString(3);
            spell.cooldown = reader.IsDBNull(4) ? DATABASE_NULL : reader.GetInt32(4);
            spell.premium = reader.GetBoolean(5);
            spell.promotion = reader.GetBoolean(6);
            spell.levelrequired = reader.GetInt32(7);
            spell.goldcost = reader.GetInt32(8);
            spell.manacost = reader.GetInt32(9);
            spell.knight = reader.GetBoolean(10);
            spell.paladin = reader.GetBoolean(11);
            spell.sorcerer = reader.GetBoolean(12);
            spell.druid = reader.GetBoolean(13);
            spell.image = reader.IsDBNull(14) ? StyleManager.GetImage("placeholder-spell.png") : Image.FromStream(reader.GetStream(14));


            command = new SQLiteCommand(String.Format("SELECT npcid, knight, druid, paladin, sorcerer FROM SpellNPCs WHERE spellid={0}", spell.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                SpellTaught t = new SpellTaught();
                t.npcid = reader.GetInt32(0);
                t.spellid = spell.id;
                t.knight = reader.GetBoolean(1);
                t.druid = reader.GetBoolean(2);
                t.paladin = reader.GetBoolean(3);
                t.sorcerer = reader.GetBoolean(4);
                spell.teachNPCs.Add(t);
            }
            return spell;
        }


        #endregion

        #region Mount Handling
        public static Mount getMount(string name) {
            name = name.ToLower().Trim();
            if (_mountNameMap.ContainsKey(name)) {
                return _mountNameMap[name];
            }
            if (mountsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Mounts WHERE LOWER(name)='{1}';", _mountProperties, name.Replace("\'", "\'\'")), mainForm.conn);
            Mount m = createMount(command.ExecuteReader());
            return registerMount(m);
        }

        public static Mount getMount(int id) {
            if (_mountIdMap.ContainsKey(id)) {
                return _mountIdMap[id];
            }
            if (mountsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Mounts WHERE id={1};", _mountProperties, id), mainForm.conn);
            Mount m = createMount(command.ExecuteReader());
            return registerMount(m);
        }

        private static Mount registerMount(Mount h) {
            if (h == null) return null;
            lock (SpellLock) {
                if (_mountIdMap.ContainsKey(h.id)) {
                    h.image.Dispose();
                    return _mountIdMap[h.id];
                }
                _mountIdMap.Add(h.id, h);
                string name = h.name.ToLower();
                if (!_mountNameMap.ContainsKey(name)) {
                    _mountNameMap.Add(h.name.ToLower(), h);
                }
            }
            return h;
        }

        private static string _mountProperties = "id, title, name, tameitemid, tamecreatureid, speed, tibiastore, image";
        private static Mount createMount(SQLiteDataReader reader) {
            if (!reader.Read()) {
                return null;
            }

            Mount mount = new Mount();
            mount.permanent = true;
            mount.id = reader.GetInt32(0);
            mount.title = reader.GetString(1);
            mount.name = reader.GetString(2);

            int tameitem = reader.IsDBNull(3) ? DATABASE_NULL : reader.GetInt32(3);
            if (tameitem > 0) mount.tameitemid = tameitem;
            else mount.tameitemid = -1;
            int tamecreature = reader.IsDBNull(4) ? DATABASE_NULL : reader.GetInt32(4);
            if (tamecreature > 0) mount.tamecreatureid = tamecreature;
            else mount.tamecreatureid = -1;
            mount.speed = reader.GetInt32(5);
            mount.tibiastore = reader.GetBoolean(6);
            mount.image = reader.IsDBNull(7) ? StyleManager.GetImage("placeholder-mount.png") : Image.FromStream(reader.GetStream(7));

            return mount;
        }
        #endregion

        #region Outfit Handling
        public static Outfit getOutfit(string name) {
            name = name.ToLower().Trim();
            if (_outfitNameMap.ContainsKey(name)) {
                return _outfitNameMap[name];
            }
            if (outfitsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Outfits WHERE LOWER(name)='{1}';", _outfitProperties, name.Replace("\'", "\'\'")), mainForm.conn);
            Outfit o = createOutfit(command.ExecuteReader());
            return registerOutfit(o);
        }

        public static Outfit getOutfit(int id) {
            if (_outfitIdMap.ContainsKey(id)) {
                return _outfitIdMap[id];
            }
            if (outfitsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM Outfits WHERE id={1};", _outfitProperties, id), mainForm.conn);
            Outfit o = createOutfit(command.ExecuteReader());
            return registerOutfit(o);
        }

        private static Outfit registerOutfit(Outfit h) {
            if (h == null) return null;
            lock (SpellLock) {
                if (_outfitIdMap.ContainsKey(h.id)) {
                    for (int i = 0; i <= 3; i++) {
                        if (h.maleImages[i] != null) h.maleImages[i].Dispose();
                        if (h.femaleImages[i] != null) h.femaleImages[i].Dispose();
                    }
                    return _outfitIdMap[h.id];
                }
                _outfitIdMap.Add(h.id, h);
                string name = h.name.ToLower();
                if (!_outfitNameMap.ContainsKey(name)) {
                    _outfitNameMap.Add(h.name.ToLower(), h);
                }
            }
            return h;
        }

        private static string _outfitProperties = "id, title, name, premium, tibiastore";
        private static Outfit createOutfit(SQLiteDataReader reader) {
            SQLiteCommand command;
            if (!reader.Read()) {
                return null;
            }

            Outfit outfit = new Outfit();
            outfit.permanent = true;
            outfit.id = reader.GetInt32(0);
            outfit.title = reader.GetString(1);
            outfit.name = reader.GetString(2);
            outfit.premium = reader.GetBoolean(3);
            outfit.tibiastore = reader.GetBoolean(4);

            // Outfit Images
            command = new SQLiteCommand(String.Format("SELECT male, addon, image FROM OutfitImages WHERE outfitid={0}", outfit.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                bool male = reader.GetBoolean(0);
                int addon = reader.GetInt32(1);
                Image image = Image.FromStream(reader.GetStream(2));

                if (male) {
                    outfit.maleImages[addon] = image;
                } else {
                    outfit.femaleImages[addon] = image;
                }
            }

            command = new SQLiteCommand(String.Format("SELECT questid FROM QuestOutfits WHERE outfitid={0}", outfit.id), mainForm.conn);
            reader = command.ExecuteReader();
            while (reader.Read()) {
                outfit.questid = reader.GetInt32(0);
            }

            return outfit;
        }
        #endregion

        #region World Object Handling
        public static WorldObject getWorldObject(string name) {
            name = name.ToLower().Trim();
            if (_worldObjectNameMap.ContainsKey(name)) {
                return _worldObjectNameMap[name];
            }
            if (worldObjectsLoaded) return null;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT {0} FROM WorldObjects WHERE LOWER(title)='{1}';", _worldObjectProperties, name.Replace("\'", "\'\'")), mainForm.conn);
            WorldObject o = createWorldObject(command.ExecuteReader());
            return registerWorldObject(o);
        }

        private static WorldObject registerWorldObject(WorldObject h) {
            if (h == null) return null;
            lock (SpellLock) {
                string name = h.title.ToLower();
                if (_worldObjectNameMap.ContainsKey(name)) {
                    h.image.Dispose();
                    return _worldObjectNameMap[name];
                }
                _worldObjectNameMap.Add(name, h);
            }
            return h;
        }

        private static string _worldObjectProperties = "title, name, image";
        private static WorldObject createWorldObject(SQLiteDataReader reader) {
            if (!reader.Read()) {
                return null;
            }

            WorldObject o = new WorldObject();
            o.title = reader.GetString(0);
            o.name = reader.GetString(1);
            o.image = Image.FromStream(reader.GetStream(2)); ;
            return o;
        }

        #endregion

        public static Quest getQuest(string name) {
            name = name.ToLower().Trim();
            if (!questNameMap.ContainsKey(name)) return null;
            return questNameMap[name];
        }

        public static Quest getQuest(int id) {
            if (!questIdMap.ContainsKey(id)) return null;
            return questIdMap[id];
        }

        public static List<TibiaObject> getNPCWithCity(string city) {
            city = city.ToLower();
            if (!npcsLoaded) {
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM NPCs WHERE LOWER(city)='{0}';", city.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                List<TibiaObject> result = new List<TibiaObject>();
                while (reader.Read()) {
                    NPC npc = getNPC(reader.GetInt32(0));
                    result.Add(npc);
                }
                return result;
            } else {
                return _npcIdMap.Values.Where(o => o.city.ToLower() == city).ToList<TibiaObject>();
            }
        }
        public static List<HuntingPlace> getHuntsInCity(string city) {
            city = city.ToLower();
            if (!huntsLoaded) {
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM HuntingPlaces WHERE LOWER(city)='{0}';", city.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                List<HuntingPlace> result = new List<HuntingPlace>();
                while (reader.Read()) {
                    HuntingPlace h = getHunt(reader.GetInt32(0));
                    result.Add(h);
                }
                return result;
            } else {
                return _huntingPlaceIdMap.Values.Where(o => o.city.ToLower() == city).ToList();
            }
        }
        public static List<HuntingPlace> getHuntsForLevels(int minlevel, int maxlevel) {
            if (!huntsLoaded) {
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM HuntingPlaces WHERE level >= {0} AND level <= {1};", minlevel, maxlevel), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                List<HuntingPlace> result = new List<HuntingPlace>();
                while (reader.Read()) {
                    HuntingPlace h = getHunt(reader.GetInt32(0));
                    result.Add(h);
                }
                return result;
            } else {
                return _huntingPlaceIdMap.Values.Where(o => o.level >= minlevel && o.level <= maxlevel).ToList();
            }
        }

        public static List<HuntingPlace> getHuntsForCreature(int creatureid) {
            List<HuntingPlace> huntingPlaces = new List<HuntingPlace>();
            if (!huntsLoaded) {
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT HuntingPlaces.id FROM HuntingPlaces INNER JOIN HuntingPlaceCreatures ON HuntingPlaces.id=HuntingPlaceCreatures.huntingplaceid AND HuntingPlaceCreatures.creatureid={0}", creatureid), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    huntingPlaces.Add(getHunt(reader.GetInt32(0)));
                }
            } else {
                foreach (HuntingPlace h in _huntingPlaceIdMap.Values) {
                    foreach (int c in h.creatures) {
                        if (c == creatureid) {
                            huntingPlaces.Add(h);
                            break;
                        }
                    }
                }
            }
            return huntingPlaces;
        }

        public static NPC getNPCSellingItemInCity(int itemid, string city) {
            if (!(npcsLoaded && itemsLoaded)) {
                SQLiteCommand command;
                SQLiteDataReader reader;
                command = new SQLiteCommand(String.Format("SELECT NPCs.id FROM BuyItems INNER JOIN NPCs ON NPCs.id=BuyItems.vendorid AND LOWER(NPCs.city)='{0}' AND BuyItems.itemid={1}", city.Replace("\'", "\'\'"), itemid), mainForm.conn);
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    return getNPC(reader.GetInt32(0));
                }
                command = new SQLiteCommand(String.Format("SELECT NPCs.id FROM SellItems INNER JOIN NPCs ON NPCs.id=SellItems.vendorid AND LOWER(NPCs.city)='{0}' AND SellItems.itemid={1}", city.Replace("\'", "\'\'"), itemid), mainForm.conn);
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    return getNPC(reader.GetInt32(0));
                }
                return null;
            } else {
                Item item = _itemIdMap[itemid];
                foreach (ItemSold itemSold in item.buyItems.Union(item.sellItems)) {
                    NPC npc = _npcIdMap[itemSold.npcid];
                    if (npc.city.ToLower() == city) {
                        return npc;
                    }
                }
                return null;
            }
        }
        public static NPC getNPCTeachingSpellInCity(int spellid, string city) {
            if (!(npcsLoaded && spellsLoaded)) {
                SQLiteCommand command;
                SQLiteDataReader reader;
                command = new SQLiteCommand(String.Format("SELECT NPCs.id FROM SpellNPCs INNER JOIN NPCs ON NPCs.id=SpellNPCs.npcid AND LOWER(NPCs.city)='{0}' AND SpellNPCs.spellid={1}", city.Replace("\'", "\'\'"), spellid), mainForm.conn);
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    return getNPC(reader.GetInt32(0));
                }
                return null;
            } else {
                Spell spell = getSpell(spellid);
                foreach (SpellTaught teach in spell.teachNPCs) {
                    NPC npc = _npcIdMap[teach.npcid];
                    if (npc.city.ToLower() == city) {
                        return npc;
                    }
                }
                return null;
            }
        }
        public static List<TibiaObject> getSpellsForVocation(string vocation) {
            vocation = vocation.ToLower();
            if (!spellsLoaded) {
                List<TibiaObject> result = new List<TibiaObject>();
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM Spells WHERE {0}=1 ORDER BY levelrequired;", vocation), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    result.Add(new LazyTibiaObject { id = reader.GetInt32(0), type = TibiaObjectType.Spell });
                }
                return result;
            } else {
                return _spellIdMap.Values.Where(o => (o.druid && vocation == "druid") ||
                (o.paladin && vocation == "paladin") || (o.sorcerer && vocation == "sorcerer") || (o.knight && vocation == "knight")).
                OrderBy(o => o.levelrequired).ToList<TibiaObject>();
            }
        }

        public static bool itemExists(string str) {
            if (_itemNameMap.ContainsKey(str)) {
                return true;
            }
            if (itemsLoaded) return false;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM Items WHERE LOWER(name)='{0}';", str.Replace("\'", "\'\'")), mainForm.conn);

            return command.ExecuteScalar() != null;
        }

        public static bool creatureExists(string str) {
            str = str.ToLower().Trim();
            if (_creatureNameMap.ContainsKey(str)) {
                return true;
            }
            if (creaturesLoaded) return false;

            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM Creatures WHERE LOWER(name)='{0}';", str.Replace("\'", "\'\'")), mainForm.conn);

            return command.ExecuteScalar() != null;
        }

        public static List<TibiaObject> searchItem(string str) {
            if (!itemsLoaded) {
                List<TibiaObject> result = new List<TibiaObject>();
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM Items WHERE LOWER(name) LIKE '%{0}%' OR LOWER(title) LIKE '%{0}%' AND category IS NOT NULL ORDER BY category,actual_value;", str.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    result.Add(new LazyTibiaObject { id = reader.GetInt32(0), type = TibiaObjectType.Item });
                }
                return result;
            } else {
                return _itemIdMap.Values.Where(o => o.GetName().ToLower().Contains(str)).ToList<TibiaObject>();
            }
        }
        public static List<TibiaObject> searchCreature(string str) {
            str = str.ToLower();
            if (!creaturesLoaded) {
                List<TibiaObject> result = new List<TibiaObject>();
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM Creatures WHERE LOWER(name) LIKE '%{0}%' OR LOWER(title) LIKE '%{0}%' ORDER BY experience;", str.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    result.Add(new LazyTibiaObject { id = reader.GetInt32(0), type = TibiaObjectType.Creature });
                }
                return result;
            } else {
                return _creatureIdMap.Values.Where(o => o.GetName().ToLower().Contains(str)).OrderBy(o => o.experience).ToList<TibiaObject>();
            }
        }
        public static List<TibiaObject> searchNPC(string str) {
            str = str.ToLower();
            if (!npcsLoaded) {
                List<TibiaObject> result = new List<TibiaObject>();
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM NPCs WHERE LOWER(name) LIKE '%{0}%' ORDER BY city;", str.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    result.Add(new LazyTibiaObject { id = reader.GetInt32(0), type = TibiaObjectType.NPC });
                }
                return result;
            } else {
                return _npcIdMap.Values.Where(o => o.name.ToLower().Contains(str)).OrderBy(o => o.city).ToList<TibiaObject>();
            }
        }
        public static List<HuntingPlace> searchHunt(string str) {
            str = str.ToLower();
            if (!huntsLoaded) {
                List<HuntingPlace> result = new List<HuntingPlace>();
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM HuntingPlaces WHERE LOWER(name) LIKE '%{0}%';", str.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    result.Add(getHunt(reader.GetInt32(0)));
                }
                return result;
            } else {
                return _huntingPlaceIdMap.Values.Where(o => o.name.ToLower().Contains(str)).ToList();
            }
        }
        public static List<TibiaObject> searchSpell(string str) {
            str = str.ToLower();
            if (!spellsLoaded) {
                List<TibiaObject> result = new List<TibiaObject>();
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM Spells WHERE LOWER(name) LIKE '%{0}%' ORDER BY levelrequired;", str.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    result.Add(new LazyTibiaObject { id = reader.GetInt32(0), type = TibiaObjectType.Spell });
                }
                return result;
            } else {
                return _spellIdMap.Values.Where(o => o.name.ToLower().Contains(str)).OrderBy(o => o.levelrequired).ToList<TibiaObject>();
            }
        }
        public static List<TibiaObject> searchSpellWords(string str) {
            str = str.ToLower();
            if (!spellsLoaded) {
                List<TibiaObject> result = new List<TibiaObject>();
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM Spells WHERE LOWER(words) LIKE '%{0}%' ORDER BY levelrequired;", str.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    result.Add(new LazyTibiaObject { id = reader.GetInt32(0), type = TibiaObjectType.Spell });
                }
                return result;
            } else {
                return _spellIdMap.Values.Where(o => o.words.ToLower().Contains(str)).OrderBy(o => o.levelrequired).ToList<TibiaObject>();
            }
        }

        public static List<TibiaObject> searchQuest(string str) {
            str = str.ToLower();
            return questIdMap.Values.Where(o => o.name.ToLower().Contains(str)).ToList<TibiaObject>();
        }
        public static List<TibiaObject> searchMount(string str) {
            str = str.ToLower();
            if (!mountsLoaded) {
                List<TibiaObject> result = new List<TibiaObject>();
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM Mounts WHERE LOWER(name) LIKE '%{0}%';", str.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    result.Add(new LazyTibiaObject { id = reader.GetInt32(0), type = TibiaObjectType.Mount });
                }
                return result;
            } else {
                return _mountIdMap.Values.Where(o => o.name.ToLower().Contains(str)).ToList<TibiaObject>();
            }
        }
        public static List<TibiaObject> searchOutfit(string str) {
            str = str.ToLower();
            if (!outfitsLoaded) {
                List<TibiaObject> result = new List<TibiaObject>();
                SQLiteCommand command = new SQLiteCommand(String.Format("SELECT id FROM Outfits WHERE LOWER(name) LIKE '%{0}%';", str.Replace("\'", "\'\'")), mainForm.conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    result.Add(new LazyTibiaObject { id = reader.GetInt32(0), type = TibiaObjectType.Outfit });
                }
                return result;
            } else {
                return _outfitIdMap.Values.Where(o => o.name.ToLower().Contains(str)).ToList<TibiaObject>();
            }
        }
        
        public static List<TibiaObject> getItemsByCategory(string str) {
            str = str.ToLower();
            if (!itemsLoaded) {
                List<TibiaObject> result = new List<TibiaObject>();
                SQLiteCommand command;
                SQLiteDataReader reader;
                command = new SQLiteCommand(String.Format("SELECT category FROM Items WHERE LOWER(category) LIKE '%{0}%' LIMIT 1", str.Replace("\'", "\'\'")), mainForm.conn);
                reader = command.ExecuteReader();
                if (reader.Read()) {
                    string category = reader.GetString(0).ToLower();
                    command = new SQLiteCommand(String.Format("SELECT id FROM Items WHERE LOWER(category)='{0}';", category.Replace("\'", "\'\'")), mainForm.conn);
                    reader = command.ExecuteReader();
                    while (reader.Read()) {
                        result.Add(new LazyTibiaObject { id = reader.GetInt32(0), type = TibiaObjectType.Item });
                    }
                    return result;
                }
                return new List<TibiaObject>();
            } else {
                return _itemIdMap.Values.Where(o => o.category.ToLower().Contains(str)).ToList<TibiaObject>();
            }
        }
    }
}

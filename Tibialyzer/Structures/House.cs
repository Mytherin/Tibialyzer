using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tibialyzer {
    class House : TibiaObject {
        public int id;
        public string name;
        public string world;
        public string city;
        public int beds;
        public int sqm;
        public int hoursleft;
        public int bid;
        public Coordinate pos;
        public bool occupied;
        public bool guildhall;
            

        public override string GetName() {
            return name;
        }
        public override Image GetImage() {
            if (sqm < 20) {
                return StyleManager.GetImage("smallhouse.png");
            } else if (sqm < 60) {
                return StyleManager.GetImage("mediumhouse.png");
            } else {
                return StyleManager.GetImage("bighouse.png");
            }
        }
        public override List<string> GetAttributeHeaders() {
            if (world == null) {
                return new List<string> { "Name", "City", "SQM", "Beds" };
            }
            return new List<string> { "Name", "City", "SQM", "Beds", "Free", "Bid", "Time" };
        }

        public override List<Attribute> GetAttributes() {
            if (world == null) {
                return new List<Attribute> {
                new StringAttribute(name, 150),
                new StringAttribute(city, 80),
                new StringAttribute(sqm.ToString(), 50),
                new StringAttribute(beds.ToString(), 50) };
            }
            return new List<Attribute> {
                new StringAttribute(name, 150),
                new StringAttribute(city, 80),
                new StringAttribute(sqm.ToString(), 50),
                new StringAttribute(beds.ToString(), 50),
                new BooleanAttribute(!occupied),
                new StringAttribute(occupied || bid < 0 ? "-" : Item.ConvertGoldToString(bid), 50),
                new StringAttribute(occupied || hoursleft < 0 ? "-" : (hoursleft < 24 ? String.Format("{0}h", hoursleft) : String.Format("{0}D", hoursleft/24)), 50) };
        }
        public override string GetCommand() {
            return String.Format("{2}{0}{1}", Constants.CommandSymbol, id, guildhall ? "guildhall" : "house");
        }

        public House() {
            world = null;
            this.pos = new Coordinate();
        }
        
        public static bool GatherInformationOnline(string world, string city, bool guildhall) {
            bool found = false;
            List<TibiaObject> houses = StorageManager.getHouseByCity(city, guildhall);
            foreach(TibiaObject obj in houses) {
                House h = obj as House;
                if (h.world == null || !h.world.Equals(world, StringComparison.InvariantCultureIgnoreCase)) {
                    found = true;
                    break;
                }
            }
            if (!found) return true;
            try {
                using (WebClient client = new WebClient()) {
                    Dictionary<int, House> idMap = guildhall ? StorageManager.guildHallIdMap : StorageManager.houseIdMap;
                    string html = client.DownloadString(String.Format("https://secure.tibia.com/community/?subtopic=houses&world={0}&town={1}{2}", world.ToTitle(), city.ToTitle().Replace("'", "%27"), guildhall ? "&type=guildhalls" : ""));

                    foreach (TibiaObject obj in houses) {
                        House h = obj as House;
                        h.world = world;
                    }

                    int index = 0;
                    Regex baseRegex = new Regex("<TR BGCOLOR=#[A-Fa-f0-9]+>");
                    Regex auctionedRegex = new Regex("<TD WIDTH=40%><NOBR>(rented|auctioned[^<]*)</NOBR></TD>");
                    Regex houseidRegex = new Regex("<INPUT TYPE=hidden NAME=houseid VALUE=([0-9]+)>");
                    Regex hoursRegex = new Regex("([0-9]+) gold[;] ([0-9]+) hours left");
                    Regex daysRegex = new Regex("([0-9]+) gold[;] ([0-9]+) days left");
                    while (index < html.Length) {
                        Match m = baseRegex.Match(html, index);
                        if (!m.Success)
                            return true;
                        index = m.Index + m.Length;

                        m = auctionedRegex.Match(html, index);
                        if (!m.Success)
                            continue;
                        index = m.Index + m.Length;

                        string status = Player.RemoveJunk(m.Groups[1].Value);
                        
                        m = houseidRegex.Match(html, index);
                        if (!m.Success)
                            continue;
                        index = m.Index + m.Length;

                        int id = int.Parse(m.Groups[1].Value);
                        
                        if (!idMap.ContainsKey(id))
                            continue;
                        House house = idMap[id];

                        house.world = world;

                        if (status.Equals("rented", StringComparison.InvariantCultureIgnoreCase)) {
                            house.occupied = true;
                            house.hoursleft = 0;
                            house.bid = 0;
                        } else {
                            house.occupied = false;
                            house.hoursleft = -1;
                            house.bid = -1;
                            if (!status.Contains("no bids left", StringComparison.InvariantCultureIgnoreCase)) {
                                bool days = status.Contains("days", StringComparison.InvariantCultureIgnoreCase);
                                Regex re = days ? daysRegex : hoursRegex;
                                m = re.Match(status);
                                if (m.Success) {
                                    int bid = int.Parse(m.Groups[1].Value);
                                    int time = int.Parse(m.Groups[2].Value);
                                    house.bid = bid;
                                    house.hoursleft = days ? time * 24 : time;
                                }
                            }
                        }
                    }

                    return true;
                }
            } catch {
                return false;
            }
        }
    }
}

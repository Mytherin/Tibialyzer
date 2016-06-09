using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;

namespace Tibialyzer {
    public enum Vocation { Knight, Druid, Sorcerer, Paladin, None };
    public enum Gender { Male, Female };
    public class Player {
        public string name = null;
        public Vocation vocation;
        public Gender gender;
        public bool promoted;
        public int level;

        // online lookup only
        public bool additionalInfo = false;
        public string guild = null;
        public string house = null;
        public bool premium = false;
        public List<string> recentDeaths = new List<string>();
        public string marriage = null;
        public string world = null;

        public int MaxLife() {
            switch (vocation) {
                case Vocation.Paladin:
                    return 5 * (2 * level + 21);
                case Vocation.Knight:
                    return 5 * (3 * level + 21);
                default: // all other vocations (no vocation, druid, sorcerer) have the same max life
                    return 5 * (level + 29);
            }
        }

        public int MaxMana() {
            switch (vocation) {
                case Vocation.Sorcerer:
                case Vocation.Druid:
                    return 5 * (6 * level - 30);
                case Vocation.Paladin:
                    return 5 * (3 * level - 6);
                default:
                    return 5 * (level + 10);
            }
        }

        public int Capacity() {
            switch (vocation) {
                case Vocation.Paladin:
                    return 10 * (2 * level + 31);
                case Vocation.Knight:
                    return 5 * (5 * level + 54);
                default:
                    return 10 * (level + 39);
            }
        }

        public int BaseSpeed() {
            return 220 + 2 * (level - 1);
        }

        public long BaseExperience() {
            return ExperienceBar.GetExperience(level);
        }

        public Image GetImage() {
            string imageName = gender == Gender.Male ? "male" : "female";
            switch(vocation) {
                case Vocation.Druid:
                    imageName += promoted ? "elderdruid" : "druid";
                    break;
                case Vocation.Sorcerer:
                    imageName += promoted ? "mastersorcerer" : "sorcerer";
                    break;
                case Vocation.Paladin:
                    imageName += promoted ? "royalpaladin" : "paladin";
                    break;
                case Vocation.Knight:
                    imageName += promoted ? "eliteknight" : "knight";
                    break;
                default:
                    imageName += "knight";
                    break;
            }
            return StyleManager.GetImage(imageName + ".png");
        }

        public string GetVocation() {
            switch (vocation) {
                case Vocation.Druid:
                    return promoted ? "Elder Druid" : "Druid";
                case Vocation.Sorcerer:
                    return promoted ? "Master Sorcerer" : "Sorcerer";
                case Vocation.Paladin:
                    return promoted ? "Royal Paladin" : "Paladin";
                case Vocation.Knight:
                    return promoted ? "Elite Knight" : "Knight";
                default:
                    return "None";
            }
        }

        public void SetVocation(string vocation) {
            if (vocation.Contains("knight")) this.vocation = Vocation.Knight;
            else if (vocation.Contains("druid")) this.vocation = Vocation.Druid;
            else if (vocation.Contains("paladin")) this.vocation = Vocation.Paladin;
            else if (vocation.Contains("sorcerer")) this.vocation = Vocation.Sorcerer;
            else this.vocation = Vocation.None;

            if (vocation.Contains("elder") || vocation.Contains("royal") || vocation.Contains("elite") || vocation.Contains("master")) this.promoted = true;
        }

        public string RemoveJunk(string junk) {
            Regex regex = new Regex("<[^>]+>");

            Match m;
            while((m = regex.Match(junk)).Success) {
                junk = junk.Remove(m.Groups[0].Index, m.Groups[0].Length);
            }

            junk = junk.Replace("&#160;", " ");
            return junk.Trim();
        }

        public string FindEntry(string html, Match m) {
            return html.Substring(m.Groups[0].Index + m.Groups[0].Length).Split(new string[] { "</td>" }, StringSplitOptions.None)[0];
        }

        /// <summary>
        /// Gets additional information about a player from the tibia.com characters page
        /// </summary>
        public bool GatherInformationOnline(bool all = false) {
            try {
                using (WebClient client = new WebClient()) {
                    string html = client.DownloadString(String.Format("https://secure.tibia.com/community/?subtopic=characters&name={0}", name.Replace(" ", "+")));

                    string baseRegex = "<td[^>]*>{0}</td[^>]*><td[^>]*>";

                    if (all) {
                        Regex genderRegex = new Regex(String.Format(baseRegex, "Sex:"));
                        Regex vocationRegex = new Regex(String.Format(baseRegex, "Vocation:"));
                        Regex levelRegex = new Regex(String.Format(baseRegex, "Level:"));

                        Match gender = genderRegex.Match(html);
                        Match vocation = vocationRegex.Match(html);
                        Match level = levelRegex.Match(html);
                        if (!gender.Success || !vocation.Success || !level.Success) {
                            return false;
                        }

                        this.gender = FindEntry(html, gender).ToLower().Contains("female") ? Gender.Female : Gender.Male;
                        if (!int.TryParse(FindEntry(html, level).Trim(), out this.level)) {
                            return false;
                        }
                        this.SetVocation(FindEntry(html, vocation).ToLower());
                    }

                    Regex marriedRegex = new Regex(String.Format(baseRegex, "Married to:"));
                    Regex houseRegex = new Regex(String.Format(baseRegex, "House:"));
                    Regex guildRegex = new Regex(String.Format(baseRegex, "Guild&#160;membership:"));
                    Regex worldRegex = new Regex(String.Format(baseRegex, "World:"));
                    Regex linkNameRegex = new Regex("<a href[^>]*>([^\n]+)</a[^>]*>");

                    Match married = marriedRegex.Match(html);
                    if (married.Success) {
                        Match player = linkNameRegex.Match(FindEntry(html, married));
                        if (player.Success) {
                            this.marriage = RemoveJunk(player.Groups[1].Value);
                        }
                    }
                    Match house = houseRegex.Match(html);
                    if (house.Success) {
                        string h = FindEntry(html, house);
                        this.house = RemoveJunk(h.Split('(')[0]);
                    }
                    Match guild = guildRegex.Match(html);
                    if (guild.Success) {
                        Match guildname = linkNameRegex.Match(FindEntry(html, guild));
                        if (guildname.Success) {
                            this.guild = RemoveJunk(guildname.Groups[1].Value);
                        }
                    }
                    Match world = worldRegex.Match(html);
                    if (world.Success) {
                        this.world = RemoveJunk(FindEntry(html, world));
                    }

                    if (this.promoted) {
                        this.premium = true;
                    } else {
                        Regex premiumRegex = new Regex(String.Format(baseRegex, "Account Status:"));
                        Match premium = premiumRegex.Match(html);
                        if (premium.Success) {
                            this.premium = FindEntry(html, premium).ToLower().Contains("premium");
                        }
                    }

                    Regex deathRegex = new Regex("<td[^>]*>((?:Killed|Died|Slain|Crushed)[^\n]+)");
                    int startIndex = 0;
                    Match m;
                    while ((m = deathRegex.Match(html, startIndex)).Success) {
                        string deathString = html.Substring(m.Groups[1].Index).Split(new string[] { "</td>" }, StringSplitOptions.None)[0];
                        this.recentDeaths.Add(RemoveJunk(deathString));
                        startIndex += (m.Groups[1].Index - startIndex) + deathString.Length;
                    }

                    additionalInfo = true;
                }
                return true;
            } catch {
                return false;
            }
        }

        public int SharedLevelMin() { return (level * 2) / 3; }
        public int SharedLevelMax() { return (level * 3) / 2; }
    }   
}

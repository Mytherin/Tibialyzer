
import urllib.request
import sqlite3
import base64
import re



mounts = ["Widow_Queen", "Racing_Bird", "War_Bear", "Black_Sheep", "Midnight_Panther", "Draptor", "Titanica", "Tin_Lizzard", "Blazebringer", "Rapid_Boar", "Stampor", "Undead_Cavebear", "Crystal_Wolf", "Dromedary", "Kingly_Deer", "Donkey", "King_Scorpion", "Tamed_Panda", "Tiger_Slug", "Uniwheel", "Rented_Horse_(A)", "Rented_Horse_(B)", "Rented_Horse_(C)", "Armoured_War_Horse", "War_Horse", "Lady_Bug", "Manta_Ray", "Shadow_Draptor", "Gnarlhound", "Dragonling", "Magma_Crawler", "Ironblight", "Crimson_Ray", "Steelbeak", "Water_Buffalo", "Tombstinger", "Platesaurian", "Ursagrodon", "Hellgrip", "Noble_Lion", "Desert_King", "Shock_Head", "Walker", "Azudocus", "Carpacosaurus", "Death_Crawler", "Flamesteed", "Jade_Lion", "Jade_Pincer", "Nethersteed", "Tempest", "Winter_King", "Blackpelt", "Shadow_Hart", "Black_Stag", "Emperor_Deer", "Flying_Divan", "Magic_Carpet", "Floating_Kashmir", "Doombringer", "Tundra_Rambler", "Highland_Yak", "Glacier_Vagabond", "Golden_Dragonfly", "Steel_Bee", "Copper_Fly", "Hailstorm_Fury", "Poisonbane", "Siegebreaker", "Woodland_Prince", "Glooth_Glider", "Ringtail_Wacoon", "Night_Wacoon", "Emerald_Wacoon", "Flitterkatzen", "Venompaw", "Batcat", "Sea Devil", "Coralripper", "Plumfish", "Gorongra", "Noctungra", "Silverneck", "Rented_Horse_(Recruiter)", "Slagsnare", "Nightstinger", "Razorcreep", "Rift_Runner", "Nightdweller", "Frostflare", "Cinderhoof", "Bloodcurl", "Leafscuttler", "Mouldpincer", "Sparkion", "Swamp_Snapper", "Mould_Shell", "Reed_Lurker", "Neon_Sparkid", "Vortexion", 'Ivory_Fang', 'Shadow_Claw', 'Snow_Pelt']
outfits = ["Citizen", "Hunter", "Mage", "Knight", "Nobleman", "Summoner", "Warrior", "Barbarian", "Druid", "Wizard", "Oriental", "Pirate", "Assassin", "Beggar", "Shaman", "Norseman", "Jester", "Brotherhood", "Nightmare", "Demon_Hunter", "Yalaharian", "Newly_Wed", "Warmaster", "Wayfarer", "Afflicted", "Elementalist", "Deepling", "Insectoid", "Entrepreneur", "Crystal_Warlord", "Soil_Guardian", "Demon", "Cave_Explorer", "Dream_Warden", "Jersey", "Glooth_Engineer", "Beastmaster", "Champion", "Conjurer", "Chaos_Acolyte", "Ranger", "Death_Herald", "Ceremonial_Garb", "Puppeteer", "Spirit_Caller", "Evoker", "Seaweaver", "Recruiter", "Sea_Dog", "Royal_Pumpkin", "Rift_Warrior", "Winter_Warden", "Philosopher", "Arena_Champion", 'Lupine_Warden']
genders = ["Male", "Female"]
database_file = "outfits.db"

conn = sqlite3.connect(database_file)
c = conn.cursor()

clean = True

if clean:
    c.execute("DROP TABLE IF EXISTS OutfitImages")
    c.execute("DROP TABLE IF EXISTS MountImages")

c.execute("CREATE TABLE IF NOT EXISTS OutfitImages(name STRING, male BOOLEAN, image BLOB)")
c.execute("CREATE TABLE IF NOT EXISTS MountImages(name STRING, image BLOB)")

def getImage(url):
    response = urllib.request.urlopen(url)
    base64Data = str(response.read())
    base64String = base64Data.split("data:image/png;base64,")[1].split("</pre>")[0]
    base64String = re.sub(r"\s+", "", base64String, flags=re.UNICODE)
    if base64String[0] == '\\' and base64String[1] == 'n':
        base64String = base64String[2:]
    return base64.b64decode(base64String)

try:
    for mount in mounts:
        c.execute("SELECT name FROM MountImages WHERE name=?", (mount,))
        if len(c.fetchall()) == 0:
            url = "http://tibia.wikia.com/index.php?title=Outfiter/Mount/%s&action=raw" % (mount.replace(' ', '%20'))
            print(mount)
            imgdata = getImage(url)
            c.execute("INSERT INTO MountImages (name, image) VALUES (?,?)", (mount, imgdata))
       
    for outfit in outfits:
        for gender in genders:
            print(outfit, gender)
            c.execute("SELECT name FROM OutfitImages WHERE name=? AND male=?", (outfit, 1 if gender == "Male" else 0))
            if len(c.fetchall()) == 0:
                url = "http://tibia.wikia.com/index.php?title=Outfiter/%s/%s&action=raw" % (gender, outfit.replace(' ', '%20'))
                imgdata = getImage(url)
                c.execute("INSERT INTO OutfitImages (name, male, image) VALUES (?,?,?)", (outfit, 1 if gender == "Male" else 0, imgdata))
except:
    conn.commit()
    raise


conn.commit()




import json
import sqlite3

database_file = 'database.db'
database_file = 'E:\\Github Projects\\Tibialyzer4\\Tibialyzer\\Database\\database.db'
house_file = "houses.json"
guildhalls_file = "guildhalls.json"

conn = sqlite3.connect(database_file)
c = conn.cursor()

start_x = 124 * 256
start_y = 121 * 256

c.execute("DROP TABLE IF EXISTS Houses")
c.execute("CREATE TABLE Houses(id INTEGER, name STRING, city STRING, x INTEGER, y INTEGER, z INTEGER, sqm INTEGER, beds INTEGER, guildhall BOOLEAN)")

f = open(house_file)
houses = json.load(f)
f.close()

for house in houses['Houses']:
	id = house['Tibia_ID']
	name = house['Name']
	beds = house['Beds']
	x = house['PosX'] - start_x 
	y = house['PosY'] - start_y
	z = house['PosZ']
	sqm = house['Size']
	city = house['TownName']
	guildhall = False
	c.execute('INSERT INTO Houses (id,name,city,x,y,z,sqm,beds,guildhall) VALUES (?,?,?,?,?,?,?,?,?)', (id,name,city,x,y,z,sqm,beds,guildhall))


f = open(guildhalls_file)
houses = json.load(f)
f.close()

for house in houses['Houses']:
	id = house['Tibia_ID']
	name = house['Name']
	beds = house['Beds']
	x = house['PosX'] - start_x 
	y = house['PosY'] - start_y
	z = house['PosZ']
	sqm = house['Size']
	city = house['TownName']
	guildhall = True
	c.execute('INSERT INTO Houses (id,name,city,x,y,z,sqm,beds,guildhall) VALUES (?,?,?,?,?,?,?,?,?)', (id,name,city,x,y,z,sqm,beds,guildhall))

conn.commit()

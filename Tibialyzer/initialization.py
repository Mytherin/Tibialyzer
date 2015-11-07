import nt as os
import re
settings = dict()
            
c.execute('CREATE TABLE IF NOT EXISTS RecentLootFiles(day INTEGER, hour INTEGER, minute INTEGER, message STRING);')
conn.commit()

ignore_stamp = 0


total_item_drops = dict()
total_experience_results = dict()
total_damage_results = dict()
total_commands = dict()
all_commands = dict()
new_items = list()
new_commands = list()
new_advances = list()
level_advances = list()
seen_logs = set()

start_location = 0
def search_chunk(chunk, item_drops,exp,damage_dealt,commands):
    global seen_logs
    latest = [x.split(':') for x in get_latest_timestamps(5, ignore_stamp)]
    latest = [int(x[0]) * 60 + int(x[1]) for x in latest]
    for log_message in chunk:
        t = log_message[0:5]
        hour = int(log_message[0:2])
        min = int(log_message[3:5])
        if get_stamp(hour, min) not in latest:
           continue
        if ':' in log_message[6:]:
            if len(log_message) > 14 and log_message[5:14] == ' Loot of ':
                if log_message in seen_logs: continue
                seen_logs = seen_logs.union(log_message)
                if t not in item_drops: item_drops[t] = list()
                list.append(item_drops[t], log_message)
            else:
                msg_split = log_message[6:].split(':', 1)
                command = msg_split[1]
                if '@' not in command: continue
                split_again = msg_split[0].split(' ')
                player = ""
                for sss in split_again: 
                    if '[' in sss: break
                    player = (player + " " if player != "" else "") + sss
                if t not in commands: commands[t] = list()
                list.append(commands[t], [player, command])
        elif len(log_message) > 17 and log_message[5:17] == ' You gained ':
            try:
                e = int(log_message[17:].split(' ')[0])
                if t not in exp: exp[t] = e
                else: exp[t] = exp[t] + e
            except: pass
        else:
            split = log_message.split(' ')
            if 'hitpoints' in split:
                try: d = int(split[split.index('hitpoints') - 1])
                except: continue
                if log_message[-12:] == 'your attack.': player = 'You'
                else: 
                    player = None
                    if 'by' not in split: continue
                    player = ' '.join(split[split.index('by') + 1:])[:-1]
                if player not in damage_dealt: damage_dealt[player] = dict()
                if t not in damage_dealt[player]: damage_dealt[player][t] = d
                else: damage_dealt[player][t] = damage_dealt[player][t] + d
            elif 'advanced' in split:
                if log_message[6:19] == 'You advanced ' and 'level' in log_message.lower() and log_message[-1] == '.' and log_message not in level_advances:
                    list.append(level_advances, log_message)
                    list.append(new_advances, log_message)



def item_preprocess(item):
    if item == 'nothing': return ('nothing', 0)
    if 'platinum coin' in item:
        if item == 'a platinum coin': return ('gold coin', 100)
        else: 
            try: return ('gold coin', 100 * int(re.search(r'\d+', item).group()))
            except: return ('nothing', 0)
    if 'crystal coin' in item:
        if item == 'a crystal coin': return ('gold coin', 10000)
        else: 
            try: return ('gold coin', 10000 * int(re.search(r'\d+', item).group()))
            except: return ('nothing', 0)
    if 'gold coin' in item:
        if item == 'a gold coin': return ('gold coin', 1)
        else: 
            try: return ('gold coin', int(re.search(r'\d+', item).group()))
            except: return ('nothing', 0)
    count = 1
    split = item.split(' ')
    item = ""
    for i in reversed(range(0, len(split))):
        if split[i] == 'a' or split[i] == 'an' or split[i].isdigit(): break
        item = split[i] + ' ' + item
    if split[i].isdigit(): count = int(split[i])
    item = item[:-1]
    if count > 1: 
        if item in plural_maps: 
            item = plural_maps[item]
        elif item[-1] == 's':
            item = item[:-1]
        else:  
            print "Warning %s not in plural map and item did not end with s" % item
    return (item, count)

def reset_loot():
    total_loot_results = dict()
    new_items = list()
    ignore_stamp = create_stamp()
    c.execute('DROP TABLE IF EXISTS RecentLootFiles')
    c.execute('CREATE TABLE IF NOT EXISTS RecentLootFiles(day INTEGER, hour INTEGER, minute INTEGER, message STRING);')
    conn.commit()
    
def current_timestamp():
    import datetime
    datetime = datetime.datetime.now()
    hour = datetime.hour
    minute = datetime.minute
    stamp = datetime.year * 400 + datetime.month * 40 + datetime.day
    return (stamp, hour, minute)

def get_day_stamp():
    import datetime
    datetime = datetime.datetime.now()
    return datetime.year * 400 + datetime.month * 40 + datetime.day

def parse_timestamp(message):
    if len(message) < 5: return None
    hour = int(message[:2])
    min = int(message[3:5])
    return (hour, min)

def create_stamp(hour = None, minute = None):
    if hour == None or minute == None:
        import datetime
        dt = datetime.datetime.now()
        if hour == None: hour = dt.hour
        if minute == None: minute = dt.minute
    return get_stamp(hour, minute)

def get_stamp(hour, minute):
    return hour * 60 + minute

def parse_message(message):
    try: 
        stamp = current_timestamp()[0]
        hour = int(message[:2])
        min = int(message[3:5])
        msg = message[6:]
        if hour > 23: raise Exception()
        if min > 59: raise Exception()
        return (stamp, hour, min, msg)
    except: 
        return (None, None, None, None)

def get_latest_timestamps(mins, stamp = None):
    import datetime
    timestamp = current_timestamp()
    hour = timestamp[1]
    minute = timestamp[2]
    entries = []
    for i in range(0, mins):
        hr = str(hour)
        min = str(minute) if minute >= 10 else '0%s' % minute
        list.append(entries, '%s:%s' % (hr, min))
        if get_stamp(hour, minute) == stamp: return entries
        minute = minute - 1
        if minute < 0:
            minute = 59
            hour = hour - 1
            if hour < 0:
                hour = 23
    return entries


item_regex = re.compile('[0-9]{1,2}:[0-9]{1,2} Loot of(?: a[n]?)? ([^:]*): ([^\n]*)')
def parse_loot_message(loot_message):
    loot_message = loot_message[6:]
    if ': ' not in loot_message: return (None, None)
    splits = loot_message.split(': ')
    creature = ""
    space_splits = splits[0].split(' ')
    for j in reversed(range(0, len(space_splits))):
        if space_splits[j] == 'of' or space_splits[j] == 'a' or space_splits[j] == 'an': break
        creature = space_splits[j] + " " + creature
    creature = creature[:len(creature) - 1] # strip final space
    return (creature, splits[1])

creature_cache = dict()
item_cache = dict()

def invalidate_item(name):
    if name in item_cache:
        del item_cache[name]

def invalidate_creature(name):
    if name in creature_cache:
        del creature_cache[name]


global_min_kills = 0
gold_display_type = 'platinum coin'
gold_display_value = 100
minimum_display_value = 500
def get_recent_drops(parameter):
    min_kills = global_min_kills
    only_creature = ""
    min_value = 0
    all_mode = False
    raw_mode = False
    try: min_value = int(parameter)
    except: 
        if parameter == "all":
            all_mode = True
        elif parameter == "raw":
            raw_mode = True
        else: only_creature = parameter
    recent_drops = dict()
    creature_kills = dict()
    c.execute('SELECT message FROM RecentLootFiles')
    item_drops = c.fetchall()
    skiplist = list()
    killdict = dict()
    if min_kills > 0:
        for item in item_drops:
            msg = item[0]

            (creature, matches) = parse_loot_message(msg)
            if creature == None: continue
            if creature not in killdict: killdict[creature] = 0
            killdict[creature] = killdict[creature] + 1
        for creature in killdict.keys():
            if killdict[creature] < min_kills: 
                list.append(skiplist, creature)
    for item in item_drops:
        msg = item[0]
        (creature, matches) = parse_loot_message(msg)
        if creature == None: continue
        if (only_creature != '' and creature != only_creature) or (creature in skiplist): continue
        items = matches.split(', ')
        for item in items:
            (actual_item, count) = item_preprocess(item)
            if actual_item not in recent_drops: recent_drops[actual_item] = 0
            recent_drops[actual_item] = recent_drops[actual_item] + count
        if creature == 'skin': continue
        if creature not in creature_kills: creature_kills[creature] = 0
        creature_kills[creature] = creature_kills[creature] + 1
    return_creatures = list()
    for creature in creature_kills.keys():
        if creature == 'skin': continue
        if creature.lower() not in creature_cache:
            c.execute('SELECT * FROM Creatures WHERE LOWER(name)="%s"' % creature)
            creature_information = c.fetchall()
            creature_cache[creature.lower()] = creature_information
        creature_information = creature_cache[creature.lower()]
        if len(creature_information) == 0: continue
        list.append(return_creatures, (list(creature_information[0]), creature_kills[creature]))
    return_items = list()
    extra_gold_count = 0
    for item in recent_drops.keys():
        item_name = item
        if item == 'gold coin': 
            item_name = gold_display_type
            if recent_drops[item] < gold_display_value: continue
            recent_drops[item] = recent_drops[item] / gold_display_value
        if item_name.lower() not in item_cache:
            c.execute('SELECT id, name, actual_value, vendor_value, stackable, capacity, category, image, discard, convert_to_gold, look_text FROM Items WHERE LOWER(name)="%s"' % item_name)
            item_information = c.fetchall()
            item_cache[item_name.lower()] = item_information
        item_information = item_cache[item_name.lower()]
        if len(item_information) == 0: continue
        item_value = 0
        vendor_value = 0
        actual_value = 0
        try: vendor_value = int(item_information[0][3])
        except: pass
        try: actual_value = int(item_information[0][2])
        except: pass
        item_value = max(vendor_value, actual_value)
        convert_to_gold = item_information[0][9]
        discard = item_information[0][8]
        if (not all_mode) and discard: continue
        if (not raw_mode) and convert_to_gold: 
            extra_gold_count = extra_gold_count + vendor_value
            continue
        list.append(return_items, [list(item_information[0]), recent_drops[item], recent_drops[item] * item_value])
    found_gold = -1
    for i in range(0, len(return_items)):
        if return_items[i][0][1] == gold_display_type: found_gold = i
    if found_gold >= 0:
        return_items[found_gold][1] += extra_gold_count / gold_display_value
        return_items[found_gold][2] += (extra_gold_count / gold_display_value) * gold_display_value
    elif extra_gold_count > gold_display_value:
        item_name = gold_display_type
        c.execute('SELECT id, name, actual_value, vendor_value, stackable, capacity, category, image, discard, convert_to_gold, look_text FROM Items WHERE LOWER(name)="%s"' % item_name)
        item_information = c.fetchall()
        list.append(return_items, [list(item_information[0]), extra_gold_count / gold_display_value, (extra_gold_count / gold_display_value) * gold_display_value])
         
    return_creatures = sorted(return_creatures, key=lambda x: x[1], reverse=True)
    return_items = sorted(return_items, key=lambda x: x[2], reverse=True)
    return (return_creatures, return_items)

def read_settings(settings_file):
    current_setting = 'default'
    global settings 
    settings = dict()
    with open(settings_file) as f:
        for line in f:
            if line[0] == '@': current_setting = line[1:-1]
            else: 
                if current_setting not in settings: settings[current_setting] = list()
                list.append(settings[current_setting], line.rstrip())
    if 'Names' not in settings: settings['Names'] = list()

def write_settings(settings_file):
    with open(settings_file, 'w+') as f:
        for key in settings.keys():
            f.write('@' + str(key) + '\n')
            for line in settings[key]:
                f.write(line + '\n')

def save_log(log_file):
    c.execute('SELECT message FROM RecentLootFiles');
    messages = [x[0] for x in c.fetchall()]
    f = open(log_file, 'w+')
    for message in messages:
        f.write(message + '\n')
    f.close()

def load_log(log_file):
     c.execute('DELETE FROM RecentLootFiles')
     f = open(log_file, 'r')
     day = 0
     for line in f:
        try:
            message = line.rstrip()
            hour = int(message[0:2])
            minute = int(message[3:5])
            c.execute('INSERT INTO RecentLootFiles (day, hour, minute, message) VALUES (?,?,?,?)', [day, hour, minute, message])
        except: pass
     conn.commit()
     f.close()

def insert_skin(item_name):
    stamp = get_latest_timestamps(1)[0]
    message = stamp + " Loot of a skin: " + item_name
    hour = int(message[0:2])
    minute = int(message[3:5])
    day = 0
    c.execute('INSERT INTO RecentLootFiles (day, hour, minute, message) VALUES (?,?,?,?)', [day, hour, minute, message])
    conn.commit()

def delete_logmessage(parameter):
    creature = ""
    kill_threshold = 0
    try: kill_threshold = int(parameter)
    except: creature = parameter
    c.execute('SELECT day, hour, minute, message FROM RecentLootFiles')
    result = c.fetchall()
    creature_kills = dict()
    if kill_threshold > 0:
        for row in result:
            (creature, matches) = parse_loot_message(row[3])
            if creature not in creature_kills: creature_kills[creature] = 0
            creature_kills[creature] = creature_kills[creature] + 1
    delete_rows = list()
    for row in result:
        (creature, matches) = parse_loot_message(row[3])
        if creature == None or creature == 'skin': continue
        if parameter == creature:
            list.append(delete_rows, row)
        elif creature in creature_kills and creature_kills[creature] < kill_threshold:
            list.append(delete_rows, row)
    for row in delete_rows:
        c.execute('DELETE FROM RecentLootFiles WHERE day=? and hour=? and minute=? and message=?', row)
    conn.commit()

def set_gold_ratio(ratio):
    c.execute('SELECT id, actual_value, vendor_value, capacity FROM Items')
    result = c.fetchall()
    for item in result:
        id = item[0]
        value = max(0, max(item[1], item[2]))
        capacity = float(item[3])
        if capacity == 0: continue
        goldratio = value / capacity
        if goldratio < ratio:
            c.execute('UPDATE Items SET discard=1 WHERE id=?', [id])
        else: c.execute('UPDATE Items SET discard=0 WHERE id=?', [id])
    conn.commit();

def set_convert_ratio(ratio, stackable):
    c.execute('SELECT id, actual_value, vendor_value, capacity FROM Items WHERE stackable=?', [stackable])
    result = c.fetchall()
    for item in result:
        id = item[0]
        value = max(0, max(item[1], item[2]))
        capacity = float(item[3])
        if capacity == 0: continue
        goldratio = value / capacity
        if goldratio < ratio: c.execute('UPDATE Items SET convert_to_gold=1 WHERE id=?', [id])
        else: c.execute('UPDATE Items SET convert_to_gold=0 WHERE id=?', [id])
    conn.commit();


dps = dict()
for player in damage_dealt:
    if player not in total_damage_results:
        total_damage_results[player] = dict()
    for t in damage_dealt[player]:
        if t not in total_damage_results[player]: total_damage_results[player][t] = damage_dealt[player][t]  
        else: total_damage_results[player][t] = max(damage_dealt[player][t], total_damage_results[player][t])

for player in damage_dealt:
    damage = 0
    for t in get_latest_timestamps(15):
        if t in total_damage_results[player]: damage = damage + total_damage_results[player][t]
    dps[player] = damage
    
for t in exp:
    if t not in total_experience_results: total_experience_results[t] = exp[t]  
    else: total_experience_results[t] = max(exp[t], total_experience_results[t])

exph = 0
for t in get_latest_timestamps(15):
    if t in total_experience_results: exph = exph + total_experience_results[t]
exph = exph / 15.0 * 60.0

new_commands = []
for t in commands:
    if t not in total_commands: total_commands[t] = list()
    if len(commands[t]) > len(total_commands[t]):
        unseen_commands = list()
        commands_list = total_commands[t][:]
        for command in commands[t]:
            if command not in total_commands[t]:
                unseen_commands.append(command);
                player = command[0]
                cmd = command[1]
                if player in settings['Names']:
                    list.append(new_commands, cmd)
            else: 
                del total_commands[t][total_commands[t].index(command)]
        total_commands[t] = commands_list + unseen_commands


new_items = list()
for t in item_drops:
    if t not in total_item_drops: total_item_drops[t] = list()
    if len(total_item_drops[t]) < len(item_drops[t]): 
        stamp = get_day_stamp()
        hour = int(t[:2])
        min = int(t[3:])
        for msg in item_drops[t]:
            if msg not in total_item_drops[t]:
                message = msg[14:]
                if ':' not in message: continue
                matches = message.split(':')
                creature = matches[0]
                if creature[0][0] == 'a': creature = ' '.join(creature.split(' ')[1:])
                items = matches[1].split(', ')
                print items
                for item in items:
                    (actual_item, count) = item_preprocess(item)
                    # check if the item exists in the database
                    c.execute("SELECT id, MAX(actual_value, vendor_value) FROM Items WHERE LOWER(name)=\"%s\"" % actual_item)
                    values = c.fetchall()
                    if len(values) == 0: 
                        print "Warning: Could not find item %s in the database!" % actual_item
                        continue
                    else: 
                        itemid = values[0][0]
                        itemvalue = 0 if values[0][1] == '' else values[0][1]
                        list.append(new_items, [creature.title(), actual_item, itemvalue])
        c.execute('DELETE FROM RecentLootFiles WHERE day=%s AND hour=%s AND minute=%s' % (stamp, hour, min))
        for msg in item_drops[t]:
            c.execute('INSERT INTO RecentLootFiles VALUES (?,?,?,?)', (stamp, hour, min, msg))
        total_item_drops[t] = item_drops[t]
    conn.commit()


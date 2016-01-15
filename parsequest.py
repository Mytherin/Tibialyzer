
cities = ["ab'dendriel", "ankrahmun", "carlin", "darashia", "edron", "kazordoon", "liberty bay", "port hope", "svargrond", "thais", "venore", "yalahar"]

import re

#.replace('&quot;','"')
bracketRegex = re.compile('\\[\\[([^]]+)\\]\\]')
numberRegex = re.compile('([0-9]+[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*[,.]?[0-9]*)')


def parseQuest(title, attributes, c, rewardItems, questDangers, getURL):
    name = title
    if 'name' in attributes:
        name = attributes['name']
    lvl = 0
    if 'lvl' in attributes:
        try: lvl = int(numberRegex.search(attributes['lvl']).groups()[0])
        except: pass
    premium = False
    if 'premium' in attributes:
        premium = attributes['premium'].strip().lower() == 'yes'
    legend = None
    if 'legend' in attributes and attributes['legend'] != None:
        legend = attributes['legend'].replace('[', '').replace(']', '').replace('&quot;','"').replace('&amp;','&')
    city = None
    if 'location' in attributes:
        loclow = attributes['location'].lower()
        for candidatecity in cities:
            if candidatecity in loclow:
                city = candidatecity
                break
    if legend == None and city == None:
        return False
    c.execute('INSERT INTO Quests (title, name, minlevel, premium, city, legend) VALUES (?,?,?,?,?,?)', (title, name, lvl, premium, city, legend))
    questid = c.lastrowid
    if 'reward' in attributes:
        index = 0
        rewardItems[questid] = list()
        while True:
            match = bracketRegex.search(attributes['reward'][index:])
            if match == None: break
            rewardItems[questid].append(match.groups()[0])
            index += match.end()
    if 'dangers' in attributes:
        index = 0
        questDangers[questid] = list()
        while True:
            match = bracketRegex.search(attributes['dangers'][index:])
            if match == None: break
            questDangers[questid].append(match.groups()[0])
            index += match.end()
    return True
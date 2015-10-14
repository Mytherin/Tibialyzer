
music_player_path = 'C:/Program Files (x86)/foobar2000/foobar2000.exe'
music_path = 'D:/Browser Downloads'

import nt
import os

command = '"%s" ' % music_player_path
directories = [music_path]
files = nt.listdir(music_path)
for f in files:
    if os.path.isdir(music_path + '/' + f):
        list.append(directories, music_path + '/' + f)

split = _parameter.split(' ')
from random import shuffle
shuffle(directories)
for dir in directories:
    files = nt.listdir(dir)
    for f in files:
        if not os.path.isdir(f) and '.mp3' in f:
            match = True
            if _parameter != "all":
                for spl in split:
                    if spl not in f.lower(): match = False
            if match:
                new_command = command + ' "%s"' % (dir + '\\' + f)
                if len(new_command) < 32767:
                    command = new_command

if command != '"%s" ' % music_player_path:
    nt.system(command)

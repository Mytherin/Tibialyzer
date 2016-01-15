# This file contains operations for coordinate conversion
# We convert from tibia wikis weird x/y system to floating numbers in the range of 0-1

start_x = 124 * 256
start_y = 121 * 256

def convert_position(pos, min):
    if '.' not in pos: pos = pos + '.0'
    if pos[len(pos)-1] == '.': pos = pos + '0'
    split = [int(x) for x in str(pos).split('.')]
    return split[0] * 256 + split[1] - min

def convert_x(x_pos):
    return convert_position(x_pos, start_x)

def convert_y(y_pos):
    return convert_position(y_pos, start_y)

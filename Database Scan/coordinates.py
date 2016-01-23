
# Copyright 2016 Mark Raasveldt
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#   http://www.apache.org/licenses/LICENSE-2.0
# 
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
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

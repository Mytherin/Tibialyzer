
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

# This file contains various operations to perform on images
# Note that the operations aren't very cleanly implemented; they just call the imagemagick library on the shell in the tmp directory 
# (and as such these only work on a system with imagemagick installed). These files are normally run on Ubuntu 15.04, but will likely work
# on any Linux OS with imagemagick installed. 

def crop_image(image_binary):
    path = '/tmp/uncropped_image'
    f = open(path, 'wb')
    f.write(image_binary)
    f.close()
    if gif_is_animated(path):
        return crop_image_gif(path)
    else:
        return crop_image_gif(path)

def crop_image_png(path):
    import os
    os.system('convert %s -trim +repage /tmp/cropped_image > /dev/null 2>&1' % (path))
    f = open('/tmp/cropped_image', 'rb')
    img = f.read()
    f.close()
    return img

def crop_image_gif(path):
    import re
    import os
    imgsize = image_get_size(path)
    os.system('rm -r /tmp/converttest')
    os.system('mkdir /tmp/converttest')
    os.system('convert -coalesce %s /tmp/converttest/convert.png  > /dev/null 2>&1' % path)
    minx, miny, maxx, maxy = (10000, 10000, 0, 0)
    for f in os.listdir('/tmp/converttest'):
        o = os.popen('convert /tmp/converttest/%s -trim -verbose /tmp/converttest/%s 2>&1' % (f, f))
        output = o.read()
        match = re.search('([0-9]+)x([0-9]+) ', output)
        if match == None:
            continue
        width = int(match.groups()[0])
        height = int(match.groups()[1])
        match = re.search(' [0-9]+x[0-9]+[+]([0-9]+)[+]([0-9]+)', output)
        if match == None:
            continue
        x = int(match.groups()[0])
        y = int(match.groups()[1])
        if x < minx: minx = x
        if y < miny: miny = y
        if x + width > maxx: maxx = x + width
        if y + height > maxy: maxy = y + height
    os.system('convert %s -background transparent -crop %sx%s+%s+%s +repage /tmp/cropped_image.gif' % 
        (path, min(maxx, imgsize[0]) - minx,min(maxy, imgsize[1]) - miny, minx, miny))
    #os.system('gifsicle --crop %s,%s-%s,%s --output /tmp/cropped_image.gif %s' % (minx,miny,min(maxx, imgsize[0]),min(maxy, imgsize[1]), path))
    f = open('/tmp/cropped_image.gif', 'rb')
    img = f.read()
    f.close()
    return img

def properly_crop_item(image_binary):
    import re
    import os
    f = open('/tmp/uncropped_item.gif', 'wb')
    f.write(image_binary)
    f.close()
    imgsize = image_get_size('/tmp/uncropped_item.gif')
    if imgsize[0] > 32 or imgsize[1] > 32:
        os.system('rm -r /tmp/converttest')
        os.system('mkdir /tmp/converttest')
        os.system('convert -coalesce /tmp/uncropped_item.gif /tmp/converttest/convert.png  > /dev/null 2>&1')
        minx, miny, maxx, maxy = (10000, 10000, -1, -1)
        f = 'convert.png' if 'convert.png' in os.listdir('/tmp/converttest') else 'convert-0.png'
        o = os.popen('convert /tmp/converttest/%s -trim -verbose /tmp/converttest/%s 2>&1' % (f, f))
        output = o.read()
        match = re.search('([0-9]+)x([0-9]+) ', output)
        width = int(match.groups()[0])
        height = int(match.groups()[1])
        match = re.search(' [0-9]+x[0-9]+[+]([0-9]+)[+]([0-9]+)', output)
        if match == None:
            print(output)
            exit()
        x = int(match.groups()[0])
        y = int(match.groups()[1])
        centerx, centery = (int(x + width / 2), int(y + height / 2))
        minx, miny = (min(max(centerx - 16, 0), imgsize[0] - 32), min(max(centery - 16, 0), imgsize[1] - 32))
        maxx, maxy = (minx + 32, miny + 32)
        os.system('convert /tmp/uncropped_item.gif -background transparent -gravity center -crop %sx%s+%s+%s +repage -extent 32x32 /tmp/cropped_image_item' % 
            (32, 32, minx, miny))
        #os.system('gifsicle --crop %d,%d-%d,%d --output /tmp/cropped_image_item /tmp/uncropped_item.gif' % (minx,miny,maxx,maxy))
    elif imgsize[0] < 32 or imgsize[1] < 32:
        os.system('convert /tmp/uncropped_item.gif -background transparent -gravity center -extent 32x32 /tmp/cropped_image_item')
    else: 
        minx,miny,maxx,maxy = (0,0,32,32)
        os.system('convert /tmp/uncropped_item.gif -background transparent /tmp/cropped_image_item')
        #os.system('convert /tmp/uncropped_item.gif -background transparent -crop %sx%s+%s+%s +repage -extent 32x32 /tmp/cropped_image_item' % (32, 32, 0, 0))
        #os.system('gifsicle --crop %d,%d-%d,%d --output /tmp/cropped_image_item /tmp/uncropped_item.gif' % (minx,miny,maxx,maxy))

    newsize = image_get_size('/tmp/cropped_image_item')
    if newsize[0] != 32 or newsize[1] != 32:
        print('Incorrect conversion: Image of size', imgsize, 'was converted to an image of size', newsize, '. The resulting image must be of size (32, 32).')
        exit()
    f = open('/tmp/cropped_image_item', 'rb')
    img = f.read()
    f.close()
    return img

def image_get_size(path):
    from PIL import Image
    img = Image.open(path)
    return(img.size)

def gif_is_animated(path):
    from PIL import Image
    gif = Image.open(path)
    try:
        gif.seek(1)
    except EOFError:
        return False
    else:
        return True

def convert_to_png(image_binary):
    import os
    if os.path.isfile('/tmp/convert.gif'): os.remove('/tmp/convert.gif')
    if os.path.isfile('/tmp/convert.png'): os.remove('/tmp/convert.png')
    #os.remove('/tmp/convert.png')
    f = open('/tmp/convert.gif', 'wb')
    f.write(image_binary)
    f.close()
    if gif_is_animated('/tmp/convert.gif'): return image_binary
    os.system('convert -verbose -coalesce /tmp/convert.gif /tmp/convert.png > /dev/null 2>&1')
    f = open('/tmp/convert.png', 'rb')
    img = f.read()
    f.close()
    return img


stream = _parameter
default_quality = 'high'

import nt
nt.system('start "" /B livestreamer twitch.tv/%s %s' % (stream, default_quality))


stream = _parameter
default_quality = 'high'

import nt
import subprocess
CREATE_NO_WINDOW = 0x08000000
subprocess.call('livestreamer twitch.tv/%s %s' % (stream, default_quality), creationflags=CREATE_NO_WINDOW)

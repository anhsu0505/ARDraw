Unity3D iOS/Android/Standalone plugin

Microphone Recorder Wrapper

---------------------------------------------------------------------------
Description: 
This plugin works on iOS, Android and Standalone (fully tested)

Features:
- Record audio from microphone
- Implementation of endless recording
- Pause and Unpause recording process
- Save recorded audioclip as *.wav file in Application.persistentDataPath (changable)
- Rename and Delete *.wav files
- Load AudioClip from file and play in any AudioSourse
- Load file names from saved directory
- Full sources included
- Ready-to-go asset. Easily deployable in existing project.
- Developed with love <3

Full example of using included in package as Demo scene.

Check DemoController.cs for more info.

Support: kv@siberian.pro

---------------------------------------------------------------------------
Version Changes:
1.0:
	- Initial version.
2.0:
    - Fixed some bugs
	- Added check of input devices on Awake
3.0
    - Timers fixes
3.1
    - Fixed distortion bug on Samsung
	- Added automatic frequency selection for recording device on Awake
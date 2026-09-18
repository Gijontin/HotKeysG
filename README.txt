-------------------------------------------------------------- IN THEORY -----------------------------------------------------------------
This program, if compiled (and not installed, no installation feature yet anyway), should be compatible all the way back to Windows 2000.
------------------------------------------------------------------------------------------------------------------------------------------

TL;DR:
- Just a barebones program for Windows >ONLY< that allow for custom keybinds to launch/open any files your PC can read.

  You'll only find the program accessible through the TrayIcon menu (the little tab with programs usually next to the time/date display on the taskbar)
  which, by righting-clicking and then selecting the "Configure Settings" option will take you the hotkey binding option.

DESIGN PHILOSOPHY(?):
- The idea is to have a light-weight program that launches on startup which allows you to quickly start your most commonly used
  files and/or programs fast and without cluttering your desktop/taskbar with shortcut icons.

- ASS UI design (Aesthetic Simple Shell)
  ...is a feature not a flaw! - It aims to take up minimal computational memory

- Expected size per keybind would be around 0.25KB in Memory, meaning:
	- 10  Saved Hotkey Binds = ~2.5KB.
	- 50  Saved Hotkey Binds = ~12KB.
	- 100 Saved Hotkey Binds = ~25KB.

- If the idle state of the program ever goes beyond it's intended 7-10MB memory size, spit on it 
  and then delete it from your PC - it doesn't even deserve to be called ASS if that happens.

"INSTALLATION" GUIDE:
  1. Compile and then track down the .exe file, make a shortcut of said .exe-file
  2. Press Win+R and type in (without quotes): "shell:common startup"
  3. Drag your shortcut into that directory and your HotKeysG should be active on (you guessed it!) startup
     NOTE: admin prompt should popup, force Windows to submit to your adminstrative will!

OPTIONAL:
- Cry tears of joy at this feature complete program.

- Cry tears of anger at Discord and other Electron-based apps being utter pieces of shit to find 
  permanent pathing to... (have fun in the %localappdata% swamp)
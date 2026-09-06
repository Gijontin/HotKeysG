--------------------------------------------- IN THEORY --------------------------------------------
This program, if compiled (and not installed), should be compatible all the way back to Windows 2000.
-----------------------------------------------------------------------------------------------------

TL;DR:
- Just a barebones program for Windows >ONLY< that allow for custom keybinds to launch/open any files your PC can read.

  You'll only find the program accessible through the TrayIcon menu (the little tab with programs usually next to the time/date display on the taskbar)
  which, by righting-clicking and then selecting the "Configure Settings" option will take you the hotkey binding option.

DESIGN PHILOSOPHY(?):
- The idea is to have a light-weight program that launches on startup which allows you to quickly start your most commonly used
  files and/or programs fast and without cluttering your desktop/taskbar with shortcut icons.

- ASS UI design (Aesthetic Simple Shell)
...is a feature not a flaw! - It aims to take up minimal computational memory

- Expected size per keybind would be around 0.25KB RAM, meaning:
	- 10  Saved Hotkey Binds = ~2.5KB.
	- 50  Saved Hotkey Binds = ~12KB.
	- 100 Saved Hotkey Binds = ~25KB.

- If the idle state of the program ever goes beyond it's 7-10MB memory size, spit on it and then delete it from your PC - it doesn't even deserve to be called ASS if that happens.
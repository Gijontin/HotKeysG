--------------------------------------------------- IN THEORY ----------------------------------------------------------------
            This program, if compiled (and not installed), should be compatible all the way back to Windows 2000.
------------------------------------------------------------------------------------------------------------------------------

TL;DR

- Windows ONLY
  allows for custom keybinds to launch/open any files your PC is capable of launch/open.


- The only working keybinding combos are:

  Shift + ALT + Key( only letters A to Ö OR numbers 0 to 9 will work)
  Shift + CTRL + Key( only letters A to Ö OR numbers 0 to 9 will work)

  (the keybind limitation is for the purpose of having less conflicts with other programs and what not...)


- You'll only find the program accessible through the TrayIcon menu (the little tab with programs usually next to the 
  time/date display on the taskbar), which, by right-clicking and then selecting the "Configure Settings" option, will
  take you to the hotkey binding menu.

- NOTE: There are two things that really make this program shine:
        1. The launch on startup implementation
        2. The fact that you (should be able to) keybind anything
           - not only launch software but quickly access commonly used text files, images etc 
           - OR even make your own .bat script files and easily keybind them for more elaborate tasks
           
------------------------------------------------------------------------------------------------------------------------------

DESIGN PHILOSOPHY (?)

- The idea is to have a light-weight program that launches on startup which allows you to quickly start your most commonly
  used files and/or programs fast and without cluttering your desktop/taskbar with shortcut icons.

- ASS UI design (Aesthetic Simple Shell)
  ...is a feature not a flaw! It aims to take up minimal computational memory.

- Expected size per keybind would be around 0.25KB in memory, meaning:
    - 10  Saved Hotkey Binds = ~2.5KB
    - 50  Saved Hotkey Binds = ~12KB
    - 100 Saved Hotkey Binds = ~25KB

- If the idle state of the program ever goes beyond its intended 7–10MB memory size, spit on it and then delete it 
  from your PC — it doesn't even deserve to be called ASS if that happens.

------------------------------------------------------------------------------------------------------------------------------

"INSTALLATION" GUIDE

1. Compile and then track down the .exe file, make a shortcut of said .exe-file.
2. Press Win+R and type in (without quotes): shell:common startup
3. Drag your shortcut into that directory and your HotKeysG should be active on (you guessed it!) startup.
   NOTE: admin prompt should pop up — force Windows to submit to your administrative will!

------------------------------------------------------------------------------------------------------------------------------

OPTIONAL

- Cry tears of joy at this feature-complete program.
- Cry tears of anger at Discord and other Electron-based apps being utter pieces of shit to find permanent pathing to...
  (have fun in the %localappdata% swamp)
  
------------------------------------------------------------------------------------------------------------------------------

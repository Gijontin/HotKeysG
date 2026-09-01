/*

USE THIS FORM TO MAKE A CONFIGURATION SETTINGS GUI OPTION WHERE SAVING AND LOADING HOTKEY COMBOS ARE MANAGED

1. MAKE A PICK PROGRAM/FILE TO HOTKEY BUTTON
    LET IT DONE BY OPENING FILE EXPLORER AND EXTRACT PATH AND NAME FROM THE OBJECT THE USER PICKS

2. AFTER THAT THERE'S A SMALL INPUT WINDOW NEXT TO THE PROGRAM (DISPLAYED BY NAME AND MAYBE EVEN ITS FETCHED ICON) WHERE HOTKEYS ARE CHOSEN
    UNSURE IF HOTKEYS ARE CLICKED IN OR REGISTERED BY USER'S KEYBOARD INPUT (LATTER IS PREFERRED SO THEY ALSO GET A FEEL FOR IT IN THE PROCESS)

*/
namespace HotkeysG {
    public partial class Form2 : Form {
        
        //declare few variables for GUI

        //let SettingsManager.cs have most (if not all) of the functions used

        //init GUI elements with the constructor
        //&& MOST IMPORTANTLY:
            //Make sure you open this Form2 with the lambda call in Form1's trayIcon part
        
        public Form2() { //construct0r time baaaabeeeyyy
            
        }
    }
}
/*
LIST FOR SUPPOSEDLY SAFE COMBOS THAT WON'T CONFLICT WITH WINDOWS NOR VS CODE, CAN'T GUARANTEE SHIT FOR OTHER PROGRAMS:

----------------SHIFT ALT COMBO------------

Shift + Alt + Q
Shift + Alt + W
Shift + Alt + E
Shift + Alt + R
Shift + Alt + T
Shift + Alt + Y
Shift + Alt + U
Shift + Alt + O
Shift + Alt + P

Shift + Alt + S
Shift + Alt + D
Shift + Alt + G
Shift + Alt + H
Shift + Alt + J
Shift + Alt + K
Shift + Alt + L

Shift + Alt + Z
Shift + Alt + X
Shift + Alt + C
Shift + Alt + V
Shift + Alt + B
Shift + Alt + N
Shift + Alt + M

Shift + Alt + 0
Shift + Alt + 1
Shift + Alt + 2
Shift + Alt + 3
Shift + Alt + 4
Shift + Alt + 5
Shift + Alt + 6
Shift + Alt + 7
Shift + Alt + 8
Shift + Alt + 9

Shift + Alt + F1
Shift + Alt + F2
Shift + Alt + F3
Shift + Alt + F5
Shift + Alt + F6
Shift + Alt + F7
Shift + Alt + F8
Shift + Alt + F9
Shift + Alt + F10
Shift + Alt + F11
Shift + Alt + F12

Shift + Alt + -
Shift + Alt + =
Shift + Alt + [
Shift + Alt + ]
Shift + Alt + ;
Shift + Alt + '
Shift + Alt + ,
Shift + Alt + .
Shift + Alt + /

---------------CTRL ALT COMBO ------------------

Ctrl + Alt + Q
Ctrl + Alt + W
Ctrl + Alt + E
Ctrl + Alt + R
Ctrl + Alt + T
Ctrl + Alt + Y
Ctrl + Alt + U
Ctrl + Alt + O
Ctrl + Alt + P

Ctrl + Alt + S
Ctrl + Alt + D
Ctrl + Alt + G
Ctrl + Alt + H
Ctrl + Alt + J
Ctrl + Alt + K
Ctrl + Alt + L

Ctrl + Alt + Z
Ctrl + Alt + X
Ctrl + Alt + C
Ctrl + Alt + V
Ctrl + Alt + B
Ctrl + Alt + N
Ctrl + Alt + M

Ctrl + Alt + 0
Ctrl + Alt + 1
Ctrl + Alt + 2
Ctrl + Alt + 3
Ctrl + Alt + 4
Ctrl + Alt + 5
Ctrl + Alt + 6
Ctrl + Alt + 7
Ctrl + Alt + 8
Ctrl + Alt + 9

Ctrl + Alt + F1
Ctrl + Alt + F2
Ctrl + Alt + F3
Ctrl + Alt + F5
Ctrl + Alt + F6
Ctrl + Alt + F7
Ctrl + Alt + F8
Ctrl + Alt + F9
Ctrl + Alt + F10
Ctrl + Alt + F11
Ctrl + Alt + F12

*/
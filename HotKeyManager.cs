
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HotkeysG {
    public static class HotKeyManager {

    //IMPORTERAR LITE user32.dll KRIMS OF THE KRAMS
        //square brackets i C# betyder att det är en "attribute", typ metadata som ändrar hur saken under ska bete sig (tex att det är C/C++ kodad metod från en dll fil utanför C#)
        [DllImport("user32.dll")] //säger åt .net att importera C/C++ kod ifrån denna inbyggda DLL filen och metoden under refererar dll skiten (very cool)
        private static extern bool RegisterHotKey(
            IntPtr hWnd, //är vilket program som ska läsa av det, i mitt fall är det alltid HotKeysG fönster (Form1) som ska göra det AKA "this.Handle"
            int id, //id är till för varje unik keybind, dvs edge och git-bash kan inte dela id-värde (och inget annat program iheller för den delen)
            uint fsModifiers, //modifiers är bara alt/ctrl/shift/win(super) och de skickar bara signal att de är nedtryckta "Keys" agerar som en "tru trigger" och är tänkt att aldrig vara någon av modifier-knapparna
            Keys vk
        );
            //call ex: RegisterHotKey(this.Handle, 2, MOD_ALT | MOD_SHIFT, Keys.E);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(
            IntPtr hWnd,
            int id
        );

    //WRAP:AR IMPORTERNA SÅ JAG HAR FUNKTIONER SOM FUNKAR SÅ SOM JAG VILL...
        public static void RegKeybinds(Form1 form1, List<KeyBindings> list) {
            foreach (KeyBindings bind in list) {
                RegisterHotKey(form1.Handle, bind.ID, bind.winSignal1 | bind.winSignal2, bind.triggerKey & Keys.KeyCode); //viktigt att filtrera ut KeyCode här (också)
            }
        }
        public static void UnregKeybinds(Form1 form1, List<KeyBindings> list) {
            foreach (KeyBindings bind in list) {
                UnregisterHotKey(form1.Handle, bind.ID);
            }
        }
        public static void ReloadRegKeybinds(Form1 form1, List<KeyBindings> list) {
            UnregKeybinds(form1, list);
            RegKeybinds(form1, list);
        }
        public static void LaunchProgram(int id, List<KeyBindings> list) {
            try {
                ProcessStartInfo program = new ProcessStartInfo();
                    program.FileName = list[id].filePath;
                    program.UseShellExecute = true;         //den här bool:en behöver aktiveras(set to true) för att launch:a filformat som inte är .exe
                
                if (!string.IsNullOrEmpty(list[id].filMapp)) {
                    program.WorkingDirectory = list[id].filMapp;
                }
                
                Process.Start(program);
            } 
            catch {
                
            }

        }
    }
}
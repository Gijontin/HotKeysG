using System;
using System.Runtime.InteropServices; //ansvarar för att DllImport funkar
using System.Windows.Forms;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Configuration;
using System.Text.Json;
using System.ComponentModel.Design;

namespace HotkeysG {
    /*

    MED DLL OCH OVERRIDE WINDOWS SAKERNA SÅ ÄR DET NÅGOT I VS CODE SOM GÖR ATT BRACKET FORMATET FUCKAS, KANSKE

    HAR JAG TUR ÄR LÖSNINGEN ATT SEPARERA DLLIMPORT OCH OVERRIDE FUNKTIONERNA TILL EN ANNA .cs FIL...

    */
    public partial class Form1 : Form {
    //square brackets i C# betyder att det är en "attribute", typ metadata som ändrar hur saken under ska bete sig (tex att det är C/C++ kodad metod från en dll fil utanför C#)
        [DllImport("user32.dll")] //säger åt .net att importera C/C++ kod ifrån denna inbyggda DLL filen och metoden under refererar dll skiten
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
        //call ex: UnregisterHotKey(this.Handle, 1);

        public void RegKeybinds() {
            //foreach (KeyBindings bind in SettingsManager.loadedKB) {
            foreach (KeyBindings bind in settings.loadedKB) {
                RegisterHotKey(this.Handle, bind.ID, bind.winSignal1 | bind.winSignal2, bind.triggerKey);
            }
        }
        public void UnregKeybinds() {
            foreach (KeyBindings bind in settings.loadedKB) {
                UnregisterHotKey(this.Handle, bind.ID);
            }
        }
        //hexadecimalkoderna för modifier-flags i Windows för respektive keyboard knapp, de är mer windows-specifika kodade signaler från knapparna och inte registrering av knapptrycken...
        //har med hur windows läser bits istället för uint fsModifiers parametern i RegisterHotKey
        private const uint MOD_ALT = 0x0001; // NOT == Keys.Alt
        private const uint MOD_SHIFT = 0x0004; //NOT == Keys.Shift
        private const uint WM_HOTKEY = 0x0312; //typ signalen ditt program/fönster får när en valid key-kombo har tryckts (WndProc som fångar upp den)
        private NotifyIcon trayIcon;
        private SettingsManager settings;
        public Form1(){ //constructor (gör en osynlig winform app för att lätt komma åt hotkey funktionalitet i windows)
        //start form(app)
            InitializeComponent();
            settings = new SettingsManager();

        //Add as TrayIcon
            trayIcon = new NotifyIcon();
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;
            trayIcon.Text = "HotKeysG - Running";
            
            //TrayIcon menu (right click and get a "Close" option)
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Close", null, (se, e) => this.Close());
                trayIcon.ContextMenuStrip = menu;

        //Disable GUI 
            // (avnänd inte .Hide(), den stänger av efter en ruta poppar upp, ser fult och malware-igt ut)
            //this.Hide();
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;

        //Import keybind settings
            //SettingsManager
            //parse out the settings ID for the app(Windows) to know how many and which int ID's to listen to    
            RegKeybinds();

        }
    //WindowProcedure funktionen som lyssnar efter WM_HOTKEY som då är en return som signalerar att wParam har skickats ut, dvs en bekräftelse på att ett ID från RegisterHotKeys har tryckts ned och vilken
        protected override void WndProc(ref Message m) {
            /*
            det här är winform feature som då lyssnar på vad som händer, mus-movement, keybord tryck, window refresh etc

            det override gör att jag ändrar funktionalitet vad som händer i funktionen men behåller winforms inbyggda "lyssning" på events n shid
            basically moddar man en redan existernade funktion, orkar ännu inte läsa på hur den fungerar exakt och vad som "stannar" när man ändrar...

            windows skickar en miljon message (funktionens parameter) men detta gör att den bara filtrerar för att upptäcka en viss hotkey
            */
            if (m.Msg == WM_HOTKEY){
                int id = m.WParam.ToInt32();
                foreach (KeyBindings bind in settings.loadedKB){
                    if (bind.ID == id){
                        LaunchProgram(id);
                        break;
                    }
                }
            }

            //använder detta call:et för att låta WndProcs normala checks/funktioner hanteras ändå, if är bara ett filter för all the "noise"
            base.WndProc(ref m);
        }

        private void LaunchProgram(int id) {
            try {
                ProcessStartInfo program = new ProcessStartInfo();
                program.FileName = settings.loadedKB[id].filePath;
                if (!string.IsNullOrEmpty(settings.loadedKB[id].filMapp)) {
                    program.WorkingDirectory = settings.loadedKB[id].filMapp;
                }
                Process.Start(program);
            } 
            catch {
                
            }

        }
        protected override void OnFormClosing(FormClosingEventArgs e){
            //moddar denna biten så att Windows inte tror att programmet fortfarande "äger" keybind:en efter programmet stängs ned
            UnregKeybinds();

            //removes potential "ghost icon" remaining after closing software
            trayIcon.Visible = false;
            trayIcon.Dispose();

            //Ser till att onformclosing's standard processer fortsätter som vanligt utöver modden ovanför
            base.OnFormClosing(e);
        }
    }
}
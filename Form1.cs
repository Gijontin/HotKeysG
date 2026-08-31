using System;
using System.Runtime.InteropServices; //ansvarar för att DllImport funkar
using System.Windows.Forms;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Configuration;

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

        //hexadecimalkoderna för modifier-flags i Windows för respektive keyboard knapp, de är mer windows-specifika kodade signaler från knapparna och inte registrering av knapptrycken...
        //har med hur windows läser bits istället för uint fsModifiers parametern i RegisterHotKey
        private const uint MOD_ALT = 0x0001; // NOT == Keys.Alt
        private const uint MOD_SHIFT = 0x0004; //NOT == Keys.Shift
        private const uint WM_HOTKEY = 0x0312; //typ signalen ditt program/fönster får när en valid key-kombo har tryckts (WndProc som fångar upp den)
        private NotifyIcon trayIcon;

        /*
        Använder KeyBindings som en blueprint strukt just nu istället för en riktig data bas som user kan customize:a i
        Egentligen ska en funktion i KeyBindings eller detta objektet skapa egna ProcessStartInfo class som sedan matas in i
        LaunchProgram baserat på keybinds
        */
        private KeyBindings gitBash;
        private KeyBindings edge;

        public Form1(){ //constructor (gör en osynlig winform app för att lätt komma åt hotkey funktionalitet i windows)
        //start form(app)
            InitializeComponent();

            gitBash = new KeyBindings {
                ID = 1,
                filePath = @"C:\Program Files\Git\git-bash.exe",
                filMapp = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                winSignal1 = MOD_ALT,
                winSignal2 = MOD_SHIFT,
                triggerKey = Keys.T,
            };
            edge = new KeyBindings {
                ID = 2,
                filePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                filMapp = "",
                winSignal1 = MOD_ALT,
                winSignal2 = MOD_SHIFT,
                triggerKey = Keys.E,
            };

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

        //listen for hotkeys and execute
            //denna kompilerar för att man hämtat funktionen från user32.dll via [DllImport("<filnamn>)]...
            //TLDR; denna key kombon gör om signalen av alla tre till int id = 1, senare i WndProc moddar vi den till att bara koll efter id == 1
            RegisterHotKey(this.Handle, 1, gitBash.winSignal1 | gitBash.winSignal2, gitBash.triggerKey); // | fungerar som && i windows syntax, eller nått...
            RegisterHotKey(this.Handle, 2, MOD_ALT | MOD_SHIFT, Keys.E);

        }
    //WindowProcedure funktionen som lyssnar efter WM_HOTKEY som då är en return som bekräftar id't på någon av de registrerade RegisterHotKey() Keybindingsen
        protected override void WndProc(ref Message m) {
            /*
            det här är winform feature som då lyssnar på vad som händer, mus-movement, keybord tryck, window refresh etc

            det override gör att jag ändrar funktionalitet vad som händer i funktionen men behåller winforms inbyggda "lyssning" på events n shid
            basically moddar man en redan existernade funktion, orkar ännu inte läsa på hur den fungerar exakt och vad som "stannar" när man ändrar...

            windows skickar en miljon message (funktionens parameter) men detta gör att den bara filtrerar för att upptäcka en viss hotkey
            */
            if (m.Msg == WM_HOTKEY){
                int id = m.WParam.ToInt32();
                if (id == 1){
                    LaunchProgram();
                }
            }

            //använder detta call:et för att låta WndProcs normala checks/funktioner hanteras ändå, if är bara ett filter för all the "noise"
            base.WndProc(ref m);
        }

        private void LaunchProgram(){ //eget skit i C# 
        
            //Process.Start(@"C:\Program Files\Git\git-bash.exe"); //opens the process based on the .exe location

            //edge
            ProcessStartInfo edge = new ProcessStartInfo();
            edge.FileName = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
            Process.Start(edge);

            //bash
            ProcessStartInfo bash = new ProcessStartInfo();
            bash.FileName = @"C:\Program Files\Git\git-bash.exe";
            bash.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Process.Start(bash);
        }
        private void LaunchProgram(KeyBindings key) {
                
            }
        protected override void OnFormClosing(FormClosingEventArgs e){
            //moddar denna biten så att Windows inte tror att programmet fortfarande "äger" keybind:en efter programmet stängs ned
            UnregisterHotKey(this.Handle, 1);

            //removes potential "ghost icon" remaining after closing software
            trayIcon.Visible = false;
            trayIcon.Dispose();

            //Ser till att onformclosing's standard processer fortsätter som vanligt utöver modden ovanför
            base.OnFormClosing(e);
        }
    }
}
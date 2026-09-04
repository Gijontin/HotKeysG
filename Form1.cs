namespace HotkeysG {
    public partial class Form1 : Form {

        //hexadecimalkoderna för modifier-flags i Windows för respektive keyboard knapp, de är mer windows-specifika kodade signaler från knapparna och inte registrering av knapptrycken...
        //har med hur windows läser bits istället för uint fsModifiers parametern i RegisterHotKey
        private const uint MOD_ALT = 0x0001; //NOT the same as: Keys.Alt
        private const uint MOD_SHIFT = 0x0004; //NOT the same as: Keys.Shift
        private const uint WM_HOTKEY = 0x0312; //typ signalen ditt program/fönster får när en valid key-kombo har tryckts (WndProc som fångar upp den)
        
        private NotifyIcon trayIcon;
        private SettingsManager settings;
        public Form1(){ //constructor (gör en osynlig winform app för att lätt komma åt hotkey funktionalitet i windows)
        //start form(app)
            InitializeComponent();
            settings = new SettingsManager();

            //Import the ID for configured hotkeys så att man kan skriva en kod som fattar upp ID:na i WndProc
            //HandleCreated blir som en aktiv lyssnare efter ändringar och uppdaterar då "spontant" Form1 med RegKeybinds
            this.HandleCreated += (s, e) => HotKeyManager.ReloadRegKeybinds(this, settings.loadedKB); //bara WinForm32 quirk som kräver denna reloadgrejen här...

        //TrayIcon
            trayIcon = new NotifyIcon();
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;
            trayIcon.Text = "HotKeysG - Running";
            
            //TrayIcon menu (right click and get a "Close" option)
                ContextMenuStrip menu = new ContextMenuStrip();

            //"Knapp"-funktioner
                menu.Items.Add("Configure Settings", null, (se, e) => new Form2(this ,settings).Show());

                menu.Items.Add("Close", null, (se, e) => this.Close());
                trayIcon.ContextMenuStrip = menu;

        //Disable GUI 
            //this.Hide(); // (avnänd inte .Hide(), den stänger av efter en ruta poppar upp, ser fult och malware-igt ut)
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
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
                        HotKeyManager.LaunchProgram(id, settings.loadedKB);
                        //LaunchProgram(id);
                        break;
                    }
                }
            }

            //använder detta call:et för att låta WndProcs normala checks/funktioner hanteras ändå, if är bara ett filter för all the "noise"
            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e){
            //moddar denna biten så att Windows inte tror att programmet fortfarande "äger" keybind:en efter programmet stängs ned
            HotKeyManager.UnregKeybinds(this, settings.loadedKB);

            //removes potential "ghost icon" remaining after closing software
            trayIcon.Visible = false;
            trayIcon.Dispose();

            //Ser till att onformclosing's standard processer fortsätter som vanligt utöver modden ovanför
            base.OnFormClosing(e);
        }
    }
}
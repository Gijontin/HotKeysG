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
        private const int heightWnd = 360;
        private const int widthWnd = 640;

        //private Form1 huvudForm; //dålig idé, kommer nog ej använda...
        private SettingsManager _settings;
        private readonly int buttonHeight; //init in the constructor in a DPI friendly way, "readonly" closest I can get to declaring this variable a 'const'
        private Button addKnapp;
        private Button removeKnapp;
        private Button stängKnapp;
        private ListView programLista;
        private KeyBindings pathToObj(string path) {
            
            //skapa en liten prompt ruta som försvinner man angivet en Keys
            Keys tempKey = SettingsManager.CaptureKey(); //GLÖM EJ ATT DENNA SKAPAR EN LITEN MINI-FORM DEN MED
            
            //om user trycker 'Esc' så ska hela sparningsprocessen avbrytas
                KeyBindings kb = new KeyBindings {
                    ID = _settings.loadedKB.Count,
                    namn = Path.GetFileName(path),
                    filePath = path,
                    filMapp = Path.GetDirectoryName(path),
                    winSignal1 = 0x0001,
                    winSignal2 = 0x0004,
                    triggerKey = tempKey,
                };

            return kb;
        }
        private void taBortEnKeyBind() {
            try {
                var select = programLista.SelectedItems[0];
                var kb = (KeyBindings)select.Tag;

                _settings.loadedKB.Remove(kb);
                programLista.Items.Remove(select);
                _settings.reloadSetting();
                uppdateraProgramListan();
            } catch {
                
            }
        }
        private void uppdateraProgramListan() {
        
        //rensa FÖRST
            programLista.Items.Clear();
        
        //importera KeyBindings
            foreach (var prgm in _settings.loadedKB) {

                string modkeys = "Alt ";
                    if (prgm.winSignal2 == 0x0004) {
                        modkeys += "+ Shift";
                    } else {
                        modkeys += "+ Ctrl";
                    }

                var item = new ListViewItem(prgm.namn);            //Program/Filnamn
                    item.SubItems.Add($"{prgm.ID}");               //ID
                    item.SubItems.Add(prgm.filePath);              //Filväg
                    item.SubItems.Add(modkeys);                    //Modifier Keys
                    item.SubItems.Add(prgm.triggerKey.ToString()); //Key
                    item.Tag = prgm;
                    programLista.Items.Add(item);
            }
        }
        private void väljFiler() {
        //öppnar Window's File Explorer
            using (OpenFileDialog dlg = new OpenFileDialog()) {
                
            //filtrerar i explorer vilka filer du ser baserat på .filformat
                dlg.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";

            //om du inte väljer program/fil (tex trycker 'Cancel' eller stänger fönstret)
                if (dlg.ShowDialog() != DialogResult.OK) {
                    return;
                }

            //försök fylla i kb (program och dess hotkeys)
                KeyBindings kb;
                try {
                    kb = pathToObj(dlg.FileName);
                } 
                catch {
                    return; //tror inget felmeddelande behövs här
                }

            //Om användaren tryckt escape så avbryter vi sparandet och återgår till Settings rutan där 
            // man kan trycka 'Add' igen för ett nytt försök
                if (kb.triggerKey == Keys.Escape) {
                        return;
                }

            //Vid succee, spara allting och uppdatera/ladda om list(-or)
                _settings.loadedKB.Add(kb);
                _settings.sparaSettings();
                uppdateraProgramListan();
                //fungerar ej utan omstart vilket betyder meningslös placering av call:et
                //HotKeyManager.RegKeybinds(huvudForm, _settings.loadedKB);
            }
        }
        private void startaOmAppenEfterSettings(object s, FormClosedEventArgs e) {
            /*
                Hacky lösning för att få nya tillagda hotkeys att registrera utan att
                manuellt starta om programmet.
                
                Att fixa det snyggt involvera att felsöka varför "RegisterHotKey(form1.Handle, bind.ID, bind.winSignal1 | bind.winSignal2, bind.triggerKey);"
                varken funkar att kallas via Form2 settings eller som en omstarts-grej i början av Form1 med en handle(fönster)-change lyssnare:
                this.HandleCreated += (s, e) => HotKeyManager.ReloadRegKeybinds(this, settings.loadedKB);
            */
            Application.Restart();
        }
        
        public Form2(Form form1, SettingsManager settings) { //construct0r time baaaabeeeyyy
            
            _settings = settings;

            this.FormClosed += startaOmAppenEfterSettings; //hacky lösning för att registrera nya konfigurerade hotkeys

            buttonHeight = TextRenderer.MeasureText("A", this.Font).Height + 12;

            this.ClientSize = new Size(widthWnd, heightWnd);
            this.Text = "Settings";
            this.StartPosition = FormStartPosition.CenterScreen;

            //lock the appWindow-size
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;

        //(BUTTON)ADD PRGM/FILE W/ HOTKEY
            addKnapp = new Button();
            //general specs
                addKnapp.Text = "Add...";
                addKnapp.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                addKnapp.AutoSize = false;
                addKnapp.TextAlign = ContentAlignment.MiddleCenter;
                addKnapp.FlatStyle = FlatStyle.System;
                addKnapp.UseVisualStyleBackColor = true;
                addKnapp.TabIndex = 1;

            // size (same measurements as Close button)
                addKnapp.Size = new Size(
                    TextRenderer.MeasureText(addKnapp.Text, addKnapp.Font).Width + 40,
                    buttonHeight
                );

            // position (upper right corner with 2% padding)
                int padX = (int)(this.ClientSize.Width * 0.02);
                int padY = (int)(this.ClientSize.Height * 0.02);

                addKnapp.Location = new Point(
                    //this.ClientSize.Width - addKnapp.Width - padX,
                    padX,
                    this.ClientSize.Height - padY - addKnapp.Height
                );
            //funktion
                addKnapp.Click += (s, e) => väljFiler();
        //(BUTTON) REMOVE A KEYBOUND PROGRAM/FILE
            removeKnapp = new Button();
            //general specs
                removeKnapp.Text = "Remove";
                removeKnapp.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                removeKnapp.AutoSize = false;
                removeKnapp.TextAlign = ContentAlignment.MiddleCenter;
                removeKnapp.FlatStyle = FlatStyle.System;
                removeKnapp.UseVisualStyleBackColor = true;
                removeKnapp.TabIndex = 1;
            // size (same measurements as Close button)
                removeKnapp.Size = new Size(
                    TextRenderer.MeasureText(removeKnapp.Text, removeKnapp.Font).Width + 40,
                    buttonHeight
                );

                removeKnapp.Location = new Point(
                    (2 * padX) + addKnapp.Width,
                    this.ClientSize.Height - padY - removeKnapp.Height
                );
            //funktion
                removeKnapp.Click += (s, e) => taBortEnKeyBind();

        //(BUTTON) CLOSE SETTINGS WINDOW
            stängKnapp = new Button();
            //general specs
                stängKnapp.Text = "Close";
                stängKnapp.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                stängKnapp.AutoSize = false;
                stängKnapp.TextAlign = ContentAlignment.MiddleCenter;
                stängKnapp.FlatStyle = FlatStyle.System;
                stängKnapp.UseVisualStyleBackColor = true;
                stängKnapp.TabIndex = 0;
            //storlek
                stängKnapp.Size = new Size(
                    TextRenderer.MeasureText(stängKnapp.Text, stängKnapp.Font).Width + 20,
                    buttonHeight
                );

                int padXX = (int)(this.ClientSize.Width * 0.02);
                int padYY = (int)(this.ClientSize.Height * 0.02);

                stängKnapp.Location = new Point(
                    this.ClientSize.Width - stängKnapp.Width - padXX,
                    this.ClientSize.Height - stängKnapp.Height - padYY
                );
            //funktion
                stängKnapp.Click += (s, e) => this.Close();

        //DISPLAY SAVED HOTKEYS (AND THEIR PRGM/FILE)
            programLista = new ListView();
                programLista.View = View.Details; //formatet som står för att items visa på samma sätt som i gamla task manager
                //drygt sätt att ändra kolumnernas färg...
                    programLista.Columns.Add("Program/Filnamn");
                    programLista.Columns.Add("ID");
                    programLista.Columns.Add("Filväg");
                    programLista.Columns.Add("Modifier Keys");
                    programLista.Columns.Add("Key");

                programLista.Location = new Point(
                    (int)(this.ClientSize.Width * 0.02),
                    (int)(this.ClientSize.Height * 0.02)
                );
                programLista.Size = new Size(
                    (int)(this.ClientSize.Width * 0.75),
                    (int)(this.ClientSize.Height * 0.85)
                );
            
            //fördelar .width jämnt till kolumerna
                for (int i = 0; i < programLista.Columns.Count; i++) {
                    programLista.Columns[i].Width = (int)(programLista.ClientSize.Width / programLista.Columns.Count);
                }
                //programLista.Columns[2].Width = 200;
                uppdateraProgramListan();

            this.Controls.Add(programLista);
            this.Controls.Add(addKnapp);
            this.Controls.Add(removeKnapp);
            this.Controls.Add(stängKnapp);
        }
    }
}

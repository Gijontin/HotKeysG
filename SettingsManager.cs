using System.Text.Json;

namespace HotkeysG {
    public class SettingsManager {

        public List<KeyBindings>? loadedKB;
        private string HämtaFilväg() {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        }
        //global funktion för att väldigt snabbt, hacky och lätt fånga knapptryck från keyboard:et
        public static Keys CaptureKey() {
            Keys key = Keys.None;

        //skapa en snabb, ful, temporär prompt-ruta
            using (var f = new Form()) {
                f.KeyPreview = true; //prio lyssning på input över annat skit
                f.StartPosition = FormStartPosition.CenterScreen;
                
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.MinimizeBox = false;
                f.MaximizeBox = false;
                f.TopMost = true; //typ "override" att den är främst av alla program på skärmen...

                f.Width = 300;
                f.Height = 120;

                var label = new Label() {
                    Text = "Press a key to set your hotkey...\nOR press 'Esc' to cancel...",
                    AutoSize = false,
                    TextAlign =  ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                };
                
                f.Controls.Add(label);

            //funktion som faktiskt läser av keypress
                f.KeyDown += (s, e) => {
                    if (e.KeyCode == Keys.Escape) {
                        f.Close();
                        //return; //avbryter lyssningseventet    
                    }
                    //filter om bara EN modifier är nedtryckt
                    //if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu || e.KeyCode == Keys.ShiftKey) {return;}
                    
                    //fånga ctrl ELLER alt modifier
                    Keys modifierCheck = Control.ModifierKeys;

                    //Common Denominator
                    bool shift = modifierCheck.HasFlag(Keys.Shift);

                    //XOR(X-clusive OR) - bara en kan vara sann annars blir bool:en falsk, krejsi C# stuff...
                    bool ctrlXORalt = modifierCheck.HasFlag(Keys.Control) ^ modifierCheck.HasFlag(Keys.Alt);
                    
                    //Final Key Filter
                    bool bokstav = e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z;
                    bool bokstavSV = e.KeyCode == Keys.Oem6 || e.KeyCode == Keys.Oem3 || e.KeyCode == Keys.Oem7;

                    bool nummer = e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9;

                    if (shift && ctrlXORalt && (bokstav || bokstavSV || nummer)){
                        key = e.KeyCode | modifierCheck;
                        f.Close();
                    }
                };
                f.ShowDialog();
            }
            //let the logic of pressing Escape key to cancel the saving be handled in the function där jag kallar denna funktionen...
            return key;
        }
        //läsa settings.json och importera till en List<KeyBinding> 
        public void laddaSettings() { //ladda = deserialisera
            //spara hela filen som en string-text
            

            //om sökvägen inte hittas, skapa en tom-lista istället för att undvika krasch...
            if (!File.Exists(HämtaFilväg())){
                loadedKB = new();
                return;
            }

            string tempStr = File.ReadAllText(HämtaFilväg());
            //parse:a hela string:en i ett List<Type> format till en List med samma typ - MEN annars (??) gör en ny lista istället...
            loadedKB = JsonSerializer.Deserialize<List<KeyBindings>>(tempStr) ?? new List<KeyBindings>();
        }
        
        //skriva settings.json och importera till en List<KeyBinding> 
        public void sparaSettings() {

            //fixa till int ID variablerna i kronologiskt följd igen innan listan sparas
                //ingen loop lär hända om listan är 0 så behöver inte skriva en skip-if check
            if (loadedKB != null) {
                for (int i = 0; i < loadedKB.Count; i++) {
                    loadedKB[i].ID = i;
                }
            }

            string json = JsonSerializer.Serialize(
                loadedKB,                                           //List<type>
                new JsonSerializerOptions {WriteIndented = true}    //skumt json-feature skit
            );

        //så både test och release skriver (GARANTERAT) i sina respektiva folders
            string mappnamn = AppDomain.CurrentDomain.BaseDirectory;
            string filväg = Path.Combine(mappnamn, "settings.json");
            File.WriteAllText(HämtaFilväg(), json);
            //File.WriteAllText("settings.json", json);
        }
        public void reloadSetting() {
            sparaSettings();
            laddaSettings();
        }
        public SettingsManager() {
            laddaSettings();
        }
    }
}
using System.Text.Json;

namespace HotkeysG {
    public class SettingsManager {

        public List<KeyBindings>? loadedKB;
        
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
                    key = e.KeyCode;
                    f.Close();
                };
                f.ShowDialog();
            }
            //let the logic of pressing Escape key to cancel the saving be handled in the function där jag kallar denna funktionen...
            return key;
        }
        //läsa settings.json och importera till en List<KeyBinding> 
        public void laddaSettings() { //ladda = deserialisera
            //spara hela filen som en string-text
            string tempStr = File.ReadAllText("settings.json");

            //parse:a hela string:en i ett List<Type> format till en List med samma typ.
            loadedKB = JsonSerializer.Deserialize<List<KeyBindings>>(tempStr);
        }
        
        //skriva settings.json och importera till en List<KeyBinding> 
        public void sparaSettings() {

            //fixa till int ID variablerna i kronologiskt följd igen innan listan sparas
                //ingen loop lär hända om listan är 0 så behöver inte skriva en skip-if check
            for (int i = 0; i < loadedKB.Count; i++) {
                loadedKB[i].ID = i;
            }

            string json = JsonSerializer.Serialize(
                loadedKB,                                           //List<type>
                new JsonSerializerOptions {WriteIndented = true}    //skumt json-feature skit
            );

        //så både test och release skriver (GARANTERAT) i sina respektiva folders
            string mappnamn = AppDomain.CurrentDomain.BaseDirectory;
            string filväg = Path.Combine(mappnamn, "settings.json");
            File.WriteAllText(filväg, json);
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
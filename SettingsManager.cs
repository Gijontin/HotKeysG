using System.Text.Json;

namespace HotkeysG {
    public class SettingsManager {

        public List<KeyBindings>? loadedKB;
        
        //läsa settings.json och importera till en List<KeyBinding> 
        public void laddaSettings() { //ladda = deserialisera
            //spara hela filen som en string-text
            string tempStr = File.ReadAllText("settings.json");

            //parse:a hela string:en i ett List<Type> format till en List med samma typ.
            loadedKB = JsonSerializer.Deserialize<List<KeyBindings>>(tempStr);
        }
        
        //skriva settings.json och importera till en List<KeyBinding> 
        public void sparaSettings() {
            
        }

        public SettingsManager() {
            laddaSettings();
        }
    }
}
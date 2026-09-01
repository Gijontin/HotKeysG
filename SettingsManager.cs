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
            /*
            Det meckiga här bli att koda WinForms till att öppna filsökvägen och sedan genom att välja en fil kopiera över dennes information
            till min KeyBindings class(struct, really) samt ange hotkey:sen man vill använda som också ska över till den struct:en
            ...för att sedan slutligen läggas till i loadedKB listan med .Add featuren

            EFTER DEN PROCESSEN LÄR MAN FAKTISKT KALLA "sparaSetting()" för att göra nedanstående process...

            .json kan visst bara skriva om hela filer, det är allt eller inget, intre ändra något i mitten eller lägga till något i slutet etc...

            Så, stegen för när något nytt ska sparas
            1. Spara ett nytt objekt i loadedKB
                loadedKB.Add(<ny KeyBindings>)

            2. Formattera om hela loadedKB listan via någon serialize json feature eller något
                string tempStr = JsonSerializer.Serialize( .. );

            3. Overwrite:a settings.json med den nya stringen som innehåller det uppdaterade objeketet i Listan
                File.WriteAllText("settings.json", tempStr);

            */
        }

        public SettingsManager() {
            laddaSettings();
        }
    }
}
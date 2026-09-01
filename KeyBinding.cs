namespace HotkeysG {
    public class KeyBindings {
        public required int ID {get; set;}
        public required string namn {get; set;}
        public required string filePath {get; set;}
        public string? filMapp {get; set;} //mest för git-bash som annars öppnas där .exe ligger

        public required uint winSignal1 {get; set;}
        public required uint winSignal2 {get; set;}

        public required Keys triggerKey {get; set;}
    }
}

/*
        //TEMP .JSON CREATION
            List<KeyBindings> jsonlist = new List<KeyBindings>();

            gitBash = new KeyBindings {
                ID = 0,
                namn = "Bash",
                filePath = @"C:\Program Files\Git\git-bash.exe",
                filMapp = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                winSignal1 = MOD_ALT,
                winSignal2 = MOD_SHIFT,
                triggerKey = Keys.T,
            };
            edge = new KeyBindings {
                ID = 1,
                namn = "Edge",
                filePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                filMapp = "",
                winSignal1 = MOD_ALT,
                winSignal2 = MOD_SHIFT,
                triggerKey = Keys.E,
            };

            jsonlist.Add(gitBash);
            jsonlist.Add(edge);

            string json = JsonSerializer.Serialize(
                jsonlist,                                           //List<type>
                new JsonSerializerOptions {WriteIndented = true}    //skumt json-feature skit
            );
            
            File.WriteAllText("settings.json", json);
        //END .JSON
*/
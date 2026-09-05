namespace HotkeysG {
    public class KeyBindings {
        public required int ID {get; set;}
        public required string namn {get; set;}
        public required string filePath {get; set;}
        public string? filMapp {get; set;} //mest för git-bash som annars öppnas där .exe ligger

        public required uint winSignal1 {get; set;}
        public required uint winSignal2 {get; set;}

        //public required Keys triggerKey {get; set;}
        public Keys triggerKey {get; set;}
    }
}
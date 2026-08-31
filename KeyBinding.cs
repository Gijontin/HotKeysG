namespace HotkeysG {
    class KeyBindings {
        public required int ID;
        public required string filePath;
        public string? filMapp; //mest för git-bash som annars öppnas där .exe ligger

        public required uint winSignal1;
        public required uint winSignal2;

        public required Keys triggerKey;
    }
}
namespace HotkeysG;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        //DOTNET WINFORM PREMADE COMMENTS, VERY COOL:

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }    
}
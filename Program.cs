using DoomFire.App;

internal static class Program
{
    [System.STAThread]
    private static void Main()
    {
        using var host = new DoomFireHost();
        host.Run();
    }
}

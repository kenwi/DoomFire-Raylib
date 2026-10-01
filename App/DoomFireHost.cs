using DoomFire.Fire;
using Raylib_cs;

namespace DoomFire.App;

/// <summary>
/// Composition root. Opens the window and runs the fire slice.
/// </summary>
public sealed class DoomFireHost : IDisposable
{
    private readonly FireSlice _fire = new();
    private bool _disposed;

    public void Run()
    {
        Raylib.SetConfigFlags(ConfigFlags.VSyncHint);
        Raylib.InitWindow(960, 540, "Doom Fire");
        Raylib.SetExitKey(KeyboardKey.Escape);
        Raylib.ToggleFullscreen();
        Raylib.SetTargetFPS(60);
        _fire.Load();

        while (!Raylib.WindowShouldClose())
        {
            _fire.Update();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            _fire.Draw();
            Raylib.EndDrawing();
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _fire.Dispose();
        if (Raylib.IsWindowReady())
        {
            Raylib.CloseWindow();
        }
    }
}

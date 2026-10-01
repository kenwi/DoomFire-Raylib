using System.Numerics;
using Raylib_cs;

namespace DoomFire.Fire;

/// <summary>
/// Draws the heat buffer through the palette lookup shader.
/// </summary>
internal sealed class FirePresenter : IDisposable
{
    private readonly Shader _present;
    private readonly int _paletteLoc;
    private bool _disposed;

    public FirePresenter()
    {
        string vertex = EmbeddedShader.Read("Fullscreen.vert");
        string fragment = EmbeddedShader.Read("FirePresent.frag");
        _present = Raylib.LoadShaderFromMemory(vertex, fragment);
        if (_present.Id == 0)
        {
            throw new InvalidOperationException("Fire present shader failed to compile.");
        }

        _paletteLoc = Raylib.GetShaderLocation(_present, "palette");
        if (_paletteLoc < 0)
        {
            _paletteLoc = Raylib.GetShaderLocation(_present, "palette[0]");
        }

        if (_paletteLoc < 0)
        {
            throw new InvalidOperationException("Present shader is missing uniform 'palette'.");
        }
    }

    public void Draw(Texture2D heat, int width, int height)
    {
        // A color array stays on its own uniform. A second sampler is easy to
        // lose because raylib binds the drawn texture on unit 0.
        Raylib.SetShaderValueV(
            _present,
            _paletteLoc,
            FirePalette.Colors,
            ShaderUniformDataType.Vec3,
            FirePalette.Count);

        Raylib.BeginShaderMode(_present);

        // Row 0 of the heat texture is the top of the fire.
        var source = new Rectangle(0, 0, heat.Width, heat.Height);
        var destination = new Rectangle(0, 0, width, height);
        Raylib.DrawTexturePro(heat, source, destination, Vector2.Zero, 0, Color.White);

        Raylib.EndShaderMode();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        Raylib.UnloadShader(_present);
    }
}

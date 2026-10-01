namespace DoomFire.Fire;

internal static class EmbeddedShader
{
    public static string Read(string fileName)
    {
        var assembly = typeof(EmbeddedShader).Assembly;
        string? name = assembly
            .GetManifestResourceNames()
            .SingleOrDefault(resource => resource.EndsWith(fileName, StringComparison.Ordinal));

        if (name is null)
        {
            string available = string.Join(", ", assembly.GetManifestResourceNames());
            throw new InvalidOperationException($"Missing shader '{fileName}'. Embedded resources: {available}");
        }

        using var stream = assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException($"Could not open shader '{name}'.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}

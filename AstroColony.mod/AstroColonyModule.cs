using System.Globalization;
using System.Text;
using WindowsGSH.Core.Modules;

namespace WindowsGSH.Modules.AstroColony;

public sealed class AstroColonyModule : ManifestBackedGameServerModule, IModuleExistingServerImportCapability
{
    private const string ConfigPath = @"AstroColony\Saved\Config\WindowsServer\ServerSettings.ini";
    private const string Section = "/Script/ACFeature.EHServerSubsystem";
    private static readonly Encoding Utf8NoBom = new UTF8Encoding(false);
    private static readonly Mapping[] Mappings =
    [
        new("ServerPassword", "server.password", ""),
        new("MapName", "server.mapName", "Astro Colony"),
        new("MaxPlayers", "server.maxPlayers", "20"),
        new("ShouldLoadLatestSavegame", "server.loadLatestSave", "True"),
        new("AdminList", "server.adminIds", ""),
        new("SharedTechnologies", "game.sharedTechnologies", "True"),
        new("OxygenConsumption", "game.oxygenConsumption", "True"),
        new("FreeConstruction", "game.freeConstruction", "False"),
        new("Sandbox", "game.sandbox", "False"),
        new("AutosaveInterval", "server.autosaveInterval", "5"),
        new("AutosavesCount", "server.autosaveCount", "10")
    ];

    public bool CanImport(string path) => ExistingInstallImport.CanImport(this, path);

    public Task<ModuleExistingServerImportProbe> PreviewImportAsync(string path, CancellationToken cancellationToken) =>
        ExistingInstallImport.PreviewAsync(this, path, cancellationToken);

    public override Task<IReadOnlyDictionary<string, object?>> ReadConfigFileSettingsAsync(ServerInstance instance, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var path = Path.Combine(instance.InstallPath, ConfigPath);
        var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        if (!File.Exists(path)) return Task.FromResult<IReadOnlyDictionary<string, object?>>(result);
        foreach (var line in File.ReadLines(path))
        {
            var separator = line.IndexOf('=');
            if (separator <= 0 || line.TrimStart().StartsWith(';') || line.TrimStart().StartsWith('#')) continue;
            var key = line[..separator].Trim();
            var mapping = Mappings.FirstOrDefault(item => item.IniKey.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (mapping != null) result[mapping.SettingKey] = line[(separator + 1)..].Trim();
            else if (key.Equals("Seed", StringComparison.OrdinalIgnoreCase)) result["server.seed"] = line[(separator + 1)..].Trim();
        }
        return Task.FromResult<IReadOnlyDictionary<string, object?>>(result);
    }

    public override Task WriteConfigFileSettingsAsync(ServerInstance instance, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var path = Path.Combine(instance.InstallPath, ConfigPath);
        var lines = File.Exists(path) ? File.ReadAllLines(path).ToList() : new List<string> { $"[{Section}]" };
        foreach (var mapping in Mappings) SetValue(lines, mapping.IniKey, GetSetting(instance, mapping.SettingKey, mapping.DefaultValue));
        var seed = GetSetting(instance, "server.seed", "");
        if (string.IsNullOrWhiteSpace(seed)) seed = GetExistingValue(lines, "Seed");
        if (string.IsNullOrWhiteSpace(seed)) seed = Random.Shared.Next(1, int.MaxValue).ToString(CultureInfo.InvariantCulture);
        SetValue(lines, "Seed", seed);
        var directory = Path.GetDirectoryName(path)!;
        Directory.CreateDirectory(directory);
        var temporary = path + ".windowsgsh.tmp";
        try { File.WriteAllLines(temporary, lines, Utf8NoBom); File.Move(temporary, path, true); }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
        return Task.CompletedTask;
    }

    private static void SetValue(List<string> lines, string key, string value)
    {
        for (var index = 0; index < lines.Count; index++)
        {
            var separator = lines[index].IndexOf('=');
            if (separator > 0 && lines[index][..separator].Trim().Equals(key, StringComparison.OrdinalIgnoreCase)) { lines[index] = $"{key}={value}"; return; }
        }
        lines.Add($"{key}={value}");
    }

    private static string GetExistingValue(IEnumerable<string> lines, string key)
    {
        foreach (var line in lines)
        {
            var separator = line.IndexOf('=');
            if (separator > 0 && line[..separator].Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                return line[(separator + 1)..].Trim();
        }
        return string.Empty;
    }

    private sealed record Mapping(string IniKey, string SettingKey, string DefaultValue);
}

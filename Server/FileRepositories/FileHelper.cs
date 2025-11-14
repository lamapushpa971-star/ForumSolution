using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FileRepositories;

internal static class FileHelper
{
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static List<T> LoadList<T>(string filePath)
    {
        var dir = Path.GetDirectoryName(filePath)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
    }

    public static void SaveList<T>(string filePath, List<T> list)
    {
        var dir = Path.GetDirectoryName(filePath)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonSerializer.Serialize(list, _options);
        File.WriteAllText(filePath, json);
    }
}


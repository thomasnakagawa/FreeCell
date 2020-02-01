using System.IO;

/// <summary>
/// Writes the results of the game to a JSON file
/// </summary>
public static class ResultsExporter
{
    /// <summary>
    /// Builds JSON object of the results and writes it to a file
    /// </summary>
    /// <param name="time"></param>
    /// <param name="usedCheats"></param>
    public static void WriteResultsToFile(float time, bool usedCheats)
    {
        var outputObject = new SimpleJSON.JSONObject();
        outputObject["completionTime"] = time;
        outputObject["usedCheats"] = usedCheats;

        string file = Path.Combine(Directory.GetCurrentDirectory(), GameConfiguration.Instance.OutputFile);
        FileInfo fi = new FileInfo(file);
        if (!fi.Directory.Exists)
        {
            Directory.CreateDirectory(fi.DirectoryName);
        }
        File.WriteAllText(file, outputObject.ToString());
    }
}

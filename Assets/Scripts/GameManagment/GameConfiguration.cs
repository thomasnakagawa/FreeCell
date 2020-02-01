using UnityEngine;
using SimpleJSON;

/// <summary>
/// Provides config variables for other classes to access
/// </summary>
public class GameConfiguration : MonoBehaviour
{
    [SerializeField] private TextAsset JSONConfig = default;

    public static GameConfiguration Instance;

    public Color HoverEnabledColor { get; private set; } = Color.green;
    public Color HoverDisabledColor { get; private set; } = Color.red;
    public bool CheatsEnabled { get; private set; } = false;
    public int RNGSeed { get; private set; } = 0;
    public string OutputFile { get; private set; } = "results.json";

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        ParseJSONConfig();
    }

    private void ParseJSONConfig()
    {
        var config = JSON.Parse(JSONConfig.text);

        HoverEnabledColor = ParseColor(config["HoverEnabledColor"], Color.blue);
        HoverDisabledColor = ParseColor(config["HoverDisabledColor"], Color.red);
        CheatsEnabled = config["CheatsEnabled"].AsBool;
        RNGSeed = config["RNGSeed"].AsInt;
        OutputFile = config["OutputFile"];
    }

    private Color ParseColor(string colorString, Color defaultColor)
    {
        Color parsedColor = defaultColor;
        ColorUtility.TryParseHtmlString(colorString, out parsedColor);
        return parsedColor;
    }
}

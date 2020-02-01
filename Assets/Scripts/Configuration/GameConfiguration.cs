using UnityEngine;
using SimpleJSON;

public class GameConfiguration : MonoBehaviour
{
    [SerializeField] private TextAsset JSONConfig = default;

    public static GameConfiguration Instance;

    public Color HoverEnabledColor { get; private set; } = Color.green;
    public Color HoverDisabledColor { get; private set; } = Color.red;
    public bool CheatsEnabled { get; private set; } = false;
    public int RNGSeed { get; private set; } = 0;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        var config = JSON.Parse(JSONConfig.text);

        HoverEnabledColor = ParseColor(config["HoverEnabledColor"], Color.blue);
        HoverDisabledColor = ParseColor(config["HoverDisabledColor"], Color.red);
        CheatsEnabled = config["CheatsEnabled"].AsBool;
        RNGSeed = config["RNGSeed"].AsInt;
    }

    private Color ParseColor(string colorString, Color defaultColor)
    {
        Color parsedColor = defaultColor;
        ColorUtility.TryParseHtmlString(colorString, out parsedColor);
        return parsedColor;
    }
}

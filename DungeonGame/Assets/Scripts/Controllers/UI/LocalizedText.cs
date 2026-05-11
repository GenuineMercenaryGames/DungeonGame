using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string localizationString;

    public string LOC { get { return localizationString; } set { localizationString = value; UpdateText(); } }

    void Start()
    {
        UpdateText();
    }

    void OnEnable()
    {
        UpdateText();
    }

    void OnValidate()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (TryGetComponent<TMP_Text>(out var text))
        {
            text.text = LanguageManager.GetString(localizationString);
        }
    }
}

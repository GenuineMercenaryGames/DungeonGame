using TMPro;
using UnityEngine;

public class LocalizedTextFixed : MonoBehaviour
{
    [SerializeField] private string localizationString;
    [SerializeField] private Language language;

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
            text.text = LanguageManager.GetString(language, localizationString);
        }
    }
}

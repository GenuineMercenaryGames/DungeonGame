using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string localizationString;

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

using UnityEngine;
using UnityEngine.UI;

public class AttireSelectMenu : MonoBehaviour
{
    [Header("Body Reference")]
    [SerializeField] private Renderer body;

    [Header("Color Reference")]
    [SerializeField] private Image colorImage;
    [SerializeField] private Slider sliderR;
    [SerializeField] private Slider sliderG;
    [SerializeField] private Slider sliderB;

    private Material instance;

    void Start()
    {
        instance = new Material(body.sharedMaterial);
        body.material = instance;
        Color color = instance.GetColor("_Color");
        sliderR.value = color.r;
        sliderG.value = color.g;
        sliderB.value = color.b;
    }

    public void SetColor()
    {
        Color color = new Color(sliderR.value, sliderG.value, sliderB.value);
        instance.SetColor("_Color", color);
        GameConfigManager.Instance.skinColor = color;
        colorImage.color = color;
    }

}

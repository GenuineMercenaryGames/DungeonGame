using UnityEngine;

public class PlayerSkinController : MonoBehaviour
{
    [SerializeField] private Renderer rendererReference;

    private Material instance;

    void Start()
    {
        instance = new Material(rendererReference.sharedMaterial);
        instance.SetColor("_Color", GameConfigManager.Instance.skinColor);
        rendererReference.material = instance;
    }
}

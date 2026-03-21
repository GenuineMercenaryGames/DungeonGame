using UnityEngine;

[ExecuteAlways]
public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private GameObject newVisual;
    [SerializeField] private GameObject visual;

    void Start()
    {
        UpdateVisuals();
    }

    void OnValidate()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (visual != null)
        {
            Destroy(visual);
        }
        if (newVisual != null)
        {
            var go = Instantiate(newVisual, this.transform);
            visual = go;
        }
    }
}

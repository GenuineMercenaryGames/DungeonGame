using UnityEngine;

public class OpenURLController : MonoBehaviour
{
    [SerializeField] private string URL;

    public void OpenURL()
    {
        Application.OpenURL(URL);
    }
}

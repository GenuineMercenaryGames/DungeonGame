using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{

    [SerializeField] private HealthController healthController;
    [SerializeField] private Image healthFill;
    [SerializeField] private Image healthFillBackground;
    [SerializeField] private Image warnImage;
    [SerializeField] private float healthShowTime = 3f;
    [SerializeField] private float warningShowTime = 1f;
    private float hideHealthCooldown;
    private float hideWarnCooldown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ShowWarnSign()
    {
        warnImage.enabled = true;
        hideWarnCooldown = warningShowTime;
    }

    public void HideWarnSign()
    {
        warnImage.enabled = false;
    }

    public void HideBar()
    {
        healthFill.enabled = false;
        healthFillBackground.enabled = false;
    }

    public void ShowBar()
    {
        healthFill.enabled = true;
        healthFillBackground.enabled = true;
    }

    void Start()
    {
        healthController = GetComponent<HealthController>();
        HideBar();
        HideWarnSign();

        ObservableVariable<float>.FuncIn2<float> callback = (oldHealth, newHealth) =>
        {
            healthFill.fillAmount = newHealth / healthController.MaxHealth.Value;
            hideHealthCooldown = healthShowTime;
            ShowBar();
        };

        healthController.Health.AddListener(callback);
    }

    // Update is called once per frame
    void Update()
    {
        healthFill.transform.forward = Camera.main.transform.forward; // TODO: Saco la cámara principal, preguntar a Dani si vamos a gestionar más cámaras y si la saco del manager.
        healthFillBackground.transform.forward = Camera.main.transform.forward;
        warnImage.transform.forward = Camera.main.transform.forward;

        if (hideHealthCooldown <= 0)
        {
            HideBar();
            hideHealthCooldown = 0.0f;
        }
        else
        {
            hideHealthCooldown -= Time.deltaTime;
        }

        if (hideWarnCooldown <= 0)
        {
            HideWarnSign();
            hideWarnCooldown = 0.0f;
        }
        else
        {
            hideWarnCooldown -= Time.deltaTime;
        }
    }
}

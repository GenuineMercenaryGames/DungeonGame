using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{

    [SerializeField] private HealthController healthController;
    [SerializeField] private Image healthFill;
    [SerializeField] private Image healthFillBackground;
    [SerializeField] private float showTime = 3f;
    private float hideCooldown;

    // Por ahora he hecho que solo se muestre cuando recibe daño, pero se puede modificar para que salga en otras ocasiones.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthController = GetComponent<HealthController>();
        healthFill.enabled = false;
        healthFillBackground.enabled = false;

        ObservableVariable<float>.FuncIn2<float> callback = (oldHealth, newHealth) =>
        {
            healthFill.fillAmount = newHealth / healthController.MaxHealth.Value;
            hideCooldown = showTime;
            healthFill.enabled = true;
            healthFillBackground.enabled = true;
        };

        healthController.Health.AddListener(callback);
    }

    // Update is called once per frame
    void Update()
    {
        healthFill.transform.forward = Camera.main.transform.forward; // TODO: Saco la cámara principal, preguntar a Dani si vamos a gestionar más cámaras y si la saco del manager.
        healthFillBackground.transform.forward = Camera.main.transform.forward;

        if (hideCooldown <= 0)
        {
            healthFill.enabled = false;
            healthFillBackground.enabled = false;
            hideCooldown = 0.0f;
        }
        else
        {
            hideCooldown -= Time.deltaTime;
        }
    }
}

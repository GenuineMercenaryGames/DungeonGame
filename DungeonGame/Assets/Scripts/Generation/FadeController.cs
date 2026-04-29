using System.Collections;
using UnityEngine;

public sealed class FadeController : MonoBehaviour
{
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float startScaleFactor = 0.75f;
    [SerializeField] private float endScaleFactor = 0.75f;
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Vector3 originalScale;
    private Coroutine currentFade;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        PlayFadeIn();
    }

    private void PlayVFX()
    {
        float height = 1f;
        Renderer targetRenderer = GetComponentInChildren<Renderer>();

        if (targetRenderer != null)
        {
            height = targetRenderer.bounds.size.y;
        }

        VFXManager.Instance.InstantiateVFX("SpawnAsset", transform.position, height * 0.2f);
    }

    public void PlayFadeIn()
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        PlayVFX();
        currentFade = StartCoroutine(FadeIn());
    }

    public void PlayFadeOutAndDisable()
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        PlayVFX();
        currentFade = StartCoroutine(FadeOutAndDisable());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        transform.localScale = originalScale * startScaleFactor;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            float scaleFactor = Mathf.Lerp(startScaleFactor, 1f, curve.Evaluate(t));

            transform.localScale = originalScale * scaleFactor;

            yield return null;
        }

        transform.localScale = originalScale;
        currentFade = null;
    }

    private IEnumerator FadeOutAndDisable()
    {
        float elapsedTime = 0f;
        Vector3 currentScale = transform.localScale;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            float scaleFactor = Mathf.Lerp(1f, endScaleFactor, curve.Evaluate(t));

            transform.localScale = originalScale * scaleFactor;

            yield return null;
        }

        transform.localScale = currentScale;
        currentFade = null;
        gameObject.SetActive(false);
    }
}
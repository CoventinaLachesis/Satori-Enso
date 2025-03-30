using UnityEngine;

public class PopIn : MonoBehaviour
{
    public float popTime = 0.2f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        StartCoroutine(makePopIn());
    }

    private System.Collections.IEnumerator makePopIn()
    {
        float t = 0f;
        while (t < popTime)
        {
            t += Time.deltaTime;
            float scale = Mathf.SmoothStep(0f, 1f, t / popTime);
            transform.localScale = originalScale * scale;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}

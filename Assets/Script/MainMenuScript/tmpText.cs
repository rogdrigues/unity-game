using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class tmpText : MonoBehaviour
{
    [SerializeField]TMP_Text textTMP;
    public float animationDuration = 1f;

    private bool isAnimating = false;
    private Color startColor;
    private Color endColor;

    private void Start()
    {
        // L?y màu s?c ban ??u t? Text Mesh Pro
        startColor = textTMP.color;

        // Thi?t l?p màu s?c k?t thúc v?i alpha = 0
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    public void StartAnimation()
    {
        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(AlphaAnimationCoroutine());
        }
    }

    private System.Collections.IEnumerator AlphaAnimationCoroutine()
    {
        float currentTime = 0f;

        while (currentTime < animationDuration)
        {
            // Tính toán giá tr? alpha d?a trên th?i gian
            float alpha = Mathf.Lerp(1f, 0f, currentTime / animationDuration);

            // T?o màu s?c m?i v?i alpha t??ng ?ng
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // Gán màu s?c m?i cho Text Mesh Pro
            textTMP.color = newColor;

            currentTime += Time.deltaTime;

            yield return null;
        }

        // Thi?t l?p màu s?c cu?i cùng cho ??i t??ng
        textTMP.color = endColor;

        // ?ánh d?u hoàn t?t animation
        isAnimating = false;
        yield return new WaitForSeconds(2f); 
        textTMP.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingGame : MonoBehaviour
{
    public GameObject healthGUI;
    public GameObject goldGUI;
    public float startTargetY = 202.94f;
    public float endTargetY = 166.8f;
    public float animationDuration = 1.0f;

    private Vector3 healthGUIStartPosition;
    private Vector3 goldGUIStartPosition;

    private void Start()
    {
        // L?u l?i v? tr� ban ??u c?a hai gameObject
        healthGUIStartPosition = healthGUI.transform.position;
        goldGUIStartPosition = goldGUI.transform.position;

        // B?t ??u animation
        StartAnimation();
    }

    private void StartAnimation()
    {
        // T?o coroutine ?? th?c hi?n animation t? v? tr� ban ??u ??n v? tr� m?i
        StartCoroutine(AnimateObjects());
    }

    private System.Collections.IEnumerator AnimateObjects()
    {
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            // T�nh to�n t?a ?? Y m?i c?a c�c gameObject d?a tr�n th?i gian ?� tr�i qua v� th?i gian animation
            float t = elapsedTime / animationDuration;
            float newY1 = Mathf.Lerp(healthGUIStartPosition.y, endTargetY, t);
            float newY2 = Mathf.Lerp(goldGUIStartPosition.y, endTargetY, t);

            // C?p nh?t v? tr� m?i cho c�c gameObject
            Vector3 newPosition1 = new Vector3(healthGUI.transform.position.x, newY1, healthGUI.transform.position.z);
            Vector3 newPosition2 = new Vector3(goldGUI.transform.position.x, newY2, goldGUI.transform.position.z);
            healthGUI.transform.position = newPosition1;
            goldGUI.transform.position = newPosition2;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ??m b?o ??t v? tr� cu?i c�ng c?a c�c gameObject th�nh v? tr� m?i
        healthGUI.transform.position = new Vector3(healthGUI.transform.position.x, endTargetY, healthGUI.transform.position.z);
        goldGUI.transform.position = new Vector3(goldGUI.transform.position.x, endTargetY, goldGUI.transform.position.z);
    }
}

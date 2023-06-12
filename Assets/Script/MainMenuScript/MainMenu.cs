using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Animator animator;
    [SerializeField] tmpText tmpText;
    [SerializeField] tmpText subText;
    [SerializeField] tmpText buttonPress;
    [SerializeField] Animator fadeButton;
    [SerializeField] List<menuOptionsText> menuOption;
    [SerializeField] float delayBeforeHide = 2f;
    bool isClick;

    void Update()
    {
        if (Input.anyKeyDown && isClick == false)
        {
            isClick = true;
            audioSource.Play();
            ShowOptionsMenu();
            foreach (var optionText in menuOption)
            {
                optionText.ShowMenuOptions();
            }
        }
    }

    void ShowOptionsMenu()
    {
        animator.enabled = true;
        fadeButton.enabled = false;
        fadeButton.gameObject.SetActive(false);

        tmpText.StartAnimation();
        subText.StartAnimation();
        buttonPress.StartAnimation();
        StartCoroutine(HideGameObjectCoroutine());
    }

    private IEnumerator HideGameObjectCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < delayBeforeHide)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}

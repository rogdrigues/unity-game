using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewTower : MonoBehaviour, IPointerClickHandler
{
    public GameObject TowerPrefab;
    public GameObject TowerPrefabUpgrade;

    private bool isTowerPrefabActive = false;
    private bool isBuildingTower = false;
    private bool isMouseClickOnCollider = false;
    private bool hasClickedOutsideCollider = false;
    private bool isDeactivating = false;
    private bool hasClickedOnCollider = false;

    public AudioClip clickSound;
    private AudioSource audioSource;

    public Animator TowerPrefabAnimation;
    public Animator TowerPrefabUpgradeAnimation;

    public GameObject[] listTowerAvailable;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                string clickedObjectTag = clickedObject.tag;

                if (clickedObjectTag == "Arrow" || clickedObjectTag == "Mage" || clickedObjectTag == "Military" || clickedObjectTag == "Artillerist")
                {
                    isBuildingTower = true;
                }
                else
                {
                    isBuildingTower = false;
                }
            }
        }

        if (isTowerPrefabActive && Input.GetMouseButtonDown(0) && !isMouseClickOnCollider && hasClickedOutsideCollider && !isDeactivating && !hasClickedOnCollider)
        {
            StartCoroutine(DeactivateTowerPrefabWithDelay(isBuildingTower ? 1f : 0.45f));
            hasClickedOutsideCollider = false;
        }

        isMouseClickOnCollider = false;
        hasClickedOnCollider = false;

        if (Input.GetMouseButtonDown(0) && !IsMouseOverTowerPrefab())
        {
            hasClickedOutsideCollider = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isBuildingTower)
        {
            if (!isDeactivating)
            {
                ActivateTowerPrefab();
            }
            isMouseClickOnCollider = true;
            hasClickedOnCollider = true;
        }
    }

    private bool IsMouseOverTowerPrefab()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapPoint(mousePosition);
        if (collider != null && (collider.gameObject == TowerPrefab || collider.gameObject == TowerPrefabUpgrade))
        {
            return true;
        }
        return false;
    }

    private void ActivateTowerPrefab()
    {
        if (gameObject.CompareTag("Empty"))
        {
            TowerPrefab.SetActive(true);
        }
        else
        {
            TowerPrefabUpgrade.SetActive(true);
            GameObject towerObject = Array.Find(listTowerAvailable, element => element.tag + "Sentines" == gameObject.tag) as GameObject;
            towerObject.SetActive(true);
        }

        isTowerPrefabActive = true;

        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private IEnumerator DeactivateTowerPrefabWithDelay(float delay)
    {
        isDeactivating = true;

        if (TowerPrefab != null && gameObject.CompareTag("Empty"))
        {
            TowerPrefabAnimation.SetBool("isClosed", true);
            yield return new WaitForSeconds(delay);

            if (TowerPrefab.activeSelf)
            {
                TowerPrefab.SetActive(false);
            }
        }

        if (TowerPrefabUpgrade != null && !gameObject.CompareTag("Empty"))
        {
            TowerPrefabUpgradeAnimation.SetBool("isClosed", true);
            yield return new WaitForSeconds(delay);

            if (TowerPrefabUpgrade.activeSelf)
            {
                TowerPrefabUpgrade.SetActive(false);
            }
        }

        foreach (GameObject tower in listTowerAvailable)
        {
            if (tower.activeSelf)
            {
                tower.SetActive(false);
            }
        }

        isTowerPrefabActive = false;
        isDeactivating = false;
    }

    public void StartBuildingTower()
    {
        isBuildingTower = true;
    }

    public void FinishBuildingTower()
    {
        isBuildingTower = false;
    }
}
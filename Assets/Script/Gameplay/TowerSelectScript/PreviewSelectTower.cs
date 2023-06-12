using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class PreviewSelectTower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite hoverSprite;
    private Sprite originalSprite;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public AudioClip hoverSFX;
    public AudioClip towerBuildingSFX;
    public GameObject tower;

    public SpriteRenderer landSprite;
    public GameObject towerSprite;
    //SoliderTower
    public GameObject soliderObjectA;
    public GameObject soliderObjectB;
    //For Military Door Animation
    public GameObject militaryDoorObject;
    //For Arti tower
    public GameObject firePower;
    public GameObject firepowerTowerSprite;
    public GameObject firepowerTowerBuilding;
    //For replaced solider for each tower
    public GameObject replacedSoliderA;
    public GameObject replacedSoliderB;
    //Replaced land building to current build
    public Sprite replacedLand;
    //Sprite current building
    public Sprite landTowerBuilding;
    //Replaced landtowerbuilding to a completed tower
    public Sprite replacedTower;
    //ProgressBar process building and a song when finished
    public GameObject progressBar;
    public Image fillCircle;
    public GameObject cloudEffect;
    //Close list tower build
    public GameObject previewTower;
    public GameObject RangeAttack;
    public GameObject bulletPrefab;

    private enum TowerType
    {
        ArrowTower,
        MageTower
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        previewTower.GetComponent<PreviewTower>().StartBuildingTower();
        GameObject clickedObject = eventData.pointerPress;
        previewTower.tag = clickedObject.tag + "Sentines";
        landSprite.sprite = replacedLand;
        SpriteRenderer towerSpriteRenderer = towerSprite.GetComponent<SpriteRenderer>();
        towerSpriteRenderer.sprite = landTowerBuilding;
        progressBar.SetActive(true);
        if (towerBuildingSFX != null)
        {
            audioSource.PlayOneShot(towerBuildingSFX);
        }
        StartCoroutine(FillCircleOverTime());
    }

    private IEnumerator FillCircleOverTime()
    {
        float elapsedTime = 0f;
        float startFillAmount = fillCircle.fillAmount;
        float targetFillAmount = 1f;
        float fillDuration = 0.8f;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentFillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / fillDuration);
            fillCircle.fillAmount = currentFillAmount;

            yield return null;
        }

        fillCircle.fillAmount = targetFillAmount;
        TowerBuildComplete();
    }

    private IEnumerator SetActiveAfterDelay(GameObject gameObject, bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(active);
    }

    private void TowerBuildComplete()
    {
        progressBar.SetActive(false);
        SpriteRenderer towerSpriteRenderer = towerSprite.GetComponent<SpriteRenderer>();
        Animator towerAnimator = towerSprite.GetComponent<Animator>();

        if (replacedTower != null && militaryDoorObject == null)
        {
            HandleNonFirepowerTower(towerSpriteRenderer, towerAnimator);
        }
        else if (replacedTower == null && militaryDoorObject == null)
        {
            HandleFirepowerTower(towerSpriteRenderer, towerAnimator);
        }
        else
        {
            HandleMilitaryTower(towerSpriteRenderer, towerAnimator);
        }
        RangeAttack.GetComponent<PolygonCollider2D>().enabled = true;
        previewTower.GetComponent<PreviewTower>().FinishBuildingTower();
        cloudEffect.SetActive(true);
        StartCoroutine(SetActiveAfterDelay(cloudEffect, false, 1f));
    }

    private void HandleNonFirepowerTower(SpriteRenderer towerSpriteRenderer, Animator towerAnimator)
    {
        towerSpriteRenderer.sprite = replacedTower;
        Vector3 newPosition = new Vector3(535.1f, 254.3f, towerSprite.transform.position.z);
        Quaternion newRotation = Quaternion.Euler(0f, 0f, 90f);
        towerSprite.transform.position = newPosition;
        towerSprite.transform.rotation = newRotation;

        SpriteRenderer soliderA = soliderObjectA.GetComponent<SpriteRenderer>();
        Animator Ani_soliderA = soliderObjectA.GetComponent<Animator>();

        DestroyAllComponents(soliderObjectA);

        //Arrow Tower
        if (replacedSoliderA != null && replacedSoliderB != null)
        {
            DestroyAllComponents(soliderObjectB);
            SpriteRenderer soliderB = soliderObjectB.GetComponent<SpriteRenderer>();
            Animator Ani_soliderB = soliderObjectB.GetComponent<Animator>();
            //Modifier Tower Information
            HandleSolider(soliderA, Ani_soliderA, 534.25f, 269f, soliderA.transform.position.z - 1.0f, 0f, replacedSoliderA);
            HandleSolider(soliderB, Ani_soliderB, 545.88f, 269f, soliderB.transform.position.z - 1.0f, 0f, replacedSoliderB);
            SetRangeAttackScale(6.3f, 4.04f);
            AddTowerComponent(soliderObjectA, TowerType.ArrowTower, 0, 300);
            AddTowerComponent(soliderObjectB, TowerType.ArrowTower, 0, 300);
        }
        //Mage Tower
        else if(replacedSoliderA != null && replacedSoliderB == null)
        {
            //Modifier Tower Information
            HandleSolider(soliderA, Ani_soliderA, 537.6f, 269.6f, soliderA.transform.position.z - 1.0f, 90f, replacedSoliderA);
            SetRangeAttackScale(6.3f, 4.04f);
            AddTowerComponent(soliderObjectA, TowerType.MageTower, 0, 240);
        }
    }
    private void SetRangeAttackScale(float xScale, float yScale)
    {
        Vector3 rangeAttackScale = new Vector3(xScale, yScale, RangeAttack.transform.localScale.z);
        RangeAttack.transform.localScale = rangeAttackScale;
    }
    
    private void AddTowerComponent(GameObject gameObject, TowerType towerType, int arcHeight, int speed)
    {
        switch (towerType)
        {
            case TowerType.ArrowTower:
                var arrowTower = gameObject.AddComponent<ArrowTower>();
                arrowTower.polygonCollider = RangeAttack.GetComponent<PolygonCollider2D>();
                arrowTower.arcHeight = arcHeight;
                arrowTower.speed = speed;
                arrowTower.bulletPrefab = bulletPrefab;
                RangeAttack.GetComponent<TowerDetected>().SoliderA = soliderObjectA;
                RangeAttack.GetComponent<TowerDetected>().SoliderB = soliderObjectB;
                break;
            case TowerType.MageTower:
                var mageTower = gameObject.AddComponent<MageTower>();
                mageTower.polygonCollider = RangeAttack.GetComponent<PolygonCollider2D>();
                mageTower.arcHeight = arcHeight;
                mageTower.speed = speed;
                mageTower.bulletPrefab = bulletPrefab;
                RangeAttack.GetComponent<TowerDetected>().SoliderA = soliderObjectA;
                break;
        }

    }

    private void DestroyAllComponents(GameObject gameObject)
    {
        Component[] components = gameObject.GetComponents<MonoBehaviour>();
        foreach (var component in components)
        {
            Destroy(component);
        }
    }

    private void HandleSolider(SpriteRenderer soliderA, Animator Ani_soliderA, float xPos, float yPos, float zPos, float zRotate, GameObject replacedSolider)
    {
        Vector3 soliderPosition = new Vector3(xPos, yPos, zPos);
        Quaternion soliderRotate = Quaternion.Euler(0f, 0f, zRotate);
        soliderA.transform.position = soliderPosition;
        soliderA.transform.rotation = soliderRotate;

        soliderA.sprite = replacedSolider.GetComponent<SpriteRenderer>().sprite;
        Ani_soliderA.runtimeAnimatorController = replacedSolider.GetComponent<Animator>().runtimeAnimatorController;
    }

    private void HandleFirepowerTower(SpriteRenderer towerSpriteRenderer, Animator towerAnimator)
    {
        SpriteRenderer firepowerSpriteRenderer = firepowerTowerSprite.GetComponent<SpriteRenderer>();
        Animator firepowerAnimator = firepowerTowerSprite.GetComponent<Animator>();

        towerSpriteRenderer.sprite = firepowerSpriteRenderer.sprite;
        towerAnimator.runtimeAnimatorController = firepowerAnimator.runtimeAnimatorController;

        Vector3 newPosition = new Vector3(537f, 251.4f, towerSprite.transform.position.z);
        towerSprite.transform.position = newPosition;

        firePower.SetActive(true);

        HandleSolider(soliderObjectA.GetComponent<SpriteRenderer>(), soliderObjectA.GetComponent<Animator>(), 559.2f, 253.2f, soliderObjectA.transform.position.z - 1f, 0f, replacedSoliderA);
        HandleSolider(soliderObjectB.GetComponent<SpriteRenderer>(), soliderObjectB.GetComponent<Animator>(), 515.5f, 257.5f, soliderObjectB.transform.position.z, 0f, replacedSoliderB);

        towerAnimator.enabled = false;
        firepowerAnimator.enabled = false;
        firepowerTowerBuilding.SetActive(true);
        SetRangeAttackScale(7.03f, 4.04f);
    }

    private void HandleMilitaryTower(SpriteRenderer towerSpriteRenderer, Animator towerAnimator)
    {
        towerSpriteRenderer.sprite = replacedTower;
        Vector3 newPosition = new Vector3(534f, 256f, towerSpriteRenderer.transform.position.z);
        Quaternion newRotation = Quaternion.Euler(0f, 0f, 90f);
        towerSprite.transform.position = newPosition;
        towerSprite.transform.rotation = newRotation;
        militaryDoorObject.SetActive(true);
        //Modifier Tower Information
        SetRangeAttackScale(6.46f, 4.04f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spriteRenderer.sprite = hoverSprite;
        tower.SetActive(true);
        if (hoverSFX != null)
        {
            audioSource.PlayOneShot(hoverSFX);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.sprite = originalSprite;
        tower.SetActive(false);
    }
}
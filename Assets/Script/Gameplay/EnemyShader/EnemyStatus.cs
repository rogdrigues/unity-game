using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    public int maxHealth;
    public int health;

    public int minPhysicalDamage;
    public int maxPhysicalDamage;

    public float attackRate;

    public int armorRating;
    public int magicResistance;

    public int livesTaken;
    public int bounty;

    private Animator animator;
    public GameObject HPBarrage;
    private Image HPBarrageFill;
    private PathScript pathScript;

    private void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        pathScript = GetComponent<PathScript>();

        HPBarrageFill = HPBarrage.GetComponent<Image>();
        HPBarrageFill.fillAmount = 1f;
    }

    public void TakeDamage(int damage, string typeDamage)
    {
        if (typeDamage == "Physical")
        {
            int effectivePhysicalDamage = Mathf.Max((int)(damage * (1 - (armorRating / (armorRating + 100)))), 0);
            effectivePhysicalDamage -= armorRating;
            effectivePhysicalDamage = Mathf.Max(effectivePhysicalDamage, 0);
            health -= effectivePhysicalDamage;
        }
        else if (typeDamage == "Magic")
        {
            float magicDamageReductionPercentage = (float)magicResistance / (magicResistance + 100);
            damage = Mathf.RoundToInt(damage * (1 - magicDamageReductionPercentage));
            health -= damage;
        }

        health = Mathf.Max(health, 0);

        HPBarrageFill.fillAmount = (float)health / maxHealth;

        if (health <= 0)
        {
            pathScript.moveSpeed = 0f;
            animator.SetTrigger("isDead");
            Destroy(gameObject, 2f);
        }
    }
}
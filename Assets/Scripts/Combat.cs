using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// inventory, attributes etc. can influence max health
public interface ICombatBonus
{
    int GetDamageBonus();
    int GetDefenseBonus();
}

public class UnityEventGameObjectInt : UnityEvent<GameObject, int> { }

public class Combat : MonoBehaviour
{
    public int baseDamage;
    public int baseDefense;
    public GameObject onDamageEffect;

    // events
    public UnityEventGameObjectInt onReceivedDamage;

    ICombatBonus[] bonusComponents;
    void Awake()
    {
        bonusComponents = GetComponentsInChildren<ICombatBonus>();
    }

    public int damage
    {
        get
        {
            return baseDamage + bonusComponents.Sum(b => b.GetDamageBonus());
        }
    }

    public int defense
    {
        get
        {
            return baseDefense + bonusComponents.Sum(b => b.GetDefenseBonus());
        }
    }

    public void DealDamageAt(GameObject other, int amount, Vector3 hitPoint, Vector3 hitNormal, Collider hitCollider)
    {
        if (other != null)
        {
            Health otherHealth = other.GetComponent<Health>();
            Combat otherCombat = other.GetComponent<Combat>();
            if (otherHealth != null && otherCombat != null)
            {
                if (otherHealth.current > 0)
                {
                    int damageDealt = amount - Mathf.Max(otherCombat.defense, 1);

                    // deal the damage
                    otherHealth.current -= damageDealt;

                    // used for mob's onAggro event
                    otherCombat.onReceivedDamage.Invoke(gameObject, damageDealt);

                    // send damage to other client
                    // otherCombat.RpcOnDamageReceived(damageDealt, hitPoint, hitNormal);
                }
            }
        }
    }


    /*

    probably something like this?

    [ClientRpc]
    public void RpcOnDamageReceived(int amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (onDamageEffect)
            Instantiate(onDamageEffect, hitPoint, Quaternion.LookRotation(-hitNormal));
    } */
}
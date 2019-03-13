using System.Linq;
using UnityEngine;

public interface IHealthBonus
{
    int GetHealthBonus(int baseHealth);
    int GetHealthRecoveryBonus();
}

public class Health : Energy
{
    public int baseHealth = 100;

    IHealthBonus[] bonusComponents;

    void Awake()
    {
        bonusComponents = GetComponentsInChildren<IHealthBonus>();
        current = max;
    }

    public override int max
    {
        get
        {
            int bonus = bonusComponents.Sum(b => b.GetHealthBonus(baseHealth));
            return baseHealth + bonus;
        }
    }

    public override void OnEmpty()
    {
        // SendMessage of Death
    }
}
using System.Linq;
using UnityEngine;

public interface IManaBonus
{
    int GetManaBonus(int baseHealth);
    int GetManaRecoveryBonus();
}

public class Mana : Energy
{
    public int baseMana = 100;

    IManaBonus[] bonusComponents;

    void Awake()
    {
        bonusComponents = GetComponentsInChildren<IManaBonus>();
        current = max;
    }

    void Start()
    {
        current = max;
    }

    public override int max
    {
        get
        {
            int bonus = bonusComponents.Sum(b => b.GetManaBonus(baseMana));
            return baseMana + bonus;
        }
    }

    public override void OnEmpty()
    {
        // SendMessage of Death
    }
}
using System.Linq;
using UnityEngine;
using Photon.Pun;

public interface IHealthBonus
{
    int GetHealthBonus(int baseHealth);
    int GetHealthRecoveryBonus();
}

public class Health : Energy , IPunObservable
{
    public int baseHealth = 100;

    IHealthBonus[] bonusComponents;

    void Awake()
    {
        bonusComponents = GetComponentsInChildren<IHealthBonus>();
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
            int bonus = bonusComponents.Sum(b => b.GetHealthBonus(baseHealth));
            return baseHealth + bonus;
        }
    }

    public override void OnEmpty()
    {
        // SendMessage of Death
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(current);
        }
        else
        {
            current = (int)stream.ReceiveNext();
        }
    }
}
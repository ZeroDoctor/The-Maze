using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerControl : MonoBehaviourPun
{
    public void EnableComponents()
    {
        PlayerMovement movement = GetComponent<PlayerMovement>();
        PlayerAnimation animation = GetComponent<PlayerAnimation>();
        PlayerLook look = GetComponent<PlayerLook>();
        PlayerHotbar hotbar = GetComponent<PlayerHotbar>();
        Health health = GetComponent<Health>();
        Mana mana = GetComponent<Mana>();
        Combat combat = GetComponent<Combat>();

        if (photonView.IsMine == true)
        {
            movement.enabled = true;
            animation.enabled = true;
            look.enabled = true;
            hotbar.enabled = true;
            health.enabled = true;
            mana.enabled = true;
            combat.enabled = true;
        }
        else
        {
            movement.enabled = false;
            animation.enabled = false;
            look.enabled = false;
            hotbar.enabled = false;
            health.enabled = false;
            mana.enabled = false;
            combat.enabled = false;
        }
    }
}

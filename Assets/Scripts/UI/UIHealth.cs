using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    public Slider healthSlider;
    [SerializeField]
    private GameObject player;

    void Update()
    {
        Health health = player.GetComponent<Health>();
        healthSlider.value = health.Percent();
    }

}

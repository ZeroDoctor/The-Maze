using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMana : MonoBehaviour
{
    public Slider manaSlider;
    [SerializeField]
    private GameObject player;

    void Update()
    {
        Mana mana = player.GetComponent<Mana>();
        manaSlider.value = mana.Percent();
    }

}

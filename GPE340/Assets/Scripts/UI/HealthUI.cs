using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthUI : MonoBehaviour
{    
    private int currentHP;
    public PlayerData data;
    public Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHP = (int)(data.percentHP * 100);
        SetHealth(); // update health bar
    }

    public void SetHealth()
    {
        slider.maxValue = (int)data.maxHP;
        slider.value = currentHP; //Change the slider
    }
}

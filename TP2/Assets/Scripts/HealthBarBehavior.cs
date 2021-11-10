using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehavior : MonoBehaviour
{
    // Inspiré du tutoriel https://www.youtube.com/watch?v=v1UGTTeQzbo

    [SerializeField] private Slider slider;
    [SerializeField] private Color low;
    [SerializeField] private Color medium;
    [SerializeField] private Color high;
    [SerializeField] private Vector3 offset;

    private const float highColorMinValue = 0.5f; // 50%
    private const float mediumColorMinValue = 0.25f; // 25%

    private void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void SetHeatlh(float health, float maxHealth)
    {
        // Actif seulement si le magicien a perdu de la vie
        slider.gameObject.SetActive(health < maxHealth);

        slider.value = health;
        slider.maxValue = maxHealth;
        // Changement de la couleur
        if (slider.normalizedValue >= highColorMinValue)
        {
            slider.fillRect.GetComponentInChildren<Image>().color = high;

        } else if(slider.normalizedValue >= highColorMinValue)
        {
            slider.fillRect.GetComponentInChildren<Image>().color = medium;

        } else
        {
            slider.fillRect.GetComponentInChildren<Image>().color = low;
        }
        
        //slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);

    }
}

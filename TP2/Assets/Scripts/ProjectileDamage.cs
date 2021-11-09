using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private const int PROJECTILE_SPEED = 10;
    private const int LIFE_SPAN = 2; // Durée de vie du projectile en secondes

    private WizardManager wizardSource;
    private int damage;
    private Vector3 direction;
    private float timeSinceLastActivation = 0;

    private void Update()
    {
        if (timeSinceLastActivation >= LIFE_SPAN)
        {
            gameObject.SetActive(false);
            timeSinceLastActivation = 0;
            return;
        }
        transform.position += direction * Time.deltaTime * PROJECTILE_SPEED;
        timeSinceLastActivation += Time.deltaTime;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
    public void SetSource(WizardManager source)
    {
        wizardSource = source;
    }
    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public WizardManager GetSource()
    {
        return wizardSource;
    }

    public int GetDamage()
    {
        return damage;
    }
}

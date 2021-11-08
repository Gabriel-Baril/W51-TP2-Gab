using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private WizardManager wizardSource;
    private float damage;

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    public void SetSource(WizardManager source)
    {
        wizardSource = source;
    }

    public WizardManager GetSource()
    {
        return wizardSource;
    }

    public float GetDamage()
    {
        return damage;
    }
}

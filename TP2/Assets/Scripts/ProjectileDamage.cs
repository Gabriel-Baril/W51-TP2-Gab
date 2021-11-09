using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private WizardManager wizardSource;
    private int damage;
    private Vector3 direction;

    private void Update()
    {
        transform.position += direction * Time.deltaTime;
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

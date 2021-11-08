using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    private WizardManager wizardSource;

    public void SetSource(WizardManager source)
    {
        wizardSource = source;
    }

    public WizardManager GetSource()
    {
        return wizardSource;
    }
}

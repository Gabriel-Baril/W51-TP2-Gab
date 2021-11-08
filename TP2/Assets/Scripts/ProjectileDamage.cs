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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

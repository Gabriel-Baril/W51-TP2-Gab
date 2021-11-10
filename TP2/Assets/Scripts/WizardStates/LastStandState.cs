using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStandState : WizardState
{
    private void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageStateChange();
    }

    public override void Shoot()
    {
    }

    public override void Move()
    {
    }

    public override void ManageStateChange()
    {
        if(EnemyAroundCount() <= 0)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.NORMAL);
        }
        else if (wizardManager.GetLifePercentage() <= 0.0f)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.DEAD);
        }
    }
}

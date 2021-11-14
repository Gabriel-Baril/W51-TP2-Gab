using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStandState : IWizardState
{
    private const int REGENERATION_PER_SECONDS = 2;

    private new void Awake()
    {
        base.Awake();
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
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
            wizardManager.ChangeWizardState(WizardState.NORMAL);
        }
        else if (!wizardManager.IsAlive())
        {
            wizardManager.ChangeWizardState(WizardState.INACTIVE);
        }
    }
}

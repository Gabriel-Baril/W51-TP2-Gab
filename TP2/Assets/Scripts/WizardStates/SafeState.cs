using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeState : WizardState
{
    private const float REGENERATION_PER_SECONDS = 2.0f;
    private new void Awake()
    {
        base.Awake();
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
    }

    // Not shooting 
    public override void Shoot()
    {
    }

    // 
    public override void Move()
    {
    }

    public override void ManageStateChange()
    {
    }
}

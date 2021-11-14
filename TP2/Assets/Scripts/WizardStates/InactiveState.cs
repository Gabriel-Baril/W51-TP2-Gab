using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// N'est pas r�ellement un "�tat", repr�sente seulement le comportement
/// d'un magicien quand la partie est termin�e.
/// </summary>
public class InactiveState : IWizardState
{
    private const int REGENERATION_PER_SECONDS = 0;

    private new void Awake()
    {
        base.Awake();
        SetRegenerationPerSeconds(REGENERATION_PER_SECONDS);
    }

    public override void ManageStateChange(){}

    public override void Move(){}

    public override void Shoot(){}

    private new void OnTriggerEnter2D(Collider2D collision){}

    private new void OnTriggerExit2D(Collider2D collision){}
}

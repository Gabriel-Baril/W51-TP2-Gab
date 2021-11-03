using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    private WizardState wizardState;
    [SerializeField] private Team wizardTeam;
    [SerializeField] private Team wizardOpponentTeam;

    public enum WizardStateToSwitch { DEAD, ESCAPE, HIDDEN, INTREPID, NORMAL, SAFE }

    private void Awake()
    {
        wizardState = GetComponent<WizardState>();
    }

    public void ChangeWizardState(WizardStateToSwitch nextState)
    {
        Destroy(wizardState);

        switch (nextState)
        {
            case WizardStateToSwitch.DEAD:
                {
                    wizardState = gameObject.AddComponent<DeadState>() as DeadState;
                    break;
                }
            case WizardStateToSwitch.ESCAPE:
                {
                    wizardState = gameObject.AddComponent<EscapeState>() as EscapeState;
                    break;
                }
            case WizardStateToSwitch.HIDDEN:
                {
                    wizardState = gameObject.AddComponent<HiddenState>() as HiddenState;
                    break;
                }
            case WizardStateToSwitch.INTREPID:
                {
                    wizardState = gameObject.AddComponent<IntrepidState>() as IntrepidState;
                    break;
                }
            case WizardStateToSwitch.NORMAL:
                {
                    wizardState = gameObject.AddComponent<NormalState>() as NormalState;
                    break;
                }
            case WizardStateToSwitch.SAFE:
                {
                    wizardState = gameObject.AddComponent<SafeState>() as SafeState;
                    break;
                }
        }
    }

    public Team GetTeam()
    {
        return wizardTeam;
    }

    public Team GetOpponentTeam()
    {
        return wizardOpponentTeam;
    }

    public string GetOpponentTag()
    {
        if (GetTeam() == Team.BLUE)
            return Tags.GREEN_WIZARD;
        return Tags.BLUE_WIZARD;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    protected WizardState wizardState;
    protected Team wizardTeam;

    public enum WizardStateToSwitch { DEAD, ESCAPE, HIDDEN, INTREPID, NORMAL, SAFE }

    private void Awake()
    {
        wizardState = GetComponent<WizardState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Team GetTeam()
    {
        return wizardTeam;
    }
}

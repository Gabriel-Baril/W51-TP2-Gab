using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WizardState : MonoBehaviour
{
    protected WizardManager wizardManager;

    protected int healthPoint = 50;
    protected float speed;

    private void Awake()
    {
        wizardManager = GetComponent<WizardManager>();
    }

    void Start()
    {
        
    }

    void Update() {}

    public abstract void Move();
    public abstract void ManageStateChange();

}

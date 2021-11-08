using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WizardState : MonoBehaviour
{
    protected WizardManager wizardManager;

    protected int healthPoint = 50;
    protected float speed = 3;
    protected bool enemyAround = false;
    protected float targetRadius = 1.5f;

    private void Awake()
    {
        InitState();
    }

    protected void InitState()
    {
        wizardManager = GetComponent<WizardManager>();
    }

    void Update() {}

    public abstract void Shoot();
    public abstract void Move();
    public abstract void ManageStateChange();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        enemyAround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyAround = false;
    }
}

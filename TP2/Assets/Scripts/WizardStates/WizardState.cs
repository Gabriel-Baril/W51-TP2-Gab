using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WizardState : MonoBehaviour
{
    protected WizardManager wizardManager;

    protected float speed = 3;
    protected bool enemyAround = false;
    protected float targetRadius = 1.5f;

    protected void Awake()
    {
        wizardManager = GetComponent<WizardManager>();
    }

    void Update() {}

    public abstract void Shoot();
    public abstract void Move();
    public abstract void ManageStateChange();
    
    protected void OnTriggerEnter2D(Collider2D other)
    {
        enemyAround = true;
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        enemyAround = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IWizardState : MonoBehaviour
{
    protected WizardManager wizardManager;

    protected float speed = 0;
    protected int regenPerSeconds = 0;
    protected int enemyAroundCount = 0;
    protected float targetRadius = 1.5f;

    private float regenTickRate = 1f;
    private float timer = 0;
    
    protected void Awake()
    {
        wizardManager = GetComponent<WizardManager>();
    }

    void Update() 
    {
        if(timer >= regenTickRate)
        {
            timer = 0;
            wizardManager.RegenHP(regenPerSeconds);
        }

        timer += Time.deltaTime;
    }

    public abstract void Shoot();
    public abstract void Move();
    public abstract void ManageStateChange();

    public void MoveTo(GameObject target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        LookAt(target);
    }
    public void LookAt(GameObject target)
    {
        transform.up = target.transform.position - transform.position;
    }

    public void SetRegenerationPerSeconds(int regen)
    {
        regenPerSeconds = regen;
    }

    public int EnemyAroundCount()
    {
        return enemyAroundCount;
    }

    protected void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    /// <summary>
    /// Retourne vrai si la collision est causée par un magicien ennemie ET que le magicien n'est pas caché.
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    protected bool IsEnemyTargetable(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(wizardManager.GetOpponentWizardTag())) return false;
        WizardManager enemyWizard = collision.gameObject.GetComponent<WizardManager>();
        return !enemyWizard.IsHiddenInForest();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnemyTargetable(collision))
        {
            enemyAroundCount++;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (IsEnemyTargetable(collision))
        {
            enemyAroundCount--;
        }
    }
}

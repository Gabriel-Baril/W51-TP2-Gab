using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : WizardState
{
    private GameObject closestTower;
    private GameManager manager;
    [SerializeField] private GameObject lastTargetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        closestTower = manager.FindClosestTower(transform.position, wizardManager.GetOpponentTeam());


        Move();
        ManageStateChange();
    }

    public override void Move()
    {
        if (lastTargetEnemy != null)
        {
            MoveTo(lastTargetEnemy.transform.position);
            transform.right = lastTargetEnemy.transform.position - transform.position;
        }
        else
        {
            MoveTo(closestTower.transform.position);
            transform.right = closestTower.transform.position - transform.position;
        }
    }

    public override void ManageStateChange()
    {
    }

    private void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(wizardManager.GetOpponentTag()))
        {
            lastTargetEnemy = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(wizardManager.GetOpponentTag()))
        {
            lastTargetEnemy = null;
        }
    }
}

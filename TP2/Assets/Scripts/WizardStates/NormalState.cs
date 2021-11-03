using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : WizardState
{
    private GameObject closestTower;
    private GameManager manager;


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
        transform.position = Vector3.MoveTowards(transform.position, closestTower.transform.position, speed * Time.deltaTime);
    }

    public override void ManageStateChange()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.CompareTag(Tags.))
        {

        }

        enemyAround = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : WizardState
{
    private GameObject closestTower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        closestTower = GameManager.Instance.FindClosestTower(transform.position, wizardManager.GetTeam());

        Move();
        ManageStateChange();
    }

    public override void Move()
    {
        Debug.Log(closestTower.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, closestTower.transform.position, speed * Time.deltaTime);
    }

    public override void ManageStateChange()
    {
    }
}

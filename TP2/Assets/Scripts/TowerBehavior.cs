using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    private const int DEFAULT_TOWER_HEALTH = 100;

    [SerializeField] int towerHealth = DEFAULT_TOWER_HEALTH;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool IsAlive()
    {
        return towerHealth > 0;
    }
}

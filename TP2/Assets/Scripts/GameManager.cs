using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject[] blueTowers;
    [SerializeField] GameObject[] greenTowers;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public GameObject FindClosestTower(Vector3 position, Team team)
    {
        if (team == Team.BLUE) return FindClosestTowerFromArray(position, blueTowers);
        if (team == Team.GREEN) return FindClosestTowerFromArray(position, greenTowers);
        return null;
    }

    private GameObject FindClosestTowerFromArray(Vector3 position, GameObject[] towers)
    {
        GameObject closestTower = towers[0];
        float minDistance = Vector3.Distance(position, closestTower.transform.position);
        
        foreach(GameObject tower in towers)
        {
            if(Vector3.Distance(position, tower.transform.position) < minDistance)
            {
                closestTower = tower;
            }
        }
        return closestTower;
    }

    public GameObject[] GetBlueTowers()
    {
        return blueTowers;
    }

    public GameObject[] GetGreenTowers()
    {
        return greenTowers;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject[] blueTowers;
    [SerializeField] GameObject[] greenTowers;
    [SerializeField] Text[] uiTexts;

    private int blueWizardsAlive = 0;
    private int greenWizardsAlive = 0;


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
        uiTexts[0].text = blueWizardsAlive.ToString();
        uiTexts[1].text = greenWizardsAlive.ToString();
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
        
        for(int i = 1; i < towers.Length; i++)
        {
            if (Vector3.Distance(position, towers[i].transform.position) < minDistance)
            {
                closestTower = towers[i];
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

    public void AddWizardCount()
    {
        // Les magiciens apparaîssent en même temps, donc on peut mettre à jour le "ui" en même temps.
        blueWizardsAlive++;
        uiTexts[0].text = blueWizardsAlive.ToString();
        greenWizardsAlive++;
        uiTexts[1].text = greenWizardsAlive.ToString();
    }

    public void RemoveWizardCount(Team team)
    {
        if (team == Team.BLUE)
        {
            blueWizardsAlive--;
            uiTexts[0].text = blueWizardsAlive.ToString();

        }
        else
        {
            greenWizardsAlive--;
            uiTexts[1].text = greenWizardsAlive.ToString();
        }

        // A FAIRE
        // CHECKER POUR LA FIN DE LA PARTIE
    }
}

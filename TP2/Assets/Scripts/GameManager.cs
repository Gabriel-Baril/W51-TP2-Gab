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
    [SerializeField] Button x1Button;
    [SerializeField] Button x2Button;
    [SerializeField] Button x4Button;

    private int blueWizardsAlive = 0;
    private int greenWizardsAlive = 0;
    private string blankText = "";
    private string blueTeamWinsMessage = "L'�quipe bleue a gagn� !";
    private Color blue = Color.blue;
    private string greenTeamWinsMessage = "L'�quipe verte a gagn� !";
    private Color green = Color.green;

    private int minSpeed = 1;
    private int mediumSpeed = 2;
    private int maxSpeed = 4;

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
        uiTexts[2].text = blankText;

        x1Button.onClick.AddListener(() => ChangeGameSpeed(minSpeed));
        x2Button.onClick.AddListener(() => ChangeGameSpeed(mediumSpeed));
        x4Button.onClick.AddListener(() => ChangeGameSpeed(maxSpeed));
    }

    void ChangeGameSpeed(int newGameSpeed)
    {
        Time.timeScale = newGameSpeed;
    }

    public GameObject FindClosestTower(Vector3 position, Team team)
    {
        return FindClosestTowerFromArray(position, team);
    }

    private GameObject FindClosestTowerFromArray(Vector3 position, Team team)
    {
        List<GameObject> filteredTowers = GetFilteredTowers(team);
        if (filteredTowers.Count <= 0)
            return null;

        GameObject closestTower = filteredTowers[0];
        float minDistance = Vector3.Distance(position, closestTower.transform.position);
        
        for(int i = 1; i < filteredTowers.Count; i++)
        {
            if (Vector3.Distance(position, filteredTowers[i].transform.position) < minDistance)
            {
                closestTower = filteredTowers[i];
            }
        }

        return closestTower;
    }

    public GameObject[] GetBlueTowers()
    {
        return blueTowers;
    }

    public List<GameObject> GetFilteredTowers(Team team)
    {
        if (team == Team.BLUE) return GetFilteredTowersFromArray(blueTowers);
        if (team == Team.GREEN) return GetFilteredTowersFromArray(greenTowers);
        return null;
    }

    public List<GameObject> GetFilteredTowersFromArray(GameObject[] towers)
    {
        List<GameObject> filteredTowers = new List<GameObject>();
        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i].GetComponent<TowerBehavior>().IsAlive())
            {
                filteredTowers.Add(towers[i]);
            }
        }
        return filteredTowers;
    }

    public GameObject[] GetGreenTowers()
    {
        return greenTowers;
    }
    public void AddWizardCount(Team team)
    {
        if (team == Team.BLUE) AddBlueWizardCount();
        else if (team == Team.GREEN) AddGreenWizardCount();
    }

    public void AddBlueWizardCount()
    {
        blueWizardsAlive++;
        uiTexts[0].text = blueWizardsAlive.ToString();
    }

    public void AddGreenWizardCount()
    {
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

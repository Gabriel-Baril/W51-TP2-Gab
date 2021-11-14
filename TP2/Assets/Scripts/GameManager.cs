using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject[] blueTowers;
    [SerializeField] GameObject[] greenTowers;
    [SerializeField] GameObject[] forests;
    [SerializeField] Text[] uiTexts;
    [SerializeField] Button x1Button;
    [SerializeField] Button x2Button;
    [SerializeField] Button x4Button;

    private int blueWizardsAlive = 0;
    private int greenWizardsAlive = 0;
    private string blankText = "";
    private string blueTeamWinsMessage = "L'équipe bleue a gagné !";
    private string greenTeamWinsMessage = "L'équipe verte a gagné !";

    private int minSpeed = 1;
    private int mediumSpeed = 2;
    private int maxSpeed = 4;

    private bool gameOver = false;

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

    [System.Obsolete]
    private void Update()
    {
        // Le jeu se termine si le joueur appuie sur "Escape".
        if (Input.GetButtonDown("Cancel"))
        {
            // Quitte l'application si on la roule à partir de l'éditeur.
            UnityEditor.EditorApplication.isPlaying = false;
            // Quitte l'application si on la roule à partir du fichier .exe.
            Application.Quit();
        }

        // Le jeu se termine si le joueur appuie sur "Enter" ou "Spacebar".
        if (Input.GetButtonDown("Submit"))
        {
            // Recommence le jeu.
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void ChangeGameSpeed(int newGameSpeed)
    {
        Time.timeScale = newGameSpeed;
    }

    public GameObject FindClosestForest(Vector3 position)
    {
        GameObject closestForest = forests[0];
        float minDistance = Vector3.Distance(position, closestForest.transform.position);

        for (int i = 1; i < forests.Length; i++)
        {
            if (Vector3.Distance(position, forests[i].transform.position) < minDistance)
            {
                closestForest = forests[i];
            }
        }

        return closestForest;
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
    }

    // Lorsqu'une tour est détruite, vérifie si c'était la dernière de son équipe afin d'arrêter la partie.
    public void CheckGameState(Team team)
    {
        List<GameObject> filteredTowers = new List<GameObject>();

        if (team == Team.BLUE) filteredTowers = GetFilteredTowersFromArray(blueTowers);
        if (team == Team.GREEN) filteredTowers = GetFilteredTowersFromArray(greenTowers);

        // Il n'y a plus de tour
        if(filteredTowers.Count == 0)
        {
            if(team == Team.BLUE)
            {
                uiTexts[2].text = greenTeamWinsMessage;
                uiTexts[2].color = Color.green;
                
            } else
            {
                uiTexts[2].text = blueTeamWinsMessage;
                uiTexts[2].color = Color.blue;
            }

            // Rendre tous les magiciens inactifs
            GameObject.FindObjectOfType<WizardSpawner>().StopAllWizards();
            gameOver = true;
        }
    }

    public bool GameOver()
    {
        return gameOver;
    }
}

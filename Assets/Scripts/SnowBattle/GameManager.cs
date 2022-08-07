using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool weAreInPause = false;
    public int numberOfPlayers = 1;
    public PlayerData player1ScriptableObject;
    public PlayerData player2ScriptableObject;
    public PlayerData player3ScriptableObject;
    public PlayerData player4ScriptableObject;
    public GameObject canvas;
    public GameObject playerPrefab;
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;
    public GameObject squareMenuPrefab;
    private GameObject squareMenu;
    public GameObject rulesPrefab;
    private GameObject rules;
    public GameObject snowBattleTitlePrefab;
    private GameObject snowBattleTitle;
    public GameObject leftTeamWonPrefab;
    private GameObject leftTeamWon;
    public GameObject rightTeamWonPrefab;
    private GameObject rightTeamWon;
    public GameObject retryPrefab;
    private GameObject retry;
    public MenuData menuData;
    public GameObject quitPrefab;
    private GameObject quit;
    private bool gameHasStarted = false;
    private bool gameEnded = false;
    private List<List<GameObject>> teams;


    // Start is called before the first frame update
    void Start()
    {
        numberOfPlayers = menuData.numberOfPlayers;
        squareMenu = Instantiate(squareMenuPrefab, canvas.transform);
        snowBattleTitle = Instantiate(snowBattleTitlePrefab, canvas.transform);
        rules = Instantiate(rulesPrefab, canvas.transform);
        rules.SetActive(false);
        leftTeamWon = Instantiate(leftTeamWonPrefab, canvas.transform);
        leftTeamWon.SetActive(false);
        rightTeamWon = Instantiate(rightTeamWonPrefab, canvas.transform);
        rightTeamWon.SetActive(false);
        quit = Instantiate(quitPrefab, canvas.transform);
        quit.SetActive(false);
        quit.GetComponent<Button>().onClick.AddListener(Application.Quit);
        StartCoroutine(EnableMe(rules, 2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHasStarted == true)
        {
            if(gameEnded == false)
            {
                // Echap mode
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    if (weAreInPause == false)
                    {
                        PauseGame();
                        weAreInPause = true;
                    }
                    else if (weAreInPause == true)
                    {
                        ResumeGame();
                        weAreInPause = false;
                    }
                }

                if (teams != null)
                {
                    if (teams[0].LongCount(teamPlayer => teamPlayer.GetComponent<PlayerMovement>().youAreDead == true) == teams[0].Count)
                    {
                        RightTeamWonScreen();
                        gameEnded = true;
                    }
                    if (teams[1].LongCount(teamPlayer => teamPlayer.GetComponent<PlayerMovement>().youAreDead == true) == teams[1].Count)
                    {
                        LeftTeamWonScreen();
                        gameEnded = true;
                    }
                }
            }
        }
        

        // Cinematic before playing, with title and rules
        if(Input.GetKeyDown(KeyCode.Space) && gameHasStarted != true)
        {
            if (rules.activeSelf == true)
            {
                rules.GetComponent<TypeWriterUI>().timeBtwChars = 0.01f;
                
                if (rules.GetComponent<TypeWriterUI>().isOver == true)
                {
                    rules.SetActive(false);
                    squareMenu.SetActive(false);
                    snowBattleTitle.SetActive(false);
                    StartCoroutine(SpawnPlayers());
                    gameObject.GetComponent<AudioSource>().enabled = true;
                    gameHasStarted = true;
                }
            }
        }
    }

    private IEnumerator EnableMe(GameObject gobj, float secs)
    {
        yield return new WaitForSeconds(secs);
        gobj.SetActive(true);
    }

    private IEnumerator SpawnPlayers()
    {
        yield return new WaitForSeconds(3);
        if(numberOfPlayers == 1)
        {
            player1 = Instantiate(playerPrefab,  new Vector3(-7f, 0f, 0f) , Quaternion.identity);
            player1.GetComponent<PlayerMovement>().playerData = player1ScriptableObject;
            player1.GetComponent<PlayerMovement>().playerData.playerNumber = 1;
        }
        else if(numberOfPlayers == 2)
        {
            player1 = Instantiate(playerPrefab,  new Vector3(-7f, 0f, 0f) , Quaternion.identity);
            player1.GetComponent<PlayerMovement>().playerData = player1ScriptableObject;
            player1.GetComponent<PlayerMovement>().playerData.playerNumber = 1;

            player2 = Instantiate(playerPrefab,  new Vector3(7f, 0f, 0f) , Quaternion.identity);
            player2.GetComponent<PlayerMovement>().playerData = player2ScriptableObject;
            player2.GetComponent<PlayerMovement>().playerData.playerNumber = 2;

            teams = new List<List<GameObject>>{{new List<GameObject>{player1}},{new List<GameObject>{player2}}};
        }
        else if(numberOfPlayers == 4)
        {
            player1 = Instantiate(playerPrefab,  new Vector3(-7f, 1f, 0f) , Quaternion.identity);
            player1.GetComponent<PlayerMovement>().playerData = player1ScriptableObject;
            player1.GetComponent<PlayerMovement>().playerData.playerNumber = 1;

            player2 = Instantiate(playerPrefab,  new Vector3(7f, 1f, 0f) , Quaternion.identity);
            player2.GetComponent<PlayerMovement>().playerData = player2ScriptableObject;
            player2.GetComponent<PlayerMovement>().playerData.playerNumber = 2;

            player3 = Instantiate(playerPrefab,  new Vector3(-7f, -1f, 0f) , Quaternion.identity);
            player3.GetComponent<PlayerMovement>().playerData = player3ScriptableObject;
            player3.GetComponent<PlayerMovement>().playerData.playerNumber = 3;

            player4 = Instantiate(playerPrefab,  new Vector3(7f, -1f, 0f) , Quaternion.identity);
            player4.GetComponent<PlayerMovement>().playerData = player4ScriptableObject;
            player4.GetComponent<PlayerMovement>().playerData.playerNumber = 4;

            teams = new List<List<GameObject>>{{new List<GameObject>{player1, player3}},{new List<GameObject>{player2, player4}}};
        }
    }


    private void PauseGame()
    {
        Time.timeScale = 0;
        squareMenu.SetActive(true);
        snowBattleTitle.SetActive(true);
        quit.SetActive(true);
        gameObject.GetComponent<AudioSource>().Pause();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        squareMenu.SetActive(false);
        snowBattleTitle.SetActive(false);
        quit.SetActive(false);
        gameObject.GetComponent<AudioSource>().UnPause();
    }

    private void LeftTeamWonScreen()
    {
        PauseGame();
        squareMenu.SetActive(true);
        leftTeamWon.SetActive(true);
        retry = Instantiate(retryPrefab, canvas.transform);
        retry.GetComponent<Button>().onClick.AddListener(reloadScene);
    }

    private void RightTeamWonScreen()
    {
        Time.timeScale = 0;
        squareMenu.SetActive(true);
        rightTeamWon.SetActive(true);
        retry = Instantiate(retryPrefab, canvas.transform);
        retry.GetComponent<Button>().onClick.AddListener(reloadScene);
    }

    public void reloadScene()
    {
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}


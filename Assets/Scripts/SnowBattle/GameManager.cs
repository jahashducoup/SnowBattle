using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool weAreInPause = false;
    public int numberOfPlayers = 1;
    public GameObject canvas;
    public GameObject playerPrefab1;
    private GameObject player1;
    public GameObject playerPrefab2;
    private GameObject player2;
    public GameObject playerPrefab3;
    private GameObject player3;
    public GameObject playerPrefab4;
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
    private bool gameHasStarted = false;
    private List<List<GameObject>> teams;



    // Start is called before the first frame update
    void Start()
    {
        squareMenu = Instantiate(squareMenuPrefab, canvas.transform);
        snowBattleTitle = Instantiate(snowBattleTitlePrefab, canvas.transform);
        rules = Instantiate(rulesPrefab, canvas.transform);
        rules.SetActive(false);
        StartCoroutine(EnableMe(rules, 2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHasStarted == true)
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
                }
                if (teams[1].LongCount(teamPlayer => teamPlayer.GetComponent<PlayerMovement>().youAreDead == true) == teams[1].Count)
                {
                    LeftTeamWonScreen();
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
            player1 = Instantiate(playerPrefab1,  new Vector3(-7f, 0f, 0f) , Quaternion.identity);
        }
        else if(numberOfPlayers == 2)
        {
            player1 = Instantiate(playerPrefab1,  new Vector3(-7f, 0f, 0f) , Quaternion.identity);

            player2 = Instantiate(playerPrefab2,  new Vector3(7f, 0f, 0f) , Quaternion.identity);
            player2.GetComponent<PlayerMovement>().reloadKey = KeyCode.Y;
            player2.GetComponent<PlayerMovement>().shootKey = KeyCode.R;
            player2.GetComponent<PlayerMovement>().crouchingKey = KeyCode.N;
            teams = new List<List<GameObject>>{{new List<GameObject>{player1}},{new List<GameObject>{player2}}};
        }
        else if(numberOfPlayers == 4)
        {
            player1 = Instantiate(playerPrefab1,  new Vector3(-7f, 1f, 0f) , Quaternion.identity);

            player2 = Instantiate(playerPrefab2,  new Vector3(7f, 1f, 0f) , Quaternion.identity);
            player2.GetComponent<PlayerMovement>().reloadKey = KeyCode.Y;
            player2.GetComponent<PlayerMovement>().shootKey = KeyCode.R;
            player2.GetComponent<PlayerMovement>().crouchingKey = KeyCode.N;

            player3 = Instantiate(playerPrefab3,  new Vector3(-7f, -1f, 0f) , Quaternion.identity);
            player3.GetComponent<PlayerMovement>().reloadKey = KeyCode.O;
            player3.GetComponent<PlayerMovement>().shootKey = KeyCode.U;
            player3.GetComponent<PlayerMovement>().crouchingKey = KeyCode.Period;

            player4 = Instantiate(playerPrefab4,  new Vector3(7f, -1f, 0f) , Quaternion.identity);
            player4.GetComponent<PlayerMovement>().reloadKey = KeyCode.Alpha1;
            player4.GetComponent<PlayerMovement>().shootKey = KeyCode.Alpha2;
            player4.GetComponent<PlayerMovement>().crouchingKey = KeyCode.Alpha0;
            teams = new List<List<GameObject>>{{new List<GameObject>{player1, player3}},{new List<GameObject>{player2, player4}}};
        }
    }


    private void PauseGame()
    {
        Time.timeScale = 0;
        squareMenu.SetActive(true);
        snowBattleTitle.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        squareMenu.SetActive(false);
        snowBattleTitle.SetActive(false);
    }

    private void LeftTeamWonScreen()
    {
        PauseGame();
        squareMenu.SetActive(true);
        leftTeamWon.SetActive(true);
    }

    private void RightTeamWonScreen()
    {
        Time.timeScale = 0;
        squareMenu.SetActive(true);
        rightTeamWon.SetActive(true);
    }


}


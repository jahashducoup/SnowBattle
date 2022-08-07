using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button p2Button;
    public Button p4Button;
    public Button quit;
    public MenuData menuData;
    public Toggle windowMode;
    public Slider volume;

    // Start is called before the first frame update
    void Start()
    {
        p2Button.GetComponent<Button>().onClick.AddListener(players2Fight);
        p4Button.GetComponent<Button>().onClick.AddListener(players4Fight);
        quit.GetComponent<Button>().onClick.AddListener(Application.Quit);
        windowMode.onValueChanged.AddListener(delegate {switchWindowMode();});
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = volume.value;
    }

    public void players2Fight()
    {
        menuData.numberOfPlayers = 2;
        SceneManager.LoadScene("Game_SnowBattle");
    }

    public void players4Fight()
    {
        menuData.numberOfPlayers = 4;
        SceneManager.LoadScene("Game_SnowBattle");
    }

    public void switchWindowMode()
    {
        if (windowMode.isOn == true)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else if (windowMode.isOn == false)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }
}

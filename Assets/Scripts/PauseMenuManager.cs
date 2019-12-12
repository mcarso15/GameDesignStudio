using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;

    
    public Button playButton;

    public Button mainButton;
    public bool isShow;

    public GameController gameController;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")){
            isShow = !isShow;
            pauseMenu.SetActive(isShow);
        }
    }

    void Start(){
        gameController = GameObject.FindWithTag("MainCamera").GetComponent<GameController>();
    }
    public void PlayGame(){
        isShow = !isShow;
        pauseMenu.SetActive(isShow);
    }

    public void GoMainMenu(){
        gameController.MainMenu();
    }

    public void InitializeMenu(){
        pauseMenu.SetActive(true);
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(PlayGame);

        mainButton = GameObject.Find("MainButton").GetComponent<Button>();
        mainButton.onClick.AddListener(GoMainMenu);
        pauseMenu.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool canTime = false;

    private playerController pc;
    private enemyController ec;

    private PauseMenuManager pauseMenuManager;

    private int enemyCount;
    private List<GameObject> enemies = new List<GameObject>();

    private int activeSceneIndex;

    public int numScenes = 9;

    public int sceneWaitTime = 3;

    public bool isPaused;

    public bool playerDead = false;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindWithTag("Player").GetComponent<playerController>();
        ec = GameObject.FindWithTag("Enemy").GetComponent<enemyController>();
        pauseMenuManager = GameObject.Find("PauseManager").GetComponent<PauseMenuManager>();

        //Populate the list of enemies
        foreach(GameObject enemyBall in GameObject.FindGameObjectsWithTag("Enemy")){            
            enemies.Add(enemyBall);
        }

        enemyCount = enemies.Count;

        //Get active scene index
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

        isPaused = pauseMenuManager.isShow;

        pauseMenuManager.InitializeMenu();
    }

    // Update is called once per frame
    void Update()
    {
        canTime = pc.GetCanTime();

        if(enemyCount == 0){
            //Start the waiting coroutine which will handle scene transition
            StartCoroutine(WaitBeforeTransition());            
        }

        isPaused = pauseMenuManager.isShow;
    }

    IEnumerator WaitBeforeTransition(){
        yield return new WaitForSeconds(sceneWaitTime);
        ChangeScene();
        
    }

    public bool GetCanTime(){
        return canTime;
    }

    public void EnemyDeath(){
        enemyCount--;
    }

    void ChangeScene(){
        print(playerDead);
        if(playerDead){
            ResetScene();
        }
        else if(activeSceneIndex < numScenes){
                SceneManager.LoadScene(activeSceneIndex + 1);
            }

        else{
                SceneManager.LoadScene(0); //Scene ID 0 is the Main Menu
            }
    }

    void ResetScene(){
        print("I am not");
        SceneManager.LoadScene(activeSceneIndex);
    }

    public void PlayerDeath(){
        playerDead = true;
        StartCoroutine(WaitBeforeTransition());
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}

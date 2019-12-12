using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenController : MonoBehaviour
{
    // Start is called before the first frame update

    public Button playButton;

    void Start(){
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(PlayGame);
    }
    public void PlayGame(){
        SceneManager.LoadScene(0);
    }

}

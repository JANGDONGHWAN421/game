using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Uimanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string a)
    {
        switch (a)
        {
            case "ch":
                SceneManager.LoadScene("MainMenuScene");
                break;
            case "Setting":
                SceneManager.LoadScene("SettingScene");
                break;
            case "Newgame":
                SceneManager.LoadScene("GameScene");
                break;
            case "Raking":
                SceneManager.LoadScene("RakingScene");
                break;
            case "Quit":
                SceneManager.LoadScene("QuitScene");
                break;
        }

    }
}

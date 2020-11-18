using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Uimanager : MonoBehaviour
{
    public bool isPause = false;
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


        
    }

    public void OnclickLoad(string a)
    {
        switch (a)
        {
            case "ch":
                SceneManager.LoadScene("MainMenuScene");
                Debug.Log("MainMenuScene가 클릭 되었습니다.");
                break;
            case "Setting":
                SceneManager.LoadScene("SettingScene");
                Debug.Log("SettingScene 클릭 되었습니다.");
                break;
            case "Newgame":
                SceneManager.LoadScene("MainMenuScene");
                Debug.Log("GameScene 클릭 되었습니다.");
                break;
            case "Raking":
                SceneManager.LoadScene("RakingScene");
                Debug.Log("RakingScene 클릭 되었습니다.");
                break;
           
        }

    }

    public void newGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void endGame()
    {
        Application.Quit();
    }


    
public void PauseGame(string b)
    {
        switch (b)
        {
            case "PauseGame":
                Time.timeScale = 0;
                isPause = true;
                PauseMenu.SetActive(true);
                Debug.Log("PauseGame가 클릭 되었습니다.");
                
                
                break; 
        }

    }

public void ReturnGame()
    {
       
                Time.timeScale = 1;
                isPause = false;
                PauseMenu.SetActive(false);
                Debug.Log("return 클릭 되었습니다.");
    }
    /*
    public void Left(string L)
    {
        switch (L)
        {
            case "L":
                Event.KeyboardEvent(KeyCode.LeftArrow.ToString());
                Debug.Log("LeftArrow key is pressed.");
                break;
        }
       
            
    }
    public void Right(string R)
    {
        switch (R)
        {
            case "R":
                Event.KeyboardEvent(KeyCode.RightArrow.ToString());
                Debug.Log("LeftArrow key is pressed.");
                break;
        }
        
    }
    public void Jump()
    {

        
        Debug.Log("Jump key is pressed.");

    }*/
}

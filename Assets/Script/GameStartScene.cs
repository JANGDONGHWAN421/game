using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class GameStartScene : MonoBehaviour
{
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
            case "ch" :          
                SceneManager.LoadScene("MainMenuScene");
                break;
      }
        
    }

   
}

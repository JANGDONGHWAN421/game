using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //게임 메니저는 점수 관리 및 스테이지 관리.

    public int totalPoint;
    public int stagePoint;
    public int stageindex;
    public int Health;
    public PlayerMove player;
    public GameObject[] Stages;
    public string btnchange;

    //UI관리
    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartBtn;
   





    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }



    public void NextStage()
    {
        Debug.Log("스테이지 인덱스 = " + stageindex);
        if (stageindex < Stages.Length - 1)
        {
            //스테이지 변환
            Stages[stageindex].SetActive(false);
            stageindex = stageindex + 1;
            Stages[stageindex].SetActive(true);
            PlayerReposion();

            UIStage.text = "STAGE" + (stageindex +1); 
        }
        else
        {
            //게임 클리어시!
            Time.timeScale = 0;
            //디버그찍고
            Debug.Log("게임클리어");
            // 다시 시작하기 위한 것 다만 클리어랑 리트라이 다시 확인 하기 위해!
            Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Game Clear!";
            btnchange = btnText.text;
            UIRestartBtn.SetActive(true);
        }

        //총점 계산!
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (Health > 1) 
        {
            Health--;
            UIhealth[Health].color = new Color(1,0,0,0.2f);
        }
        
        else
        {
            UIhealth[0].color = new Color(1, 0, 0, 0.2f);

            player.OnDie();

            UIRestartBtn.SetActive(true);
        }
    }


    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 화면 밖으로 떨어진다면.... 그때 목숨 깎는 코드
        if (collision.gameObject.tag == "Player")
        {
            // 목숨깎는 것은 기본.
            if (Health > 1) 
            {
                //떨어졌으니 원래 출발선으로 복귀 시키기
                //collision.transform.position을 새로 만들어서 돌아가게 하는것.
                PlayerReposion();
            }
             
            HealthDown();

        }

    }

    void PlayerReposion()
    {
        
        player.transform.position = new Vector3(-6.75f, 5.0f, 0);

        player.VelocityZero();
    }

    public void Restart()
    {
        if (btnchange == "Game Clear!")
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("MainMenuScene");
        }

        else
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GameScene");
        }
        
    }


    
        
    
}

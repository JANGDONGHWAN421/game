using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //게임 메니저는 점수 관리 및 스테이지 관리.

    public int totlaPoint;
    public int stagePoint;
    public int stageindex;

    public void NextStage()
    {
        stageindex++;
        totlaPoint += stagePoint;
        stagePoint = 0;
    }

    
    void Update()
    {
        
    }
}

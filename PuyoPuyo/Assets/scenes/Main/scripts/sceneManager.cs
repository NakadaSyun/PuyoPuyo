using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    [SerializeField]Share_Variable SV;
    

    //リザルトシーンへ飛ばす関数
    public void GotoResult()
    {
        SV.Set_Score(gameObject.GetComponent<ScoreManager>().Get_Score());

        SceneManager.LoadScene("result", LoadSceneMode.Single);
    }
}

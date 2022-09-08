using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class resultManager : MonoBehaviour
{
    [SerializeField]Share_Variable SV;
    int score;

    [SerializeField] Text score_text;

    // Start is called before the first frame update
    void Start()
    {
        score = SV.Get_Score();
        score_text.text = "SCORE : " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //リトライボタンを押した際の関数呼び出し
    public void OncrickRetry()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }


    //タイトルボタンを押した際の関数呼び出し
    public void OncrickTitle()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }


    //Exitボタンを押した際の関数呼び出し
    public void OncrickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}

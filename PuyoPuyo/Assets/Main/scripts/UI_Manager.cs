using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]Text stage_info;
    [SerializeField]Text chain_text;

    [SerializeField] Text score_text;
    PuyoManager PM;
    ScoreManager SM;
    // Start is called before the first frame update
    void Start()
    {
        PM = GameObject.Find("PuyoManager").GetComponent<PuyoManager>();
        SM = this.gameObject.GetComponent<ScoreManager>();

    }

    // Update is called once per frame
    void Update()
    {
        stage_view();

        UI_view();
    }

    void stage_view()
    {
        int[,] st_info = PM.get_Stage_info();

        string hoge = "";

        for (int i = 13; i >= 0; i--)
        {
            for (int j = 0; j < 6; j++)
            {
                if(st_info[j, i] > 0)
                {
                    hoge += st_info[j, i] + " ";
                }
                else
                {
                    hoge += "+ ";
                }
            }
            hoge += "\n";
        }

        stage_info.text = hoge;
    }

    void UI_view()
    {
        Score_view();
        chain_view();
    }

    public void chain_view()
    {
        chain_text.text = PM.Get_chain() + "連鎖";
    }
    public void Score_view()
    {
        int score = SM.Get_Score();

        score_text.text = "SCORE\n" + score.ToString();

    }
}

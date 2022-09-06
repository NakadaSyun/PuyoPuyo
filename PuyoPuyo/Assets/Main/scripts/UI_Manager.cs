using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]Text stage_info;
    PuyoManager PM;
    // Start is called before the first frame update
    void Start()
    {
        PM = GameObject.Find("PuyoManager").GetComponent<PuyoManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        stage_view();
    }

    void stage_view()
    {
        int[,] st_info = PM.get_Stage_info();

        string hoge = "";

        for (int i = 13; i >= 0; i--)
        {
            for(int j = 0; j < 6; j++)
            {
                hoge += st_info[j, i] + " ";
            }
            hoge += "\n";
        }

        stage_info.text = hoge;
    }
}

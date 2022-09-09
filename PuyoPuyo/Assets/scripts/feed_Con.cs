using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class feed_Con : MonoBehaviour
{
    private Color orgColor;
    private float Alpha;
    public bool IsNextScene;        //次のシーンへの遷移を開始

    // Start is called before the first frame update
    void Start()
    {
        IsNextScene = false;
        Alpha = 1f;
        orgColor = new Color(0, 0, 0, Alpha);
        GetComponent<Image>().color = orgColor;

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsNextScene) FeedIn();
        else FeedOut();


        GetComponent<Image>().color = new Color(0, 0, 0, Alpha);
    }

    public void Is_feedout()
    {
        IsNextScene = true;
    }


    void FeedIn()
    {
        if (Alpha > 0)
        {
            Alpha -= Time.deltaTime / 1.5f;
            orgColor = new Color(0, 0, 0, Alpha);
        }
        else
        {
            //フェードの終了
        }
    }

    public void FeedOut()
    {
        if (Alpha < 1f)
        {
            Alpha += Time.deltaTime / 1.5f;
            orgColor = new Color(0, 0, 0, Alpha);
        }
    }
}

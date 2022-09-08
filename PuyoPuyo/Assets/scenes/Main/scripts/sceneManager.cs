using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    [SerializeField]Share_Variable SV;
    [SerializeField]Color_Order CO;

    //色数取得用
    [SerializeField] mode_vari MV;
    private void Start()
    {
        CO.Mk_color_num(1000);      //ぷよの色を作成
        CO.List_reset(MV.Get_color_max());
    }

    //リザルトシーンへ飛ばす関数
    public void GotoResult()
    {
        SV.Set_Score(gameObject.GetComponent<ScoreManager>().Get_Score());

        SceneManager.LoadScene("result", LoadSceneMode.Single);
    }
}

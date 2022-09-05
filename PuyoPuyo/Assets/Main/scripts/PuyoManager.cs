using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoManager : MonoBehaviour
{
    //ステージマスの大きさ
    Vector2Int STAGE_MASS = new Vector2Int(14,6);

    //落ちる量 1f=1マス
    float down_vol = 0.25f;

    //ステージのぷよ用格納配列 縦,14マス 横6マス
    private int[,] Stage_puyo = new int[14,6];

    private GameObject[] active_puyo = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        stage_mass_Init();

        active_puyo = this.gameObject.GetComponent<PuyoRespown>().Respown();
        set_active_puyo_Position();


        StartCoroutine(Puyo_Down());
    }

    // Update is called once per frame
    void Update()
    {
        //右矢印キーを押したとき
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Active_Puyo_RightMove();
        }
        //左矢印キーを押したとき
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Active_Puyo_RightMove();
        }
        //上矢印キーを押したとき
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {

        }
        //下矢印キーを押したとき
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {

        }
    }

    private void FixedUpdate()
    {
    }

    //ぷよ右移動
    void Active_Puyo_RightMove()
    {
        if (!Is_con()) return;  //操作可能時のみ

        //原点のぷよのオブジェクト(一番右側のぷよ)
        GameObject ori_puyo = active_puyo[0];
        if(active_puyo[0].transform.position.x < active_puyo[1].transform.position.x)
        {
            ori_puyo = active_puyo[1];
        }

        if(ori_puyo.transform.position.x <= 4)
        {
            //active_puyo[0].transform.position.x
        }



    }


    void stage_mass_Init()
    {
        //ステージマス上のデータの初期化
        for (int i = 0; i < STAGE_MASS.x; i++)
        {
            for (int j = 0; j < STAGE_MASS.y; j++)
            {
                Stage_puyo[i, j] = 0;
            }
        }
    }

    //ぷよの初期位置の設定
    void set_active_puyo_Position()
    {
        active_puyo[0].transform.position = new Vector3(2.0f, 14.0f, 0.0f);
        active_puyo[1].transform.position = new Vector3(2.0f, 13.0f, 0.0f);
    }

    //ぷよの落下処理
    private IEnumerator Puyo_Down(float delayTime = 0.2f)
    {
        yield return new WaitForSeconds(delayTime);

        if(active_puyo[0] != null && active_puyo[1] != null)
        {
            Vector3 down_vec = new Vector3(0.0f, down_vol, 0.0f);

            active_puyo[0].transform.position -= down_vec;
            active_puyo[1].transform.position -= down_vec;

            //ぷよの着地判定
            if (Is_Puyo_Landing(active_puyo[0]))
            {
                Puyo_Landing(0);
            }
            else if (Is_Puyo_Landing(active_puyo[1]))
            {
                Puyo_Landing(1);
            }
            else
            {
                //どちらのぷよも着地していなければ再帰処理
                StartCoroutine(Puyo_Down());
            }
            
        }
        
    }

    //ぷよの着地判定処理
    private bool Is_Puyo_Landing(GameObject puyo)
    {
        //posから配列の場所を特定
        Vector2Int pos = new Vector2Int((int)puyo.transform.position.x, (int)puyo.transform.position.y);

        //一番下のマスに来た場合
        if (puyo.transform.position.y == 0.0f)
        {
            return true;
        }

        //ぷよの一つ下にぷよがあった場合　かつ　マス目にちょうどの時
        if (puyo.transform.position.y % 1.0f == 0　
            && Stage_puyo[pos.y - 1, pos.x] != 0)
        {
            return true;
        }

        return false;
    }

    //ぷよの着地処理
    private void Puyo_Landing(int puyo_num = 99)
    {
        //最初のぷよが落ちた時
        if(Is_con())
        {
            Vector2Int pos = new Vector2Int((int)active_puyo[puyo_num].transform.position.x, (int)active_puyo[puyo_num].transform.position.y);

            Stage_puyo[pos.y, pos.x] = active_puyo[puyo_num].GetComponent<Puyo_Color>().Get_Color();
            
            //着地したぷよを削除
            active_puyo[puyo_num] = null;

            //着地していないほうのぷよの探索
            int num = 1 - puyo_num;

            if (Is_Puyo_Landing(active_puyo[num]))
            {
                Puyo_Landing(num);
            }
            else
            {
                //着地していないぷよの下に空きがある場合
                StartCoroutine(Puyo_Solo_Down(active_puyo[num]));
            }


        }

        //2個目のぷよが落ちた時
        else if(active_puyo[0] != null)
        {
            Second_Puyo_Landing(0);
        }
        else if (active_puyo[1] != null)
        {
            Second_Puyo_Landing(1);
        }
    }

    void Second_Puyo_Landing(int num)
    {
        Vector2Int pos = new Vector2Int((int)active_puyo[num].transform.position.x, (int)active_puyo[num].transform.position.y);

        Stage_puyo[pos.y, pos.x] = active_puyo[num].GetComponent<Puyo_Color>().Get_Color();

        //着地したぷよを削除
        active_puyo[num] = null;

        //操作可能なぷよが全部落ちたので新しいぷよを呼び出し
        active_puyo = this.gameObject.GetComponent<PuyoRespown>().Respown();
        set_active_puyo_Position();


        StartCoroutine(Puyo_Down());
        
    }

    //
    private IEnumerator Puyo_Solo_Down(GameObject puyo)
    {
        //操作時よりは早く落ちる
        yield return new WaitForSeconds(0.1f);
        if (Is_Puyo_Landing(puyo))
        {
            Puyo_Landing();
            yield break;
        }

        Vector3 down_vec = new Vector3(0.0f, down_vol, 0.0f);

        puyo.transform.position -= down_vec;
        
    }

    //どちらも着地していないとき
    bool Is_con()
    {
        if(active_puyo[0] != null && active_puyo[1] != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

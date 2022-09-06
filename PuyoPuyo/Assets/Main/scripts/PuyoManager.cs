using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoManager : MonoBehaviour
{
    //ステージマスの大きさ
    Vector2Int STAGE_MASS = new Vector2Int(6, 14);

    //落ちる量 1f=1マス
    float down_vol = 0.25f;

    //ステージのぷよ用格納配列 縦,14マス 横6マス
    private int[,] Stage_puyo = new int[6, 14];

    private GameObject[] active_puyo = new GameObject[2];

    private float down_delay = 0.2f;

    private Vector2Int[] last_puyos_pos = new Vector2Int[2];          //最後に置いたぷよの場所(２つ)

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
        Key_input();        //キー入力があった時の処理


    }

    private void FixedUpdate()
    {
    }

    void Key_input()
    {
        if (Is_con())       //両方のぷよが落下していないときのみ
        {
            //右矢印キーを押したとき
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Active_Puyo_RightMove();        //右に移動
            }
            //左矢印キーを押したとき
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Active_Puyo_LeftMove();         //左に移動
            }
            //上矢印キーを押したとき
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Active_Puyo_RotMove();          //ぷよを回転
            }

            //下矢印キーを押したとき
            if (Input.GetKey(KeyCode.DownArrow))
            {
                down_delay = 0.05f;             //落下速度上昇(時間間隔を短く)
            }
            else 
            {
                down_delay = 0.2f;            //離したら初期値に
            }

        }
    }

    //ぷよ右移動
    void Active_Puyo_RightMove()
    {
        if (!Is_Right_hit(Get_Puyos_Pos()))
        {
            active_puyo[0].transform.position += new Vector3(1.0f, 0.0f, 0.0f);
            active_puyo[1].transform.position += new Vector3(1.0f, 0.0f, 0.0f);
        }
    }

    void Active_Puyo_LeftMove()
    {
        if (!Is_Left_hit(Get_Puyos_Pos()))
        {
            active_puyo[0].transform.position -= new Vector3(1.0f, 0.0f, 0.0f);
            active_puyo[1].transform.position -= new Vector3(1.0f, 0.0f, 0.0f);
        }

    }

    Vector2[] Get_Puyos_Pos()
    {
        Vector2[] puyos = new Vector2[2];
        puyos[0] = active_puyo[0].transform.position;
        puyos[1] = active_puyo[1].transform.position;

        return puyos;
    }


    void Active_Puyo_RotMove()
    {
        //下側のぷよ(１番目)を起点に時計回りで回転    0番目のぷよを移動

        Vector2 move_puyo = active_puyo[0].transform.position;

        //0番目のぷよの場所
        Vector2 dif = active_puyo[0].transform.position - active_puyo[1].transform.position;


        if (dif.y != 0)
        {
            Vector2 puyo = new Vector2(move_puyo.x + dif.y, move_puyo.y - dif.y);

            //ぶつかったら移動しない
            if (dif.y > 0)
            { //右への当たり判定

                if (puyo.x < STAGE_MASS.x)
                {
                    if (Stage_puyo[(int)puyo.x, (int)puyo.y] != 0)
                    {
                        return;
                    }
                }
                else { return; }
            }
            else
            {
                //左への当たり判定
                if (puyo.x >= 0)
                {
                    if (Stage_puyo[(int)puyo.x, (int)puyo.y] != 0)
                    {
                        return;
                    }
                }
                else { return; }
            }

            //上から右に　と　下から左に
            active_puyo[0].transform.position = new Vector2(move_puyo.x + dif.y, move_puyo.y - dif.y);
        }
        else
        {
            //下に行くとき
            if (dif.x == 1)
            {
                Vector2Int pos = new Vector2Int((int)active_puyo[1].transform.position.x, (int)active_puyo[1].transform.position.y);
                if (pos.y != 0)
                {
                    //下のマス
                    if (Stage_puyo[pos.x, pos.y - 1] != 0)
                    {
                        return;
                    }
                }
                else { return; }
            }

            //右から下に と　左から上に
            active_puyo[0].transform.position = new Vector2(move_puyo.x - dif.x, move_puyo.y - dif.x);
        }
    }

    //右側用の当たり判定
    bool Is_Right_hit(Vector2[] puyo)
    {//原点のぷよのオブジェクト(一番右側のぷよ)
        Vector2 ori_puyo = puyo[0];
        if (puyo[0].x < puyo[1].x)
        {
            ori_puyo = puyo[1];
        }

        //ステージの範囲内で
        if (ori_puyo.x < STAGE_MASS.x - 1)
        {
            //両方の右のマス上に遮るものがないとき
            Vector2Int pos1 = new Vector2Int((int)puyo[0].x, (int)puyo[0].y);
            Vector2Int pos2 = new Vector2Int((int)puyo[1].x, (int)puyo[1].y);
            if (Stage_puyo[pos1.x + 1, pos1.y] == 0 && Stage_puyo[pos2.x + 1, pos2.y] == 0)
            {
                return false;
            }
        }
        return true;
    }

    //左側用の当たり判定 ぶつかるとtrue
    bool Is_Left_hit(Vector2[] puyo)
    {//原点のぷよのオブジェクト(一番左側のぷよ)
        Vector2 ori_puyo = puyo[0];
        if (puyo[0].x > puyo[1].x)
        {
            ori_puyo = puyo[1];
        }

        //ステージの範囲内で
        if (ori_puyo.x > 0)
        {
            //両方の左のマス上に遮るものがないとき
            Vector2Int pos1 = new Vector2Int((int)puyo[0].x, (int)puyo[0].y);
            Vector2Int pos2 = new Vector2Int((int)puyo[1].x, (int)puyo[1].y);
            if (Stage_puyo[pos1.x - 1, pos1.y] == 0 && Stage_puyo[pos2.x - 1, pos2.y] == 0)
            {
                return false;
            }
        }
        return true;
    }


    void stage_mass_Init()
    {
        //ステージマス上のデータの初期化
        for (int i = 0; i < STAGE_MASS.y; i++)
        {
            for (int j = 0; j < STAGE_MASS.x; j++)
            {
                Stage_puyo[j, i] = 0;
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
    private IEnumerator Puyo_Down()
    {
        yield return new WaitForSeconds(down_delay);

        if (active_puyo[0] != null && active_puyo[1] != null)
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
            && Stage_puyo[pos.x, pos.y - 1] != 0)
        {
            return true;
        }

        return false;
    }

    //ぷよの着地処理
    private void Puyo_Landing(int puyo_num = 99)
    {

        //最初のぷよが落ちた時
        if (Is_con())
        {
            Vector2Int pos = new Vector2Int((int)active_puyo[puyo_num].transform.position.x, (int)active_puyo[puyo_num].transform.position.y);

            Stage_puyo[pos.x, pos.y] = active_puyo[puyo_num].GetComponent<Puyo_Color>().Get_Color();
            last_puyos_pos[0] = new Vector2Int(pos.x, pos.y);

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
        else if (active_puyo[0] != null)
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

        Stage_puyo[pos.x, pos.y] = active_puyo[num].GetComponent<Puyo_Color>().Get_Color();
        last_puyos_pos[1] = new Vector2Int(pos.x, pos.y);

        //着地したぷよを削除
        active_puyo[num] = null;

        //ぷよが４つ以上つながってるかチェック
        check_puyo();

    }

    //
    private IEnumerator Puyo_Solo_Down(GameObject puyo)
    {
        //操作時よりは早く落ちる
        yield return new WaitForSeconds(0.03f);
        if (Is_Puyo_Landing(puyo))
        {
            Puyo_Landing();
            yield break;
        }

        Vector3 down_vec = new Vector3(0.0f, down_vol, 0.0f);

        puyo.transform.position -= down_vec;

        StartCoroutine(Puyo_Solo_Down(puyo));

    }

    //どちらも着地していないときtrue
    bool Is_con()
    {
        if (active_puyo[0] != null && active_puyo[1] != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    Vector2Int[] temp = { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
    List<Vector2Int> Link_puyo = new List<Vector2Int>();
    List<int>Link_Color = new List<int>();      //順に色を格納
    void check_puyo()
    {
        //つながっているぷよを初期化
        Link_puyo.Clear();
        Link_Color.Clear();

        Link_puyo.Add(last_puyos_pos[0]);
        Link_Color.Add(Stage_puyo[last_puyos_pos[0].x, last_puyos_pos[0].y]);

        //探索開始
        Link_storage(last_puyos_pos[0]);

    }

    void Link_storage(Vector2Int pos)
    {
        Deb_Stage_poyo();
        int link_puyo_num = Link_puyo.Count;
        bool[] Is_someCol = Is_SameColor(pos);
        for (int i = 0; i < temp.Length; i++)
        {
            if (Is_someCol[i])
            {
                bool Is_ident = true;
                foreach (Vector2Int linkpos in Link_puyo)
                {
                    if (linkpos == (pos + temp[i]))      //同じものがあったら終了
                    {
                        Is_ident = false;
                        break;
                    }
                }

                if (Is_ident)       //同じものがなければリストに追加して再帰処理
                {
                    Link_puyo.Add(pos + temp[i]);
                    Link_storage(pos + temp[i]);
                }
            }
        }

        if (Link_puyo.Count == link_puyo_num)
        {
            //探索しきったら
            if (Link_puyo.Count >= 4)
            {
                //ぷよを削除
                del_Puyo();
            }
            else
            {
                create_new_puyo();
            }

        }
    }

    //ぽよを消す処理
    void del_Puyo()
    {
        GameObject[] Puyos = GameObject.FindGameObjectsWithTag("Puyo");

        foreach (GameObject puyo in Puyos)
        {
            foreach (Vector2Int del_pos in Link_puyo)
            {
                if (del_pos.x == (int)puyo.transform.position.x
                    && del_pos.y == (int)puyo.transform.position.y)
                {
                    Destroy(puyo);
                    Stage_puyo[del_pos.x, del_pos.y] = 0;
                }
            }
        }

        create_new_puyo();
    }


    //４方向の同じ色を探索
    //２回目以降なら第二引数あり
    bool[] Is_SameColor(Vector2Int pos)
    {
        bool[] FDir = { false, false, false, false };

        for (int i = 0; i < temp.Length; i++)
        {
            Vector2Int next_pos = pos + temp[i];

            if (next_pos.x >= 0 && next_pos.x < STAGE_MASS.x
                && next_pos.y >= 0 && next_pos.y < STAGE_MASS.y)         //ステージの範囲内
            {
                if (Stage_puyo[pos.x, pos.y] == Stage_puyo[next_pos.x, next_pos.y])
                {
                    FDir[i] = true;
                }
            }
        }

        return FDir;
    }


    void create_new_puyo()
    {
        //操作可能なぷよが全部落ちたので新しいぷよを呼び出し
        active_puyo = this.gameObject.GetComponent<PuyoRespown>().Respown();
        set_active_puyo_Position();


        StartCoroutine(Puyo_Down());

    }




    //////////////////////////////////デバック用
    void Deb_Stage_poyo()
    {
        for (int i = STAGE_MASS.y - 1; i >= 0; i--)
        {
            Debug.Log(i + "行目\t\t" + Stage_puyo[0, i] + "\t" + Stage_puyo[1, i] + "\t" + Stage_puyo[2, i] + "\t" + Stage_puyo[3, i] + "\t" + Stage_puyo[4, i] + "\t" + Stage_puyo[5, i]);

        }
    }

    public int[,] get_Stage_info()
    {
        return Stage_puyo;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CPU_puyo_Manager : MonoBehaviour
{
    //ステージマスの大きさ
    Vector2Int STAGE_MASS = new Vector2Int(6, 15);

    //落ちる量 1f=1マス
    float down_vol = 0.25f;

    //ステージのぷよ用格納配列 縦,15マス 横6マス
    private int[,] Stage_puyo = new int[6, 15];

    private GameObject[] active_puyo = new GameObject[2];

    private float down_delay = 0.2f;

    private Vector2Int[] last_puyos_pos = new Vector2Int[2];          //最後に置いたぷよの場所(２つ)

    private int chain;

    private readonly float width_collect_val = 17.5f;

    //スコア用
    [SerializeField] GameObject MM;

    // Start is called before the first frame update
    void Start()
    {

        chain = 0;

        stage_mass_Init();

        create_new_puyo();
    }

    // Update is called once per frame
    void Update()
    {


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

            ////回転できる時(次のぷよが横に来るとき)
            ////1 次の回転座標が0の時
            ////2 次の回転座標が0以外でも回転座標と対称のマスが0の時

            Next_Pos_side();        //次の移動先が横の場合
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

    void Next_Pos_side()
    {//下側のぷよ(１番目)を起点に時計回りで回転    0番目のぷよを移動

        Vector2 move_puyo = active_puyo[0].transform.position;

        //0番目のぷよの場所
        Vector2 dif = active_puyo[0].transform.position - active_puyo[1].transform.position;
        Vector2 puyo = new Vector2(move_puyo.x + dif.y, move_puyo.y - dif.y);
        //上(0,1) 下(0,-1)

        //回転できる時(次のぷよが横に来るとき)
        //1 次の回転座標が0の時
        //2 次の回転座標が0以外でも回転座標と対称のマスが0の時


        //1
        if (Is_Stage_Range(puyo))
        {
            if (Stage_puyo[(int)puyo.x, (int)puyo.y] == 0)
            {
                Vector2[] move_poss = { puyo, active_puyo[1].transform.position };
                Move_Active_Poyos_Pos(move_poss);

                //移動できたので終了
                return;
            }
            else
            {
                //割り込み移動
                Oneself_Move(dif);
            }
        }
        else
        {
            //割り込み移動
            Oneself_Move(dif);
        }
    }

    void Oneself_Move(Vector2 dif)
    {
        Vector2 move_puyo = active_puyo[0].transform.position;
        //対称の場所が空いていたら1が0の場所に移動して
        //0が空いているマスに移動
        Vector2 puyo = new Vector2(move_puyo.x - dif.y, move_puyo.y);
        if (!Is_Stage_Range(puyo)) return;
        if (Stage_puyo[(int)puyo.x, (int)puyo.y] == 0)
        {
            Vector2[] move_poss = { active_puyo[0].transform.position, puyo };
            Move_Active_Poyos_Pos(move_poss);

            //移動できたので終了
            return;
        }
    }


    void Move_Active_Poyos_Pos(Vector2[] poss)
    {
        active_puyo[0].transform.position = poss[0];
        active_puyo[1].transform.position = poss[1];
    }

    //右側用の当たり判定
    bool Is_Right_hit(Vector2[] puyo)
    {//原点のぷよのオブジェクト(一番右側のぷよ)
        Vector2 ori_puyo = puyo[0];
        if (puyo[0].x < puyo[1].x)
        {
            ori_puyo = puyo[1];
        }

        //両方の右のマス上に遮るものがないとき
        Vector2Int pos1 = new Vector2Int((int)puyo[0].x + 1, (int)puyo[0].y);
        Vector2Int pos2 = new Vector2Int((int)puyo[1].x + 1, (int)puyo[1].y);
        if (!Is_Stage_Range(pos1)) return true;
        if (!Is_Stage_Range(pos2)) return true;
        if (Stage_puyo[pos1.x, pos1.y] == 0 && Stage_puyo[pos2.x, pos2.y] == 0)
        {
            return false;
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
        //両方の左のマス上に遮るものがないとき
        Vector2Int pos1 = new Vector2Int((int)puyo[0].x - 1, (int)puyo[0].y);
        Vector2Int pos2 = new Vector2Int((int)puyo[1].x - 1, (int)puyo[1].y);

        if (!Is_Stage_Range(pos1)) return true;
        if (!Is_Stage_Range(pos2)) return true;
        if (Stage_puyo[pos1.x, pos1.y] == 0 && Stage_puyo[pos2.x, pos2.y] == 0)
        {
            return false;
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
        Vector2[] poss = { new Vector3(2.0f + width_collect_val, 15.0f), new Vector3(2.0f + width_collect_val, 14.0f) };
        Move_Active_Poyos_Pos(poss);
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
        //check_puyo();
        check_Link();

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

    private IEnumerator Puyo_Solo_Down(GameObject puyo, Vector2 pos)
    {
        //操作時よりは早く落ちる
        yield return new WaitForSeconds(0.03f);
        if (puyo.transform.position.y <= pos.y)
        {
            yield break;
        }

        Vector3 down_vec = new Vector3(0.0f, down_vol, 0.0f);

        puyo.transform.position -= down_vec;

        StartCoroutine(Puyo_Solo_Down(puyo, pos));

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
    List<Vector2Int> Link_puyo = new List<Vector2Int>();        //つながっているぷよを一時保存先

    List<List<Vector2Int>> del_puyos = new List<List<Vector2Int>>();        //消えるぷよを格納する　2次元配列

    void check_Link()
    {//つながっているぷよを初期化
        del_puyos.Clear();
        Link_puyo.Clear();

        for (int i = 0; i < STAGE_MASS.y; i++)
        {
            for (int j = 0; j < STAGE_MASS.x; j++)
            {
                Vector2Int pos = new Vector2Int(j, i);
                puyo_Link_check(pos);
            }
        }

        if (del_puyos.Count != 0)
        {
            //連鎖開始
            chain++;
            MM.GetComponent<ScoreManager>().Set_Score(Get_Del_Puyo_sum(), chain);

            //つながっているぷよがあるまで繰り返す
            //del_Puyo() => Stage_mass_make() => StartCoroutine(puyo_all_Down()) => check_Link() => del_Puyo() ....
            Invoke("del_Puyo", 1.0f);
            //del_Puyo();
        }
        else
        {
            //連鎖終了
            chain = 0;


            //ゲームオーバーチェック
            if (Is_Check_GameOver())
            {
                //ゲームオーバー処理
                StartCoroutine(puyo_over_anim());
            }
            else
            {
                //つながっているぷよがなくなれば終了して次のぷよを生成
                create_new_puyo();
            }


        }
    }

    void puyo_Link_check(Vector2Int pos)
    {
        if (Stage_puyo[pos.x, pos.y] == 0) return;      //空白    
        if (!Is_same_pos(pos)) return;                  //探索済みの座標

        Link_puyo.Clear();
        Link_puyo.Add(pos);
        for (int i = 0; i < temp.Length; i++)
        {
            if (Is_sameColor(pos, i))
            {
                Link_storage(pos);
            }
        }

        if (Link_puyo.Count >= 4)
        {
            del_puyos.Add(new List<Vector2Int>(Link_puyo));
        }
        return;
    }

    bool Is_same_pos(Vector2Int pos)
    {
        //同じ座標が存在したらfalse
        for (int i = 0; i < del_puyos.Count; i++)
        {
            for (int j = 0; j < del_puyos[i].Count; j++)
            {
                if (del_puyos[i][j] == pos) return false;
            }
        }
        return true;
    }

    void Link_storage(Vector2Int pos)
    {
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
    }

    //ぷよを消す処理
    void del_Puyo()
    {
        GameObject[] Puyos = GameObject.FindGameObjectsWithTag("Puyo");

        foreach (GameObject puyo in Puyos)
        {
            foreach (List<Vector2Int> col in del_puyos)
            {
                foreach (Vector2Int del_pos in col)
                {
                    if (del_pos.x == (int)puyo.transform.position.x
                    && del_pos.y == (int)puyo.transform.position.y)
                    {
                        StartCoroutine(puyo.GetComponent<puyo_anim>().destroy_anim());
                        Stage_puyo[del_pos.x, del_pos.y] = 0;
                    }
                }
            }
        }
        Stage_mass_make();
    }

    void Stage_mass_make()
    {
        for (int i = 0; i < STAGE_MASS.x; i++)
        {
            List<int> col = new List<int>();
            col.Clear();

            for (int j = 0; j < STAGE_MASS.y; j++)
            {
                if (Stage_puyo[i, j] != 0)
                {
                    col.Add(Stage_puyo[i, j]);
                }
            }
            for (int j = 0; j < STAGE_MASS.y; j++)
            {
                if (col.Count > j)
                {
                    Stage_puyo[i, j] = col[j];
                }
                else
                {
                    Stage_puyo[i, j] = 0;
                }
            }
        }
        StartCoroutine(puyo_all_Down());
    }

    IEnumerator puyo_all_Down()
    {
        yield return new WaitForSeconds(0.1f);
        bool Is_all_down = true;


        for (int i = 0; i < STAGE_MASS.x; i++)
        {
            List<GameObject> col_puyos = new List<GameObject>(Col_Puyos(i));
            for (int j = 0; j < col_puyos.Count; j++)
            {
                //今は一瞬で移動
                //col_puyos[j].transform.position  = move_pos;


                //目標地点
                Vector2 move_pos = new Vector2(i, j);

                //ゆっくり落ちる
                StartCoroutine(Puyo_Solo_Down(col_puyos[j], move_pos));
            }

        }


        if (Is_all_down)
        {
            check_Link();
        }
        else
        {
            StartCoroutine(puyo_all_Down());
        }
    }

    List<GameObject> Col_Puyos(int col)
    {
        List<GameObject> col_puyos = new List<GameObject>();

        GameObject[] Puyos = GameObject.FindGameObjectsWithTag("Puyo");
        foreach (GameObject puyo in Puyos)
        {
            if ((int)puyo.transform.position.x == col && !puyo.GetComponent<puyo_anim>().Is_destroy)
            {
                col_puyos.Add(puyo);
            }
        }

        for (int i = 0; i < col_puyos.Count; i++)
        {
            for (int j = i; j < col_puyos.Count; j++)
            {
                if (col_puyos[i].transform.position.y > col_puyos[j].transform.position.y)
                {
                    GameObject temp = col_puyos[i];
                    col_puyos[i] = col_puyos[j];
                    col_puyos[j] = temp;
                }
            }
        }

        return col_puyos;
    }


    //４方向の同じ色を探索
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

    bool Is_sameColor(Vector2Int pos, int dir)
    {
        Vector2Int next_pos = pos + temp[dir];
        if (next_pos.x >= 0 && next_pos.x < STAGE_MASS.x
                && next_pos.y >= 0 && next_pos.y < STAGE_MASS.y)         //ステージの範囲内
        {
            if (Stage_puyo[pos.x, pos.y] == Stage_puyo[next_pos.x, next_pos.y])
            {
                return true;
            }
        }
        return false;
    }


    public void create_new_puyo()
    {
        //操作可能なぷよが全部落ちたので新しいぷよを呼び出し
        active_puyo = this.gameObject.GetComponent<PuyoRespown>().Respown();
        set_active_puyo_Position();


        StartCoroutine(Puyo_Down());

    }

    bool Is_Stage_Range(Vector2 pos)
    {
        bool v = 0 <= pos.x && pos.x < STAGE_MASS.x;
        return v && 0 <= pos.y && pos.y < STAGE_MASS.y;
    }

    //ゲームオーバーチェック
    bool Is_Check_GameOver()
    {
        return (Stage_puyo[2, STAGE_MASS.y - 1] != 0);
    }

    public int Get_chain()
    {
        return chain;
    }

    public int Get_Del_Puyo_sum()
    {
        //消えるぷよの総数を返す
        int puyo_sum = 0;
        foreach (List<Vector2Int> col in del_puyos)
        {
            foreach (Vector2Int puyo in col)
            {
                puyo_sum++;
            }
        }
        return puyo_sum;
    }

    IEnumerator puyo_over_anim()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject[] Puyos = GameObject.FindGameObjectsWithTag("Puyo");
        bool Is_puyo_cam = false;
        foreach (GameObject puyo in Puyos)
        {
            puyo.transform.position -= new Vector3(0.0f, 0.5f, 0.0f);
            if (puyo.transform.position.y >= -3.0f) Is_puyo_cam = true;
        }

        if (Is_puyo_cam) StartCoroutine(puyo_over_anim());
        else { MM.GetComponent<sceneManager>().GotoResult(); }
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

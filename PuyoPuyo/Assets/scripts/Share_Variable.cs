using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data")]
public class Share_Variable : ScriptableObject
{
    [SerializeField]int Score;
    [SerializeField]int HighChain;

    public void Set_Score(int score)
    {
        Score = score;
    }
}

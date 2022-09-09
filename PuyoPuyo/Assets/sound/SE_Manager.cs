using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]AudioClip[] _SE;
    AudioSource audio;
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void SE_Play(int num)
    {
        if(_SE.Length > num)
        {
            audio.PlayOneShot(_SE[num]);
        }
    }
}

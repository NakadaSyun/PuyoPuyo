using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{

    [SerializeField]float Max_vol = 0.2f;
    [SerializeField] float delay_time = 0.05f;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        audio.volume = Max_vol;
    }

    // Update is called once per frame
    public IEnumerator BGM_feedout()
    {
        yield return new WaitForSeconds(delay_time);
        float vol = audio.volume;

        vol -= (Max_vol * delay_time);

        if(vol <= 0.0f)
        {
            audio.volume = 0.0f;
        }
        else
        {
            audio.volume = vol;
            StartCoroutine(BGM_feedout());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] BGM_Manager BM;
    [SerializeField] SE_Manager SM;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SM.SE_Play(0);
            StartCoroutine(BM.BGM_feedout());
            StartCoroutine(GotoSelect(1.0f));
        }
    }

    public IEnumerator GotoSelect(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("select", LoadSceneMode.Single);
    }

    public void Oncrick()
    {
        //SceneManager.LoadScene("select", LoadSceneMode.Single);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    private GameObject rg;
   
    public GameObject endui;
    //public GetTime timer;
    public int finishedsec;
    public int finishedmillsec;
    public int finishedmin;
    Text finaltimeshow;
    // Start is called before the first frame update
    void Start()
    {
        //gameover.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        rg.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Exit(Clone)")

        {

            //rg.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //rg.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            //gameover.SetActive(true);
            Debug.Log("out");
            finishedmin = GetTime.minute;
            finishedsec = GetTime.second;
            finishedmillsec = GetTime.second;
            Debug.Log(finishedmin);
            Debug.Log(finishedsec);
            Debug.Log(finishedmillsec);
        
            //finishedsec = GetTime.second;
            //Debug.Log(finishedsec);
            //Debug.Log(GetTime.millisecond);
            GameObject finishtime = GameObject.Find("FinalTime");
            Debug.Log(finishtime.name);
            finaltimeshow = finishtime.GetComponent<Text>();
            finaltimeshow.text = string.Format("{0:D2}:{1:D2}:{2:D2}",finishedmin,finishedsec,finishedmillsec );
            endui.SetActive(true);
        }
    }
}

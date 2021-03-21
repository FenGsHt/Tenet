using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrolDot : MonoBehaviour
{
    
    public int waitTime;  //等候的时间


    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        //开始运行时则关闭sprite的显示
        sr = GetComponent<SpriteRenderer>();

        //sr.enabled = false;
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

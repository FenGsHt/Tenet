using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : MonoBehaviour
{
    // Start is called before the first frame update
    //剑击中目标时的效果
    private int timer;
    private int totalTimer=100; 

    private SpriteRenderer sr;

    private void Awake()
    {
        //this.gameObject.SetActive(false);  //先直接设置不激活
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.timer++;
        if (this.timer > totalTimer)
        {
            this.timer = 0;
            Destroy(this.gameObject);
            //this.gameObject.SetActive(false);

        }
        //Debug.Log(0 + 1f / (float)totalTimer * (float)this.timer);

        float temp = 1 / (float)totalTimer * (float)this.timer;

        sr.color = new Color(1,0+temp,0+ temp, 1-temp);
       
        //变换颜色


    }
}

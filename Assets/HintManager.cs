using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public static int timer;  //文字显示的时间
    //管理提示文本
    public static Text text;  
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer--;
        }

        if (timer == 0)
        {
            text.text = "";   //将提示文本设为空
        }

    }

    public static void SetText(string text,int timer)
    {
        HintManager.text.text =text;
        HintManager.timer = timer;
    }
}

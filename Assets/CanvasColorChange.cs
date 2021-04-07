using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasColorChange : MonoBehaviour
{
    // Start is called before the first frame update
    private Image image;  //图片引用

    private int lastStatus=0;
    private int lastCurrentDirection=1;
    private int timer;

    private float changingR;
    private float changingG;

    private float changingB;

    private float changingA;


    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //要慢慢变,不要一下子变过去

        //当master的变量较之前发生改变时使用colorchange
        if (lastStatus != Master.status || lastCurrentDirection != Master.currentDirection)
        {
            lastStatus = Master.status;
            lastCurrentDirection = Master.currentDirection;

            //Debug.Log("开始改变");

            ColorChange();   //进行颜色的改变
           
        }

        if (timer > 0)
        {
            ChangeSlowly(changingR,changingG,changingB,changingA);
            timer--;
        }
    }

    void ChangeSlowly(float r,float g,float b,float a)
    {
        //只有当
        //if (image.color.r  r)
        //Debug.Log("在变"+changingA);
        
        image.color = new Color(image.color.r + r, image.color.g + g, image.color.b + b, image.color.a + a);


        

    }

    void ColorChange()
    {
        //四种状态下背景变成什么颜色
        if (Master.currentDirection == 1 && Master.status == 0)
        {
            //啥都不用,调回默认的无颜色
            float r = 1;
            float g = 1;
            float b = 1;
            float a = 0;
            timer = 20;

            

            ColorChangeSet(r, g, b,a, timer);


        }
        else if (Master.currentDirection == 1 && Master.status == 2)
        {
            //回退,用红色
            Debug.Log("变红");
            float r = 236f / 255f;
            float g = 84f / 255f;
            float b = 84f / 255f;
            float a = 109f / 255f;
            timer = 20;

            ColorChangeSet(r, g, b, a, timer);

            //image.color = new Color(179f / 255f, 18f / 255f, 16f / 255f, 161f / 255f);


        }
        else if (Master.currentDirection == 0 && Master.status == 0)
        {
            //逆向前进,用红色
            Debug.Log("变红");
            float r = 236f / 255f;
            float g = 84f / 255f;
            float b = 84f / 255f;
            float a = 109f / 255f;
            timer = 20;

            ColorChangeSet(r, g, b, a, timer);

        }
        else if (Master.currentDirection == 0 && Master.status == 2)
        {
            //逆向倒退,用蓝色
            float r = 65f / 255f;
            float g = 131f / 255f;
            float b = 74f / 255f;
            float a = 137f / 255f;

            timer = 20;

            ColorChangeSet(r, g, b, a, timer);

        }
        else if (Master.status == 1)
        {
            //暂停时,用黄色
            float r = 181f / 255f;
            float g = 150f / 255f;
            float b = 42f / 255f;
            float a = 137 / 255f;

            timer = 10;

            ColorChangeSet(r, g, b, a, timer);

        }

    }

    void TimerChange()
    {
        //if (Master.currentDirection == 1 && Master.status == 0)
        //{
        //}
        //else if (Master.currentDirection == 1 && Master.status == 2)
        //{


        //}
        //else if (Master.currentDirection == 0 && Master.status == 0)
        //{

        //}
        //else if (Master.currentDirection == 0 && Master.status == 2)
        //{

        //}
        //else if (Master.status == 1)
        //{
        //}
    }

    void ColorChangeSet(float r,float g,float b,float a,float timer)
    {
        changingR = (r - image.color.r) / timer;
        changingG = (g - image.color.g) / timer;
        changingB = (b - image.color.b) / timer;
        changingA = (a - image.color.a) / timer;
    }
}

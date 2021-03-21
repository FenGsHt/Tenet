using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{

    public static int status = 0;  //游戏状态,0为正常,1为暂停,2为倒放,倒放时又分为正向倒放和反向倒放

    public static List<humanBody> bodyCollector = new List<humanBody>();  //用来收集所有的人物的变量

    public static int currentDirection=1;  //记录当前游戏的方向,是在正向还是反向

    public int targetFrameRate = 60;

    public static int frame;  //总的时间帧数

    //public static int timer;   //记录

    bool timeBack_Forward;   //时间逆转按钮
    bool timeBack_Backward;   //时间方向时逆转按钮
    bool pause;  //暂停按钮


    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private static void SetAnimSpeed(float speed)
    {
        for (int i = 0; i < Master.bodyCollector.Count; i++)
        {
            Animator anim = Master.bodyCollector[i].GetComponent<Animator>();
            anim.SetFloat("Speed", speed);

            anim.SetFloat("Horizontal", 0f);
           
            //niahol123nihao313
        }
    }

    private static void SetGravity(float gravity)
    {
        //1表示恢复正常,0表示关闭

        for (int i = 0; i < Master.bodyCollector.Count; i++)
        {

            //rigidbody要先关掉(重力关掉)
            if (gravity == 1)
            {
                Master.bodyCollector[i].GetComponent<humanBody>().gravityScale =
               Master.bodyCollector[i].GetComponent<humanBody>().normalGravityScale;
            }
            else if(gravity==0)
            {
                Master.bodyCollector[i].GetComponent<humanBody>().gravityScale = 0;

            }
           


        }
    }

    // Update is called once per frame
    void Update()
    {
        //每过一帧总的时间帧数加1
        if (Master.status == 0)
        {
            Master.frame++;

        }

        timeBack_Forward = Input.GetButton("TimeBack_Forward");   //时间逆转按钮
        timeBack_Backward = Input.GetButton("TimeBack_Backward");   //时间反向时逆转按钮
        pause = Input.GetButton("Pause");  //暂停按钮

        if(Master.frame!=0)      //frame为0表示逆转到尽头了
        if (timeBack_Forward == true)
        {
            //进入倒放状态
            Master.status = 2;

            if (Master.frame > 0)
            {
                Master.frame--;

            }

            for (int i = 0; i < Master.bodyCollector.Count; i++)
            {
                MasterController mController = Master.bodyCollector[i].GetComponent<MasterController>();
                SetAnimSpeed(-1f);
                SetGravity(1);

               
            }

        }

        if (timeBack_Forward == false && Master.status == 2)
        {
            //表示回放键已经松开了,此时进入暂停状态
            pause = true;
            Debug.Log("pause");

        }

       


        if (pause == true)
        {
            if (Master.status == 1)
            {
                //从暂停状态恢复
                Resume();

            }
            else
            {
                Stop();


            }
           
        }


       
        //Debug.Log(Master.frame);
    }

    public static void Resume()
    {
        //从暂停状态恢复
        if (Master.status == 1)
        {
            Master.status = 0;

            //遍历所有的bodyCollector,并让其中的anim的speed变化

            SetAnimSpeed(1f);

            SetGravity(1);

        }

    }

    

    public static void Stop()
    {

        Master.status = 1;

        SetAnimSpeed(0f);

        SetGravity(0);

    }
   
}

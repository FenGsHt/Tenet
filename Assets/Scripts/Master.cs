using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    //public Material reverseMaterial;  //给逆转主角用的
    public GameObject ReverseMainGuyHair;  

    public GameObject mainGuyPrefabs;   //逆转主角的预制体

    public static GameObject ReverseMainGuy;  

    public static int status = 0;  //游戏状态,0为正常,1为暂停,2为倒放,倒放时又分为正向倒放和反向倒放

    public static List<humanBody> bodyCollector = new List<humanBody>();  //用来收集所有的人物的变量

    public static int currentDirection=1;  //记录当前游戏的方向,是在正向还是反向

    public int targetFrameRate = 60;

    public static int frame;  //总的时间帧数

    public static int finalFrame;  //开始逆转后最大frame固定

    public static int hitPause;   //2,1,0 2为开始,1为恢复,0为无行动

    //public static int timer;   //记录

    bool timeBack_Forward;   //时间逆转按钮
    bool timeBack_Backward;   //时间方向时逆转按钮
    bool pause;  //暂停按钮

    private Transform head;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;

        Master.ReverseMainGuy = Instantiate(mainGuyPrefabs, new Vector3(0, 0, 0), Quaternion.identity);

        Master.ReverseMainGuy.SetActive(false);

        //Master.ReverseMainGuy.GetComponent<SpriteRenderer>().material = this.reverseMaterial;  //设置材质
        head=Master.ReverseMainGuy.transform.Find("chest/head");

        Instantiate(ReverseMainGuyHair,new Vector3(0,0.5f,0), Quaternion.identity, head);


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

            //if(!(Master.currentDirection==0&&Master.bodyCollector[i].tenetDirection==1)
            //    || !(Master.currentDirection == 0 && Master.bodyCollector[i].tenetDirection == 0))
            if (Master.currentDirection == 0 && Master.bodyCollector[i].tenetDirection == 1)
            {
                //speed = Mathf.Abs(speed);   //此时的情况确保speed为正数
                speed = speed * -1;   //与其他的相反
            }

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

    private void FrameChange()
    {
        //每过一帧总的时间帧数加1
        if (Master.status == 0)
        {

            if (Master.currentDirection == 1)
            {
                //表示是正常的正向
                Master.frame++;

            }
            else if (Master.currentDirection == 0)
            {
                //此时已经在反向
                if (Master.frame > 0)
                {
                    Master.frame--;

                }

                if (Master.frame == 0)
                {
                    Debug.Log("frame为0在master中暂停 cD==0");
                    Master.Stop();
                }
            }

        }
        else if (Master.status == 2)
        {
            if (Master.currentDirection == 1)
            {
                if (Master.frame > 0)
                {
                    frame--;
                }

                
                if (Master.frame == 0)
                {
                    Debug.Log("frame为0在master中暂停");
                    Master.Stop();
                }
            }
            else if (Master.currentDirection == 0)
            {

                if (Master.frame < Master.finalFrame)
                {
                    frame++;
                }
            }
        }
    }

    public static void HitPause()
    {
        //击中时卡一下
        if (hitPause >=2)
        {
            System.Threading.Thread.Sleep(50);
            Debug.Log("hitPause暂停中");
            //Master.status = 1;
            //Master.SetAnimSpeed(0f);

            hitPause--;
        }
        else if (hitPause == 1)
        {
            //Master.SetAnimSpeed(1f);
           // Master.status = 0;
            hitPause = 0;
        }

        //Debug.Log(Master.status);
    }

    // Update is called once per frame
    void Update()
    {
        HitPause();   //击中时的暂停判断

        FrameChange();  //根据status和currentDirection来决定frame的增减

        // Debug.Log(Master.frame);


        timeBack_Forward = Input.GetButton("TimeBack_Forward");   //时间逆转按钮
        timeBack_Backward = Input.GetButton("TimeBack_Backward");   //时间反向时逆转按钮
        pause = Input.GetButton("Pause");  //暂停按钮


        if(Master.frame>0&&Master.currentDirection==1)      //frame为0表示逆转到尽头了
        {
            if (timeBack_Forward == true)
            {
                //进入倒放状态
                Master.Reverse();

            }
        }
           

        if(Master.frame <= Master.finalFrame && Master.currentDirection==0)
        {
            if ( timeBack_Backward == true)
            {
                Debug.Log("reverse");
                Master.Reverse();
            }
           
        }
            

        
        //当为正常正向且回退键松开时,则自动暂停
        if ((timeBack_Backward==false&&timeBack_Forward == false) && Master.status == 2)
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

            if (Master.currentDirection == 1)
            {

                Master.status = 0;

            }
            else if (Master.currentDirection == 0)
            {

                if (Master.frame > 0)
                {
                    Master.status = 0;

                }
                else
                {
                    Debug.Log("frame已经为0了,无法resume");
                }
            }


            SetAnimSpeed(1f);

            SetGravity(1);

        }

    }

    public static void Reverse()
    {
        Master.status = 2;

        SetAnimSpeed(-1f);
        SetGravity(1);

        //for (int i = 0; i < Master.bodyCollector.Count; i++)
        //{
        //    MasterController mController = Master.bodyCollector[i].GetComponent<MasterController>();
            


        //}
    }

    public static void AddReverseMainGuy(Vector3 position)
    {
        Master.ReverseMainGuy.SetActive(true);


        Master.ReverseMainGuy.transform.position = position;

        Master.ReverseMainGuy.GetComponent<humanBody>().tenetDirection = 0;  //设置方向为逆转

        Master.currentDirection = 0;   //当前世界为逆转状态

        Master.finalFrame = Master.frame;

        Master.CurrentDirectionChange(0);
    }

    public static void Stop()
    {

        Master.status = 1;

        SetAnimSpeed(0f);

        SetGravity(0);

    }
    //当游戏世界的currentDirection变化时世界发生的改变
    public static void CurrentDirectionChange(int direction)  
    {
       
        if (direction == 0)
        {
            SetAnimSpeed(1f);

        }
    }
   
}

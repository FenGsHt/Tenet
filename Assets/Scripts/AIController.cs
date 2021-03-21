using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    //不在u3d调试面板中显示
    [HideInInspector]
    public int patrolState = 0;  //记录当前巡逻到哪个点了.
    [HideInInspector]
    public int timer;   //用来计时


    //用于设置敌人参数的公有变量
    public float lostDistance;  //设置自身丢失目标所需的距离
    //public int speed=5;   //行走速度
    //public GameObject target;  //当前的目标
    public GameObject[] patrolDot;
    public int faceDistance;   //贴目标脸的距离

    

    //todo 巡逻点的添加,用数组的方式添加后,如果为非空则进行巡逻(当未检测到目标时)


    //用于所有敌人的AI操作的判断
    private int jumpHeight;   //跳跃时指定的参数
    private humanBody body;  //自身身体的引用
    private MasterController mController;   //时间控制器的引用
    private Rigidbody2D rigidbody;
    private Animator anim; 
   
    


    private void Awake()
    {
        body = GetComponent<humanBody>();
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mController = GetComponent<MasterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveControl()
    {
         //控制敌人的行动的函数

    }

    void Update()
    {

        if (Master.status !=1)     //当全局状态为正常时才进行所有动作
        {

            Vector2 position = transform.position;


            //没死才有动作
            if (Master.status==0)    //正常状态下
            if (anim.GetBool("dead") == false)
            {

                position += AITool.Patrol(body.target, body, this, body.speed);

                position += AITool.ApproachOrAway(body.target, body, faceDistance, body.speed, 1);

                body.DetectGround();
                position += body.Gravity( new Vector2());
                

                //因为跳跃是要连续使用的函数,所以放在update中
                position += AITool.Jump(body, jumpHeight);

                if (body.target != null)
                {

                    AITool.RandomAttack(body, 5);
                    AITool.IfLost(body.target, body, lostDistance);
                }


            }


            if (Master.status == 2)
            {
                //进行查找egg
                position = mController.FindEgg();
            }



            rigidbody.MovePosition(position);

        }



    }
       

       

  

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainGuyController : MonoBehaviour
{

    public int attackSpeed = 13;   //攻击时的速度

    public int normalSpeed = 5;   //默认速度


    private Rigidbody2D rigidbody;

    //public int speed=10;
    private MasterController mController;

    private Animator anim;

    private humanBody body;

   // private int updateTimer=0;   //用于frame的添加工作,当updateTimer为1时表示已经有一次没有添加了,此时添加

    //private Vector2 lastPosition;  //记录上一次更新时的坐标

    float horizontal ;   //水平方向运动
    float vertical;    //垂直方向运动
    float jump;
    bool attack ;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        body = GetComponent<humanBody>();

        mController = GetComponent<MasterController>();
    }
    // Start is called before the first frame update
    void Start()
    {

       
    }

    void SpeedUpWhileAttack(bool attack)
    {

        if (anim.GetInteger("Attack") == 1)
        {
            body.speed = attackSpeed;
        }
        else
        {
            body.speed = normalSpeed;
        }

    }

    private Vector2 UpdateInfo()
    {
        SpeedUpWhileAttack(attack);

        Vector2 position = new Vector2() ;

        

        //当没死时可以用
        if (anim.GetBool("dead") == false)
        {
            position += body.Jumping(jump,0,new Vector2());  //调用Jump函数

            position += body.Walk(horizontal, new Vector2());   //调用humanBody的Walk;

            body.Attack(horizontal, attack);
        }

        //Debug.Log("qwe");


        return position;
    } 

    // Update is called once per frame
    void Update()
    {

        horizontal = Input.GetAxis("Horizontal");   //水平方向运动
        vertical = Input.GetAxis("Vertical");    //垂直方向运动
        jump = Input.GetAxis("Jump");
        attack = Input.GetButton("Fire1");

        bool horizontalDown = Input.GetButtonDown("Horizontal");

        Vector2 position= transform.position;


        //if (this.updateTimer > 0)
        //{
        //    body.AddFrame(position, this.lastPosition);  //根据情况增加frame

        //    this.updateTimer = 0;

        //}

        //this.lastPosition = transform.position;  //记录当前的位置


       // this.updateTimer++;


        if (Master.status == 1)
        {
            //游戏暂停时按下任意按键后继续,
            //倒放后松开按键后自动进入暂停状态
            if (jump > 0 || attack ==true || horizontalDown==true)
            {
                
                Master.Resume();
            }
        }

        body.DetectGround();
        body.DetectWall();


        




        if (Master.status ==0)     //当全局状态为正常时才进行所有动作
        {
            //每时每刻都有重力

            position += body.Gravity(new Vector2());

            position +=UpdateInfo();
        }

        if (Master.status == 2)
        {
            position = mController.FindEgg();
        }


        body.AutoStop(mController.eggList,Master.frame);   


        rigidbody.MovePosition(position);

       



        //Debug.Log(mController.eggList.Count);

    }

}

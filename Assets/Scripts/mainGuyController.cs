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

    public ParticleSystem sprintLeaf;  //冲刺时掉落树叶

    public ParticleSystem blood;   //死亡时的特效

    private GameObject shield;

   // private int updateTimer=0;   //用于frame的添加工作,当updateTimer为1时表示已经有一次没有添加了,此时添加

    //private Vector2 lastPosition;  //记录上一次更新时的坐标

    float horizontal ;   //水平方向运动
    float vertical;    //垂直方向运动
    float jump;
    bool attack;
    bool shieldPressed;   //盾牌
    
    private void Awake()
    {
        

        sprintLeaf = Instantiate(sprintLeaf, transform.position, Quaternion.identity, transform);

        sprintLeaf.Stop();

        blood = Instantiate(blood, transform.position, Quaternion.identity, transform);

        blood.Stop();


        rigidbody = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        body = GetComponent<humanBody>();

        if (body.tenetDirection == 0)   //表示是逆转主角 ,此时寻找盾牌
        {
            shield=gameObject.transform.Find("Shield").gameObject;
            Debug.Log("找到盾牌了");
            shield.SetActive(false);  //默认为不开启状态

        shield.transform.parent = null;  //将从属关系去掉,这样不会跟着旋转
         }

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
            sprintLeaf.Play();

            body.speed = attackSpeed;
        }
        else
        {
            body.speed = normalSpeed;
        }

    }

    private Vector2 UpdateInfo()
    {

        if (body.tenetDirection == 0)   //只有逆向主角才会冲刺
        {
            SpeedUpWhileAttack(attack);

        }

        Vector2 position = new Vector2() ;


        //当没死时可以用
        if (anim.GetBool("dead") == false)
        {
            position += body.Jumping(jump,0,new Vector2());  //调用Jump函数

            position += body.Walk(horizontal,Vector2.zero);   //调用humanBody的Walk;

            body.Attack(horizontal, attack,Vector2.zero);
        }

        //Debug.Log("qwe");


        return position;
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
        
    //}

    //private void OnAnimatorMove()
    //{

    //    //Debug.Log("有在move");
    //    gameObject.transform.Find("rightArm1").transform.rotation = CursorController.direction;

    //}

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    gameObject.transform.Find("rightArm1").transform.rotation = CursorController.direction;

    //}
    //public void ArmRotationChange()
    //{
    //    Debug.Log("有在变");
    //}

    public void Shield()
    {
        //打开盾牌

        //画一个盾牌类,打开后根据鼠标的角度生成盾牌,盾牌可以抵挡子弹
        this.shield.SetActive(false);   //先关闭,提前判断条件不满足

        if (shieldPressed == true)
        {
            this.shield.SetActive(true);

            //设置位置和角度,角度为cursorController.direction,位置为主角位置加上单位方向乘距离
            this.shield.transform.position = this.transform.position + CursorController.normalizedDirection * 2;

            this.shield.transform.rotation = CursorController.direction;

        }


    }
    // Update is called once per frame

    public void Dead()
    {

        if (body.beginDead == 1)
        {
            blood.Play();
            body.beginDead = 0;

            if(Master.currentDirection==1)
                HintManager.SetText("你被杀死了,按Q键倒转.",400);
            else
                HintManager.SetText("你被杀死了,按E键倒转.", 400);

        }

    }

    void Update()
    {
        //Debug.Log(anim.GetFloat("Speed"));
        //Instantiate(bullet)

        horizontal = Input.GetAxis("Horizontal");   //水平方向运动
        vertical = Input.GetAxis("Vertical");    //垂直方向运动
        jump = Input.GetAxis("Jump");
        attack = Input.GetButton("Fire1");
        shieldPressed = Input.GetMouseButton(1);  //点击鼠标右键


        bool horizontalDown = Input.GetButtonDown("Horizontal");

        Vector2 position= transform.position;

       // Debug.Log(" Master.currentDirection "+Master.currentDirection+" master.status "+Master.status);

        if (Master.status == 1)
        {
            //游戏暂停时按下任意按键后继续,
            //倒放后松开按键后自动进入暂停状态
            if (jump > 0|| horizontalDown==true)
            {
                
                Master.Resume();
            }
        }

        body.DetectGround();
        //body.DetectWall();
        Dead();
      

        //Debug.Log(Master.currentDirection);

        //$$$$#$@#$@!$!
        //正向
        if (body.tenetDirection == 1&&Master.status!=1)
        {

            if (Master.currentDirection == 0||Master.status==2)
            {
                //当为回放状态或者逆转方向时进行查找egg
                    position = mController.FindEgg(body.tenetDirection);


            }
            else if (Master.status == 0 && Master.currentDirection == 1)     //当全局状态为正常时才进行所有动作
            {
                //每时每刻都有重力
                // Debug.Log("还在动");

                

                position += body.Gravity(Vector2.zero);

                position += UpdateInfo();


            }


        }

        //$$$$#$@#$@!$!
        //逆向
        if (body.tenetDirection == 0&&Master.status!=1)
        {
            //此部分为逆转主角所独有的.

            

            if (Master.status == 0)
            {

                //盾牌函数
                Shield();


                //此时表示正常的逆向主角的操作
                position += body.Gravity(Vector2.zero);

                position += UpdateInfo();

                //Debug.Log("逆转");

            }
            else if (Master.status == 2)
            {
                //表示逆向的方向在回退
                position = mController.FindEgg(body.tenetDirection);
            }


        }

        //Debug.Log(mController.eggList.Count);
        //Debug.Log(body.tenetDirection + " " + Master.currentDirection);


        // body.AutoStop(mController.eggList,Master.frame);   

        //position = Input.mousePosition;

        rigidbody.MovePosition(position);




        body.AutoStop(mController.eggList, Master.frame);

        //Debug.Log(mController.eggList.Count);

    }

}

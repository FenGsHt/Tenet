﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanBody : MonoBehaviour
{

    //此函数用于存放所有的对人物身体的操控的方法 ,具体方法放在mainGuyController
    private Rigidbody2D rigidbody;
    
    private Animator anim;

    private MasterController mController;

    private Transform leftLeg2;

    private Transform rightLeg2;

    // private BoxCollider2D collider;

    public int jumpingTimer = 90;

    public int gravityScale = 15;  //设置重力的大小

    [HideInInspector]
    public int normalGravityScale;   //用来保存gravityScale的值

    public int jumpHeight = 5;

    public int speed = 5;   //行走速度

    public int ifTag = 0;  //玩家进行点击

    public GameObject hitGround;  //地面检测的物体,如果有检测到则有

    int gravityCount;   //跳跃时有多少次重力的判定

    int frameCount;  //记录frame有多少次

    public Transform weaponHand; // 获取武器手的引用

    public  swordBody sword;

    public GameObject target;  //当前的目标

    public int direction;   //用来保存主角方向

    //public bool attacking;  //表示是否正在攻击,并传给sword


    // Start is called before the first fr  ame update

    private void Awake()
    {

        leftLeg2 = transform.Find("leftLeg1/leftLeg2");
        rightLeg2 = transform.Find("rightLeg1/rightLeg2");
        

        rigidbody = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        mController = GetComponent<MasterController>();

        this.normalGravityScale = gravityScale;
        //rigidbody.gravityScale = this.gravityScale;

        //GameObject pfb = (GameObject)Resources.Load("Prefabs/sword");

        //Debug.Log("123 "+pfb);

        sword = Instantiate(sword, weaponHand.transform.position, Quaternion.identity, weaponHand.transform);

        sword.father = this.gameObject;

        sword.transform.localEulerAngles = new Vector3(0,0,-90);



        Master.bodyCollector.Add(this);   //加入管理器中


        //collider.GetComponent<BoxCollider2D>();
    }
    void Start()
    {

        //Resources.Load("Prefabs/sword") as GameObject;

        
       // prefabInstance.transform.parent = rightArm;
        //prefabInstance.transform
          //  prefabInstance=Instantiate()

    }

    public Vector2 Walk(float horizontal,Vector2 eggPosition)
    {
        //当status为0时才会添加事件帧,


        //给基本方法添加一个正向与反向标识,当是反向标识时执行不添加事件帧.
        //获取移动所需的参数后进行移动
        //同时需要调动状态机

        //Debug.Log(horizontal);

        if (Mathf.Abs(horizontal) > 0f)
        {
            //表示开始走动,并在这里判断走动的距离,走动的动画在animator中设置
            anim.SetBool("Walking", true);

        }
        else
        {
            anim.SetBool("Walking", false);

        }

        anim.SetFloat("Horizontal", horizontal);

        Vector2 position = new Vector2();

        if (Master.status == 0)
        {
            position.x = position.x + speed * horizontal * Time.deltaTime;

        }
        else if(Master.status==2)
        {
            //position.x = position.x + horizontal;   //当为逆转时则参数为位移的量
            position = eggPosition;
        }
        else
        {
            Debug.Log("Walk的position.x为0");
        }



        if (Master.status == 0)
        {
            DirectionChange(horizontal);  //朝向改变
            Flip(horizontal);
        }
        else if (Master.status == 2)
        {
            DirectionChange(horizontal);  //朝向改变
            Flip(-horizontal);
        }
       

        if (Mathf.Abs(horizontal) > 0f&&Master.status==0)
        {
            if (anim.GetBool("HitWall") == false)   //当没有碰到墙面时,添加事件帧
            {
                AddFrame(this.mController,Master.frame, ActionEnum.movement.walk, transform.position, position.x, 0, 1);  //加入事件帧
            }

        }

        

        return position;

    }

    public static void AddFrame(MasterController mController,int frame, ActionEnum.movement movement,Vector2 position, float parm1, float parm2, int direction)
    {
        //给该目标的时间轴增添事件

        Egg egg = new Egg(frame, movement,position, parm1, parm2, direction);

        //if (egg.movement == ActionEnum.movement.walk)
        //{
        //    Debug.Log(egg.parm1);
        //}

        mController.eggList.Add(egg);   //加入事件帧



    }

    public void AddFrame1(Vector2 position,Vector2 lastPosition)
    {
        //根据目标坐标是否真的改变了来判断是否进行了位移,并对相应的egg进行删减

        for (int i = 0; i < mController.tempList.Count; i++)
        {
            Egg egg = mController.tempList[i];

            if (egg.movement == ActionEnum.movement.walk)  //走路
            {

                if (Mathf.Abs(lastPosition.x - position.x) > 0.01f)  //表示有变化
                {

                    Debug.Log("walk " + Mathf.Abs(lastPosition.x - position.x));
                    mController.eggList.Add(egg);
                }

            }
            else if (egg.movement == ActionEnum.movement.gravity)  //重力
            {

                if (Mathf.Abs(lastPosition.y - position.y) > 0.00001f)  //表示有变化
                {
                    //Debug.Log("gravity " + Mathf.Abs(lastPosition.y - position.y));

                    mController.eggList.Add(egg);
                }

            }
            else   //其他的情况
            {
                mController.eggList.Add(egg);
            }

        }

        mController.tempList.Clear();  //清空暂存表

    }

    public Vector2 Gravity(Vector2 eggPosition)
    {
        //由于rigidbody自带的重力系统怪怪的,所以自己弄一个
        Vector2 position = new Vector2();

        //当参数不为0时执行这个步骤并返回
        if (Master.status == 2)
        {
            //if (parm != 0f)
            //{

                //position.y += parm;
                //Debug.Log("yes");

                position = eggPosition;


                return position;

            //}
        }
        


        if (anim.GetBool("InGround") == false)
        {
            position.y = (float)(position.y - 0.5 * gravityScale * Time.deltaTime);



            if (Master.status == 0)
            {
                AddFrame(this.mController, Master.frame, ActionEnum.movement.gravity,transform.position, position.y, 0, 1);  //加入事件帧
                
            }

        }


        return position;
    }

    public void StopWalk()
    {
        //停止移动,更改状态机中的变量
        anim.SetBool("Walking", false);

        anim.SetFloat("Horizontal", 0);

    }

    private void DirectionChange(float horizontal)
    {
        //改变朝向的方法
        if (horizontal > 0)
        {
            direction = 1;   //朝右
        }

        if (horizontal < 0)
        {
            direction = 0;
        }

    }

    void Flip(float horizontal)
    {
        bool playerHasXSpeed = Mathf.Abs(horizontal) > Mathf.Epsilon;


        // anim.SetBool("ifMirror",horizontal<0?true:false );

            if (horizontal > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);

                //Debug.Log("yes");
            }

            if (horizontal < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

           

    }

    public Vector2 Jumping(float jump,int jumpingParm,Vector2 eggPosition)
    {
        //按住后可以跳更高

        

        //获取anim中jumping的值
        int jumping = anim.GetInteger("Jumping");

        Vector2 position = new Vector2();

        if (Mathf.Abs(jump) > 0)
        {

            if(Master.status==0)
            if (anim.GetBool("InGround") == true)
            {
                anim.SetInteger("Jumping", jumpingTimer);

                
                
            }

            //如果是倒放时,更新之前保存的jumping的值
            if (Master.status == 2)
            {
                anim.SetInteger("Jumping", jumpingParm);
            }

            if (jumping > 0)
            {
                anim.SetInteger("Jumping", jumping-1);


                if (Master.status == 0)
                {
                    //正放时
                    position.y = (float)(position.y + 3 * (float)jumpHeight * Time.deltaTime);


                }
                else if(Master.status!=2)
                {
                    Debug.Log("Jump的position.y为0");
                }

                //此时满足1.按下了jump键,2.jumping仍然大于0
                //position.y = (float)(position.y + 3 * (float)jumpHeight * Time.deltaTime);


                if (Master.status == 0)
                {
                    //必须为正常播放时才会添加事件帧
                    AddFrame(this.mController, Master.frame, ActionEnum.movement.jump, transform.position, position.y,anim.GetInteger("Jumping"), 1);  //加入事件帧
                    //Debug.Log(Master.frame + " " + position.y + " " + anim.GetInteger("Jumping"));

                }



            }

            if (Master.status == 2)
            {
                //回放时
                //position.y = (float)(position.y + height);

                position = eggPosition;



            }
            //表示开始走动,并在这里判断走动的距离,走动的动画在animator中设置

            anim.SetBool("InGround", false);

            return position;

        }
        else
        {
            //anim.SetInteger("Jumping", 0);
            if (jumping > 0)
            {
                anim.SetInteger("Jumping", jumping - 1);
            }

            return position;
        }



    }

    public void Attack(float horizontal,bool attack)
    {
        //攻击状态
        // Debug.Log((float)attack);
        if (attack==true)
        {
            //表示开始走动,并在这里判断走动的距离,走动的动画在animator中设置
            anim.SetInteger("Attack", 1);

        }
        else
        {
            anim.SetInteger("Attack", 0);
        }

        if (attack==true&&Master.status==0)
        {
            AddFrame(this.mController, Master.frame, ActionEnum.movement.attack, transform.position, 0,0, 1);  //加入事件帧
        }

        Flip(horizontal);
    }
 
    public void Dead(int parm)
    {
        //当parm为0时表示是反过来的起死回生
        if (parm == 1)
        {
            anim.SetBool("dead", true);
            AddFrame(this.mController, Master.frame, ActionEnum.movement.dead, transform.position, 0, 0, 1);  //加入事件帧


        }
        else
        {
            anim.SetBool("dead", false);
        }


    }


    public void Fire()
    {
        //调整枪的角度并且发出子弹

        //首先从aiController中获取target,如果没有则朝前方射击
        if (target != null)
        {
            

            Vector2 direction = (target.transform.position - transform.position).normalized;  //获取向量并初始化

            //Debug.Log(direction);

            gunBody gun = (gunBody)sword;   //说明有枪,所以可以转换

            gun.Fire(direction,this.direction);

            //Debug.Log("direction " + this.direction);

            

        }
        else
        {
            //无目标

        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        
    }

    public void DetectWall()
    {
        //检测墙面
        //根据direction不同来射出不同方向的射线,如果射线有检测到物体,则表示人物贴着墙,则位移不算入事件帧

        float directionNum = 0;
        if (this.direction == 0)
        {
            directionNum = -1;
        }
        else
        {
            directionNum = 1;
        }

        Debug.DrawRay(gameObject.transform.position, new Quaternion(directionNum, 0, 0, 0) * gameObject.transform.right.normalized * 0.3f, Color.black);
        Debug.DrawRay(rightLeg2.transform.position, new Quaternion(1f, 0, 0, 0) * gameObject.transform.right.normalized * 0.2f, Color.black);
        Debug.DrawRay(leftLeg2.transform.position, new Quaternion(-1f, 0, 0, 0) * gameObject.transform.right.normalized * 0.2f, Color.black);

        //Debug.DrawRay(rightLeg2.transform.position, new Quaternion(0, 0, -1, 0) * gameObject.transform.up.normalized * 0.5f, Color.black);

        Vector2 direction = new Vector2(directionNum*1f, 0);  //向下的射线

        RaycastHit2D hit;

        Physics2D.queriesStartInColliders = false;  //避免检测到自己

        hit = Physics2D.Raycast(gameObject.transform.position, direction, 0.3f, 1 << LayerMask.NameToLayer("Terrain"));
        // Debug.Log("hit.collider.name " + hit.collider.name);




        if (hit.collider != null)
        {

            //anim.SetBool("InGround", true);
            anim.SetBool("HitWall", true);
            //hitGround = hit.collider.gameObject;

            return;
        }
        else
        {
            Transform temp;
            //左右脚检测
            if (this.direction == 1)
            {
                temp = rightLeg2;
            }
            else
            {
                temp = leftLeg2;
            }

            hit = Physics2D.Raycast(temp.transform.position, direction, 0.2f, 1 << LayerMask.NameToLayer("Terrain"));

            if (hit.collider != null)
            {

                //anim.SetBool("InGround", true);
                anim.SetBool("HitWall", true);
                //hitGround = hit.collider.gameObject;

                return;
            }
            else
            {
                //hitGround = null;
                anim.SetBool("HitWall", false);
            }

           
            //anim.SetBool("InGround", false);
        }

    }


    
    public void DetectGround()
    {
        //检测脚下是否有地板

        //通过射线检测,发射一个向下的射线,如果这个射线和该角色碰撞的物体的其中之一
        //相等,则表示碰到地面,此时anim的InGround为true,InAir为false,可
        //再进行跳跃

        //1.1
        //用左右脚发射射线,先判断左脚,后右脚,如果有则直接返回即可

        Debug.DrawRay(gameObject.transform.position, new Quaternion(0,0,-1,0) * gameObject.transform.up.normalized * 1f, Color.black);
        //Debug.DrawRay(rightLeg2.transform.position, new Quaternion(0, 0, -1, 0) * gameObject.transform.up.normalized * 0.5f, Color.black);

        Vector2 direction = new Vector2(0, -1f);  //向下的射线

        RaycastHit2D hit;

        Physics2D.queriesStartInColliders = false;  //避免检测到自己

        hit = Physics2D.Raycast(gameObject.transform.position, direction,1f, 1 << LayerMask.NameToLayer("Terrain"));
        // Debug.Log("hit.collider.name " + hit.collider.name);

        if (hit.collider != null)
        {
            
            anim.SetBool("InGround", true);

            hitGround = hit.collider.gameObject;

            return;
        }
        else
        {
            hitGround = null;

            //Debug.Log("hitGround no");

            anim.SetBool("InGround", false);
        }


    }


    public void BeginAttack()
    {
        swordBody weapon =sword.GetComponent<swordBody>();

        
        weapon.attacking = true;

        if (Master.status == 2)
        {
            anim.SetInteger("Attack", 0);   //逆转状态时攻击状态也不能太久
            weapon.attacking = false;
        }



        //todo
        //增加伤害计算
    }

    public void AnimRecover()
    {
        //因为当动画播放速度为-1时无法进行正常地切换动画,所以在这里切换
        if (Master.status == 2)
        {
            anim.SetBool("Recover", true);

        }

    }

    //public void AnimNormalize()
    //{
    //    anim.SetFloat("Recover", 0);

    //}



    public void EndAttack()
    {
        swordBody weapon = sword.GetComponent<swordBody>();

        anim.SetInteger("Attack", 0);

        weapon.attacking = false;




    }

    public void AutoStop(List<Egg> eggList,int frame)
    {
        //根据自身的eggList的最后的frame与Master.frame是否超过一定值,如果超过一定值则暂停
        if (frame-eggList[eggList.Count - 1].frame > 200)
        {
            //最后记录的帧数如果和现在的帧数超过200则自动暂停
            Master.Stop();

        }

    }

    // Update is called once per frame
    void Update()
    {
      
    }
}

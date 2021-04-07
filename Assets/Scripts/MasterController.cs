using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    //所有人物做的所有动作都要经过这个方法
    public List<Egg> eggList=new List<Egg>();   //存放事件帧

    //public List<Egg> tempList = new List<Egg>();  //用于刚创建的egg,但是还不一定添加的egg
    //public LinkedList
    // public int a;
    private int frameIndex=0;  //当currentDirection为0时存储索引位置的索引

    //private int lastFrame=0;  //记录上一次访问的frame

    private int frameEqualled;   //frame已经相等过了,用于FindEgg中

    private Rigidbody2D rigidbody;

    private humanBody body;

    private void Awake()
    {
        body = GetComponent<humanBody>();

        rigidbody = GetComponent<Rigidbody2D>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //awdqwfqwwad
    private Vector2 Replay(Egg egg,int direction)
    {

        Vector2 position = transform.position;
        
        //direction决定是否和egg原有的direction一致,如果direction都为1则一致,为-1则相反

        //根据参数来进行各自的动作
        switch(egg.movement)
        {
            case ActionEnum.movement.walk:
                position=body.Walk(direction*-egg.parm1,egg.position);
                break;
            case ActionEnum.movement.attack:
                position=body.Attack(body.direction, true,egg.position);
                //Debug.Log("Attack");
                break;
            case ActionEnum.movement.jump:
                position = body.Jumping(1, (int)egg.parm2,egg.position);   //实现跳一百帧就是插入一百次即可
                //Debug.Log(egg.parm1 + "   " + egg.parm2);
                break;
            case ActionEnum.movement.dead:
                //Debug.Log("复活");
                body.Dead(1);
                position = egg.position;  //可能也要赋予位置
                break;
            case ActionEnum.movement.gravity:
                position=body.Gravity(egg.position);
                //Debug.Log("gravity");
                break;
            case ActionEnum.movement.bulletMove:
                position = egg.position;
                //Debug.Log("gravity");
                break;
            case ActionEnum.movement.bulletBack:
             
                transform.GetComponent<bullet>().speed = direction * -egg.parm1;

                //gameObject.SetActive(true);
                //Debug.Log(egg.parm1);
                GetComponent<BoxCollider2D>().enabled = true;
                
                //修改子弹的z坐标为-3
                transform.position = new Vector3(egg.position.x,egg.position.y,3);

                position = transform.position;
                //Debug.Log(transform.position);
                //Debug.Log("gravity");
                break;
            case ActionEnum.movement.bulletGone:
                position = egg.position;
                //当frame还可以计入事件帧时才会销毁
                if (direction == Master.currentDirection)
                {
                    Debug.Log("bullet销毁");
                    Destroy(gameObject, 0);    //去除子弹

                }
               
                break;
            case ActionEnum.movement.targetConfirm:
                position = egg.position;
                body.GetAnimator().SetFloat("Speed", 1f);
                //body.SetFloat("Speed", 1f);

                body.tenetDirection = 1;     //将其状态改回原来的
                break;

            case ActionEnum.movement.findMainguy:
                position = egg.position;

                if (body.tenetDirection == Master.currentDirection)
                {
                    body.target = null;
                }

                break;
            default:
                break;

        }

        return position;

        //123
    }

    public Vector2 FindEgg(int tenetDirection)
    {
        //需要知道当前生物的tenetDirection


        //当在进行回放时,根据Master的frame的值来再eggList中寻找对应的egg并执行
        Egg currentEgg;

        Vector2 position = transform.position;


        if ((Master.currentDirection == 1&&tenetDirection==1)||
            (Master.currentDirection==0&&tenetDirection==0))
        {

            //正常找蛋
            while (true)
            {
                //Debug.Log(eggList.Count) ;

                if (eggList.Count > 0)
                {
                    currentEgg = eggList[eggList.Count - 1];  //获取最后一个元素
                }
                else
                {

                    //Debug.Log("frame不为0");
                    if (Master.frame == 0)
                    {
                        Debug.Log("frame为0");

                        //此时表示已经又到了原点,则进入暂停状态
                        Master.Stop();
                    }
                    else
                    {
                        Debug.Log("eggList.count为0");
                    }

                    return position;
                }


                if (tenetDirection == 1)
                {
                    if (currentEgg.frame == Master.frame)
                    {
                        position = Replay(currentEgg, 1);
                        eggList.Remove(currentEgg);
                    }
                    else if (currentEgg.frame > Master.frame)
                    {
                        eggList.Remove(currentEgg);
                    }
                    else
                    {
                        return position;
                    }
                }
                else if (tenetDirection == 0)
                {
                    if (currentEgg.frame == Master.frame)
                    {
                        position = Replay(currentEgg, 1);
                        eggList.Remove(currentEgg);
                    }
                    else if (currentEgg.frame < Master.frame)
                    {
                        eggList.Remove(currentEgg);
                    }
                    else
                    {
                        return position;
                    }
                }
                

            }

        }
        else if ((Master.currentDirection == 0&&tenetDirection==1)||
            (Master.currentDirection==1&&tenetDirection==0))
        {
            //此时可能有两个方向的差别
            //正向的敌人两个方向都只是读蛋且不删蛋,要保存索引

            int beenLeft = 0;   //表示到过坐左边
            int beenRight = 0;    //表示到过右边


            while (true)
            {

                if (frameIndex == eggList.Count - 1)
                {
                    beenRight = 1;   //表示只会往左边遍历
                }
                if (frameIndex == 0)
                {
                    beenLeft = 1;    //只会往右边遍历
                }


                if (beenLeft == 1 && beenRight == 1)
                {
                    //表示两边都到过了,此时返回
                    return position;
                }

                //Debug.Log(frameIndex);

                //Debug.Log(frameIndex + " " + eggList.Count);

                if (eggList[frameIndex].frame == Master.frame)
                    {

                    int temp = Master.status == 2 ? 1 :-1;
                        //Debug.Log("执行");
                        //执行
                        position = Replay(eggList[frameIndex], temp);  //Replay表示正放回放,哈哈咯

                    //lastFrame = eggList[frameIndex].frame;  //更新lastFrame的值
                        frameEqualled = 1;

                        if (beenRight==1)
                        {
                            frameIndex--;
                        }
                        else if (beenLeft == 1)
                        {
                            frameIndex++;

                        }
                        else
                        {
                            frameIndex++;
                        }
                    }
                    else if (eggList[frameIndex].frame < Master.frame)
                    {
                        beenLeft = 1;

                        if (frameEqualled == 1)
                        {
                        frameEqualled = 0;

                        return position;
                        }
                        else
                        {
                            frameIndex++;
                        }
                    }
                    else if (eggList[frameIndex].frame > Master.frame)
                    {

                    beenRight = 1;
                        if (frameEqualled == 1)
                        {
                            frameEqualled = 0;
                            return position;
                        }
                        else
                        {
                            frameIndex--;
                        }

                    }

                if (frameIndex >= eggList.Count - 1 || frameIndex == 0)
                {
                    //表示已经是最大索引,返回
                   // Debug.Log("向右或向右B类FindEgg到最大值啦");

                    frameIndex = eggList.Count - 1;

                    frameEqualled = 0;

                    //Master.Stop();

                    return position;
                }

                
            }


        
            

        }


        Debug.Log("找蛋失败");   //如果到这里说明出现问题
        return position;   //
        
    

    }

    // Update is called once per frame
    void Update()
    {

        //如果Master.status为2表示在倒放

        //if(gameObject.tag=="Player")
            //Debug.Log(eggList.Count);
    }
}

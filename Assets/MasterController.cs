using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{
    //所有人物做的所有动作都要经过这个方法
    public List<Egg> eggList=new List<Egg>();   //存放事件帧

    public List<Egg> tempList = new List<Egg>();  //用于刚创建的egg,但是还不一定添加的egg
    //public LinkedList
   // public int a;

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
        
        //direction决定是否和egg原有的direction一致,如果direction都为1则一致,为0则相反

        //根据参数来进行各自的动作
        switch(egg.movement)
        {
            case ActionEnum.movement.walk:
                position=body.Walk(-egg.parm1,egg.position);
                break;
            case ActionEnum.movement.attack:
                body.Attack(body.direction, true);
                Debug.Log("Attack");
                break;
            case ActionEnum.movement.jump:
                position = body.Jumping(1,(int)egg.parm2,egg.position);   //实现跳一百帧就是插入一百次即可
                //Debug.Log(egg.parm1 + "   " + egg.parm2);
                break;
            case ActionEnum.movement.dead:
                body.Dead(-1);
                break;
            case ActionEnum.movement.gravity:
                position=body.Gravity(egg.position);
                //Debug.Log("gravity");
                break;
            case ActionEnum.movement.bulletMove:
                position = body.Gravity(egg.position);
                //Debug.Log("gravity");
                break;
            case ActionEnum.movement.bulletBack:
                position = body.Gravity(egg.position);
                //Debug.Log("gravity");
                break;
            case ActionEnum.movement.bulletGone:
                position = body.Gravity(egg.position);
                //Debug.Log("gravity");
                break;

            default:
                break;

        }

        return position;


    }

    public Vector2 FindEgg()
    {
        //当在进行回放时,根据Master的frame的值来再eggList中寻找对应的egg并执行
        Egg currentEgg;

        Vector2 position = transform.position;
       

        while (true)
        {
            if (eggList.Count > 0)
            {
                currentEgg = eggList[eggList.Count - 1];  //获取最后一个元素
            }
            else
            {

                Debug.Log("frame不为0");
                if (Master.frame == 0)
                {
                    Debug.Log("frame为0");

                    //此时表示已经又到了原点,则进入暂停状态
                    Master.Stop();
                }

                return position;
            }


            if (currentEgg.frame == Master.frame)
            {
                position=Replay(currentEgg, 1);
                //Debug.Log("current:master" + currentEgg.frame + " " + Master.frame);

                eggList.Remove(currentEgg);

                
            }
            else if (currentEgg.frame > Master.frame)
            {
                eggList.Remove(currentEgg);
                Debug.Log("no");
                
            }
            else
            {
                //Debug.Log("current:master " + currentEgg.frame + " " + Master.frame) ;
                return position;
            }
            
        }

        
    

    }

    // Update is called once per frame
    void Update()
    {

        //如果Master.status为2表示在倒放

        //if(gameObject.tag=="Player")
            //Debug.Log(eggList.Count);
    }
}

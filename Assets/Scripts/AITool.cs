using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITool : MonoBehaviour
{
    // Start is called before the first frame update

    //存放用于控制敌人的静态方法

    private void Awake()
    {
        Random.InitState(1000);  //刷新随机数
    }

    public static Vector2 Move(humanBody body,int direction,int speed)
    {
        

       
        //控制敌人的行走
        if (direction == 1)
        {
           return body.Walk(1,new Vector2());
        }
        else
        {
           return body.Walk(-1, new Vector2());
        }
    }

    public static void Attack(humanBody body)
    {
        body.Attack(body.direction, true);
    }

    public static void RandomAttack(humanBody body,int IQ)
    {
        //根据IQ的高低来决定攻击的频率
        

        Random.Range(1, 2);

        //随机到数时才会攻击

        if (Random.Range(1, 300/IQ) == 1)
        {
            //此时攻击
            AITool.Attack(body);
        }
       

    }

    public static Vector2 Jump(humanBody body,int jumpHeight)
    {
        if (jumpHeight > 0)
        {
            return body.Jumping(1,0, new Vector2());
        }

        //如果jumpHeight为0表示跳跃结束
        return new Vector2(0, 0);
    }


    public static void IfLost(GameObject target,humanBody body,float lostDistance)
    {
        //检测当前目标与自身的距离,跳远则舍弃目标
        if (target != null)
        {
            float dis = (target.transform.position - body.transform.position).magnitude;   //看下当前的距离

            if (dis > lostDistance)
            {
                //大于丢失距离,目标舍弃
                target = null;
                Debug.Log("距离太远,目标丢失");

            }

            //Debug.Log(dis);
        }


    }

    public static Vector2 ApproachOrAway(GameObject target,humanBody body,float faceDistance,int speed,int e)
    {
        //e参数为1时表示靠近目标,为0时表示远离目标
        Vector2 position = new Vector2(0, 0);

        

        float distance = 0;   //根据目标的类型来决定要靠多近,如果是巡逻点则为0即可

        //当有目标后靠近目标或者远离
        if (target != null)
        {
            //靠近还是远离由这个参数决定
            int leftX = 0;
            int rightX = 1;

            if (e == 1)
            {
                //靠近
                leftX = 0;
                rightX = 1;
            }
            else if (e == 0)
            {
                //远离
                leftX = 1;
                rightX = 0;
            }

            
            //如果是玩家,则要根据faceDirection来决定靠的距离

            if (target.layer == LayerMask.NameToLayer("Player"))
            {
                //表示目标是玩家
                distance = faceDistance;

                //Debug.Log("目标为玩家");
            }

            


            //Debug.Log("正在调用");
            //判断目标在左边还是右边来靠近
            if (body.transform.position.x - target.transform.position.x >= 0.9+distance)
            {

               // Debug.Log(body.transform.position.x - target.transform.position.x);
                //向左
                position+=AITool.Move(body, leftX, speed);
            }
            else if (target.transform.position.x - body.transform.position.x >= 0.9+ distance)
            {
                //Debug.Log(target.transform.position.x - body.transform.position.x);
               
                //向右
                position += AITool.Move(body, rightX, speed);

            }
            else    //表示背对着的时候  分别为两个方向背对的时候
            {


                //前面有讲到distance不为0时表示目标为玩家,此时进行转向的判定
                //表示目标为人
                if (distance != 0)
                {

                    //humanBody targetBody = target.GetComponent<humanBody>();


                    if (((target.transform.position.x < body.transform.position.x && body.direction == 1) ||
                     (target.transform.position.x > body.transform.position.x && body.direction == 0)))
                    {

                        position+=AITool.ApproachOrAway(target, body, -0.9f, speed, e);
                    }
                    else
                    {
                        body.StopWalk();
                    }


                    //if (targetBody.direction == body.direction)
                    //{
                    //    //表示要转向了.
                    //    //再调用一次,这次肯定会转向
                        

                    //}

                }
                else
                {
                    //目标为物体
                    body.StopWalk();
                }

               
            }
               
                

        }

        return position;

    }

    public static Vector2 Patrol(GameObject target, humanBody body,AIController aiController,int speed)
    {
        //如果有巡逻点 并且无目标时进行巡逻操作
        Vector2 position = new Vector2(0, 0);
        int size = aiController.patrolDot.Length;  //记录巡逻点的个数

        if (target == null&&size>0)   //无目标且巡逻个数大于0
        {
            //向这个巡逻点靠近
            position+=AITool.ApproachOrAway(aiController.patrolDot[aiController.patrolState], body,aiController.faceDistance, speed, 1);

            //Debug.Log("qwe");


            //如果和巡逻点无限靠近了,则patrolstate++
            float dis = (aiController.patrolDot[aiController.patrolState].transform.position - body.transform.position).magnitude;
            //Debug.Log(size-1);
            if (dis< 1){

                int waitTime = aiController.patrolDot[aiController.patrolState].GetComponent<patrolDot>().waitTime;   //获取巡逻节点的等待时间

                if (aiController.timer < waitTime)
                {
                    aiController.timer++;
                }
                else
                {
                    if (aiController.patrolState < size - 1)
                    {
                        aiController.patrolState++;
                    }
                    else
                    {
                        //回到起点
                        aiController.patrolState = 0;
                    }

                    aiController.timer = 0;   //计时器归零

                }

                

            }

        }

        return position;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float VectorAngle(Vector2 from, Vector2 to)
     {
         float angle;
        
         Vector3 cross = Vector3.Cross(from, to);
         angle = Vector2.Angle(from, to);
         return cross.z > 0 ? -angle : angle;
     }


}

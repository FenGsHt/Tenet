using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    //子弹的信息  粒子系统的触发都在这里改
    public float speed=8f;  //控制速度
    public Vector2 direction;  //角度

    private MasterController mController;   //控制frame;

    [HideInInspector]
    public GameObject owner;  //知道是谁发射的

    Rigidbody2D rigidbody;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        mController = GetComponent<MasterController>();

        //生成时就要加一个frame,因为倒放的时候到这里就直接删除即可
        humanBody.AddFrame(this.mController, Master.frame, ActionEnum.movement.bulletGone, transform.position, 0, 0, 1);


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //当碰到物体时,使用粒子效果,并且消失,如果碰到的是玩家的话(非自己),则进入伤害步骤

        if(collision.gameObject.tag!="JumpingBoard")
        if (collision.gameObject != owner)
            {

           
           // Debug.Log("name " + owner);
           // Debug.Log("bullet "+ collision.gameObject.name);


            //如果击中的是人或敌人
                if (collision.gameObject.layer == LayerMask.NameToLayer("Player")||collision.gameObject.layer==LayerMask.NameToLayer("Enemy"))
                {
                    //如果碰到的是玩家,则进入伤害判断
                    //gameObject.tag

                    //设置死亡动画
                    humanBody body = collision.GetComponent<humanBody>();

                    // Debug.Log(col)
                    //123
                    body.Dead(1);

                }

                speed = 0f;
                gameObject.active = false;   //暂时不进行运动
                humanBody.AddFrame(this.mController, Master.frame, ActionEnum.movement.bulletBack, transform.position, 0, 0, 1);

                //Destroy(this.gameObject);


            }

    }

    // Update is called once per frame
    void Update()
    {
        //根据角度和速度进行位移
        Vector2 position = transform.position;

        if (Master.status == 0)
        {
            position += direction * speed * Time.deltaTime;

            //增加frame
            humanBody.AddFrame(this.mController, Master.frame, ActionEnum.movement.bulletMove, transform.position, 0, 0, 1);

        }


        if (Master.status == 2)
        {
            position = mController.FindEgg();
        }




        rigidbody.MovePosition(position);

        
    }
}

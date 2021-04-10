using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    //子弹的信息  粒子系统的触发都在这里改
    public float speed=8f;  //控制速度

    //[HideInInspector]
    //public float normalSpeed;  //z正常的速度

    public Vector2 direction;  //角度

    private int timer; //记录子弹消失的时间

    //[HideInInspector]
    public int tenetDirection;   //

    private MasterController mController;   //控制frame;

    [HideInInspector]
    public GameObject owner;  //知道是谁发射的

    Rigidbody2D rigidbody;

    public ParticleSystem particle;  


    private void Awake()
    {
        // normalSpeed = speed;
        particle = Instantiate(particle, transform.position, Quaternion.identity, transform) ;

        particle.Stop();

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
        if(Master.status==0)
        if(collision.gameObject.tag!="JumpingBoard")
        if (collision.gameObject != owner)
            {


                    // Debug.Log("name " + owner);
                    // Debug.Log("bullet "+ collision.gameObject.name);
                    particle.Play();

            //如果击中的是人或敌人
                if (collision.gameObject.layer == LayerMask.NameToLayer("Player")||collision.gameObject.layer==LayerMask.NameToLayer("Enemy"))
                {
                    //如果碰到的是玩家,则进入伤害判断
                    //gameObject.tag

                    //设置死亡动画
                    humanBody body = collision.GetComponent<humanBody>();


                    swordBody.AttackJudge(body, this.owner.GetComponent<humanBody>());
                    // Debug.Log(col)
                    //123
                    //body.Dead(0);

                }

                GetComponent<BoxCollider2D>().enabled = false;
                //修改子弹的z坐标为-3
                
                    //gameObject.SetActive(false);   //暂时不进行运动
                    //Debug.Log("123");
                humanBody.AddFrame(this.mController, Master.frame, ActionEnum.movement.bulletBack, transform.position, this.speed, 0, this.tenetDirection);

                transform.position = new Vector3(transform.position.x, transform.position.y, -10);
                speed = 0f;

                    //Destroy(this.gameObject);

                }

    }

    // Update is called once per frame
    void Update()
    {
        //123

        

            //根据角度和速度进行位移
            Vector2 position = transform.position;

            //if(this.speed!=0)
            if(Master.status!=1)
            if (Master.status == 2||Master.currentDirection!=this.tenetDirection)
            {
                this.timer--;

                position = mController.FindEgg(tenetDirection);
            }
            else if (Master.status == 0)
                {

                this.timer++;

                if (this.timer > 300)   //相当于超过8秒后子弹消失
                {
                    Destroy(gameObject);
                }

                position += direction * speed * Time.deltaTime;

                    //增加frame
                    humanBody.AddFrame(this.mController, Master.frame, ActionEnum.movement.bulletMove, transform.position, 0, 0, this.tenetDirection);

                }


            




            rigidbody.MovePosition(position);


        
       

        
    }
}

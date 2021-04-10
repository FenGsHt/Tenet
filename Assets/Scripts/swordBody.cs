using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordBody : MonoBehaviour
{
    // Start is called before the first frame update
    public bool attacking;
    public int damage;  //通过父类的信息传递伤害值
    public GameObject father;   //剑的持有人,防止打到自己

    //public ParticleSystem particle; //剑击中时的特效
    public GameObject crack;

    public ParticleSystem blood;   //保存blood的particles

    public ParticleSystem ghosting;  //武器拖影


    private Transform hitPoint;  //特效释放的位置

    [HideInInspector]
    public GameObject owner;


    private void Awake()
    {
        hitPoint = transform.Find("HitPoint");

        blood = Instantiate(blood, hitPoint.position, Quaternion.identity,hitPoint);

        ghosting= Instantiate(ghosting, hitPoint.position, Quaternion.identity, hitPoint);

        blood.Stop();

        ghosting.Stop();
        //particle.Stop();
    }

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当在攻击时进行攻击判断
        if(collision.gameObject.layer != father.gameObject.layer)   //同类之间不会打到
        if (attacking == true)
        {
            Debug.Log("击中的是 "+collision);

            //设置死亡动画
            humanBody body=collision.GetComponent<humanBody>();

                // Debug.Log(col)
                //123
                bool hit;

            //是否命中的判断
            hit=AttackJudge(body, father.GetComponent<humanBody>());

                if (hit == true)   //表示命中,此时播放动画
                {
                    blood.Play();

                    Instantiate(crack, hitPoint.position, this.transform.rotation);
                }





                attacking = false;
                //crack.SetActive(true);
                // particle.Play();
                //Destroy(particle, particle.main.duration);
                

            }


    }

    public static bool AttackJudge(humanBody targetBody,humanBody sourceBody)
    {
        
        //先判断是否已经死亡
        //分为多种情况
        //1.回放时,则如果是同纬度的人则不判断,非同纬度则倒退
        //2.正常情况,正常死亡
        //3.正常情况下,但是打到的不是同一纬度的人,则倒退
        if (targetBody == null || sourceBody == null)
        {
            Debug.Log("body为null");
        }

        //用于攻击后是否命中的判断
        if (Master.status == 2 && targetBody.tenetDirection == sourceBody.tenetDirection)
        {
            Debug.Log("1");
        }
        else if (Master.currentDirection != sourceBody.tenetDirection
                && targetBody.tenetDirection == sourceBody.tenetDirection)
        {
            //表示攻击者不在当前纬度且攻击到的目标和其一样不在当前纬度,则不进行判断
            Debug.Log("2");
        }         
        else if (targetBody.tenetDirection != sourceBody.tenetDirection) //伤害判断前先判断是否在同一纬度,如果不在同一纬度则发生事件
        {

            //此时为发生错误,需要让玩家自己进行倒退

            Master.mistakeTimer = 40;  //进行强制倒退
        }
        else
        {

            Debug.Log("有死");
            targetBody.Dead(0);

            return true;
        }



        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.attacking == true)
        {
            ghosting.Play();  //播放拖影动画
        }

    }
}

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
        if (attacking == true)
        {
            Debug.Log("击中的是 "+collision);

            //设置死亡动画
            humanBody body=collision.GetComponent<humanBody>();

           // Debug.Log(col)
           //123
            body.Dead(0);

            Instantiate(crack, hitPoint.position, this.transform.rotation);

            attacking = false;
            //crack.SetActive(true);
            // particle.Play();
            blood.Play();
            //Destroy(particle, particle.main.duration);
            

        }


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

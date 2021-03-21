using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordBody : MonoBehaviour
{
    // Start is called before the first frame update
    public bool attacking;
    public int damage;  //通过父类的信息传递伤害值
    public GameObject father;   //剑的持有人,防止打到自己

    [HideInInspector]
    public GameObject owner;

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
            body.Dead(1);
            

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

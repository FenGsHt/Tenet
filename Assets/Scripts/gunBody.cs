using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunBody : swordBody
{

    //继承自swordBody
    public bullet bulletPrefabs;


    


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
       // ActionEnum.action a;
    }

    public void Fire(Vector2 direction,int horizontal)
    {
        //horizontal表示主角的朝向
        //+horizontal*new Vector3(1.2f,0,0)
        //赋予角度即可

        if (horizontal == 0)
        {
            horizontal = -1;
        }

        //生成并发射子弹
        bullet b = Instantiate(bulletPrefabs, transform.position + new Vector3(horizontal*1.2f, 0, 0), Quaternion.identity);

        b.direction = direction;   //赋予角度
        b.owner = this.father;

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

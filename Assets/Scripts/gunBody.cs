using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunBody : swordBody
{

    //继承自swordBody
    public bullet bulletPrefabs;

    public ParticleSystem particle;   //粒子系统

    private Transform firePoint;   //子弹出口点
    


    private void Awake()
    {
        firePoint= transform.Find("FirePoint");

        particle = Instantiate(particle, firePoint);
        //particle.gameObject.transform.parent = this.transform;
        particle.Stop();
        //particle = Instantiate(particle);
       
    }

    // Start is called before the first frame update
    void Start()
    {
       // ActionEnum.action a;
    }

    public void Fire(Vector2 direction,int horizontal,int tenetDirection)
    {
        //horizontal表示主角的朝向
        //+horizontal*new Vector3(1.2f,0,0)
        //赋予角度即可

        if (horizontal == 0)
        {
            horizontal = -1;
        }

        //生成并发射子弹
        bullet b = Instantiate(bulletPrefabs,firePoint.position , Quaternion.identity);

        b.direction = direction;   //赋予角度
        b.owner = this.father;
        b.tenetDirection = tenetDirection;

        //Debug.Log(particle);
        particle.Play();   //播放粒子效果
        //Instantiate(particle).Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

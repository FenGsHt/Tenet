using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenetDoor : MonoBehaviour
{
    //public GameObject mainGuyPrefabs;   //逆转主角的预制体
    //玩家碰到该物体后开启逆转

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //判断是否是主角
        //然后生成二号主角
        //操作柄手交换
        //改变世界状态

        if (collision.gameObject.tag == "Player")
        {

           

            collision.gameObject.SetActive(false);

            Master.AddReverseMainGuy(new Vector3(transform.position.x, transform.position.y+5,0));

            

            //Master.Reverse();  //进入逆转状态

            

            //暂时关闭碰撞器
            //Destroy(gameObject);
            GetComponent<BoxCollider2D>().enabled = false;   

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

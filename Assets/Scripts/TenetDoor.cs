using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenetDoor : MonoBehaviour
{
    //public GameObject mainGuyPrefabs;   //逆转主角的预制体
    //玩家碰到该物体后开启逆转
    //CursorController cController;
    // Start is called before the first frame update
    void Start()
    {
        //cController = CursorController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //判断是否是主角
        //然后生成二号主角
        //操作柄手交换
        //改变世界状态

        if (collision.gameObject.tag == "Player")
        {
            if (Master.killedEnemyNum == Master.totalEnemyNum)    //当敌人都杀完后才会触发反向
            {



                //collision.gameObject.SetActive(false);

                Master.AddReverseMainGuy(new Vector3(transform.position.x, transform.position.y + 5, 0));

                Master.CalculateStageEnemy();   //计算阶段敌人数量

                Master.EnemyFrozen(1f);   //解封

                //Master.Reverse();  //进入逆转状态



                //暂时关闭碰撞器
                //Destroy(gameObject);
                GetComponent<BoxCollider2D>().enabled = false;

            }
            else
            {
                HintManager.SetText("需要把敌人杀完才能进逆转门", 200);
            }
        }
            
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}

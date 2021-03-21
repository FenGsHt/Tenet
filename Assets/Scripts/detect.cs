using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detect : MonoBehaviour
{

    public float angle = 90f;   //检测的角度范围

    public float distance = 0.5f;   //检测距离

    public float rotatePerSecond = 90f;   //每秒旋转角度

    public float accuracy = 1f;   //检测精度

    public humanBody body;

    private AIController aiController;  //存放这个



    //用于敌人检测周围的状况,这个游戏中只需要检测玩家即可


    // Start is called before the first frame update
    void Start()
    {
        //transform.for
        body = GetComponent<humanBody>();

        aiController = GetComponent<AIController>();

        
    }
    
    
    private bool LookAround(GameObject controller,Quaternion eulerAnger,float distance,Color DebugColor)
    {

        Vector3 rotation=new Vector3(0,0,0);


        if (body.direction >= 1)
        {
            Debug.DrawRay(controller.transform.position, eulerAnger* controller.transform.right.normalized * distance, DebugColor);

            rotation = eulerAnger * controller.transform.right.normalized * distance;
        }
        else if (body.direction == 0)
        {
          
            //角度转换,乘(0,0,1)后方向转180度
            eulerAnger *= Quaternion.Euler(new Vector3(0, 0, 1));
           
            Debug.DrawRay(controller.transform.position, eulerAnger * controller.transform.right.normalized * distance, DebugColor);

            rotation = eulerAnger * controller.transform.right.normalized * distance;
        }



        Physics2D.queriesStartInColliders = false;  //避免检测到自己

        

        RaycastHit2D hit = Physics2D.Raycast(controller.transform.position,rotation, distance, 1 << LayerMask.NameToLayer("Player")|1<< LayerMask.NameToLayer("Terrain"));
        //// && hit.collider.CompareTag("Player")
        if (hit.transform!=null)
        {
            //Debug.Log(hit.collider.gameObject.layer);

            // int layer = ;
            //Debug.Log(LayerMask.NameToLayer("Player"));

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //表示检测到人了,此时返回人的变量给aiController

                body.target = hit.collider.gameObject;

               // Debug.Log("打中的是人");
            }
            
            return true;
        }
        return false;


    }

    private bool Look()
    {
        float subAngle = angle / accuracy;

        for(int i = 0; i < accuracy; i++)
        {
            if(LookAround(gameObject,Quaternion.Euler(0, 0, -angle / 2 + i * subAngle + Mathf.Repeat(rotatePerSecond * Time.time, subAngle)), distance, Color.black))
            {

                return true;
            }

        }

        return false;

    }


    // Update is called once per frame
    void Update()
    {
        Look();
    }
}

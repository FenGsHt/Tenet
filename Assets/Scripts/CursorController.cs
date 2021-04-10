using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController:MonoBehaviour
{
    //public Texture2D killCursor;  //保存击杀图标

    //public Sprite ki
    //private static CursorController instance;
    RaycastHit[] hits;

    public static Quaternion direction;  //记录鼠标和主角之间的方向向量

    public static Vector3 normalizedDirection;   //单位化的角度

    public GameObject mainGuy;  //存储主角

    [HideInInspector]
    public humanBody target;   //要进行变更的目标
    //RaycastHit hit;   //进行射线检测



    //控制鼠标图案的变化等    
    private void Awake()
    {
        //instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void SetDefaultCursor()
    {

        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void SetKillCursor()
    {
        //Cursor.SetCursor(killCursor, Vector2.zero, CursorMode.Auto);

    }

    public void GetDirection()
    {
        Vector3 mainGuyPosition = Vector3.zero ;

        if (Master.currentDirection == 1)
        {
            mainGuyPosition = mainGuy.transform.position;
        }
        else
        {
            //当为反方向时从master中提取mainGuy
            mainGuyPosition = Master.ReverseMainGuy.transform.position;
        }

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float y = position.y - mainGuyPosition.y;
        float x = position.x - mainGuyPosition.x;
        //this.direction = new Vector3(0, 0, Mathf.Asin(y/(Mathf.Abs(x*x)+Mathf.Abs(y*y))));
        //this.direction = direction.Normalize();
        //Debug.Log(this.direction);
        //Camera.main.ScreenToWorldPoint

        //主角根据角度的不同改变一个骨骼的角度

        float temp = Mathf.Sqrt(Mathf.Abs(x * x) + Mathf.Abs(y * y));

        if (x > 0)
        {

            //Debug.Log(Mathf.Asin(y / temp) * 180f / Mathf.PI);

            temp = Mathf.Asin(y / temp) * 180f / Mathf.PI;
        }
        else
        {
            //Debug.Log(180-(Mathf.Asin(y / temp) )* 180f / Mathf.PI);
            temp = 180 - (Mathf.Asin(y / temp)) * 180f / Mathf.PI;

        }

        CursorController.normalizedDirection = new Vector3(x, y,0).normalized;

        //Debug.Log(CursorController.normalizedDirection);
        CursorController.direction = Quaternion.Euler(new Vector3(0,0,temp));
        //mainGuy.transform.Find("rightArm1").transform.rotation = CursorController.direction;

        //mainGuy.transform.Find("rightArm1").transform.rotation = CursorController.direction;

        //Mathf.Sin(30f/180f*Mathf.PI) 为0.5
    }

    private void FindTarget()
    {

        //if(this.target!=null)
        //Debug.Log(this.target);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //检测结果
        RaycastHit hitInfo;
     
        
        Physics2D.queriesStartInColliders = false;  //避免检测到自己
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Quaternion(0, 0, -1, 0) * gameObject.transform.up.normalized * 0.3f, Color.black);
        Debug.DrawRay(ray.origin, ray.direction*50f , Color.black);

        //hits = Physics2D.RaycastAll(ray.origin,new Vector3(0,0,50),50f,1<<LayerMask.NameToLayer("Terrain"));
        Physics.Raycast(ray, out hitInfo, 200,1<<LayerMask.NameToLayer("CursorCollider"));


        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.GetComponent<ChildCollider>() != null)
            {
                ChildCollider cCollider= hitInfo.collider.gameObject.GetComponent<ChildCollider>();

                if (cCollider.father.GetComponent<Animator>().GetBool("dead") == false)    //且不为死亡状态
                {
                    cCollider.SetKillCursor();

                    this.target = cCollider.father.GetComponent<humanBody>();

                    return;
                }
            }
            else
            {
                Debug.Log("判定失败");
            }

           
                //Debug.Log(hitInfo);
                //Debug.Log(hit.collider.gameObject.name);//打印鼠标点击到的物体名称

                //Debug.Log(LayerMask.NameToLayer(""));



                //Debug.Log("鼠标检测到敌人");

               

        }


        //此时表示鼠标移出了物体,物体原有的预约图案变没
        if (this.target != null&&this.target.tenetDirection==1)
        {

            //Debug.Log("改回来");
            ChildCollider cCollider = this.target.gameObject.transform.Find("CursorCollider").GetComponent<ChildCollider>();
            cCollider.SetDefaultCursor();
        }

        this.target = null;  
        


    }

    private void TargetConfirm()
    {
        //确定target,此时将target的tenetDirection改为0,并给target加一个frame;
        bool mouseDown = Input.GetMouseButtonDown(1);

        if (mouseDown == true&&Master.status!=2)
        {
            if (this.target != null)
            {
                this.target.tenetDirection = 0;

                //this.target.GetAnimator().SetFloat("Speed", 0f);
                Master.EnemyFrozen(0f);    //不同维度敌人封印

                this.target.transform.Find("CursorCollider").GetComponent<SpriteRenderer>().color = Color.gray;

                
                humanBody.AddFrameWithoutConditions(this.target.GetMController(), Master.frame, ActionEnum.movement.targetConfirm, this.target.transform.position, 0, 0);  //加入事件帧
            }
        }


    }
    // Update is called once per frame
    void Update()
    {

        if (Master.currentDirection == 1)
        {

            //游戏处在正向时才进行这些判断
            TargetConfirm();

            FindTarget();
        }

        GetDirection();
        
    }
}

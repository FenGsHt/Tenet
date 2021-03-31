using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg 
{

    //用来保存事件帧的信息
    public int frame;
    public ActionEnum.movement movement;
    public float parm1;  //参数
    public float parm2;
    public Vector2 position;   //保存位置信息
    //public int direction;    //1表示正向,0表示反向

    public Egg(int frame,ActionEnum.movement movement,Vector2 position,float parm1,float parm2)
    {
        this.frame = frame;
        this.movement = movement;
        this.position = position;
        this.parm1 = parm1;
        this.parm2 = parm2;

       // this.direction = direction;    
    }
    


}

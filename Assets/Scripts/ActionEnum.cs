using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ActionEnum
{
    //用来存放所有的动作序号

    public enum movement
    {
        walk = 0,
        attack = 1,
        jump = 2,
        dead = 3,
        tag=4,    //玩家的标记
        gravity=5,   //重力
        bulletMove=6,
        bulletBack=7,
        bulletGone=8   //子弹回到起点就消失
    }
}
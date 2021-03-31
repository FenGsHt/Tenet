using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Master.currentDirection == 0)
        {
            //当转换到逆向世界时
            if (Master.ReverseMainGuy != null)
            {
                GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = Master.ReverseMainGuy.transform;
                GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = Master.ReverseMainGuy.transform;
                Destroy(this);  //只是销毁这个脚本文件
            }

        }

    }
}

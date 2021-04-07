using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillHint : MonoBehaviour
{
    Text text;
    ParticleSystem particle;  //当目标达成时,即该杀的敌人杀完时,此时播放粒子动画

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
        text = GetComponent<Text>();
    }
    //从master中获取还需要击杀的敌人数量然后显示在屏幕上
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void TextChange()
    {
        //根据游戏中的状态来改变对话框中的内容
        text.text = "待击杀敌人 " + Master.killedEnemyNum + " / " + Master.totalEnemyNum;

        if (Master.killedEnemyNum == Master.totalEnemyNum)
        {
            text.text = "敌人已全部击杀 " + Master.killedEnemyNum + " / " + Master.totalEnemyNum;

        }

    }

    // Update is called once per frame
    void Update()
    {
        TextChange();   //文本信息改变

        if (Master.killedEnemyNum == Master.totalEnemyNum)
        {
            if (particle.isStopped == true)
            {
                //播放粒子效果
                particle.Play();
            }

            text.color = new Color(219f / 255f, 210f / 255f, 24f / 255f);



        }
        else
        {
            if (particle.isPlaying == true)
            {
                particle.Stop();
            }
            text.color = new Color(154f / 255f, 63f / 255f, 63f / 255f);


        }



    }
}

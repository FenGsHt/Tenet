using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Sprite pause;
    public Sprite resume;

    private Image image;


    private void Awake()
    {
        image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PauseOrResume(int num)
    {
        Debug.Log("123");

        if (Master.status == 1)
        {
            //表示当前是暂停转继续
            Master.Resume();
            image.sprite = pause;
        }
        else
        {
            Master.Stop();
            image.sprite = resume;
        }
       

    }

    //public void Resume()
    //{
        

    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}

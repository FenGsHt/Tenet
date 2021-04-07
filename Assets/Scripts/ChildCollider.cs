using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour
{
    public GameObject father;

    public Sprite killSprite;

    private SpriteRenderer sr;
    //用于保存父类的引用并进行一定的操作

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetKillCursor()
    {
        sr.sprite = killSprite;


    }

    public void SetDefaultCursor()
    {
        sr.sprite = null;
    }

    
    private void ChangeToDefaultCursor()
    {
        if(sr.sprite!=null)
        if (father.GetComponent<humanBody>().tenetDirection == 1||father.GetComponent<humanBody>().tenetDirection==Master.currentDirection)
        {
                //即变回来了,此时sprite也变为null
                sr.sprite = null;
                sr.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeToDefaultCursor();
        
    }
}

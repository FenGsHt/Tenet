using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{

    public string hint;

    //SpriteRenderer sr;

    private void Awake()
    {
        //sr = GetComponent<SpriteRenderer>();
       // sr.sprite = null;   //将sprite消失
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SendInfo();
    }

    public void SendInfo()
    {
        //给提示开关输送提示文字
        HintManager.SetText(this.hint, 100);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

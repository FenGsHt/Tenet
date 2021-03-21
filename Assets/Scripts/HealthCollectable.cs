﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("触发的对象是" + collision);
        
    
        RubyController rubyController = collision.GetComponent<RubyController>();

        if (rubyController != null)
        {
            rubyController.ChangeHealth(1);
            Destroy(gameObject);

        }
     
    }
}

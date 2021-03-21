using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{

    Rigidbody2D rigidbody;

    public int maxHealth = 5;
    private int currentHealth;
    public int speed=3;
    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;   //调整update的刷新率
        rigidbody = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0)||!Mathf.Approximately(move.y,0))
        {

        }


        Vector2 position = transform.position;
        position.x = position.x + speed*horizontal*Time.deltaTime;
        position.y = position.y + speed* vertical*Time.deltaTime;

        rigidbody.MovePosition(position);
       // transform.position = position;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);   //将值限制在0到5范围内

    }

    public int GetRubyHealthValue()
    {

        return currentHealth;
    }
}

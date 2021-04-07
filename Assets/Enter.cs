using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        
    }

    public void Load(int num)
    {
        Debug.Log("qwe");
        SceneManager.LoadScene("SampleScene");
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}

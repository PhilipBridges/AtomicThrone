using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dontdie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        int numMenus = GameObject.FindGameObjectsWithTag("Menu").Length;
        if (numMenus > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

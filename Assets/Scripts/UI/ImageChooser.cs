using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageChooser : MonoBehaviour
{
    public GameObject[] Border;
    // Start is called before the first frame update
    void Start()
    {
        OnBoder(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnBoder(int num)
    {
        for(int i=0;i<Border.Length;i++)
        {
            if(i==num)
                Border[i].gameObject.SetActive(true);
            else
                Border[i].gameObject.SetActive(false);
        }
    }
}

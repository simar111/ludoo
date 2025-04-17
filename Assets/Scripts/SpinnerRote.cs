using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerRote : MonoBehaviour
{
    public int speed = 10;
    float i = 0;
    void Update()
    {
        transform.localEulerAngles = new Vector3(0, 0, i);
        i += speed * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalUIManager : MonoBehaviour
{
    public Image button2P;
    public Image button3P;
    public Image button4P;

    public void button2PClick()
    {
        button2P.color = new Color(button2P.color.r, button2P.color.g, button2P.color.b, 0.3f);
        button3P.color = new Color(button3P.color.r, button3P.color.g, button3P.color.b, 1f);
        button4P.color = new Color(button4P.color.r, button4P.color.g, button4P.color.b, 1f);
    }

    public void button3PClick()
    {
        button2P.color = new Color(button2P.color.r, button2P.color.g, button2P.color.b, 1f);
        button3P.color = new Color(button3P.color.r, button3P.color.g, button3P.color.b, 0.3f);
        button4P.color = new Color(button4P.color.r, button4P.color.g, button4P.color.b, 1f);
    }

    public void button4PClick()
    {
        button2P.color = new Color(button2P.color.r, button2P.color.g, button2P.color.b, 1f);
        button3P.color = new Color(button3P.color.r, button3P.color.g, button3P.color.b, 1);
        button4P.color = new Color(button4P.color.r, button4P.color.g, button4P.color.b, 0.3f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnifierAnimation : MonoBehaviour
{
    int i = 0;
    public List<Sprite> sprites = new List<Sprite>();
    public float animationSpeed = 0.1f; // Time in seconds between each frame

    public void StartMyAnim()
    {
        StartCoroutine(Animate());
    }

    public IEnumerator Animate()
    {
        while (true)
        {
            this.GetComponent<Image>().sprite = sprites[i];
            i++;
            if (i >= sprites.Count)
                i = 0;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}

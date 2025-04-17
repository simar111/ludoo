using UnityEngine;
using UnityEngine.UI;

public class CoinMovement : MonoBehaviour
{
    public RectTransform imageRectTransform; // Reference to the Image RectTransform
    public RectTransform targetRectTransform; // Reference to the Target RectTransform
    public float moveSpeed = 5f; // Speed at which the image moves

    void Start()
    {
        if (imageRectTransform == null)
        {
            imageRectTransform = GetComponent<RectTransform>();
        }
        GetComponent<AudioSource>().Play();
    }

    void Update()
    {
        // Move the image towards the target position
        Vector2 targetPosition = targetRectTransform.anchoredPosition;
        imageRectTransform.anchoredPosition = Vector2.MoveTowards(imageRectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);

        if(imageRectTransform.anchoredPosition == targetPosition)
        {
            this.gameObject.SetActive(false);
        }
    }
}

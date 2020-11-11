using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class BallTrailFade : MonoBehaviour
{

    [SerializeField] float fadeAmount = 3f;

    [SerializeField] private SpriteRenderer spriteRenderer;


    void Update()
    {
        if (spriteRenderer.color.a > 0f)
        {
            Color newColor = spriteRenderer.color;
            newColor.a -= fadeAmount * Time.deltaTime;
            spriteRenderer.color = newColor;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

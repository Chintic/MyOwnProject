using System.Collections;
using UnityEngine;

public class FadeTest : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private IEnumerator Fade()
    {
        Color c = sprite.color;

        for (float alpha = 1f; alpha >= 0f; alpha -= 0.1f)
        {
            c.a = alpha;
            sprite.color = c;

            yield return new WaitForSeconds(0.2f);
        }

        c.a = 0f;
        sprite.color = c;
    }

    private void Start()
    {
        StartCoroutine(Fade());
    }
}
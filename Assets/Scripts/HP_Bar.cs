using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class HP_Bar : MonoBehaviour
{
    [SerializeField] private Texture2D tex;

    [SerializeField] private GameObject entity;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    public IEnumerator UpdateBar(float scaledPrevHp, float scaledCurrHp)
    {
        float t = 0;
        float dur = 1;
        Vector3 currScale = transform.localScale;
        while (t<=dur)
        {
            //Make it so the decrease is not instaneous
            currScale.x=Mathf.Lerp(scaledPrevHp, scaledCurrHp, t/dur);
            t += 0.02f;
            //Wait for 0.005 second every sprite update
            yield return new WaitForSeconds(0.001f);
            transform.localScale = currScale;
        }
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAnimation : MonoBehaviour
{
    public AnimationCurve animation;

    private Vector3 startScale;
    
    // Start is called before the first frame update
    void Start()
    {
        //LeanTween.scale(this.gameObject, new Vector3(2,2,2), 5f).setLoopPingPong();
        startScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<Renderer>().isVisible)
        {
            if (!LeanTween.isTweening(this.gameObject))
            {
                LeanTween.scale(this.gameObject, this.transform.localScale*2f, Random.Range(5f,10f)).setEase(animation).setDelay(Random.Range(0,2f));
            }
        }
        else
        {
            if (LeanTween.isTweening(this.gameObject))
            {
                LeanTween.cancel(this.gameObject);
                this.transform.localScale = startScale;
            }
        }
    }
}

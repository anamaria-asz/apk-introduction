using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class topo : MonoBehaviour
{
    public GameObject topoc;
    private Touch theTouch, theTouch0, theTouch1;
    private Vector2 touchStartPosition, touchEndPosition, touchPos0, touchPos1, touchPos00, touchPos11;
    private float small, big, normal;
    public TextMeshProUGUI directionText, touchcount;
    float xs1, ys1, xs0, ys0;
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.touchCount <= 1)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                touchStartPosition = theTouch.position;
            }

            else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = theTouch.position;
                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                {
                    
                }

                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x > 0)
                    {
                        topoc.transform.Rotate(0.0f, -2.0f, 0.0f, Space.Self);
                    }
                    else 
                    {
                        topoc.transform.Rotate(0.0f, 2.0f, 0.0f, Space.Self);
                    }
                        
                }

                else
                {
                    if (y > 0)
                    {
                        topoc.transform.Translate(Vector3.up * Time.deltaTime * 15, Space.World);
                    }
                    else
                    {
                        topoc.transform.Translate(Vector3.down * Time.deltaTime * 15, Space.World);
                    }
                }
            }
        }



    }
}

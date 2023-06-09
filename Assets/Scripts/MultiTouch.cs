using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiTouch : MonoBehaviour
{
    public TextMeshProUGUI multiTouchInfoDisplay;
    private int maxTapCount = 0;
    private string multiTouchInfo;
    private Touch theTouch;

    // Update is called once per frame
    void Update()
    {
        multiTouchInfo = string.Format("Numero de dedos: {0}\n", maxTapCount);

        if(Input.touchCount > 0)
        {
            for(int i=0; i<Input.touchCount; i++)
            {
                theTouch = Input.GetTouch(i);
                string info = "Touch: " + i.ToString() + " - " + "Tap count: " + theTouch.tapCount.ToString() + " - " +
                                "Finger ID: " + theTouch.fingerId.ToString() + " - " + "nRadius: " + theTouch.radius.ToString() +
                                "-" + ((theTouch.radius / (theTouch.radius + theTouch.radiusVariance)) * 100f).ToString() + "%" +
                                "\n";


                multiTouchInfo += info;
            }

        }

        maxTapCount = Input.touchCount;
        multiTouchInfoDisplay.SetText(multiTouchInfo);
    }
}

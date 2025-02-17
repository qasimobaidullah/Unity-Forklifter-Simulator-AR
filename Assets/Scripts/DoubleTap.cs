using UnityEngine;

public class DoubleTap : MonoBehaviour
{

    int tapCount;
    bool waitingTimeStarted = false;
    float time;
    private float waitingTime;

    void Start()
    {
        waitingTime = 0.5f;
        waitingTimeStarted = false;
        time = 0;
        tapCount = 0;
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (!waitingTimeStarted)
            {
                time = 0;
                tapCount = 0;
            }
            waitingTimeStarted = true;
            Touch touch = Input.GetTouch(0);
            if (tapCount < 1)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    tapCount++;
                }
                else if (touch.phase == TouchPhase.Moved && time > 0.2f)
                {
                    tapCount = 0;
                    time = 0;
                }
            }
            else
            {
                if (touch.phase == TouchPhase.Began)
                {
                    tapCount++;
                }
                else if (touch.phase == TouchPhase.Moved && time > 0.2f)
                {
                    tapCount = 0;
                    time = 0;
                }

            }

            if (time <= waitingTime && tapCount > 1)
            {
                if (InitialData._singleObjectPlacement)
                {
                    PlaceOnPlane.resetToInitialScale();
                }

                tapCount = 0;
                time = 0;
            }
        }
        if (time > waitingTime)
        {
            time = 0;
            waitingTimeStarted = false;
            tapCount = 0;
        }
        else
        {
            if (waitingTimeStarted)
            {
                time += Time.deltaTime;
            }
        }
    }
}
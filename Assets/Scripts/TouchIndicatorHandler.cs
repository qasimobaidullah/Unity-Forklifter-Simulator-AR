
using UnityEngine;



public class TouchIndicatorHandler : MonoBehaviour
{

    public static GameObject hitObject = null;
    public static GameObject previousHitObject;
    public static bool isTouchedTheObject;
    bool startTimer = false;
    float startTime = 0;
    bool interactable = false;

    void Update()
    {
        if ((Input.touchCount > 0))
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit))
                {
                    hitObject = Hit.transform.gameObject;
                    interactable = true;
                }
                else
                {
                    interactable = false;
                }
            }
            if (interactable && (Input.touches[0].phase == TouchPhase.Stationary || Input.touches[0].phase == TouchPhase.Moved))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit))
                {

                    hitObject = Hit.transform.gameObject;
                    if (previousHitObject == null)
                    {
                        previousHitObject = hitObject;
                    }

                    if (InitialData._singleObjectPlacement)
                    {
                        PlaceOnPlane.showTouchIndicator();
                    }


                    isTouchedTheObject = true;
                }

            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                interactable = false;

                if (InitialData._singleObjectPlacement)
                {
                    PlaceOnPlane.hideTouchIndicator();
                }


                startTimer = true;
            }
        }

        if (Input.touches.Length != 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isTouchedTheObject = false;

                if (InitialData._singleObjectPlacement)
                {
                    PlaceOnPlane.hideTouchIndicator();
                }


                startTimer = true;
            }
        }
        else
        {
            if (!isTouchedTheObject)
            {

                if (InitialData._singleObjectPlacement)
                {
                    PlaceOnPlane.hideTouchIndicator();
                }


                startTimer = true;
            }
        }

        DestroyInstaceObject();
    }

    void DestroyInstaceObject()
    {
        if (previousHitObject != hitObject)
        {
            previousHitObject.GetComponent<SpawningObjectDetails>().touchIndicator.SetActive(false);
            previousHitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.SetActive(false);
            previousHitObject = hitObject;
        }

        if (startTimer)
        {
            if ((Time.time - startTime) > 0.6f)
            {
                if (hitObject != null)
                {
                    hitObject.GetComponent<SpawningObjectDetails>().touchIndicator.SetActive(false);
                    hitObject.GetComponent<SpawningObjectDetails>().scalePersentageIndicator.SetActive(false);
                }
                hitObject = null;
                startTimer = false;
            }
        }
        else
        {
            startTime = Time.time;
        }
    }

}
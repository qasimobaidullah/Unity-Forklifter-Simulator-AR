using System.Collections;
using UnityEngine;

public class ForkController : MonoBehaviour
{
    public Transform fork;
    public Transform mast;
    public float speedTranslate = 0.1f;

    private bool mastMoveTrue = false;
    private bool moveUp = false;
    private bool moveDown = false;

    void Start()
    {
        StartCoroutine(AdjustForkPosition());
    }

    IEnumerator AdjustForkPosition()
    {
        yield return new WaitForSeconds(1f);

        if (fork != null && mast != null)
        {
            fork.SetParent(transform);
            mast.SetParent(transform);

            fork.localPosition = new Vector3(0, 0.5f, 0);
            mast.localPosition = new Vector3(0, 0.5f, 0);
        }
    }

    void FixedUpdate()
    {
        if (moveUp && fork.localPosition.y < 2f)
        {
            fork.localPosition += Vector3.up * speedTranslate * Time.deltaTime;
            if (mastMoveTrue)
            {
                mast.localPosition += Vector3.up * speedTranslate * Time.deltaTime;
            }
        }
        if (moveDown && fork.localPosition.y > 0.1f)
        {
            fork.localPosition += Vector3.down * speedTranslate * Time.deltaTime;
            if (mastMoveTrue)
            {
                mast.localPosition += Vector3.down * speedTranslate * Time.deltaTime;
            }
        }
    }

    public void StartMovingUp() => moveUp = true;
    public void StopMovingUp() => moveUp = false;
    public void StartMovingDown() => moveDown = true;
    public void StopMovingDown() => moveDown = false;
}

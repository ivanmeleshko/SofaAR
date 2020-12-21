using UnityEngine;


public class AnimController : MonoBehaviour
{

    [SerializeField]
    Animation animation;
    bool upL, upR = true;
    bool touched = false;
    Vector2 firstpoint = new Vector2(), secondpoint = new Vector2();


    private void Update()
    {
        if (DoubleClick())
        {
            //OnRightClick();
        }
    }


    public void PlayL()
    {
        if (!upL)
        {
            animation.PlayQueued("UpFirst");
            animation.PlayQueued("UpSecond");
            upL = true;
        }
        else
        {
            animation.PlayQueued("DownSecond");
            animation.PlayQueued("DownFirst");          
            upL = false;
        }
    }


    public void PlayR()
    {
        if (!upR)
        {
            animation.PlayQueued("UpFourth");
            animation.PlayQueued("UpThird");
            upR = true;        }
        else
        {
            animation.PlayQueued("DownThird");
            animation.PlayQueued("DownFourth");
            upR = false;
        }
    }


    void OnRightClick()
    {
        Ray clickPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        Collider collider = transform.GetComponent<MeshCollider>();

        if (Physics.Raycast(clickPoint, out RaycastHit hitPoint))
        {
            if (hitPoint.collider == collider)
            {
                if (collider.name.Equals("Flap_Sofa_L"))
                {
                    PlayL();
                }
                else if (collider.name.Equals("Flap_Sofa_R"))
                {
                    PlayR();
                }
            }
        }
    }


    void OnTouch()
    {
        if (Input.touchCount == 1)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstpoint = Input.GetTouch(0).position;
                //secondpoint = Input.GetTouch(0).position;
                touched = true;
            }

            //Move finger
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondpoint = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                secondpoint = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                secondpoint = Input.GetTouch(0).position;
            }

            if (Vector2.Distance(firstpoint, secondpoint) < 5 && touched)
            {
                touched = false;
                
                OnRightClick();               
            }
        }
    }


    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;

    private bool DoubleClick()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                clicked++;
                    if (clicked == 1) clicktime = Time.time;
            }
            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                return true;
            }
            else if (clicked > 2 || Time.time - clicktime > 1)
            {
                clicked = 0;
            }
        }
        return false;
    }
}

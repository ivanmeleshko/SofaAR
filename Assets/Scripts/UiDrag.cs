using UnityEngine;


public class UiDrag : MonoBehaviour
{

    [SerializeField]
    GameObject Popup;

    private float offsetX;
    private float offsetY;
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f; 
    float edge = 88f;
    Vector3 curPos = new Vector3();


    public void BeginDrag()
    {
        if (Input.touchCount == 1)
        {
            offsetX = transform.position.x - Input.touches[0].position.x;
            offsetY = transform.position.y - Input.touches[0].position.y;
            Popup.transform.SetAsLastSibling();
        }
    }


    public void OnDrag()
    {
        if (Input.touchCount == 1)
        {
            transform.position = new Vector3(offsetX + Input.touches[0].position.x, offsetY + Input.touches[0].position.y);

            if (Input.touches[0].position.x < edge || Input.touches[0].position.y < edge
                || Input.touches[0].position.x > Screen.width - edge || Input.touches[0].position.y > Screen.height - edge)
            {
                Popup.SetActive(false);
            }
        }
    }


    public void OnMouseDown()
    {
        Popup.transform.SetAsLastSibling();
    }


    private bool DoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
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
        return false;
    }


    public void ShowPopup()
    {
        Popup.SetActive(true);
        Popup.transform.SetAsLastSibling();
        if (Popup.name.Equals("PanelPopup"))
        {
            GameObject buttonSend = GameObject.Find("ButtonSend");
            buttonSend.SetActive(false);
        }
    }
    
}
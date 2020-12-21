using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    [SerializeField]
    GameObject Popup;
    [SerializeField]
    bool buttonStateActive;
    [SerializeField]
    public GameObject[] Links;
    [SerializeField]
    public Button button;
    const string selectedColorHex = "#C51844FF";
    const string selectedColorHexAv = "#26B4B7FF";
    const string selectedColorHexRes = "#62615FFF";
    const string selectedColorHexSold = "#B70F00FF";
    const string normalColorHex = "#6A1328FF";
    private Color selectedColor;
    private Color normalColor;   
    GameObject[] links;
    float sensitivity = 5f;


    public void ShowPopup()
    {
        if (!Popup.activeInHierarchy)
        {
            switch (Popup.name)
            {
                case "PanelPopup":
                case "PanelGestures":
                    Popup.transform.SetPositionAndRotation(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), Quaternion.identity);
                    Popup.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 500);
                    break;
                case "PanelConfig":
                    Popup.transform.SetPositionAndRotation(new Vector3(Screen.width / 2f, Screen.height / 7.25f, 0), Quaternion.identity);
                    Popup.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 300);
                    break;
                case "PanelPhoto":
                    Popup.transform.SetPositionAndRotation(new Vector3(Screen.width / 2f, Screen.height / 1.3f, 0), Quaternion.identity);
                    Popup.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 500);
                    break;
            }
            Popup.transform.localScale = Vector3.one;
            Popup.SetActive(true);
            Popup.transform.SetAsLastSibling();     
        }
    }


    public void ClosePopup()
    {
        Popup.SetActive(false);
    }


    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}

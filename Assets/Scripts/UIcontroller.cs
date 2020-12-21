using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIcontroller : MonoBehaviour
{

    public Image[] outlinesColor, outlinesBase, outlinesMod;
    const string colorMagentaHex = "#E6194D";
    const string colorTransparent = "#00000000";
    public static bool mobileSupport;


    public void SetBorder(Image imgOutline)
    {
            foreach (Image img in outlinesColor)
            {
                if (img.name.Equals(imgOutline.name))
                {
                    Color color;
                    if (ColorUtility.TryParseHtmlString(colorMagentaHex, out color))
                    {
                        img.color = color;
                    }    
                }
            else
            {
                Color color;
                if (ColorUtility.TryParseHtmlString(colorTransparent, out color))
                {
                    img.color = color;
                }
            }
        }

    }

}
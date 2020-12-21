using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SofaConfig : MonoBehaviour
{

    public static Material material;
    public static bool materialChanged = false;
    List<Material> materials;


    void Start()
    {
        materials = new List<Material>();
        materials.Add(Resources.Load("Materials/BrownMat", typeof(Material)) as Material);
        materials.Add(Resources.Load("Materials/BeigeMat", typeof(Material)) as Material);
        materials.Add(Resources.Load("Materials/GoldMat", typeof(Material)) as Material);
        materials.Add(Resources.Load("Materials/GreenMat", typeof(Material)) as Material);
        materials.Add(Resources.Load("Materials/RedMat", typeof(Material)) as Material);
    }


    public void ChangeMaterial(Material mat)
    {
        material = mat;
        materialChanged = true;
    }


    public GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

}

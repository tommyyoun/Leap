using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DifficultyCheck : MonoBehaviour
{

    public GameObject obj1;
    public GameObject obj2;
   
   


    // Start is called before the first frame update
    void Start()
    {
        if (MenuUI.easyMode)
        {

        }
        else
        {
            obj1.SetActive(false);
            obj2.SetActive(false);
            
            
           
        }
    }

    
}

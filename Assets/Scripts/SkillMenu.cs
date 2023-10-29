using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMenu : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject SkillMenuCanvas;
    public GameObject SkillHUD;
    public RectTransform rectTransform;
    private Vector2 mousePos;
    private FrogInputSystem script;

    private GameObject tempSkillObject;
    private static float relativeSkillXPosition = 0;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        mousePos = Input.mousePosition;
        rectTransform = GetComponent<RectTransform>();
        script = GameObject.FindWithTag("Player").GetComponent<FrogInputSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;

         if (Input.GetKeyDown(KeyCode.Tab)) 
        { 

            if (Paused)
            {
                Play();
            }

            else
            {
                Stop();
            }
        }
        if (Input.GetMouseButtonUp(0) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos) && rectTransform.CompareTag("Resist") && script.skillPoints > 0) {
            //fill in logic for Resistance skill

            // display on hud
            displaySkill(0);
        }
        if (Input.GetMouseButtonUp(0) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos) && rectTransform.CompareTag("IncJump") && script.skillPoints > 0) {
            //fil in logic for Increased Jump skill

            // display on hud
            displaySkill(1);
        }
        if (Input.GetMouseButtonUp(0) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos) && rectTransform.CompareTag("AimAssist") && script.skillPoints > 0) {
            //fill in logic for Aim Assist skill

            // display on hud
            displaySkill(2);
        }
    }

    void Stop()
    {
        SkillMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        Paused = true; 
    }
    public void Play()
    {
        SkillMenuCanvas.SetActive(false); 
        Time.timeScale = 1.0f;
        Paused = false; 
    }

    void displaySkill(int child)
    {
        tempSkillObject = SkillHUD.transform.GetChild(child).gameObject;
        tempSkillObject.SetActive(true);
        tempSkillObject.transform.position = tempSkillObject.transform.position + new Vector3(relativeSkillXPosition, 0, 0);

        relativeSkillXPosition = relativeSkillXPosition + 100;
    }
}

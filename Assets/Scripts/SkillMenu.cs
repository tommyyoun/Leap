using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEditorInternal;
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
    private GameObject line;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        mousePos = Input.mousePosition;
        rectTransform = GetComponent<RectTransform>();
        script = GameObject.FindWithTag("Player").GetComponent<FrogInputSystem>();
        line = GameObject.FindWithTag("Player").transform.Find("Line").gameObject;
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
        if (Input.GetMouseButtonUp(0) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos)
                                      && rectTransform.CompareTag("Resist") && script.skillPoints > 0 && !script.frogBrakes) {
            //fill in logic for Resistance skill or brake skill, it could be easily changed you'd just need to update all uses of the word resist with "frog brakes" or
            //something that gets the point across

            //actually increase max jump height and decrease skill points by 1
            script.skillPoints -= 1;

            //make it so it can only be bought once
            script.frogBrakes = true;

            // display on hud
            displaySkill(0);
        }
        if (Input.GetMouseButtonUp(0) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos)
                                      && rectTransform.CompareTag("IncJump") && script.skillPoints > 0 && !script.incJumpBought) {
            //fil in logic for Increased Jump skill

            //make it so it can only be bought once
            script.incJumpBought = true;

            //actually increase max jump height and decrease skill points by 1
            script.maxJumpHeight = 6.5f;
            script.skillPoints -= 1;

            // display on hud
            displaySkill(1);
        }
        if (Input.GetMouseButtonUp(0) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos)
                                      && rectTransform.CompareTag("AimAssist") && script.skillPoints > 0 && !script.aimAssistBought) {
           
            script.skillPoints -= 1;

            line.SetActive(true);

            script.aimAssistBought = true;

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

using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

public class SkillMenu : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject SkillMenuCanvas;
    public GameObject SkillHUD;
    public RectTransform rectTransform;
    private Vector2 mousePos;
    private List<FrogInputSystem> scripts = new List<FrogInputSystem>();
    private GameObject tempSkillObject;
    private static float relativeSkillXPosition = 0;
    private List<GameObject> lines = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        mousePos = Input.mousePosition;
        rectTransform = GetComponent<RectTransform>();
        GameObject[] playerParents = GameObject.FindGameObjectsWithTag("playerParent");

        foreach (GameObject g in playerParents) {
            lines.Add(g.transform.Find("LineArrow").gameObject);
            scripts.Add(g.transform.Find("Frog").gameObject.GetComponent<FrogInputSystem>());
        }
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
                                      && rectTransform.CompareTag("Resist") && scripts[0].skillPoints > 0 && !scripts[0].frogBrakes) {
            //fill in logic for Resistance skill or brake skill, it could be easily changed you'd just need to update all uses of the word resist with "frog brakes" or
            //something that gets the point across

            //actually increase max jump height and decrease skill points by 1
            scripts[0].skillPoints -= 1;

            if(scripts.Count > 1)
            scripts[1].skillPoints -= 1;

            //make it so it can only be bought once
            scripts[0].frogBrakes = true;

            if (scripts.Count > 1)
            scripts[1].frogBrakes = true;

            // display on hud
            displaySkill(0);
        }
        if (Input.GetMouseButtonUp(0) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos)
                                      && rectTransform.CompareTag("IncJump") && scripts[0].skillPoints > 0 && !scripts[0].incJumpBought) {
            //fil in logic for Increased Jump skill

            //make it so it can only be bought once
            scripts[0].incJumpBought = true;
            if (scripts.Count > 1)
                scripts[1].incJumpBought = true;

            //actually increase max jump height and decrease skill points by 1
            scripts[0].maxJumpHeight = 6.5f;
            scripts[0].skillPoints -= 1;
            if (scripts.Count > 1)
            {
                scripts[1].maxJumpHeight = 6.5f;
                scripts[1].skillPoints -= 1;
            }

            // display on hud
            displaySkill(1);
        }
        if (Input.GetMouseButtonUp(0) && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos)
                                      && rectTransform.CompareTag("AimAssist") && scripts[0].skillPoints > 0 && !scripts[0].aimAssistBought) {
           
            scripts[0].skillPoints -= 1;
            if (scripts.Count > 1)
                scripts[1].skillPoints -= 1;

            lines[0].SetActive(true);
            if (scripts.Count > 1)
                lines[1].SetActive(true);

            scripts[0].aimAssistBought = true;
            if (scripts.Count > 1)
                scripts[1].aimAssistBought = true;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneManager_1 : MonoBehaviour
{
    public static SceneManager_1 instance;
    public GameObject fadeCover;

    public bool phase_start;
    public bool phase_1, phase_2, phase_3, phase_4, phase_5;
    public bool phase_end;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        phase_start = true;
    }

    void Update()
    {
        if (phase_start)
        {
            SceneIntro();
        }
        else if (phase_1)
        {

        }
    }

    public void SceneIntro()
    {
        fadeCover.GetComponent<Image>().color = Color.Lerp(fadeCover.GetComponent<Image>().color,
        new Color(fadeCover.GetComponent<Image>().color.r,
        fadeCover.GetComponent<Image>().color.g,
        fadeCover.GetComponent<Image>().color.b, 0), 0.05f);

        if (fadeCover.GetComponent<Image>().color.a <= 0.01f)
        {
            phase_start = false;
            phase_1 = true;
        }
    }
}

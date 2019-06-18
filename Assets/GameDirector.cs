using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject timerText;
    GameObject pointText;
    public float time = 30f;
    int point = 0;
    ItemGenerator generator;

    public void GetApple()
    {
        point += 100;
    }

    public void GetBomb()
    {
        point /= 2;
    }

    // Use this for initialization
    void Start()
    {
        generator = GameObject.Find("ItemGenerator").GetComponent<ItemGenerator>();
        timerText = GameObject.Find("Time");
        pointText = GameObject.Find("Point");
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if (time < 0)
        {
            time = 0;
            generator.SetParameter(10000f, 0, 0);
        }
        else if (0 <= time && time < 5)
        {
            generator.SetParameter(.7f, -.04f, 3);
        }
        else if (5 <= time && time < 12)
        {
            generator.SetParameter(.5f, -.05f, 6);
        }
        else if (12 <= time && time < 23)
        {
            generator.SetParameter(.8f, -.04f, 4);
        }
        else if (23 <= time && time < 30)
        {
            generator.SetParameter(1f, -.03f, 2);
        }

        timerText.GetComponent<Text>().text = time.ToString("F1");
        pointText.GetComponent<Text>().text = point.ToString() + "point";
    }
}

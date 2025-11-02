using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public int agechange = 90;
    [SerializeField] private TextMeshProUGUI disasterheadingtext;
    [SerializeField] private TextMeshProUGUI descdisastertext;
    public int dis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Disasters()
    {
        dis = Random.Range(0,18);
        if (dis >= 0 && dis <= 10)
        {
            Debug.Log("Normal");
            disasterheadingtext.text = "No Disasters";
            disasterheadingtext.color = Color.white;
            descdisastertext.text = "";

        }
        else if (dis == 11)
        {
            Debug.Log("Worm disease");
            disasterheadingtext.text = "Worm Disease";
            disasterheadingtext.color = Color.magenta;
            descdisastertext.text = "Worms die at an increased rate";
        }
        else if (dis == 12)
        {
            Debug.Log("Plant disease");
            disasterheadingtext.text = "Plant Disease";
            disasterheadingtext.color = Color.green;
            descdisastertext.text = "Plants die at an increased rate";
        }
        else if (dis == 13)
        {
            Debug.Log("Rabbit disease");
            disasterheadingtext.text = "Rabbit Disease";
            disasterheadingtext.color = Color.cyan;
            descdisastertext.text = "Rabbits die at an increased rate";
        }
        else if (dis == 14)
        {
            Debug.Log("Wolf disease");
            disasterheadingtext.text = "Wolf Disease";
            disasterheadingtext.color = Color.black;
            descdisastertext.text = "Wolves die at an increased rate";
        }
        else if (dis == 15)
        {
            Debug.Log("Lion disease");
            disasterheadingtext.text = "Lion Disease";
            disasterheadingtext.color = Color.yellow;
            descdisastertext.text = "Lions die at an increased rate";
        }
        else if (dis >=16)
        {
            Debug.Log("Invasive Species");
            disasterheadingtext.text = "Invasive Species";
            disasterheadingtext.color = Color.red;
            descdisastertext.text = "An Invasive species approaches Use <color=orange> Lions to deter</color> the effects";
        }
        
        
    }
}

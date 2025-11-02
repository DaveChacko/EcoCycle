using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifePointsSystem : MonoBehaviour
{
    public int actionpoints;
    [SerializeField] private int totalactions = 5;
    [SerializeField] private GameObject SpawnChecker;
    [SerializeField] private TextMeshProUGUI lptext;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (actionpoints <= 0)
        {
            actionpoints = 0;
        }
        lptext.text = "AP : " + actionpoints;

    }
    public void LPGeneration()
    {

        actionpoints = totalactions;

    }
}
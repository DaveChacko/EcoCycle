using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowStats : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI wormstat;
    [SerializeField] private TextMeshProUGUI plantstat;
    [SerializeField] private TextMeshProUGUI rabbitstat;
    [SerializeField] private TextMeshProUGUI wolfstat;
    [SerializeField] private TextMeshProUGUI lionstat;
    [Header("Health Icons")]
    [SerializeField] private SpriteRenderer wormHealth;
    [SerializeField] private SpriteRenderer plantHealth;
    [SerializeField] private SpriteRenderer rabbitHealth;
    [SerializeField] private SpriteRenderer wolfHealth;

    [SerializeField] private SpriteRenderer lionHealth;

    private bool isHovering = false;

    void Start()
    {
        if (wormstat) wormstat.enabled = false;
        if (plantstat) plantstat.enabled = false;
        if (rabbitstat) rabbitstat.enabled = false;
        if (wolfstat) wolfstat.enabled = false;
        if (lionstat) lionstat.enabled = false;
        if (wormHealth) wormHealth.enabled = false;
        if (plantHealth) wormHealth.enabled = false;
        if (rabbitHealth) wormHealth.enabled = false;
        if (wolfHealth) wormHealth.enabled = false;
        if (lionHealth) wormHealth.enabled = false;
    }

    void OnMouseEnter()
    {
        isHovering = true;
        UpdateUI(true);
    }

    void OnMouseExit()
    {
        isHovering = false;
        UpdateUI(false);
    }

    void Update()
    {
        if (isHovering)
        {
            UpdateUI(true);
        }
    }

    void UpdateUI(bool show)
    {
        string text = "" + health.ToString("F0");

        if (gameObject.CompareTag("Worm") && wormstat)
        {
            wormstat.enabled = show;
            wormHealth.enabled = show;
            wormstat.text = text;
        }
        else if (gameObject.CompareTag("Plant") && plantstat)
        {
            plantstat.enabled = show;
            plantHealth.enabled = show;
            plantstat.text = text;
        }
        else if (gameObject.CompareTag("Rabbit") && rabbitstat)
        {
            rabbitstat.enabled = show;
            rabbitHealth.enabled = show;
            rabbitstat.text = text;
        }
        else if (gameObject.CompareTag("Wolf") && wolfstat)
        {
            wolfstat.enabled = show;
            wolfHealth.enabled = show;
            wolfstat.text = text;
        }
        else if (gameObject.CompareTag("Lion") && lionstat)
        {
            lionstat.enabled = show;
            lionHealth.enabled = show;
            lionstat.text = text;
        }
    }
}
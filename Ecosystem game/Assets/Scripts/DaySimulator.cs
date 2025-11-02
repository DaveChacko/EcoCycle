using System.Collections;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DaySimulator : MonoBehaviour
{
    public TextMeshProUGUI daytext;
    private int dayno = 2;

    [SerializeField] private GameObject buttonmanager;
    [SerializeField] private GameObject LPManager;

    [Header("GameOver Screen")]
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    [SerializeField] private int daydeathcheck = 10;
    [SerializeField] private int spAmountDeathCheck = 10;
    [SerializeField] private TextMeshProUGUI deathdaytext;
    [SerializeField] private TextMeshProUGUI speciesNeededtext;
    [SerializeField] private GameObject disastertextholder;
    [SerializeField] private GameObject UpgradeButtonsHolder;
    [SerializeField] private GameObject infopanel;
    [SerializeField] private TextMeshProUGUI results;

    [Header("Leaderboard Input")]
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private GameObject submitButton;
    [Header("Weather")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem rainparticle;
    [SerializeField] private AudioSource rainsound;
    [SerializeField] private GameObject DayNightIndicator;
    [SerializeField] private Sprite daysprite;
    [SerializeField] private Sprite nightsprite;
    public bool israining = false;
    public bool isday = false;

    private int daysSurvived;
    private int speciesAlive;
    private bool isDead = false;


    private void Start()
    {

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        rainparticle.Stop();

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
        infopanel.SetActive(true);

        // Hide name input and submit until death
        if (playerNameInput != null) playerNameInput.gameObject.SetActive(false);
        if (submitButton != null) submitButton.SetActive(false);

    }

    private void Update()
    {
        deathdaytext.text = "Deadline : Day " + daydeathcheck;
        speciesNeededtext.text = "Species Needed : " + spAmountDeathCheck;
        results.text = "Species Number : " + gameObject.GetComponent<SpawnSpecies>().speciesAmount + "\n" + "Species Required : " + spAmountDeathCheck+"\n"+"Deadline : " +daydeathcheck;
    }

    public void Play()
{
    if (isDead) return;

    daytext.text = "Day " + dayno;
    dayno += 1;
    daysSurvived = dayno - 2; // Because day starts at 2

    LPManager.GetComponent<LifePointsSystem>().LPGeneration();
    buttonmanager.GetComponent<SpawnSpecies>().AgeAllSpecies();
    buttonmanager.GetComponent<SpawnSpecies>().EatSpecies();
    buttonmanager.GetComponent<DisasterManager>().Disasters();

    speciesAlive = buttonmanager.GetComponent<SpawnSpecies>().speciesAmount;

    // Check for death conditions
    if (dayno == daydeathcheck + 1)
    {
        if (speciesAlive >= spAmountDeathCheck)
        {
            daydeathcheck += 10;
            spAmountDeathCheck += 10;
        }
        else
        {
            Death();
        }
    }

    // Handle Day/Night alpha
    if (spriteRenderer == null)
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            float nightchance = Random.value-(0.1f*gameObject.GetComponent<SpawnSpecies>().superRabbitCount);
            Color color = spriteRenderer.color;

            if (nightchance > 0.5f) // night
            {
                color.a = 0.7f;
                DayNightIndicator.GetComponent<SpriteRenderer>().sprite = nightsprite;
                isday = false;
            }
            else // day
            {
                color.a = 0.0f;
                Debug.Log("Day!");
                DayNightIndicator.GetComponent<SpriteRenderer>().sprite = daysprite;
                isday = true;
            }

            spriteRenderer.color = color;
        }
        if (rainparticle == null)
    rainparticle = GetComponent<ParticleSystem>();

if (rainsound == null)
    rainsound = GetComponent<AudioSource>();

if (rainparticle != null && rainsound != null)
{
    float rainchance = Random.value - (0.1f * gameObject.GetComponent<SpawnSpecies>().superWolfCount);

    if (rainchance > 0.05f) // no rain
    {
        if (israining) // only stop if it was raining before
        {
            rainparticle.Stop();
            rainsound.Stop();
            Debug.Log("Rain stopped.");
        }
        israining = false;
    }
    else // rain
    {
        if (!israining) // only play if not already raining
        {
            rainparticle.Play();
            rainsound.Play();
            Debug.Log("It's raining!");
        }
        israining = true;
    }
}
}

    private void Death()
    {
        isDead = true;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        FadeInDeathPanel();
        disastertextholder.SetActive(false);
        UpgradeButtonsHolder.SetActive(false);

        if (playerNameInput != null) playerNameInput.gameObject.SetActive(true);
        if (submitButton != null) submitButton.SetActive(true);
    }

    public void SubmitScore()
    {
        string playerName = "Unknown";
        if (playerNameInput != null && !string.IsNullOrWhiteSpace(playerNameInput.text))
            playerName = playerNameInput.text;

        LeaderBoardManager leaderboard = FindObjectOfType<LeaderBoardManager>();
        if (leaderboard != null)
        {
            leaderboard.AddRecord(playerName, daysSurvived, speciesAlive);
            Debug.Log($"Leaderboard updated: {playerName} - Days {daysSurvived}, Species {speciesAlive}");
            SceneManager.LoadScene(0);

        }
        else
        {
            Debug.LogWarning("LeaderBoardManager not found in scene!");
        }

        if (playerNameInput != null) playerNameInput.gameObject.SetActive(false);
        if (submitButton != null) submitButton.SetActive(false);
        
    }

    public void FadeInDeathPanel()
    {
        if (canvasGroup != null)
        {
            StartCoroutine(FadeDeathCanvasGroup(canvasGroup, 0f, 1f, fadeDuration));
        }
    }

    IEnumerator FadeDeathCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        cg.alpha = endAlpha;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void Info()
    {
        infopanel.SetActive(true);
    }
    public void ExitInfo()
    {
        infopanel.SetActive(false);        
    }
}
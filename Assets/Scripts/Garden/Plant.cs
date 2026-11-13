using System.Collections;
using UnityEngine;
public class Plant : MonoBehaviour
{
    public enum PlantState
    {
        Growing,
        Ripe,
        Overripe
    }
    public float growDuration = 12f;
    public float ripeDuration = 8f;
    public float overripeDuration = 3f;
    public float wateringGrowthMultiplier = 5f;
    public float wateredGrowthMultiplier = 2f;
    public GameObject wateredIndicator;
    public GameObject[] growingVisuals = new GameObject[0];
    public GameObject ripeVisual;
    public GameObject overripeVisual;
    public Animator plantAnimator;
    public int scoreValue = 1;
    public ParticleSystem harvestEffectPrefab;
    public ParticleSystem burstEffectPrefab;
    public AudioClip harvestSound;
    public AudioClip burstSound;
    private const string GrowTriggerName = "Grow";
    private PlantState currentState = PlantState.Growing;
    private int currentGrowingStage = -1;
    private float growthTimer = 0f;
    private float ripeTimer = 0f;
    private bool isWatered = false;
    private bool isBeingWatered = false;
    private DroneWatering droneAbove;
    private ScoreManager scoreManager;
    private SoundEffectPlayer soundEffectPlayer;
    public void Init(ScoreManager newScoreManager, SoundEffectPlayer newSoundEffectPlayer)
    {
        scoreManager = newScoreManager;
        soundEffectPlayer = newSoundEffectPlayer;
    }
    void Start()
    {
        if (wateredIndicator != null)
        {
            wateredIndicator.SetActive(false);
        }
        ShowGrowingVisual(0);
    }
    void Update()
    {
        isBeingWatered = droneAbove != null && droneAbove.IsWatering();
        if (currentState == PlantState.Growing)
        {
            UpdateGrowth();
        }
        else if (currentState == PlantState.Ripe)
        {
            UpdateRipe();
        }
        if (droneAbove != null)
        {
            ReactToDroneAbove();
        }
    }
    private void UpdateGrowth()
    {
        float growthSpeed = 1f;
        if (isBeingWatered)
        {
            growthSpeed = wateringGrowthMultiplier;
        }
        else if (isWatered)
        {
            growthSpeed = wateredGrowthMultiplier;
        }
        growthTimer += Time.deltaTime * growthSpeed;
        float growthRatio = growthTimer / growDuration;
        int stageIndex = Mathf.FloorToInt(growthRatio * growingVisuals.Length);
        ShowGrowingVisual(stageIndex);
        if (growthTimer >= growDuration)
        {
            BecomeRipe();
        }
    }
    private void UpdateRipe()
    {
        ripeTimer += Time.deltaTime;
        if (ripeTimer >= ripeDuration)
        {
            BecomeOverripe();
        }
    }
    private void ReactToDroneAbove()
    {
        if (currentState == PlantState.Growing && !isWatered && isBeingWatered)
        {
            isWatered = true;
            if (wateredIndicator != null)
            {
                wateredIndicator.SetActive(true);
            }
        }
        else if (currentState == PlantState.Ripe)
        {
            Harvest();
        }
    }
    private void BecomeRipe()
    {
        currentState = PlantState.Ripe;
        ShowOnlyVisual(ripeVisual);
        if (wateredIndicator != null)
        {
            wateredIndicator.SetActive(false);
        }
    }
    private void BecomeOverripe()
    {
        currentState = PlantState.Overripe;
        ShowOnlyVisual(overripeVisual);
        StartCoroutine(BurstAfterDelay());
    }
    private IEnumerator BurstAfterDelay()
    {
        yield return new WaitForSeconds(overripeDuration);
        Burst();
    }
    private void Harvest()
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(scoreValue);
        }
        PlayEffect(harvestEffectPrefab);
        PlaySound(harvestSound);
        Destroy(gameObject);
    }
    private void Burst()
    {
        PlayEffect(burstEffectPrefab);
        PlaySound(burstSound);
        Destroy(gameObject);
    }
    private void PlayEffect(ParticleSystem effectPrefab)
    {
        if (effectPrefab == null)
        {
            return;
        }
        ParticleSystem effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        float effectLifetime = effect.main.duration + effect.main.startLifetime.constantMax;
        Destroy(effect.gameObject, effectLifetime);
    }
    private void PlaySound(AudioClip clip)
    {
        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.PlaySound(clip);
        }
    }
    private void ShowGrowingVisual(int stageIndex)
    {
        if (growingVisuals.Length == 0)
        {
            return;
        }
        int clampedIndex = Mathf.Clamp(stageIndex, 0, growingVisuals.Length - 1);
        if (clampedIndex == currentGrowingStage)
        {
            return;
        }
        currentGrowingStage = clampedIndex;
        ShowOnlyVisual(growingVisuals[clampedIndex]);
    }
    private void ShowOnlyVisual(GameObject visualToShow)
    {
        foreach (GameObject growingVisual in growingVisuals)
        {
            if (growingVisual != null)
            {
                growingVisual.SetActive(growingVisual == visualToShow);
            }
        }
        if (ripeVisual != null)
        {
            ripeVisual.SetActive(ripeVisual == visualToShow);
        }
        if (overripeVisual != null)
        {
            overripeVisual.SetActive(overripeVisual == visualToShow);
        }
        if (plantAnimator != null)
        {
            plantAnimator.SetTrigger(GrowTriggerName);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        DroneWatering drone = other.GetComponent<DroneWatering>();
        if (drone != null)
        {
            droneAbove = drone;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        DroneWatering drone = other.GetComponent<DroneWatering>();
        if (drone != null && drone == droneAbove)
        {
            droneAbove = null;
        }
    }
    public PlantState GetState()
    {
        return currentState;
    }
}
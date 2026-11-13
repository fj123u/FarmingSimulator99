using UnityEngine;
using UnityEngine.InputSystem;

public class DronePlanter : MonoBehaviour
{
    public InputActionReference plantAction;
    public Plant[] plantPrefabs = new Plant[0];
    public Transform plantsParent;
    public float minDistanceBetweenPlants = 1f;
    public LayerMask soilLayer;
    public float maxSoilDistance = 10f;
    public float gridSize = 0f;
    public Vector3 gridOrigin = Vector3.zero;
    public ScoreManager scoreManager;
    public SoundEffectPlayer soundEffectPlayer;
    public AudioClip plantSound;
    void Start()
    {
        if (soilLayer.value == 0)
        {
            Debug.LogWarning("DronePlanter : le champ Soil Layer est vide (Nothing), impossible de planter. Choisis le layer Soil dans l'Inspector du drone.");
        }
    }
    void OnEnable()
    {
        plantAction.action.performed += OnPlant;
    }
    void OnDisable()
    {
        plantAction.action.performed -= OnPlant;
    }
    private void OnPlant(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
        PlantSeed();
    }

    private void PlantSeed()
    {
        if (plantPrefabs.Length == 0)
        {
            Debug.LogWarning("DronePlanter : aucun Prefab de plante assigné.");
            return;
        }
        RaycastHit soilHit;
        bool isAboveSoil = Physics.Raycast(transform.position, Vector3.down, out soilHit, maxSoilDistance, soilLayer, QueryTriggerInteraction.Ignore);
        if (!isAboveSoil)
        {
            return;
        }
        Vector3 plantPosition = SnapToGrid(soilHit.point);
        if (IsSpotTaken(plantPosition))
        {
            return;
        }
        Plant chosenPrefab = plantPrefabs[Random.Range(0, plantPrefabs.Length)];
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        Plant newPlant = Instantiate(chosenPrefab, plantPosition, randomRotation, plantsParent);
        newPlant.Init(scoreManager, soundEffectPlayer);
        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.PlaySound(plantSound);
        }
    }
    private Vector3 SnapToGrid(Vector3 position)
    {
        Vector3 snappedPosition = position;
        if (gridSize > 0f)
        {
            snappedPosition.x = gridOrigin.x + Mathf.Round((position.x - gridOrigin.x) / gridSize) * gridSize;
            snappedPosition.z = gridOrigin.z + Mathf.Round((position.z - gridOrigin.z) / gridSize) * gridSize;
        }
        return snappedPosition;
    }
    private bool IsSpotTaken(Vector3 plantPosition)
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(plantPosition, minDistanceBetweenPlants, Physics.AllLayers, QueryTriggerInteraction.Collide);
        foreach (Collider nearbyCollider in nearbyColliders)
        {
            if (nearbyCollider.GetComponent<Plant>() != null)
            {
                return true;
            }
        }
        return false;
    }
}
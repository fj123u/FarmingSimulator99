using UnityEngine;
using UnityEngine.InputSystem;
public class DroneWatering : MonoBehaviour
{
    public InputActionReference waterAction;
    public ParticleSystem waterParticles;
    public AudioSource waterAudioSource;
    private bool isWatering = false;
    void OnEnable()
    {
        waterAction.action.performed += OnWaterPressed;
        waterAction.action.canceled += OnWaterReleased;
    }
    void OnDisable()
    {
        waterAction.action.performed -= OnWaterPressed;
        waterAction.action.canceled -= OnWaterReleased;
        StopWatering();
    }
    private void OnWaterPressed(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
        StartWatering();
    }
    private void OnWaterReleased(InputAction.CallbackContext context)
    {
        StopWatering();
    }
    private void StartWatering()
    {
        isWatering = true;
        if (waterParticles != null)
        {
            waterParticles.Play();
        }
        if (waterAudioSource != null)
        {
            waterAudioSource.Play();
        }
    }
    private void StopWatering()
    {
        isWatering = false;
        if (waterParticles != null)
        {
            waterParticles.Stop();
        }
        if (waterAudioSource != null)
        {
            waterAudioSource.Stop();
        }
    }
    public bool IsWatering()
    {
        return isWatering;
    }
}
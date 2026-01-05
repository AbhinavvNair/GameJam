//using UnityEngine;
//using System.Collections;

//public class GunShoot : MonoBehaviour
//{
//    [Header("Combat Settings")]
//    [SerializeField] private int damage = 50;
//    [SerializeField] private float fireRate = 0.5f;
//    [SerializeField] private LayerMask EnemyLayer;

//    [Header("Shake Settings")]
//    [SerializeField] private float shakeDelay = 0.05f;
//    [SerializeField] private float shakeDuration = 0.15f;
//    [SerializeField] private float shakeStrength = 0.1f;

//    [Header("References")]
//    [SerializeField] private Animator gunAnimator;
//    [SerializeField] private Transform cameraTransform;

//    private bool isShooting = false;
//    private Vector3 defaultCameraPos;

//    void Start()
//    {
//        if (cameraTransform == null) cameraTransform = Camera.main.transform;
//        defaultCameraPos = cameraTransform.localPosition;
//    }

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0) && !isShooting)
//        {
//            StartCoroutine(ShootSequence());
//        }
//    }

//    IEnumerator ShootSequence()
//    {
//        isShooting = true;

//        // 1. Play Animation
//        if (gunAnimator != null)
//            gunAnimator.SetTrigger("shoot");

//        // 2. Wait for visual sync
//        yield return new WaitForSeconds(shakeDelay);

//        // 3. Shake & Fire
//        StartCoroutine(ScreenShake());
//        FireRaycast();

//        // 4. Cooldown
//        yield return new WaitForSeconds(fireRate);
//        isShooting = false;
//    }

//    void FireRaycast()
//    {
//        Ray gunray = new Ray(cameraTransform.position, cameraTransform.forward);

//        // We check for ANY hit first, then verify the layer manually for better accuracy
//        if (Physics.Raycast(gunray, out RaycastHit hitInfo, 100f))
//        {
//            // Bitwise check: Is the object on the Enemy Layer?
//            if (((1 << hitInfo.collider.gameObject.layer) & EnemyLayer) != 0)
//            {
//                if (hitInfo.collider.gameObject.TryGetComponent(out SubtractHealth subtractHealth))
//                {
//                    subtractHealth.Subtract(damage);
//                }
//            }
//        }
//    }

//    IEnumerator ScreenShake()
//    {
//        float elapsed = 0.0f;
//        while (elapsed < shakeDuration)
//        {
//            float x = Random.Range(-1f, 1f) * shakeStrength;
//            float y = Random.Range(-1f, 1f) * shakeStrength;

//            cameraTransform.localPosition = defaultCameraPos + new Vector3(x, y, 0);

//            elapsed += Time.deltaTime;
//            yield return null;
//        }
//        cameraTransform.localPosition = defaultCameraPos;
//    }
//}


using UnityEngine;
using System.Collections;
using TMPro; // Use UnityEngine.UI if not using TextMeshPro

public class GunShoot : MonoBehaviour
{
    [Header("Ammo Settings")]
    public int currentAmmo = 12;
    public int maxAmmo = 50;

    [Header("Combat Settings")]
    [SerializeField] private int damage = 50;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private LayerMask EnemyLayer;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDelay = 0.05f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeStrength = 0.3f;

    [Header("Audio")]
    [SerializeField] private AudioSource gunAudioSource;
    [SerializeField] private AudioClip mechanismSound;
    [SerializeField] private AudioClip boomSound;
    [SerializeField] private AudioClip emptyClickSound; // ADD THIS: Sound when out of ammo
    [Range(0.8f, 1.2f)][SerializeField] private float minPitch = 0.9f;
    [Range(0.8f, 1.2f)][SerializeField] private float maxPitch = 1.1f;

    [Header("References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private TextMeshProUGUI ammoText; // Drag your Ammo UI Text here

    private bool isShooting = false;

    void Start()
    {
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            if (currentAmmo > 0)
            {
                StartCoroutine(ShootSequence());
            }
            else
            {
                PlayEmptyClick(); // Click if empty
            }
        }
    }

    // --- NEW: Method to Add Ammo from Pickups ---
    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    IEnumerator ShootSequence()
    {
        isShooting = true;

        // Subtract Ammo
        currentAmmo--;
        UpdateAmmoUI();

        // 1. Play Animation
        if (gunAnimator != null)
            gunAnimator.SetTrigger("shoot");

        // 2. Play Sounds
        if (gunAudioSource != null)
        {
            gunAudioSource.pitch = Random.Range(minPitch, maxPitch);
            if (mechanismSound != null) gunAudioSource.PlayOneShot(mechanismSound, 1.0f);
            if (boomSound != null) gunAudioSource.PlayOneShot(boomSound, 0.7f);
        }

        // 3. Shake Delay
        yield return new WaitForSeconds(shakeDelay);

        // 4. Shake & Fire
        StartCoroutine(ScreenShake());
        FireRaycast();

        // 5. Cooldown
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    void FireRaycast()
    {
        Ray gunray = new Ray(cameraTransform.position, cameraTransform.forward);

        if (Physics.Raycast(gunray, out RaycastHit hitInfo, 100f))
        {
            if (((1 << hitInfo.collider.gameObject.layer) & EnemyLayer) != 0)
            {
                SubtractHealth subtractHealth = hitInfo.collider.GetComponentInParent<SubtractHealth>();
                if (subtractHealth != null)
                {
                    subtractHealth.Subtract(damage);
                }
            }
        }
    }

    void PlayEmptyClick()
    {
        // Optional: Play a "Click" sound if out of ammo
        if (gunAudioSource != null && emptyClickSound != null)
        {
            gunAudioSource.pitch = 1.0f;
            gunAudioSource.PlayOneShot(emptyClickSound);
        }
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();

            // Turn Red if low ammo (Low ammo warning)
            if (currentAmmo <= 5) ammoText.color = Color.red;
            else ammoText.color = Color.white;
        }
    }

    IEnumerator ScreenShake()
    {
        Vector3 originalPos = cameraTransform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float decayStrength = Mathf.Lerp(shakeStrength, 0f, elapsed / shakeDuration);
            float x = Random.Range(-1f, 1f) * decayStrength;
            float y = Random.Range(-1f, 1f) * decayStrength;

            cameraTransform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraTransform.localPosition = originalPos;
    }
}








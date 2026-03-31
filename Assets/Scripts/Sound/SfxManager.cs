using UnityEngine;

// plays sfx for attacks and footsteps
// two audiosources so attack pitch changes don't bleed into leg sounds

public class SfxManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip[] windupClips; // used for both attack windups and foot lifts
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip footLandClip;

    private AudioSource attackSource;
    private AudioSource legSource;

    void Awake()
    {
        attackSource = gameObject.AddComponent<AudioSource>();
        legSource    = gameObject.AddComponent<AudioSource>();
    }

    void OnEnable()
    {
        //Attack.OnArmSwing      += HandleArmSwing;
        //Attack.OnAttackHit     += HandleAttackHit;
        //LegMovement.OnFootLift += HandleFootLift;
        //LegMovement.OnFootLand += HandleFootLand;
    }

    void OnDisable()
    {
        //Attack.OnArmSwing      -= HandleArmSwing;
        //Attack.OnAttackHit     -= HandleAttackHit;
        //LegMovement.OnFootLift -= HandleFootLift;
        //LegMovement.OnFootLand -= HandleFootLand;
    }

    private void HandleArmSwing()
    {
        if (windupClips == null || windupClips.Length == 0) return;

        attackSource.pitch  = 1f;
        attackSource.volume = 1f;
        attackSource.PlayOneShot(windupClips[Random.Range(0, windupClips.Length)]);
    }

    private void HandleAttackHit(float intensity)
    {
        // heavier hits go lower and louder — kept subtle so they all still feel like the same weapon
        // jab(0.3) = quick pop, uppercut(0.5) = crisp, heavy(0.7) = thud, slam(1.0) = boom
        if (hitClip == null) return;

        attackSource.pitch  = Mathf.Lerp(1.05f, 0.85f, intensity);
        attackSource.volume = Mathf.Lerp(0.70f, 1.00f, intensity);
        attackSource.PlayOneShot(hitClip);
    }

    private void HandleFootLift()
    {
        if (windupClips == null || windupClips.Length == 0) return;

        legSource.pitch  = 1f;
        legSource.volume = 1f;
        legSource.PlayOneShot(windupClips[Random.Range(0, windupClips.Length)]);
    }

    private void HandleFootLand()
    {
        if (footLandClip == null) return;

        legSource.pitch  = 1f;
        legSource.volume = 1f;
        legSource.PlayOneShot(footLandClip);
    }
}

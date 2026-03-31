using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField]
    protected GameObject handObject;
    [SerializeField]
    protected MovementMatcher attachedMovementHandler;
    [SerializeField]
    protected float BaseDamage = 0;

    public abstract void OnAttack();
    protected abstract void OnAttackeEnd();
}

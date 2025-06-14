using UnityEngine;

public class KelinciController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    public void RegisterSayur(SayurController sayur)
    {
        sayur.OnSayurMatch += HappyAnimation;
        sayur.OnSayurMismatch += SadAnimation;
    }

    public void HappyAnimation()
    {
        animator.SetTrigger("isHappy");
    }

    public void SadAnimation()
    {
        animator.SetTrigger("isSad");
    }

    public void PouringAnimation()
    {
        animator.SetTrigger("isPouring");
    }

    public void JumpingAnimation()
    {
        animator.SetTrigger("isJumping");
    }
}

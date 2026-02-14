using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public string animationStateName;

    public void PlayForward()
    {
        animator.speed = 1f;
        animator.Play(animationStateName, 0, 0f);
    }

    public void PlayReverse()
    {
        animator.speed = -1f;
        animator.Play(animationStateName, 0, 1f); 
    }
}

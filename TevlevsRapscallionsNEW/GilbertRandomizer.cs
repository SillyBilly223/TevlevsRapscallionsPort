using UnityEngine;

public class GilbertRandomizer : StateMachineBehaviour
{
    public string FloatName;
    public Vector2 range;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(FloatName, Random.Range(range.x, range.y + 0.01f));
    }


}

using UnityEngine;

public class BackgroundChangeBehaviour : StateMachineBehaviour
{
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
    if(animator.TryGetComponent<BackgroundControl>(out var bg_control)){
      bg_control.UpdateBackground();
    }
  }
}

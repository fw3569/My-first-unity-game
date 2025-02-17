using System.Collections.Generic;
using UnityEngine;

public class PrecastBehaviour : StateMachineBehaviour
{
  public List<string>act_list = new();
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if(animator.gameObject.TryGetComponent<Creature>(out var creature)){
      creature.act_list = act_list;
      creature.ResetAction();
    }
  }
}

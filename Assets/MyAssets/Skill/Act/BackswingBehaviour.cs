using System.Collections.Generic;
using UnityEngine;

public class BackswingBehaviour : StateMachineBehaviour
{
  public List<string>act_black_list = new();
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if(animator.gameObject.TryGetComponent<Creature>(out var creature)){
      creature.act_list = new(creature.act_list_ori);
      foreach(string act_name in act_black_list){
        creature.act_list.Remove(act_name);
      }
    }
  }
}

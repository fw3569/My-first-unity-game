using UnityEngine;

public class MagicBehaviour : StateMachineBehaviour
{
  private Magic magic;
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if(animator.gameObject.TryGetComponent<Creature>(out var creature)){
      magic = creature.next_magic;
      creature.next_magic = null;
    }
  }
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if(magic!=null){
      if(animator.gameObject.TryGetComponent<Creature>(out var creature)){
        magic.pos = creature.weaponwrap.transform.position;
        magic.dir = creature.cur_direction;
        magic.rot = creature.weaponwrap.transform.rotation;
        magic.source = creature;
        if(creature.hate_target!=null){
          magic.target = creature.hate_target.gameObject;
        } else {
          magic.target = null;
        }
      }
      magic.GenMagic();
    }
  }
}

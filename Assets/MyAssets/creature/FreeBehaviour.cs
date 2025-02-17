using UnityEngine;

public class FreeBehaviour : StateMachineBehaviour
{
  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
    if(animator.TryGetComponent<Creature>(out var creature)){
      if(creature.hate_target == null){
        if(!creature.ignore_free_move&&creature.next_free_move_time<=Time.fixedTime){
          Vector2 move_target = creature.birth_position;
          move_target += 2 * creature.free_move_range * new Vector2(Random.value-0.5f,Random.value-0.5f);
          creature.next_free_move_time = Time.fixedTime+creature.free_move_delay;
          creature.target_direction = -creature.gameObject.transform.position;
          creature.target_direction += move_target;
          creature.target_direction.Normalize();
          creature.SetAction("FreeMove");
        }
      } else {
        creature.AutoActionInternal();
      }
    }
  }
}

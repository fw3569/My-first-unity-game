using UnityEngine;

public class MovableBehaviour : StateMachineBehaviour
{
  public float speed_rate = 0.2f;
  public bool update_toward_once = false;
  public float update_toward_once_max_angle = 0.0f;
  public bool update_toward = false;
  public bool free_toward = false;
  public float update_toward_speed = 0.0f;
  public bool force_move = false;
  public float force_move_speed = 0.0f;
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if(update_toward_once){
      Creature creature = animator.GetComponent<Creature>();
      creature.UpdateToward(update_toward_once_max_angle);
    }
  }
  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    Creature creature = animator.GetComponent<Creature>();
    if(update_toward){
      creature.UpdateToward(free_toward?180.0f:update_toward_speed/stateInfo.length*Time.fixedDeltaTime);
    }
    creature.UpdateMoveVector();
    Vector2 direction;
    float speed;
    if(force_move){
      direction = creature.cur_direction;
      speed = force_move_speed;
    } else {
      direction = creature.move_vector;
      speed = creature.speed*speed_rate;
    }
    creature.move_step+=Time.fixedDeltaTime*speed*direction;
  }
}

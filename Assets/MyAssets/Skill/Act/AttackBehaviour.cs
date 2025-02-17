// check its derived classes
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
  // if hit_interval is not 0, it is a multi hit action
  public float hit_interval = 0;
  // data of this attack action
  public int action_atk = 0;
  public float atk_rate = 1.0f;
  public int action_mat = 0;
  public float mat_rate = 1.0f;
  public float tat_rate = 1;
  public float knock_back_rate = 1.0f;
  public bool is_knock_out = false;
  public KnockOutType knock_back_type = KnockOutType.DIFFUSION;
  public float knock_out_speed = 0.0f;
  public float knock_out_time = 0.0f;
  // the hit box
  private GameObject weapon;
  private Creature owner;
  private float attack_box_update_time;
  private void SetAttackBox(GameObject obj, Creature creature){
    obj.GetComponent<Collider2D>().enabled = false;
    if(creature.is_in_guard_area){
      obj.GetComponent<Collider2D>().enabled = true;
      if(is_knock_out){
        obj.GetComponent<AttackBox>().Set(creature.atk+action_atk,creature.atk_rate*atk_rate,creature.weapon_equip.mat+action_mat,creature.mat_rate*mat_rate,creature.tat,tat_rate*creature.tat_rate,knock_back_rate,knock_back_type,knock_out_speed,knock_out_time,creature.cur_direction,creature.group);
      } else {
        obj.GetComponent<AttackBox>().Set(creature.atk+action_atk,creature.atk_rate*atk_rate,creature.weapon_equip.mat+action_mat,creature.mat_rate*mat_rate,creature.tat,tat_rate*creature.tat_rate,knock_back_rate,creature.group);
      }
    }
    attack_box_update_time = Time.fixedTime+hit_interval;
  }
  public void SetAttackBoxObject(GameObject obj, Creature creature){
    weapon = obj;
    owner = creature;
    SetAttackBox(weapon,owner);
  }
  public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if(hit_interval!=0&&attack_box_update_time<=Time.fixedTime){
      SetAttackBox(weapon,owner);
    }
  }
}

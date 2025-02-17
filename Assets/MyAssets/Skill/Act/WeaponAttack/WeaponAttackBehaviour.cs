using UnityEngine;

public class WeaponAttackBehaviour : AttackBehaviour
{
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    Creature creature = animator.GetComponent<Creature>();
    Collider2D weapon_col = creature.weapon.GetComponent<Collider2D>();
    weapon_col.enabled  = true;
    SetAttackBoxObject(creature.weapon,creature);
  }
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    Creature creature = animator.GetComponent<Creature>();
    Collider2D weapon_col = creature.weapon.GetComponent<Collider2D>();
    weapon_col.enabled  = false;
  }
}

using UnityEngine;

public class EffectAttackBehaviour : AttackBehaviour
{
  public GameObject effect_prefab;
  private GameObject new_effect_obj;
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    Creature creature = animator.GetComponent<Creature>();
    new_effect_obj = Instantiate(effect_prefab,creature.weapon.transform);
    SetAttackBoxObject(new_effect_obj, creature);
  }
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    Destroy(new_effect_obj);
    new_effect_obj = null;
  }
}

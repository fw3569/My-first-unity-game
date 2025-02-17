using UnityEngine;

public class ActionSkill : Skill
{
  public string act_name = "";
  public bool Trigger(string act_name, Vector2 pos, Vector2 dir, Quaternion rot, Creature source, GameObject target){
    if(source.act_list.Contains(act_name)){
      if((source.next_action==null||source.next_action.Length==0)&&source.TryGetComponent<Animator>(out var animator)){
        source.ResetAction();
        source.next_action = act_name;
        animator.SetBool(act_name,true);
        animator.SetBool("Any",true);
        return true;
      }
      return false;
    } else {
      return false;
    }
  }
  public override bool Trigger(Vector2 pos, Vector2 dir, Quaternion rot, Creature source, GameObject target){
    return Trigger(act_name, pos, dir, rot, source, target);
  }
}

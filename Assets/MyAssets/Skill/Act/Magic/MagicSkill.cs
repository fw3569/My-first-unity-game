using UnityEngine;

public class MagicSkill : ActionSkill
{
  public Magic magic;
  public override bool Trigger(Vector2 pos, Vector2 dir, Quaternion rot, Creature source, GameObject target){
    bool ret = Trigger(act_name, pos, dir, rot, source, target);
    if(ret && source.TryGetComponent<Creature>(out var creature)){
      Magic new_magic = Instantiate(magic,creature.instanced_prefabs.transform);
      creature.next_magic = new_magic;
    }
    return ret;
  }
}

using Unity.VisualScripting;
using UnityEngine;

public class BuffSkill : ActionSkill
{
  public Buff buff;
  public BuffIcon buff_icon;
  public float duration;
  public virtual Transform TargetTransform(Creature creature){
    return creature.transform;
  }
  public override bool Trigger(Vector2 pos, Vector2 dir, Quaternion rot, Creature source, GameObject target){
    bool ret = Trigger(act_name, pos, dir, rot, source, target);
    if(ret && source.TryGetComponent<Creature>(out var creature)){
      foreach(Buff iter in creature.buffs){
        if(iter.buff_name==buff.buff_name) {
          if(iter.IsDestroyed()==false){
            Destroy(iter.gameObject);
          }
          break;
        }
      }
      Buff new_buff = Instantiate(buff,TargetTransform(creature));
      new_buff.creature = creature;
      Destroy(new_buff.gameObject,duration);
      if(creature.buff_gauge!=null){
        BuffIcon new_buff_icon = Instantiate(buff_icon, creature.buff_gauge.transform);
        new_buff_icon.duration = duration;
        new_buff_icon.start_time = Time.fixedTime;
        new_buff.buff_icon = new_buff_icon;
      }
      creature.buffs.Add(new_buff);
      creature.UpdateStatus();
    }
    return ret;
  }
}

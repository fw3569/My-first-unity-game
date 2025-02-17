// item: a numerable object, maybe item or skill or equip. cost it invoke skill
// skill: a action can be invoke by cost item and magic gauge
// magic: a skill which generate another object
// item icon: a dragable icon, own a item
// item box, equip box, skill box: where item icon can stay in
// item slot, equip slot, skill slot: a box, have a corresponding key to invoke
using UnityEngine;

public class Item : MonoBehaviour
{
  public float skill_cold_time = 0.0f;
  public int magic_cost = 0;
  public int item_cost = 1;
  public Skill skill;
  public float skill_cold_resume_time = 0.0f;
  public virtual bool Trigger(Creature source,bool auto_trigger = false){
    return Trigger(source.weaponwrap.transform.position,source.cur_direction,source.weaponwrap.transform.rotation,source,source.hate_target==null?null:source.hate_target.gameObject,auto_trigger);
  }
  public virtual bool Trigger(Vector2 pos, Vector2 dir, Quaternion rot, Creature source, GameObject target, bool auto_trigger = false){
    int item_num = 0;
    if(item_cost!=0){
      if(GameManager.Instance().is_in_boss_room){
        return false;
      }
      item_num = source.items[name];
    }
    bool ret = false;
    if(skill!=null){
      if(source.magic>=magic_cost&&Time.fixedTime>=skill_cold_resume_time&&item_num>=item_cost){
        ret = skill.Trigger(pos, dir, rot, source, target);
        if(ret==true){
          if(magic_cost!=0){
            source.magic-=magic_cost;
            source.UpdateGauge();
          }
          if(item_cost!=0){
            source.items[name] -= item_cost;
          }
          skill_cold_resume_time=Time.fixedTime+skill_cold_time;
        }
      }
    } else {
      print("Empty Item");
    }
    if(ret==false){
      source.next_item = this;
      if(auto_trigger== false){
        source.next_item_release_time = Creature.next_item_release_delay + Time.fixedTime;
      }
    }
    return ret;
  }
}

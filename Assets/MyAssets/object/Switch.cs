using System.Collections.Generic;
using UnityEngine;

public class Switch : InteractivityObj
{
  public List<ObjectList> active_objects;
  public List<ObjectList> inactive_objects;
  public int status = 0;
  public int status_count = 2;
  public List<Sprite> sprites;
  public void SetStatus(int status){
    this.status = status;
    foreach(GameObject obj in active_objects[status].obj_list){
      obj.SetActive(true);
      if(obj.TryGetComponent<Creature>(out var creature)&&obj.transform.parent==transform.parent){
        creature.OnEnterGuardArea();
      }
    }
    foreach(GameObject obj in inactive_objects[status].obj_list){
      obj.SetActive(false);
    }
    GetComponent<SpriteRenderer>().sprite = sprites[status];
  }
  public override string Interactive(GameObject from){
    SetStatus((status+1)%status_count);
    GameManager.Instance().AddSwitchStatus(transform.GetPath(),status);
    return "";
  }
  public override void ResetInteractive(GameObject from){
  }
}

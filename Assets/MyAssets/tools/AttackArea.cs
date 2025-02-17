using UnityEngine;

public class AttackArea : MonoBehaviour
{
  public Creature owner;
  public int id = 0;
  void OnTriggerEnter2D(Collider2D col){
    if(!owner) {
      return;
    }
    Creature target = col.transform.parent.parent.gameObject.GetComponent<Creature>();
    if(target!=null&&owner.group!=target.group) {
      owner.AddAttackableTarget(target,this,id);
    }
  }
  void OnTriggerExit2D(Collider2D col){
    if(!owner) {
      return;
    }
    Creature target = col.transform.parent.parent.gameObject.GetComponent<Creature>();
    if(target!=null&&owner.group!=target.group) {
      owner.RemoveAttackableTarget(target,this,id);
    }
  }

}

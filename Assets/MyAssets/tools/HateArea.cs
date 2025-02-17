using UnityEngine;

public class HateArea : MonoBehaviour
{
  private Creature owner;
  public void Awake()
  {
    owner = transform.parent.gameObject.GetComponent<Creature>();
  }
  void OnTriggerEnter2D(Collider2D col){
    if(!owner) {
      return;
    }
    Creature target = col.transform.parent.parent.gameObject.GetComponent<Creature>();
    if(target!=null&&owner.group!=target.group) {
      owner.AddCandidateTarget(target);
    }
  }
  void OnTriggerExit2D(Collider2D col){
    if(!owner) {
      return;
    }
    Creature target = col.transform.parent.parent.gameObject.GetComponent<Creature>();
    if(target!=null&&owner.group!=target.group) {
      owner.RemoveCandidateTarget(target);
    }
  }

}

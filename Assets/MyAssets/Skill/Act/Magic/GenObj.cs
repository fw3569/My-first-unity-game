using UnityEngine;

public class GenObj : MonoBehaviour
{
  public float duration = 1;
  private float dead_time;
  public int group = 0;
  public bool one_time = true;
  void Start()
  {
    dead_time = Time.fixedTime+duration;
  }

  void Update()
  {

  }
  void FixedUpdate()
  {
    if(dead_time<=Time.fixedTime){
      Destroy(gameObject);
    }
  }
  public void OnTriggerEnter2D(Collider2D col){
    if(col.isTrigger == false&&one_time){
      if(col.transform.parent!=null&&col.transform.parent.parent!=null&&col.transform.parent.parent.TryGetComponent<Creature>(out var creature)){
        if(creature.group == group){
          return;
        }
      }
      Destroy(gameObject);
    }
  }
}

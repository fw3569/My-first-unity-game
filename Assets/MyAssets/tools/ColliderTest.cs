using UnityEngine;

public class ColliderTest : MonoBehaviour
{
  private float recover_stun_time;
  public float stun_time = 2f;
  public float knock_back_distance = 0.2f;
  void Start()
  {

  }

  void Update()
  {

  }
  void FixedUpdate(){
    if(Time.fixedTime>recover_stun_time){
      this.GetComponent<SpriteRenderer>().color = Color.white;
    }
  }
  public void Stun(ref Collider2D col){
    recover_stun_time = Time.fixedTime + stun_time;
    this.GetComponent<SpriteRenderer>().color = Color.red;
    Vector3 direct = this.transform.position-col.transform.position;
    direct.Normalize();
    this.transform.position+=direct*knock_back_distance;
  }
  void OnCollisionStay2D(Collision2D col){
    print(col.gameObject.tag);
  }
  void OnTriggerEnter2D(Collider2D col){
    print(col.tag);
    if(col.CompareTag("AttackBox")){
      Stun(ref col);
    }
  }
}

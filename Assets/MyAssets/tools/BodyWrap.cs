using UnityEngine;

public class BodyWrap : MonoBehaviour
{
  public Sprite front_sprite;
  public Sprite back_sprite;
  private GameObject body;
  private Toward2 default_toward = Toward2.RIGHT;
  private Toward4 cur_ytoward = Toward4.UNDEFINE;
  void Awake()
  {
    foreach(Transform child in transform){
      if(child!= null&&child.name=="Body"){
        body = child.gameObject;
        break;
      }
    }
  }
  private void ToLeft()
  {
    transform.rotation = default_toward==Toward2.LEFT?new Quaternion(0,0,0,0):new Quaternion(0,1,0,0);
  }
  private void ToRight()
  {
    transform.rotation = default_toward==Toward2.LEFT?new Quaternion(0,1,0,0):new Quaternion(0,0,0,0);
  }
  private void ToUp()
  {
    if(cur_ytoward!=Toward4.UP){
      body.GetComponent<SpriteRenderer>().sprite = back_sprite;
      cur_ytoward=Toward4.UP;
    }
  }
  private void ToDown()
  {
    if(cur_ytoward!=Toward4.DOWN){
      body.GetComponent<SpriteRenderer>().sprite = front_sprite;
      cur_ytoward=Toward4.DOWN;
    }
  }
  public void UpdateXToward(Vector2 dir)
  {
    if(dir.x<0){
      ToLeft();
    } else if(dir.x>0){
      ToRight();
    }
  }
  public void UpdateYToward(Vector3 dir)
  {
    if(dir.y>0){
      ToUp();
    } else{
      ToDown();
    }
  }
}

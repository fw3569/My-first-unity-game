using UnityEngine;

public enum Toward2{
  UNDEFINE,
  LEFT,
  RIGHT,
};
public enum Toward4{
  UNDEFINE,
  LEFT,
  RIGHT,
  UP,
  DOWN
};
public class WeaponWrap : MonoBehaviour
{
  public void UpdateToward(Vector2 dir)
  {
    if(dir.x==0.0f){
      if(dir.y>0){
        transform.rotation = new Quaternion(0,0,1,1);
      } else if(dir.y<0){
        transform.rotation = new Quaternion(0,0,1,-1);
      }
    } else {
      float ang = Mathf.Atan(dir.y/Mathf.Abs(dir.x));
      transform.rotation = (dir.x<0?new Quaternion(0,1,0,0):new Quaternion(0,0,0,1))*new Quaternion(0,0,Mathf.Sin(ang/2),Mathf.Cos(ang/2));
    }
  }
}

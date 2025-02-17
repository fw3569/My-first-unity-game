// here have many derived classes
using UnityEngine;

public class Skill : MonoBehaviour
{
  public virtual bool Trigger(Vector2 pos,Vector2 dir, Quaternion rot,Creature source,GameObject target){
    print("Trigger "+name);
    return true;
  }
}

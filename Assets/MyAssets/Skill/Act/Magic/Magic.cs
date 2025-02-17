using UnityEngine;

public class Magic : MonoBehaviour
{
  public bool triggered = false;
  public Vector2 pos;
  public Vector2 dir;
  public Quaternion rot;
  public Creature source;
  public GameObject target;
  public int atk = 0;
  public float atk_rate = 1.0f;
  public int mat = 0;
  public float mat_rate = 1.0f;
  public int tat = 0;
  public float tat_rate = 1.0f;
  public float knock_back_rate = 1.0f;
  public virtual void GenMagicInternal(){
    print("Empty GenMagic");
  }
  public void GenMagic(){
    if(triggered==false){
      triggered = true;
      GenMagicInternal();
      Destroy(gameObject);
    }
  }
}

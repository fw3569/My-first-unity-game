using UnityEngine;

public class FireballMagic : Magic
{
  public GameObject fireball;
  public override void GenMagicInternal(){
    GameObject new_gen = Instantiate(fireball,pos,Quaternion.Euler(0,0,Mathf.Atan2(dir.y,dir.x)/Mathf.PI*180f));
    AttackBox atkbox = new_gen.GetComponent<AttackBox>();
    atkbox.Set(atk, source.atk_rate*atk_rate, mat + source.mat, source.mat_rate*mat_rate, tat, source.tat_rate*tat_rate, knock_back_rate, source.group);
    new_gen.GetComponent<GenObj>().group = source.group;
  }
}

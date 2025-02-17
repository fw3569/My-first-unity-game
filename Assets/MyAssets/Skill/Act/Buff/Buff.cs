using UnityEngine;

public class Buff : MonoBehaviour
{
  public string buff_name;
  public int life = 0;
  public float life_rate = 1.0f;
  public int magic = 0;
  public float magic_rate = 1.0f;
  public int tough = 0;
  public float tough_rate = 1.0f;
  public int atk = 0;
  public float atk_rate = 1.0f;
  public int def = 0;
  public float def_rate = 1.0f;
  public int mat = 0;
  public float mat_rate = 1.0f;
  public int mdf = 0;
  public float mdf_rate = 1.0f;
  public int tat = 0;
  public float tat_rate = 1.0f;
  public float tdf_rate = 1.0f;
  public BuffIcon buff_icon;
  public Creature creature;
  public void OnDestroy(){
    if(buff_icon!=null){
      buff_icon.gameObject.SetActive(false);
      Destroy(buff_icon.gameObject);
    }
    if(creature!=null){
      creature.buffs.Remove(this);
      creature.UpdateStatus();
    }
  }
}

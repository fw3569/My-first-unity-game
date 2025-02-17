using UnityEngine;

public enum KnockOutType{
  DIFFUSION = 1,
  FORWARD = 2
}
public class AttackBox : MonoBehaviour
{
  public int atk = 0;
  public float atk_rate = 1.0f;
  public int mat = 0;
  public float mat_rate = 1.0f;
  public int tat = 0;
  public float tat_rate = 1.0f;
  public float knock_back_rate = 1.0f;
  public bool is_knock_out = false;
  public KnockOutType knock_out_type = KnockOutType.DIFFUSION;
  public float knock_out_speed = 0.0f;
  public float knock_out_time = 0.0f;
  public Vector2 knock_out_direction = new(0,0);
  // creature in same group don't hit each other
  public int group = 0;
  // a attack box only hit a creature once
  public int attack_box_seq = 0;
  static public int next_attack_box_seq = 0;
  public void Set(int atk, float atk_rate, int mat, float mat_rate, int tat, float tat_rate, float knock_back_rate, int group){
    this.atk = atk;
    this.atk_rate = atk_rate;
    this.mat = mat;
    this.mat_rate = mat_rate;
    this.tat = tat;
    this.tat_rate = tat_rate;
    this.knock_back_rate = knock_back_rate;
    this.is_knock_out = false;
    this.group = group;
    attack_box_seq = ++next_attack_box_seq;
  }
  public void Set(int atk, float atk_rate, int mat, float mat_rate, int tat, float tat_rate, float knock_back_rate, KnockOutType knock_out_type, float knock_out_speed, float knock_out_time, Vector2 knock_out_direction, int group){
    Set(atk, atk_rate, mat, mat_rate, tat, tat_rate, knock_back_rate, group);
    this.is_knock_out = true;
    this.knock_out_type = knock_out_type;
    this.knock_out_speed = knock_out_speed;
    this.knock_out_time = knock_out_time;
    this.knock_out_direction = knock_out_direction;
  }
  public void UpdateSeq(){
    attack_box_seq = ++next_attack_box_seq;
  }
}

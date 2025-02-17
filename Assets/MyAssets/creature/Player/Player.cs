// about character info and part of character control
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;

[System.Serializable]
public class Status{
  public int life = 1;
  public int magic = 1;
  public int tough = 1;
  public int atk = 0;
  public int def = 0;
  public int mat = 0;
  public int mdf = 0;
  public int tat = 0;
  public int next_level_exp = 0;
  public override string ToString() {
    return life +" "+
           magic +" "+
           tough +" "+
           atk +" "+
           def +" "+
           mat +" "+
           mdf +" "+
           tat
           ;
  }
}

public class Player : Creature
{
  static bool init = false;
  static public Dictionary<int,Status>level_status = new();
  public int cur_exp = 0;
  public int next_level_exp = 1;
  public List<string> birth_background_path;
  public Light2D eye;
  // sometime ignore input
  public bool receive_input = true;
  private InputAction move_action;
  private Vector2 input_vector = new(0,0);
  // don't change toward when moving
  private InputAction lock_toward_action;
  private bool lock_toward = false;
  private InputAction backjump_action;
  private bool backjump_key = false;
  private float backjump_key_release_time;
  private Item backjump_item;
  public GameObject level_up_effect;
  public override void Awake() {
    base.Awake();
    move_action = InputSystem.actions.FindAction("Move");
    lock_toward_action = InputSystem.actions.FindAction("LockToward");
    backjump_action = InputSystem.actions.FindAction("BackJump");
    if(!init){
      float a=10.0f,d=0.0f,l=100.0f,t=50.0f,ta=10.0f,m=100.0f,e=50.0f;
      float k1=1.0233f,k2=1.01115f,k3=k1*1.016218f;
      for (int i=1;i<=max_level;++i){
        float aa = a,dd=d;
        d=dd+2;
        a=(aa-dd)*k1+d;
        l*=k3;
        m*=k1;
        t*=k2;
        ta*=k2;
        e*=(2+k1*(i-1))/i;
        level_status[i]=new Status{
          life = (int)l,
          magic = (int)m,
          tough = (int)t,
          atk = (int)a,
          def = (int)d,
          mat = (int)a,
          mdf = (int)d,
          tat = (int)ta,
          next_level_exp = (int)e
        };
      }
      init = true;
    }
    if(ItemManager.items.ContainsKey("Backjump")){
      backjump_item = ItemManager.items["Backjump"];
    }
  }
  override public void Update()
  {
    base.Update();
    if(receive_input){
      input_vector = move_action.ReadValue<Vector2>();
      target_direction = input_vector;
      if(input_vector!=Vector2.zero){
        target_direction.Normalize();
        next_item = null;
      }
    } else {
      input_vector = new(0,0);
      target_direction = new(0,0);
    }
    if(lock_toward_action.IsPressed()){
      lock_toward = true;
    } else {
      lock_toward = false;
    }
    if(backjump_action.WasPressedThisFrame()){
      backjump_key = true;
      backjump_key_release_time = Time.time+Time.fixedDeltaTime;
    } else if(backjump_key_release_time<Time.time){
      backjump_key = false;
    }
  }
  public void ResetBirthPosition(){
    birth_position = transform.position;
    birth_rotation = transform.rotation;
    birth_background_path = GameManager.Instance().background_control.cur_background.transform.GetPath();
  }
  override public void Dead(){
    if(!is_alive){
      return;
    }
    foreach(Pair<int,SpriteRenderer> iter in sprite_renders){
      iter.se.color = Color.black/2+Color.white/2;
    }
    animator.Play("Dead");
    is_alive = false;
  }
  override public void AutoAction(){
    if(next_item!=null){
      if(next_item_release_time<=Time.fixedTime){
        next_item = null;
      } else {
        bool ret = next_item.Trigger(this,true);
        if(ret == true){
          next_item = null;
        }
      }
    }
  }
  public override void UpdateToward(float max_angle){
    if(lock_toward == false){
      base.UpdateToward(max_angle);
    }
  }
  public override void UpdateSpriteToward(){
    bodywrap.GetComponent<BodyWrap>().UpdateXToward(cur_directionx);
    bodywrap.GetComponent<BodyWrap>().UpdateYToward(cur_directiony);
    weaponwrap.GetComponent<WeaponWrap>().UpdateToward(cur_direction);
  }
  public override void UpdateMoveVector(){
    move_vector = input_vector;
  }
  override public void FixedUpdate(){
    base.FixedUpdate();
    if(backjump_key){
      if(backjump_item!=null){
        backjump_item.Trigger(this);
      }
    }
  }
  public void UpdateLevel(){
    life_ori = level_status[level].life;
    magic_ori = level_status[level].magic;
    tough_ori = level_status[level].tough;
    atk_ori = level_status[level].atk;
    def_ori = level_status[level].def;
    mat_ori = level_status[level].mat;
    mdf_ori = level_status[level].mdf;
    tat_ori = level_status[level].tat;
    next_level_exp = level_status[level].next_level_exp;
    UpdateStatus();
  }
  public void SetEquip(Equip equip, int accessory_id){
    if(equip.part == Equip.Part.WEAPON){
      weapon_equip = equip;
    } else if (equip.part == Equip.Part.ARMOR) {
      armor_equip = equip;
    } else if (equip.part == Equip.Part.DRIVER) {
      driver_equip = equip;
    } else if (equip.part == Equip.Part.ACCESSORY) {
      accessory_equips[accessory_id] = equip;
    }
    UpdateStatus();
  }
  public int ExpFunction(int exp, int level){
    int leveldiff = level-this.level-1;
    int fixed_exp = (int)(exp*math.pow(5,leveldiff));
    return math.min(math.max(0,fixed_exp),next_level_exp);
  }
  public void ExpUp(int exp, int killed_level){
    cur_exp+=ExpFunction(exp, killed_level);
    while(level<max_level&&cur_exp>=next_level_exp){
      cur_exp-=next_level_exp;
      ++level;
      GameObject new_level_up_effect = Instantiate(level_up_effect,body.transform);
      addition_sprite_renders.Add(new Pair<int,SpriteRenderer>(10,new_level_up_effect.transform.GetChild(0).GetComponent<SpriteRenderer>()));
      addition_sprite_renders.Add(new Pair<int,SpriteRenderer>(-10,new_level_up_effect.transform.GetChild(1).GetComponent<SpriteRenderer>()));
      UpdateLevel();
      UpdateStatus();
    }
  }
  public override void OnCollisionEnter2D(Collision2D col){
    GameObject col_obj = col.gameObject;
    if(col_obj!=null&&col_obj.CompareTag("DropItem")){
      DropItem drop_item = col_obj.GetComponent<DropItem>();
      ItemManager.Instance().Add(drop_item.type, drop_item.item_name, drop_item.number);
      Destroy(col_obj);
      return;
    }
    base.OnCollisionEnter2D(col);
  }
}

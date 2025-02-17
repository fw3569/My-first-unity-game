// creature info and general behavioral control
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Pair<T1,T2>{
  public T1 fs;
  public T2 se;
  public Pair(T1 fs, T2 se){
    this.fs = fs;
    this.se = se;
  }
}
public class Creature :MonoBehaviour
{
  public bool is_alive = true;
  public bool is_stun = false;
  public float default_stun_time = 2.0f; 
  protected float max_stun_time = 3.0f;
  public float remain_stun_time = 0.0f;
  protected float start_stun_time = 0.0f;
  // stun gauge will accumulated on hit
  const float stun_accumulate_rate = 1.0f;
  // stun will decay accelerately
  const float stun_abate_ratio = 2.0f;
  // min delay between two stun
  const float stun_protect_duration = 0.5f;
  protected float stun_protect_end_time = 0.0f;
  public GameObject stun_effect;
  // knock out: uncontrolled movement
  public bool is_in_knock_out = false;
  public bool can_knock_out = true;
  // stop movement but still can not be control
  public bool is_in_knock_out_backswing = false;
  private float knock_out_end_time = 0.0f;
  private float knock_out_end_backswing_time = 0.0f;
  const float knock_out_end_backswing_delay = 0.5f;
  private float knock_out_speed = 0.0f;
  private Vector2 knock_out_direction = new(0,0);
  public GameObject knock_out_effect;
  // knock_back_distance < collider.size / 2
  public float knock_back_distance = 0.2f;
  const float color_recover_delay = 0.2f;
  protected float color_recover_time = 0.0f;
  // speed * Time.fixedDeltaTime < collider.size / 2
  // max_move_step should <= 0.5
  const float max_move_step = 0.4f;
  public float speed = 2f;
  protected float move_eps = 0.0f;
  // some creature do not have free move, they lie in wait for player
  public bool ignore_free_move = false;
  public float free_move_range = 3.0f;
  public float next_free_move_time = 0.0f;
  public float free_move_delay = 3.0f;
  public int level = 1;
  public const int max_level = 100;
  public int life_ori = 1;
  public int magic_ori = 1;
  public int tough_ori = 1;
  public int atk_ori = 0;
  public int def_ori = 0;
  public int mat_ori = 0;
  public int mdf_ori = 0;
  public int tat_ori = 0;
  public int life_max = 1;
  public int life = 1;
  public int magic_max = 1;
  public int magic = 1;
  // the higher tough, the harder to be stun
  public int tough_max = 1;
  public int tough = 1;
  // tough will recover automatically if unhit for a period of time
  protected float tough_recover_time = 0.0f;
  protected const float tough_recover_delay = 2.0f;
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
  public List<Buff> buffs;
  // wearing equip
  public Equip weapon_equip = new(Equip.Part.WEAPON);
  public Equip armor_equip = new(Equip.Part.ARMOR);
  public Equip driver_equip = new(Equip.Part.DRIVER);
  public List<Equip> accessory_equips = new() { new(Equip.Part.ACCESSORY),new(Equip.Part.ACCESSORY)};
  // owned items
  public Dictionary<string, int> items = new();
  public Dictionary<string,int>equips = new();
  public HashSet<string>skills = new();
  [System.Serializable]
  public struct ImportItem{
    public ItemType type;
    public string name;
    public int num;
  }
  [System.Serializable]
  public struct DropItemInfo{
    public GameObject item;
    public float drop_rate;
  }
  public List<ImportItem> import_drop_items;
  public List<DropItemInfo> drop_items;
  public int drop_exp = 0;
  // creature in same group will no hit each other. player is 0, enemy is 1.
  public int group = 0; 
  public Vector3 birth_position = new();
  public Quaternion birth_rotation = new();
  public bool is_in_guard_area = false;
  public bool is_active = false;
  protected HashSet<Creature>candidate_target = new();
  protected List<HashSet<Creature>>attackable_targets = new();
  public Creature hate_target;
  // come back to birth position if lost hate target for a period of time
  protected float lost_hate_target_time = 0.0f;
  protected const float lost_hate_target_waiting_time = 1.0f;
  // Because limitations of arts asset some sprite is no top down view
  public enum SpriteType
  {
    SIDE = 1,
    TOP = 2
  }
  public SpriteType sprite_type = SpriteType.SIDE;
  // start toward in scene
  public Toward4 default_toward = Toward4.RIGHT;
  // part(son, component) of this object
  protected Animator animator;
  protected HashSet<int>hitted_attack_box_num =new();
  public List<AttackArea> attack_areas;
  public GameObject bodywrap;
  public GameObject body;
  public GameObject weaponwrap;
  public GameObject weapon;
  public GameObject canvas;
  public Gauge life_gauge;
  public Gauge magic_gauge;
  public BuffGauge buff_gauge;
  public Gauge stun_gauge;
  public GameObject instanced_prefabs;
  public List<Pair<int,SpriteRenderer>>sprite_renders;
  public List<Pair<int,SpriteRenderer>>addition_sprite_renders;
  // optional action list
  public List<string> act_list_ori = new();
  // optional action list will can be changed when act
  public List<string> act_list = new();
  public GameObject damage_text_anime;
  public Vector2 cur_direction = new(1,0);
  public Vector2 cur_directionx = new(1,0);
  public Vector2 cur_directiony = new(0,-1);
  public Vector2 target_direction = new(0,0);
  // where I want to go, as vector
  public Vector2 move_vector = new(0, 0);
  // where I really go, as step in this frame
  public Vector2 move_step = new(0, 0);
  // is a one time creature, ect. boss or elite or mechanisms of the map
  public bool one_time = false;
  // delete them if this creature dead, ect. door of boss room
  public List<GameObject> related_objs;
  // remember player input for a short time
  public Item next_item;
  public const float next_item_release_delay = 0.3f;
  public float next_item_release_time = 0.0f;
  public string next_action;
  public Magic next_magic;
  // overlay other colliders
  public List<Collider2D> colliders_tmp = new();
  public List<Collider2D> colliders = new();
  // creature in different level should no affect each other
  public int cur_background_level = 0;
  public string cur_sorting_layer_name = "Default";
  virtual public void Awake()
  {
    UpdateStatus();
    Recover();
    birth_position = transform.position;
    birth_rotation = transform.rotation;
    animator = GetComponent<Animator>();
    max_stun_time = default_stun_time*1.5f;
    move_eps = Time.fixedDeltaTime*speed;
    attackable_targets.Clear();
    for(int i=0;i<attack_areas.Count;++i){
      attack_areas[i].owner = this;
      attack_areas[i].id = i;
      attackable_targets.Add(new());
    }
    UpdateSortingLayer();
    if(transform.parent!=null){
      cur_background_level = transform.parent.GetComponent<Background>().level;
    }
    next_free_move_time = Time.fixedTime+Random.value*free_move_delay;
  }
  public void UpdateStatus(){
    life_max = life_ori + weapon_equip.life + armor_equip.life + driver_equip.life;
    magic_max = magic_ori + weapon_equip.magic + armor_equip.magic + driver_equip.magic;
    tough_max = tough_ori + weapon_equip.tough + armor_equip.tough + driver_equip.tough;
    atk = atk_ori + weapon_equip.atk + armor_equip.atk + driver_equip.atk;
    def = def_ori + weapon_equip.def + armor_equip.def + driver_equip.def;
    mat = mat_ori + weapon_equip.mat + armor_equip.mat + driver_equip.mat;
    mdf = mdf_ori + weapon_equip.mdf + armor_equip.mdf + driver_equip.mdf;
    tat = tat_ori + weapon_equip.tat + armor_equip.tat + driver_equip.tat;
    foreach(Equip equip in accessory_equips){
      life_max += equip.life;
      magic_max += equip.magic;
      tough_max += equip.tough;
      atk += equip.atk;
      def += equip.def;
      mat += equip.mat;
      mdf += equip.mdf;
      tat += equip.tat;
    }
    float life_rate = 1.0f;
    float magic_rate = 1.0f;
    float tough_rate = 1.0f;
    atk_rate = 1.0f;
    def_rate = 1.0f;
    mat_rate = 1.0f;
    mdf_rate = 1.0f;
    tat_rate = 1.0f;
    foreach(Buff buff in buffs){
      life_max += buff.life;
      magic_max += buff.magic;
      tough_max += buff.tough;
      atk += buff.atk;
      def += buff.def;
      mat += buff.mat;
      mdf += buff.mdf;
      tat += buff.tat;
      life_rate *= buff.life_rate;
      magic_rate *= buff.magic_rate;
      tough_rate *= buff.tough_rate;
      atk_rate *= buff.atk_rate;
      def_rate *= buff.def_rate;
      mat_rate *= buff.mat_rate;
      mdf_rate *= buff.mdf_rate;
      tat_rate *= buff.tat_rate;
    }
    life_max = (int)(life_max * life_rate);
    magic_max = (int)(magic_max * magic_rate);
    tough_max = (int)(tough_max * tough_rate);
    life = Mathf.Min(life, life_max);
    magic = Mathf.Min(magic, magic_max);
    tough = Mathf.Min(tough, tough_max);
    UpdateGauge();
    UpdateSortingLayer();
  }
  public void UpdateGauge(){
    if(life_gauge!=null){
      life_gauge.UpdateGauge(life, life_max);
    }
    if(magic_gauge!=null){
      magic_gauge.UpdateGauge(magic, magic_max);
    }
    if(buff_gauge!=null){
      buff_gauge.UpdateGauge();
    }
    if(stun_gauge!=null){
      if(is_stun){
        stun_gauge.gameObject.SetActive(true);
        stun_gauge.UpdateGauge(remain_stun_time,max_stun_time);
      } else {
        stun_gauge.gameObject.SetActive(false);
      }
    }
  }
  public virtual void Recover(){
    if(!is_alive){
      return;
    }
    tough = tough_max;
    life = life_max;
    magic = magic_max;
    UpdateGauge();
    canvas.SetActive(false);
  }
  public void Reset(){
    is_alive = true;
    buffs.Clear();
    UpdateStatus();
    Recover();
    transform.SetPositionAndRotation(birth_position, birth_rotation);
  }

  virtual public void Update()
  {
    if(!is_alive){
      return;
    }
  }
  virtual public void FixedUpdate(){
    if(!is_alive){
      return;
    }
    if(is_stun){
      remain_stun_time -= (1+Mathf.Max(0,Time.fixedTime-start_stun_time-default_stun_time)*stun_abate_ratio)*Time.fixedDeltaTime;
      if(remain_stun_time <= 0.0f){
        is_stun = false;
        foreach(Pair<int, SpriteRenderer> iter in sprite_renders){
          iter.se.color = Color.white;
        }
        tough = tough_max;
        tough_recover_time = 0.0f;
        if(lost_hate_target_time!=0.0f){
          lost_hate_target_time = Time.fixedTime;
        }
        color_recover_time = 0.0f;
        stun_protect_end_time = Time.fixedTime+stun_protect_duration;
        animator.SetBool("Stun",false);
      }
      UpdateGauge();
    } else if(color_recover_time!=0.0f&&color_recover_time<=Time.fixedTime){
      foreach(Pair<int, SpriteRenderer> iter in sprite_renders){
        iter.se.color = Color.white;
      }
      color_recover_time = 0.0f;
    }
    if(is_in_knock_out){
      if(knock_out_end_time<=Time.fixedTime){
        ExitKnockOut();
      }
    } else if(is_in_knock_out_backswing){
      if(knock_out_end_backswing_time<=Time.fixedTime){
        ExitKnockOutBackswing();
      }
    }
    if(is_in_knock_out){
      move_step+=Time.fixedDeltaTime*knock_out_speed*knock_out_direction;
    } else if(is_in_knock_out_backswing){
    } else if(is_stun){
    } else {
      if(tough != tough_max&&tough_recover_time<=Time.fixedTime){
        tough = tough_max;
        tough_recover_time = 0.0f;
      }
      if(lost_hate_target_time!=0.0f){
        if(lost_hate_target_time+lost_hate_target_waiting_time<=Time.fixedTime){
          transform.SetPositionAndRotation(birth_position, birth_rotation);
          lost_hate_target_time = 0.0f;
        }
        float a = lost_hate_target_time!=0.0f?1- Mathf.Min(1,(Time.fixedTime-lost_hate_target_time)/lost_hate_target_waiting_time):1;
        foreach(Pair<int, SpriteRenderer> iter in sprite_renders){
          Color color = iter.se.color;
          color.a = a;
          iter.se.color = color;
        }
      } else if(is_active){
        AutoAction();
      }
    }
    colliders = new List<Collider2D>(colliders_tmp);
    colliders_tmp.Clear();
    MoveStepTo(move_step);
    move_step = Vector2.zero;
  }
  virtual public void AutoAction(){
    if(!is_alive){
      return;
    }
    UpdateHateTarget();
    if(hate_target!=null){
      UpdateTargetDirection();
    }
  }
  public virtual void AutoActionInternal(){
    print("Empty AutoActionInternal");
  }
  public virtual void UpdateTargetDirection(){
    Vector2 direction = hate_target.body.transform.position-body.transform.position;
    if(Mathf.Abs(direction.x)>move_eps||Mathf.Abs(direction.y)>move_eps){
      direction.Normalize();
      target_direction = direction;
    } else {
      target_direction = Vector2.zero;
    }
  }
  public virtual void UpdateToward(float max_angle){
    if(max_angle<=0){
      return;
    }
    if(target_direction != Vector2.zero){
      Vector2 direction;
      if(max_angle<179.99f){
        float angle = Vector2.SignedAngle(cur_direction,target_direction);
        if(Mathf.Abs(angle)>max_angle){
          direction = (Quaternion.Euler(0,0,(angle>0?1:-1)*max_angle)*cur_direction).normalized;
        } else{
          direction = target_direction;
        }
      } else {
        direction = target_direction;
      }
      UpdateToward(direction);
    }
  }
  public void UpdateToward(Vector2 direction){
    if(direction!=Vector2.zero){
      cur_direction = direction;
      if(direction.x!=0){
        cur_directionx.x = direction.x;
      }
      cur_directiony.y = direction.y;
      if(sprite_type == SpriteType.SIDE){
        UpdateSpriteToward();
      } else if(sprite_type == SpriteType.TOP){
        if(default_toward!=Toward4.UNDEFINE){
          var from = default_toward switch
          {
            Toward4.LEFT => Vector2.left,
            Toward4.RIGHT => Vector2.right,
            Toward4.UP => Vector2.up,
            Toward4.DOWN => Vector2.down,
            _ => Vector2.zero,
          };
          bodywrap.transform.rotation=Quaternion.Euler(0,0,Vector2.SignedAngle(from,cur_direction));
        }
      }
    }
  }
  public virtual void UpdateSpriteToward(){
    if(cur_direction.x>0){
      bodywrap.transform.rotation = default_toward==Toward4.LEFT?new Quaternion(0,1,0,0):new Quaternion(0,0,0,0);
    } else if(cur_direction.x<0){
      bodywrap.transform.rotation = default_toward==Toward4.LEFT?new Quaternion(0,0,0,0):new Quaternion(0,1,0,0);
    }
  }
  public virtual void UpdateMoveVector(){
    if(hate_target!=null){
      int id = GetMoveAttackAreaId();
      Vector3 direction = hate_target.body.transform.rotation*hate_target.body.GetComponent<Collider2D>().offset-attack_areas[id].transform.rotation*attack_areas[id].GetComponent<Collider2D>().offset;
      direction += hate_target.body.transform.position-attack_areas[id].transform.position;
      direction.z = 0;
      if(Mathf.Abs(direction.x)>move_eps||Mathf.Abs(direction.y)>move_eps){
        direction.Normalize();
        move_vector = direction;
      } else {
        move_vector = Vector3.zero;
      }
    } else {
      move_vector = target_direction;
    }
  }
  public virtual int GetMoveAttackAreaId(){
    return 0;
  }
  // if there is any key pressed, skip skipable backswing
  public void SetAction(string action_name)
  {
    ResetAction();
    next_action = action_name;
    animator.SetBool("Any", true);
    animator.SetBool(action_name, true);
  }
  public void ResetAction(){
    if(next_action!=null){
      if(next_action.Length!=0){
        animator.SetBool(next_action,false);
      }
      next_action = null;
    }
    animator.SetBool("Any",false);
    if(next_magic!=null){
      if(next_magic.triggered==false){
        Destroy(next_magic.gameObject);
      }
      next_magic = null;
    }
    next_item = null;
  }
  public void MoveStepTo(Vector3 step){
    if(step.sqrMagnitude>max_move_step*max_move_step){
      step=step.normalized*max_move_step;
    }
    step.z = 0;
    if(step!=Vector3.zero){
      // prevents being pushed out of the wall
      if(colliders.Count!=0){
        Vector2 contact_point = new();
        bool has_left = false,has_right = false,has_front = false;
        float min_angle = 90;
        Vector2 center_point = body.transform.position+body.transform.rotation*body.GetComponent<Collider2D>().offset;
        foreach(Collider2D col in colliders){
          if(col!=null&&col.gameObject!=null&&!col.gameObject.IsDestroyed()){
            Vector2 point = col.ClosestPoint(center_point);
            Vector2 diff = point-center_point;
            float angle = Vector2.SignedAngle(step,diff);
            float unsigned_angle = Mathf.Abs(angle);
            if(angle == 0){
              return;
            } else if(unsigned_angle>=90){
              continue;
            } else if(angle<0){
              has_left = true;
            } else if(angle>0){
              has_right = true;
            }
            has_front = true;
            if(has_left&&has_right){
              return;
            }
            if(unsigned_angle<min_angle){
              min_angle = unsigned_angle;
              contact_point = diff;
            }
          }
        }
        if(has_front){
          step -= (Vector3)(Vector2.Dot(step,contact_point)*contact_point/contact_point.sqrMagnitude);
        }
      }
      transform.position+=step;
      UpdateSortingLayer();
    }
  }
  public void UpdateSortingLayer(string sorting_layer_name){
    int sorting_order = (int)(body.transform.position.y * -10.0f);
    foreach(Pair<int, SpriteRenderer> iter in sprite_renders){
      iter.se.sortingOrder = sorting_order+iter.fs;
      iter.se.sortingLayerName = sorting_layer_name;
    }
    List<Pair<int, SpriteRenderer>> deleted_addition_sprite_renders = new();
    foreach(Pair<int, SpriteRenderer> iter in addition_sprite_renders){
      if(iter.se == null||iter.se.gameObject.IsDestroyed()){
        deleted_addition_sprite_renders.Add(iter);
        continue;
      }
      iter.se.sortingOrder = sorting_order+iter.fs;
      iter.se.sortingLayerName = sorting_layer_name;
    }
    foreach(Pair<int, SpriteRenderer> iter in deleted_addition_sprite_renders){
      addition_sprite_renders.Remove(iter);
    }
    foreach(Buff buff in buffs){
      if(buff.TryGetComponent<SpriteRenderer>(out var buff_render)){
        buff_render.sortingOrder = sorting_order+1;
        buff_render.sortingLayerName = sorting_layer_name;
      }
    }
    if(canvas.TryGetComponent<Canvas>(out var canvas_canvas)){
      canvas_canvas.sortingOrder = sorting_order+2;
      canvas_canvas.sortingLayerName = sorting_layer_name;
    }
    cur_sorting_layer_name = sorting_layer_name;
  }
  public void UpdateSortingLayer(){
    int sorting_order = (int)(body.transform.position.y * -10.0f);
    foreach(Pair<int, SpriteRenderer> iter in sprite_renders){
      iter.se.sortingOrder = sorting_order+iter.fs;
    }
    List<Pair<int, SpriteRenderer>> deleted_addition_sprite_renders = new();
    foreach(Pair<int, SpriteRenderer> iter in addition_sprite_renders){
      if(iter.se == null||iter.se.gameObject.IsDestroyed()){
        deleted_addition_sprite_renders.Add(iter);
        continue;
      }
      iter.se.sortingOrder = sorting_order+iter.fs;
    }
    foreach(Pair<int, SpriteRenderer> iter in deleted_addition_sprite_renders){
      addition_sprite_renders.Remove(iter);
    }
    foreach(Buff buff in buffs){
      if(buff.TryGetComponent<SpriteRenderer>(out var buff_render)){
        buff_render.sortingOrder = sorting_order+1;
      }
    }
    if(canvas.TryGetComponent<Canvas>(out var canvas_canvas)){
      canvas_canvas.sortingOrder = sorting_order+2;
    }
  }
  void UpdateHateTarget(){
    if(is_in_guard_area == false){
      if(hate_target!=null){
        hate_target= null;
        lost_hate_target_time = Time.fixedTime;
      }
      return;
    }
    SortedSet<KeyValuePair<float,Creature>>sorted_hate = new();
    foreach(Creature creature in candidate_target){
      sorted_hate.Add(new(HateFunction(creature),creature));
    }
    if(sorted_hate.Count!=0){
      hate_target = sorted_hate.Max.Value;
    } else {
      if(hate_target!=null){
        hate_target = null;
        lost_hate_target_time = Time.fixedTime;
      }
    }
  }
  float HateFunction(Creature creature){
    float hate;
    if(creature.CompareTag("Player")){
      hate = 100;
    } else {
      hate = 1;
    }
    return hate + UnityEngine.Random.Range(0,1);;
  }
  public void Stun(){
    if(!is_alive){
      return;
    }
    is_stun = true;
    remain_stun_time = (1-tough/tough_max)*default_stun_time;
    remain_stun_time = Mathf.Min(remain_stun_time,max_stun_time);
    start_stun_time = Time.fixedTime;
    ResetAction();
    animator.Play("Stiff");
    animator.SetBool("Stun",true);
    color_recover_time = 0.0f;
    Color color = Color.yellow;
    foreach(Pair<int, SpriteRenderer> iter in sprite_renders){
      iter.se.color = color;
    }
    if (stun_effect != null){
      // todo: show stun effect
    }
  }
  public void KnockOut(float knock_out_speed, float knock_out_time, Vector2 knock_out_direction){
    if(!is_alive){
      return;
    }
    if(!can_knock_out){
      return;
    }
    is_in_knock_out = true;
    knock_out_end_time = Time.fixedTime + knock_out_time;
    ResetAction();
    this.knock_out_speed = knock_out_speed;
    this.knock_out_direction = knock_out_direction;
    animator.Play("Stiff");
    animator.SetBool("KnockOut",true);
  }
  public void Drop(){
    if(!is_alive){
      return;
    }
    foreach(DropItemInfo drop_item in drop_items){
      if(UnityEngine.Random.Range(0,1)<drop_item.drop_rate){
        Instantiate(drop_item.item,body.transform.position,transform.rotation,transform.parent);
      }
    }
    foreach(ImportItem item in import_drop_items){
      ItemManager.Instance().Add(item.type,item.name,item.num);
      GlobalMessageBox.Instance().AddMessage("Get "+item.name +" x "+item.num);
    }
  }
  public void Dead(string dead_anime_name){
    if(!is_alive){
      return;
    }
    Drop();
    GameManager.Instance().player.ExpUp(drop_exp, level);
    if(related_objs!=null){
      foreach(GameObject obj in related_objs){
        GameManager.Instance().AddDeletedObj(obj.transform.GetPath());
        Destroy(obj);
      }
    }
    if(one_time){
      GameManager.Instance().AddDeletedObj(transform.GetPath());
    }
    foreach(Pair<int,SpriteRenderer> iter in sprite_renders){
      iter.se.color = Color.black/2+Color.white/2;
    }
    animator.Play(dead_anime_name);
    is_alive = false;
  }
  virtual public void Dead(){
    Dead("Dead");
  }
  public void OnHit(AttackBox attack_box, Collider2D col){
    if(!is_alive){
      return;
    }
    float now = Time.fixedTime;
    int damage = (int)(Mathf.Max(0, (attack_box.atk - def)*attack_box.atk_rate*def_rate) + Mathf.Max(0, (attack_box.mat - mdf)*attack_box.mat_rate*mdf_rate));
    life -= damage;
    Vector3 pos = col.ClosestPoint(body.transform.position);
    pos+=new Vector3(UnityEngine.Random.value,UnityEngine.Random.value,UnityEngine.Random.value)*0.5f;
    pos+=new Vector3(0,1);
    if(damage_text_anime!=null){
      GameObject new_text = Instantiate(damage_text_anime,pos,new Quaternion());
      foreach(Transform child in new_text.transform){
        if(child!=null&&child.name == "DamageText"){
          child.GetComponent<Text>().text = damage.ToString();
          break;
        }
      }
    }
    Vector2 hit_direction = body.transform.position-col.transform.position;
    hit_direction.Normalize();
    move_step+=attack_box.knock_back_rate*knock_back_distance*hit_direction;
    if(attack_box.is_knock_out){
      var knock_out_direction = attack_box.knock_out_type switch
      {
        KnockOutType.FORWARD => attack_box.knock_out_direction,
        _ => (Vector2)hit_direction,
      };
      KnockOut(attack_box.knock_out_speed,attack_box.knock_out_time,knock_out_direction);
    }
    if(life<=0){
      Dead();
    } else {
      int tough_damage = (int)(attack_box.tat*attack_box.tat_rate*tdf_rate);
      if(!is_stun){
        Color color = Color.red/2+Color.white/2;
        foreach(Pair<int, SpriteRenderer> iter in sprite_renders){
          iter.se.color = color;
        }
        color_recover_time = now+color_recover_delay;
        if(stun_protect_end_time<=Time.fixedTime){
          tough -= tough_damage;
          tough_recover_time = now + tough_recover_delay;
          if(tough<=0){
            Stun();
          }
        }
      } else {
        remain_stun_time += tough_damage*1.0f/tough_max*default_stun_time*stun_accumulate_rate;
        remain_stun_time = Mathf.Min(remain_stun_time,max_stun_time);
      }
    }
    canvas.SetActive(true);
    UpdateGauge();
  }
  public void ExitKnockOut(){
    is_in_knock_out = false;
    is_in_knock_out_backswing = true;
    knock_out_end_time = 0.0f;
    knock_out_end_backswing_time = Time.fixedTime + knock_out_end_backswing_delay;
  }
  public void ExitKnockOutBackswing(){
    is_in_knock_out_backswing = false;
    knock_out_end_backswing_time = 0.0f;
    if(knock_out_effect!=null){
    }
    animator.SetBool("KnockOut",false);
  }
  public virtual void OnCollisionStay2D(Collision2D col){
    if(!is_alive){
      return;
    }
    Collider2D other_collider;
    if(col.collider.transform.parent!=null&&col.collider.transform.parent.parent!=null&&col.collider.transform.parent.parent==gameObject){
      other_collider = col.otherCollider;
    } else {
      other_collider = col.collider;
    }
    if(other_collider.gameObject.CompareTag("Enemy")||other_collider.gameObject.CompareTag("Player")){
      if(other_collider.transform.parent.parent.GetComponent<Creature>().cur_sorting_layer_name==cur_sorting_layer_name){
        colliders_tmp.Add(other_collider);
      }
    } else if(other_collider.gameObject.CompareTag("BackgroundEdge")||other_collider.gameObject.CompareTag("BackgroundEdgeEnemy")){
      if(other_collider.transform.parent.GetComponent<Background>().level==cur_background_level){
        colliders_tmp.Add(other_collider);
      }
    }
  }
  public virtual void OnCollisionEnter2D(Collision2D col){
    if(!is_alive){
      return;
    }
  }
  public void OnTriggerExit2D(Collider2D col){
    if(!is_alive){
      return;
    }
    if(col.name== "ActiveArea"){
      is_active = false;
      Reset();
    }
  }
  public void OnTriggerEnter2D(Collider2D col){
    if(!is_alive){
      return;
    }
    if(col.name== "ActiveArea"){
      is_active = true;
    }
    if(!is_active){
      return;
    }
    if(col.CompareTag("AttackBox")){
      AttackBox attack_box = col.gameObject.GetComponent<AttackBox>();
      if(attack_box.group != group){
        if(!hitted_attack_box_num.Contains(attack_box.attack_box_seq)){
          hitted_attack_box_num.Add(attack_box.attack_box_seq);
          OnHit(attack_box, col);
        }
      }
    }
  }
  public void AddCandidateTarget(Creature creature) {
    candidate_target.Add(creature);
  }
  public void RemoveCandidateTarget(Creature creature) {
    candidate_target.Remove(creature);
  }
  public void AddAttackableTarget(Creature creature, AttackArea attack_area, int id) {
    if(attack_areas[id]!=attack_area){
      return;
    }
    attackable_targets[id].Add(creature);
  }
  public void RemoveAttackableTarget(Creature creature, AttackArea attack_area, int id) {
    if(attack_areas[id]!=attack_area){
      return;
    }
    attackable_targets[id].Remove(creature);
  }
  public void OnEnterGuardArea(){
    if(is_in_guard_area==true){
      return;
    }
    is_in_guard_area = true;
    next_free_move_time = Time.fixedTime+Random.value*free_move_delay;
  }
  public void OnExitGuardArea(){
    if(is_in_guard_area==false){
      return;
    }
    is_in_guard_area = false;
  }
  public void OnDisable()
  {
    OnExitGuardArea();
  }
}

// save load and menu control and some share data
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// struct of save data
[Serializable]
public class GlobalData{ 
  // data from player
  [Serializable]
  public class PlayerInfo{
    public float knock_back_distance;
    public int level;
    public int cur_exp;
    public int life;
    public int magic;
    public bool is_alive;
    public bool is_stun;
    public int group;
    public Equip weapon_equip;
    public Equip armor_equip;
    public Equip driver_equip;
    public List<Equip> accessory_equips;
    [Serializable] 
    public class Vector2{
      public float x;
      public float y;
      public void Store(UnityEngine.Vector2 v){
        x = v.x;
        y = v.y;
      }
      public  UnityEngine.Vector2 Load(){
        return new UnityEngine.Vector2(x,y);
      }
    }
    [Serializable] 
    public class Vector3{
      public float x;
      public float y;
      public float z;
      public void Store(UnityEngine.Vector3 v){
        x = v.x;
        y = v.y;
        z = v.z;
      }
      public  UnityEngine.Vector3 Load(){
        return new UnityEngine.Vector3(x,y,z);
      }
    }
    public Vector3 cur_position;
    [Serializable] 
    public struct Quaternion{
      public float x;
      public float y;
      public float z;
      public float w;
      public void Store(UnityEngine.Quaternion v){
        x = v.x;
        y = v.y;
        z = v.z;
        w = v.w;
      }
      public UnityEngine.Quaternion Load(){
        return new UnityEngine.Quaternion(x,y,z,w);
      }
    }
    public Quaternion cur_rotation;
    public Vector2 cur_direction;
    public Vector2 cur_directionx;
    public Vector2 cur_directiony;
    public Vector3 birth_position;
    public Quaternion birth_rotation;
    public List<string> birth_background_path;
  }
  public PlayerInfo player_info;
  // data about owned items 
  public Dictionary<string, int> items = new();
  public Dictionary<string,int>equips = new();
  public HashSet<string>skills = new();
  public string scene_name;
  // setting about item skill and equip
  public List<string> item_slots;
  public List<string> skill_slots;
  public List<string> equip_slots;
  // string of current area, where is player in
  public List<string> background_path;
  // some object is onetime, ect. boss. they wiil be deleted after loading scene
  public List<List<string>> deleted_objs;
  // switch object(ect. a door) have some status, save them
  public Dictionary<List<string>, int> switch_status;
  public void StoreSceneName(string v){
    scene_name = v;
  }
  public void LoadSceneName(ref string v){
    v = scene_name;
  }
  public void StoreItemsInfo(Player v){
    items = v.items;
    equips = v.equips;
    skills = v.skills;
  }
  public void LoadItemsInfo(Player v){
    v.items = items;
    v.equips = equips;
    v.skills = skills;
  }
  public void StoreItemSlots(List<ItemSlot> v){
    item_slots = new();
    foreach(ItemSlot slot in v){
      item_slots.Add(slot.cur_item_name);
    }
  }
  public void LoadItemSlots(List<ItemSlot> v){
    foreach(ItemSlot slot in v){
      slot.LoadSlotContent();
    }
  }
  public void StoreSkillSlots(List<SkillSlot> v){
    skill_slots = new();
    foreach(SkillSlot slot in v){
      skill_slots.Add(slot.cur_item_name);
    }
  }
  public void LoadSkillSlots(List<SkillSlot> v){
    foreach(SkillSlot slot in v){
      slot.LoadSlotContent();
    }
  }
  public void StoreEquipSlots(List<EquipSlot> v){
    equip_slots = new();
    foreach(EquipSlot slot in v){
      equip_slots.Add(slot.cur_item_name);
    }
  }
  public void LoadEquipSlots(List<EquipSlot> v){
    foreach(EquipSlot slot in v){
      slot.LoadSlotContent();
    }
  }

  public void StoreBackground(BackgroundControl v){
    background_path = v.cur_background.transform.GetPath();
  }
  public void LoadBackground(BackgroundControl v, GameObject grid, Background default_background){
    Background[] backgrounds = grid.GetComponentsInChildren<Background>();
    foreach(Background background in backgrounds){
      background.gameObject.SetActive(false);
    }
    if(background_path!=null){
      string str="";
      foreach(string s in background_path){
        str+=s+" ";
      }
      GameObject cur_obj = grid.transform.FindGameObject(background_path,0);
      if(cur_obj!=null&&cur_obj.TryGetComponent<Background>(out var cur_bg)){
        v.UpdateBackground(cur_bg);
      } else {
        Debug.Log("Error");
        v.UpdateBackground(default_background);
      }
#if UNITY_EDITOR
      deleted_objs = new();
#else
      if(deleted_objs!=null){
        foreach(List<string>delete_obj in deleted_objs){
          GameObject obj = grid.transform.FindGameObject(delete_obj,0);
          if(obj!=null){
            GameObject.Destroy(obj);
          }
        }
      }
#endif
    } else {
      v.UpdateBackground(default_background);
    }
  }
  public void LoadSwitchStatus(GameObject grid){
    if(switch_status==null){
      switch_status = new();
      return;
    }
    foreach(KeyValuePair<List<string>,int>iter in switch_status){
      GameObject obj = grid.transform.FindGameObject(iter.Key,0);
      if(obj!=null){
        obj.GetComponent<Switch>().SetStatus(iter.Value);
      } else {
        Debug.Log("Error");
      }
    }
  }

  public void StorePlayerInfo(Player v){
    player_info = new()
    {
      knock_back_distance = v.knock_back_distance,
      level = v.level,
      cur_exp = v.cur_exp,
      life = v.life,
      magic = v.magic,
      is_alive = v.is_alive,
      is_stun = v.is_stun,
      group = v.group,
      weapon_equip = v.weapon_equip,
      armor_equip = v.armor_equip,
      driver_equip = v.driver_equip,
      accessory_equips = v.accessory_equips,
      cur_position = new(),
      cur_rotation = new(),
      cur_direction = new(),
      cur_directionx = new(),
      cur_directiony = new(),
      birth_position = new(),
      birth_rotation = new(),
      birth_background_path = v.birth_background_path
    };
    player_info.cur_position.Store(v.transform.position);
    player_info.cur_rotation.Store(v.transform.rotation);
    player_info.cur_direction.Store(v.cur_direction);
    player_info.cur_directionx.Store(v.cur_directionx);
    player_info.cur_directiony.Store(v.cur_directiony);
    player_info.birth_position.Store(v.birth_position);
    player_info.birth_rotation.Store(v.birth_rotation);
  }
  public void LoadPlayerInfo(Player v){
    if(player_info!=null){
      v.knock_back_distance = player_info.knock_back_distance;
      v.level = player_info.level;
      v.cur_exp = player_info.cur_exp;
      v.life = player_info.life;
      v.magic = player_info.magic;
      v.is_alive = player_info.is_alive;
      v.is_stun = player_info.is_stun;
      v.group = player_info.group;
      v.weapon_equip = player_info.weapon_equip;
      v.armor_equip = player_info.armor_equip;
      v.driver_equip = player_info.driver_equip;
      v.accessory_equips = player_info.accessory_equips;
      v.transform.SetPositionAndRotation(player_info.cur_position.Load(), player_info.cur_rotation.Load());
      v.cur_direction = player_info.cur_direction.Load();
      v.cur_directionx = player_info.cur_directionx.Load();
      v.cur_directiony = player_info.cur_directiony.Load();
      v.birth_position = player_info.birth_position.Load();
      v.birth_rotation = player_info.birth_rotation.Load();
      v.birth_background_path = player_info.birth_background_path;
      v.UpdateSpriteToward();
      v.UpdateSortingLayer();
      v.UpdateLevel();
    } else {
      v.UpdateLevel();
      v.Recover();
    }
  }
}

public class GameManager : MonoBehaviour
{
  // some gameobject pointer to object in scene
  public GameObject main_canvas;
  public GameObject main_ui;
  public GameObject main_button;
  public GameObject item_panel;
  public GameObject skill_panel;
  public GameObject equip_panel;
  public List<ItemSlot> item_slots;
  public List<SkillSlot> skill_slots;
  public List<EquipSlot> equip_slots;
  public Player player;
  public BackgroundControl background_control;
  public Background default_background;
  public GameObject grid;
  // inputs
  InputAction pause_action;
  private bool pause_key = false;
  private float pause_key_release_time;
  InputAction item_action;
  private bool item_key = false;
  private float item_key_release_time;
  InputAction skill_action;
  private bool skill_key = false;
  private float skill_key_release_time;
  InputAction equip_action;
  private bool equip_key = false;
  private float equip_key_release_time;
  InputAction cancel_action;
  private bool cancel_key = false;
  private float cancel_key_release_time;
  static private string data_path;
  static public string save_file_name = "savedata";
  static public GlobalData global_data = new();
  public string default_scene_name;
  public bool is_in_boss_room = false;
  // make instance for calling from other object
  static bool inited = false;
  static GameManager instance;
  // for test
  static bool skip_load_data = false;
  void Awake(){
    if(!inited){
      Application.targetFrameRate = 60;
      data_path = Application.persistentDataPath;
      ReadSavedata();
      inited= true;
    }
    instance = this;
  }
  public static GameManager Instance(){
    return instance;
  }
  public void Restart(){
    skip_load_data = true;
    SceneManager.LoadScene(default_scene_name);
  }
  public void RestartScene(){
    skip_load_data = true;
    string scene_name = "";
    global_data.LoadSceneName(ref scene_name);
    SceneManager.LoadScene(scene_name);
  }
  public void RestartScene(string scene_name){
    skip_load_data = true;
    SceneManager.LoadScene(scene_name);
  }
  void Start()
  {
    pause_action = InputSystem.actions.FindAction("Pause");
    item_action = InputSystem.actions.FindAction("Item");
    skill_action = InputSystem.actions.FindAction("Skill");
    equip_action = InputSystem.actions.FindAction("Equip");
    cancel_action = InputSystem.actions.FindAction("Cancel");
    if(!skip_load_data){
      // for test
      PostLoad();
    } else {
      skip_load_data = false;
    }
  }
  public void QuitGame()
  {
    Application.Quit();
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
  }
  public void AddDeletedObj(List<string>deleted_obj){
    if(global_data.deleted_objs==null){
      global_data.deleted_objs = new();
    }
    global_data.deleted_objs.Add(deleted_obj);
  }
  public void AddSwitchStatus(List<string>path,int status){
    global_data.switch_status[path] = status;
  }
  public void StoreGlobalData(){
    global_data.StoreSceneName(SceneManager.GetActiveScene().name);
    global_data.StorePlayerInfo(player);
    global_data.StoreItemsInfo(player);
    global_data.StoreItemSlots(item_slots);
    global_data.StoreSkillSlots(skill_slots);
    global_data.StoreEquipSlots(equip_slots);
    global_data.StoreBackground(background_control);
  }
  public void Save()
  {
    float cur_timescale = Time.timeScale;
    Time.timeScale = 0;
    StoreGlobalData();
    BinaryFormatter bd = new();
    FileStream save_file = File.Create(data_path+"/" + save_file_name);
    bd.Serialize(save_file,global_data);
    save_file.Close();
    Time.timeScale = cur_timescale;
  }
  public void ReadSavedata(){
    BinaryFormatter bf = new(); 
    FileStream save_file = File.Open(data_path+"/" + save_file_name,FileMode.OpenOrCreate);
    if(save_file.Length!=0){
      global_data = bf.Deserialize(save_file) as GlobalData;
    }
    save_file.Close();
  }
  public void LoadGlobalData(){
    string scene_name = "";
    global_data.LoadSceneName(ref scene_name);
    SceneManager.LoadScene(scene_name);
    Time.timeScale = 1;
  }
  public void Load()
  {
    Time.timeScale = 0;
    if(File.Exists(data_path +"/" + save_file_name)){
      ReadSavedata();
      LoadGlobalData();
    } else {
      print("NoSaveData "+data_path +"/" + save_file_name);
    }
    Time.timeScale = 1;
  }
  // load after load scene
  public void PostLoad()
  {
    if (global_data != null)
    {
      global_data.LoadBackground(background_control, grid, default_background);
      global_data.LoadSwitchStatus(grid);
      global_data.LoadPlayerInfo(player);
      global_data.LoadItemsInfo(player);
      global_data.LoadItemSlots(item_slots);
      global_data.LoadSkillSlots(skill_slots);
      global_data.LoadEquipSlots(equip_slots);
    }
  }
  // when dead and recover
  public void Reload()
  {
    StoreGlobalData();
    LoadGlobalData();
  }
  public void DeadReload(){
    StoreGlobalData();
    global_data.background_path = player.birth_background_path;
    LoadGlobalData();
  }
  public void Reload(float time){
    StoreGlobalData();
    Invoke(new string("LoadGlobalData"), time);
  }

  void Update()
  {
    float now = Time.time;
    if(!is_in_boss_room){
      if (pause_action.WasPressedThisFrame()){
        pause_key = true;
        pause_key_release_time = now + Time.fixedDeltaTime;
      } else if (pause_key_release_time < now){
        pause_key = false;
      }
      if (item_action.WasPressedThisFrame()){
        item_key = true;
        item_key_release_time = now + Time.fixedDeltaTime;
      } else if (item_key_release_time < now){
        item_key = false;
      }
      if (skill_action.WasPressedThisFrame()){
        skill_key = true;
        skill_key_release_time = now + Time.fixedDeltaTime;
      } else if (skill_key_release_time < now){
        skill_key = false;
      }
      if (equip_action.WasPressedThisFrame()){
        equip_key = true;
        equip_key_release_time = now + Time.fixedDeltaTime;
      } else if (equip_key_release_time < now){
        equip_key = false;
      }
    }
    if (cancel_action.WasPressedThisFrame()){
      cancel_key = true;
      cancel_key_release_time = now + Time.fixedDeltaTime;
    } else if (cancel_key_release_time < now){
      cancel_key = false;
    }
    if (pause_key) {
      if(main_button.activeSelf == false){
        PauseGame();
        ShowMainButton();
      } else {
        ResumeGame();
      }
      pause_key =false;
    }
    if(item_key){
      if(item_panel.activeSelf == false){
        PauseGame();
        ShowItemPanel();
      } else {
        ResumeGame();
      }
      item_key = false;
    }
    if(skill_key){
      if(skill_panel.activeSelf == false){
        PauseGame();
        ShowSkillPanel();
      } else {
        ResumeGame();
      }
      skill_key = false;
    }
    if(equip_key){
      if(equip_panel.activeSelf == false){
        PauseGame();
        ShowEquipPanel();
      } else {
        ResumeGame();
      }
      equip_key = false;
    }
    if(cancel_key){
      ResumeGame();
      cancel_key = false;
    }
  }
  public void FixedUpdate()
  {
  }
  public void PauseGame()
  {
    Time.timeScale = 0;
    main_ui.SetActive(true);
  }
  public void ResumeGame()
  {
    main_button.SetActive(false);
    item_panel.SetActive(false);
    skill_panel.SetActive(false);
    equip_panel.SetActive(false);
    main_ui.SetActive(false);
    Time.timeScale = 1;
  }
  public void ShowMainButton(){
    main_button.SetActive(true);
    item_panel.SetActive(false);
    skill_panel.SetActive(false);
    equip_panel.SetActive(false);
  }
  public void ShowItemPanel(){
    item_panel.SetActive(true);
    item_panel.GetComponent<ItemPanel>().UpdateItems();
    main_button.SetActive(false);
    skill_panel.SetActive(false);
    equip_panel.SetActive(false);
  }
  public void ShowSkillPanel(){
    skill_panel.SetActive(true);
    skill_panel.GetComponent<SkillPanel>().UpdateItems();
    main_button.SetActive(false);
    item_panel.SetActive(false);
    equip_panel.SetActive(false);
  }
  public void ShowEquipPanel(){
    equip_panel.SetActive(true);
    equip_panel.GetComponent<EquipPanel>().UpdateItems();
    main_button.SetActive(false);
    item_panel.SetActive(false);
    skill_panel.SetActive(false);
  }
}

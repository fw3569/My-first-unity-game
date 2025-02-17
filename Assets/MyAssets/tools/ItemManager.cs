using UnityEngine;
using System.Collections.Generic;

public enum ItemType{
  ITEM,
  SKILL,
  EQUIP
}

[System.Serializable]
public class Equip {
  public string name = "";
  public int atk;
  public int def;
  public int mat;
  public int mdf;
  public int tat;
  public int tough;
  public int life;
  public int magic;
  public enum Part{
    WEAPON,
    ARMOR,
    DRIVER,
    ACCESSORY
  };
  public Part part;
  public Equip(Part part){
    this.part = part;
  }
  public Equip(string name, int atk, int mat, int def, int mdf, int tat, int tough, int life, int magic, Part part){
#if UNITY_EDITOR
    if(part == Part.WEAPON){
      Debug.Assert(def == 0);
      Debug.Assert(mdf == 0);
      Debug.Assert(tough == 0);
      Debug.Assert(life == 0);
      Debug.Assert(magic == 0);
    } else if(part == Part.ARMOR){
      Debug.Assert(atk == 0);
      Debug.Assert(mat == 0);
      Debug.Assert(tat == 0);
      Debug.Assert(life == 0);
      Debug.Assert(magic == 0);
    } else if(part == Part.DRIVER){
      Debug.Assert(atk == 0);
      Debug.Assert(def == 0);
      Debug.Assert(tat == 0);
      Debug.Assert(tough == 0);
      Debug.Assert(life == 0);
    } else if(part == Part.ACCESSORY){
      Debug.Assert(tat == 0);
      Debug.Assert(tough == 0);
    }
#endif
    this.name = name;
    this.atk = atk;
    this.def = def;
    this.mat = mat;
    this.mdf = mdf;
    this.tat = tat;
    this.tough = tough;
    this.life = life;
    this.magic = magic;
    this.part = part;
  }
}

[System.Serializable]
public class ItemData{
  public string name;
  // resource path
  public string path;
  public ItemType type;
  public Equip equip = null;
}

public class ItemManager : MonoBehaviour
{
  // make me easier set item data in unity editor
  public List<ItemData> item_datas;
  static public Dictionary<string, ItemData> item_data_dir = new();
  static public Dictionary<string, ItemIcon> icons = new();
  static private bool inited = false;
  public Player player;
  // object loaded from prefab
  static public Dictionary<string, Item> items = new();
  public GameObject instanced_prefabs;
  public static ItemManager instance = null;
  // need to update them after owned item change
  public ItemPanel item_panel;
  public List<ItemBox> item_boxes = new();
  public SkillPanel skill_panel;
  public List<SkillBox> skill_boxes = new();
  public EquipPanel equip_panel;
  public List<EquipBox> equip_boxes = new();
  public void Awake()
  {
    instance = this;
    if(!inited){
      foreach(ItemData data in item_datas){
        item_data_dir[data.name] = data;
        ItemIcon icon_obj = Resources.Load<ItemIcon>(data.path);
        if(icon_obj!=null){
          ItemIcon icon = icon_obj.GetComponent<ItemIcon>();
          icon.type = data.type;
          if(data.type == ItemType.EQUIP){
            icon.equip = data.equip;
            icon.equip.name = data.name;
          }
          if(icon.item!=null){
            Item new_item = Instantiate(icon.item,instanced_prefabs.transform);
            new_item.name = data.name;
            items[data.name] = new_item;
          }
          icons[data.name] = icon_obj;
        }
      }
      inited = true;
    } else {
      foreach(KeyValuePair<string,ItemIcon> icon_iter in icons){
        string name = icon_iter.Key;
        ItemIcon icon = icon_iter.Value;
        if(icon.item!=null){
          Item new_item = Instantiate(icon.item,instanced_prefabs.transform);
          new_item.name = name;
          items[name] = new_item;
        }
      }
    }
  }

  public void Start(){}
  public void Update(){}
  static public ItemManager Instance(){
    return instance;
  }
  public void UpdateItem(){
    item_panel.UpdateItems();
    foreach(ItemBox box in item_boxes){
      box.UpdateItem();
    }
  }
  public void AddItem(string name,int num = 1){
    if(player.items.ContainsKey(name)){
      player.items[name]+=num;
    } else {
      player.items.Add(name,num);
    }
    UpdateItem();
  }
  public void UpdateSkill(){
    skill_panel.UpdateItems();
    foreach(SkillBox box in skill_boxes){
      box.UpdateItem();
    }
  }
  public void AddSkill(string name,int num = 1){
    player.skills.Add(name);
    UpdateSkill();
  }
  public void UpdateEquip(){
    equip_panel.UpdateItems();
    foreach(EquipBox box in equip_boxes){
      box.UpdateItem();
    }
  }
  public void AddEquip(string name,int num = 1){
    if(player.equips.ContainsKey(name)){
      player.equips[name]+=num;
    } else {
      player.equips.Add(name,num);
    }
    UpdateEquip();
  }
  public void Add(ItemType type,string name, int num = 1){
    if(type == ItemType.ITEM){
      AddItem(name, num);
    } else if(type == ItemType.SKILL){
      AddSkill(name, num);
    } else if(type == ItemType.EQUIP){
      AddEquip(name, num);
    }
  }
}

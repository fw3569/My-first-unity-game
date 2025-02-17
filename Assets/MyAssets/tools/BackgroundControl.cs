// update which area is player in, process the moving between area
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class BackgroundControl : MonoBehaviour
{
  public Light2D sun_light;
  // player use self light to see around if in cave
  public Light2D self_light;
  // areas player overlay
  private List<Background> backgrounds = new();
  // areas player stand on
  private List<Background> stand_backgrounds = new();
  public Background cur_background;
  // which player moving to
  public Background moving_background;
  // player eye, decide where can be seen
  public GameObject eye;
  // close light when teleportation or ladder, first frame shadding no work
  public bool skip = false;
  protected static BackgroundControl instance;
  public void Awake(){
    instance = this;
  }
  public static BackgroundControl Instance(){
    return instance;
  }

  public void FixedUpdate()
  {
    if(cur_background!=null){
    TilemapCollider2D cur_tile = cur_background.GetComponent<TilemapCollider2D>();
    if(!cur_tile.OverlapPoint(eye.transform.position)){
      Background nearest_background=cur_background;
      float dis_cur = Vector2.Distance(cur_tile.ClosestPoint(eye.transform.position),eye.transform.position);
      float dis_min = dis_cur;
      foreach(Background bg in stand_backgrounds){
        float dis = Vector2.Distance(bg.GetComponent<TilemapCollider2D>().ClosestPoint(eye.transform.position),eye.transform.position);
        if(dis<dis_min){
          dis_min = dis;
          nearest_background = bg;
        }
      }
      if(nearest_background!=cur_background){
        UpdateBackground(nearest_background);
      }
    }
    if(skip){
      // close light when teleportation or ladder, first frame shadding no work
      skip = false;
    } else {
      sun_light.intensity = cur_background.sun_intensity;
      eye.GetComponent<Light2D>().intensity = cur_background.eye_intensity;
      self_light.intensity = cur_background.light_intensity;
      self_light.falloffIntensity = cur_background.light_falloff;
      self_light.pointLightOuterRadius = cur_background.light_outer;
      self_light.pointLightInnerRadius = cur_background.light_inner;
    }
    }
  }
  public void UpdateBackground(Background to_bg){
    if(to_bg==null){
      GetComponent<Player>().Dead("FallDownDead");
      print("Error");
      return;
    }
    if(to_bg!=cur_background){
      if(to_bg.gameObject.activeSelf==false){
        sun_light.intensity = 0;
        eye.GetComponent<Light2D>().intensity = 0;
        self_light.intensity = 0;
      }
      Background form_bg = cur_background;
      cur_background = to_bg;
      GetComponent<Player>().cur_background_level = to_bg.level;
      if(form_bg!=null){
        form_bg.OnExitBackground(to_bg);
      }
      to_bg.OnEnterBackground(form_bg);
      GameManager.Instance().is_in_boss_room = to_bg.type == Background.BackgroundType.BOSS;
      skip = true;
      List<Background>delete_lsit = new();
      foreach(Background bg in stand_backgrounds){
        if(bg==to_bg){
          continue;
        }
        if(!to_bg.adjs.Contains(bg)){
          delete_lsit.Add(bg);
        }
      }
      foreach(Background bg in delete_lsit){
        stand_backgrounds.Remove(bg);
      }
    }
  }
  public void UpdateBackground(){
    UpdateBackground(moving_background);
    moving_background = null;
    GetComponent<Player>().body.GetComponent<Collider2D>().enabled = true;
  }

  public void OnTriggerEnter2D(Collider2D col){
    if(cur_background==null){
      return;
    }
    if(col.CompareTag("Background")){
      if(col.TryGetComponent<Background>(out var bg)){
        backgrounds.Add(bg);
        if(cur_background==bg||cur_background.adjs.Contains(bg)){
          stand_backgrounds.Add(bg);
        }
      }
    }
  }
  public void OnTriggerExit2D(Collider2D col){
    if(col.CompareTag("Background")){
      if(col.TryGetComponent<Background>(out var from_bg)){
        backgrounds.Remove(from_bg);
        stand_backgrounds.Remove(from_bg);
        if(from_bg==cur_background){
          if(backgrounds.Count == 0){
            GetComponent<Player>().Dead("FallDownDead");
          } else if(stand_backgrounds.Count!=0){
            UpdateBackground(stand_backgrounds.First());
          } else {
            int max_level = int.MinValue;
            Background max_bg = null;
            foreach(Background bg in backgrounds){
              if(bg.level > max_level){
                max_level = bg.level;
                max_bg = bg;
              }
            }
            moving_background = max_bg;
            GetComponent<Player>().body.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Animator>().Play("FallDown");
          }
        }
      }
    }
  }
}

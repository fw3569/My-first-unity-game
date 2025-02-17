using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

// tags declaration
// Background           ground
// BackgroundEdge       wall for player
// BackgroundEdgeEx     variable wall, like door of boss room
// BackgroundEdgeEnemy  wall for enemy
// EnvMask              environment effect, like solid color obj in high level
// EnvMaskEx            variable environment effect
public class Background : MonoBehaviour
{
  public int level = 0;
  public float sun_intensity = 0.0f;
  public float eye_intensity = 1.0f;
  public float light_intensity = 1.0f;
  public float light_inner = 2.0f;
  public float light_outer = 20.0f;
  public float light_falloff = 0.85f;
  // display these areas when player is in this area
  public List<Background> active_areas = new();
  // close collider in these areas. etc. in different level
  public List<Background> hide_collider_areas = new();
  // adjacent areas, which can go from this area 
  public List<Background> adjs = new();
  // creatures can hate player even though they are not in same area with player
  public List<Creature> far_hate_creatures = new();
  [System.Serializable]
  public struct ShadowCasterList
  {
    public Background adj_area;
    public List<Background> hide_areas;
    public List<GameObject> list;
  }
  // extra shadow casters, only valid before player pass them
  public List<ShadowCasterList> ex_shadow_casters = new();
  public enum BackgroundType
  {
    COMMON,
    BOSS
  }
  // I don't allow player open menu and use one-time item in boss room
  // I don't like player use item, change equip, change skill, S/L in boss room
  public BackgroundType type = BackgroundType.COMMON;
  void Awake()
  {
    if (active_areas == null || active_areas.Count == 0)
    {
      print("Warn, " + transform.GetPath().ToStringOverride() + " missing active_areas");
    }
  }
  public void OnEnterBackground(Background from_bg)
  {
    if (from_bg == this)
    {
      print("Error");
      return;
    }
    foreach (Background area in active_areas)
    {
      area.gameObject.SetActive(true);
      foreach (ShadowCasterList objs in area.ex_shadow_casters)
      {
        if (objs.adj_area != this && !adjs.Contains(objs.adj_area))
        {
          foreach (GameObject obj in objs.list)
          {
            if (obj != null && !obj.IsDestroyed())
            {
              obj.SetActive(true);
            }
          }
        }
      }
    }
    gameObject.SetActive(true);
    foreach (Background area in hide_collider_areas)
    {
      foreach (Transform child in area.transform)
      {
        if (child.CompareTag("Background"))
        {
        }
        else if (child.CompareTag("BackgroundEdge"))
        {
          if (child.TryGetComponent<TilemapCollider2D>(out var tile_col))
          {
            tile_col.enabled = false;
          }
          if (child.TryGetComponent<ShadowCaster2D>(out var shacas))
          {
            child.gameObject.SetActive(false);
          }
        }
        else if (child.CompareTag("BackgroundEdgeEx"))
        {
        }
        else if (child.CompareTag("BackgroundEdgeEnemy"))
        {
          if (child.TryGetComponent<Collider2D>(out var col))
          {
            col.gameObject.layer = 11;
          }
        }
        else
        {
          if (child.TryGetComponent<Creature>(out var creature))
          {
            if (creature.body.TryGetComponent<Collider2D>(out var creature_col))
            {
              creature_col.gameObject.layer = 12;
            }
            creature.UpdateSortingLayer(area.level.ToString());
          }
          if (child.TryGetComponent<SpriteRenderer>(out var sprite_renderer))
          {
            sprite_renderer.sortingLayerName = area.level.ToString();
          }
          if (child.TryGetComponent<Collider2D>(out var col))
          {
            col.enabled = false;
          }
          if (child.TryGetComponent<ShadowCaster2D>(out var shacas))
          {
            shacas.enabled = false;
          }
        }
      }
    }
    foreach (Transform child in transform)
    {
      if (child.CompareTag("Background"))
      {
      }
      else if (child.CompareTag("BackgroundEdge"))
      {
      }
      else if (child.CompareTag("BackgroundEdgeEx"))
      {
        child.gameObject.SetActive(true);
      }
      else
      {
        if (child.TryGetComponent<Creature>(out var creature))
        {
          creature.OnEnterGuardArea();
        }
      }
    }
    foreach (Creature creature in far_hate_creatures)
    {
      if (creature != null && !creature.gameObject.IsDestroyed())
      {
        creature.OnEnterGuardArea();
      }
    }
    if (from_bg != null)
    {
      foreach (ShadowCasterList objs in ex_shadow_casters)
      {
        if (objs.adj_area == from_bg)
        {
          foreach (GameObject obj in objs.list)
          {
            if (obj != null && !obj.IsDestroyed())
            {
              GameManager.Instance().AddDeletedObj(obj.transform.GetPath());
              Destroy(obj);
            }
          }
          ex_shadow_casters.Remove(objs);
          break;
        }
      }
    }
    foreach (Transform child in transform.parent)
    {
      if (child.CompareTag("EnvMaskEx"))
      {
        child.gameObject.SetActive(true);
      }
    }
  }
  public void OnExitBackground(Background to_bg)
  {
    if (to_bg == null || to_bg == this)
    {
      print("Error");
      return;
    }
    foreach (Background area in hide_collider_areas)
    {
      if (!to_bg.hide_collider_areas.Contains(area))
      {
        foreach (Transform child in area.transform)
        {
          if (child.CompareTag("Background"))
          {
          }
          else if (child.CompareTag("BackgroundEdge"))
          {
            if (child.TryGetComponent<TilemapCollider2D>(out var tile_col))
            {
              tile_col.enabled = true;
            }
            if (child.TryGetComponent<ShadowCaster2D>(out var shacas))
            {
              child.gameObject.SetActive(true);
            }
          }
          else if (child.CompareTag("BackgroundEdgeEx"))
          {
          }
          else if (child.CompareTag("BackgroundEdgeEnemy"))
          {
            if (child.TryGetComponent<Collider2D>(out var col))
            {
              col.gameObject.layer = 8;
            }
          }
          else
          {
            if (child.TryGetComponent<Creature>(out var creature))
            {
              if (creature.body.TryGetComponent<Collider2D>(out var creature_col))
              {
                creature_col.gameObject.layer = 6;
              }
              creature.UpdateSortingLayer("Default");
            }
            if (child.TryGetComponent<SpriteRenderer>(out var sprite_renderer))
            {
              sprite_renderer.sortingLayerName = "Default";
            }
            if (child.TryGetComponent<Collider2D>(out var col))
            {
              col.enabled = true;
            }
            if (child.TryGetComponent<ShadowCaster2D>(out var shacas))
            {
              shacas.enabled = true;
            }
          }
        }
      }
    }
    foreach (Transform child in transform)
    {
      if (child.CompareTag("Background"))
      {
      }
      else if (child.CompareTag("BackgroundEdge"))
      {
      }
      else if (child.CompareTag("BackgroundEdgeEx"))
      {
        child.gameObject.SetActive(false);
      }
      else
      {
        if (child.TryGetComponent<Creature>(out var creature))
        {
          if (!to_bg.far_hate_creatures.Contains(creature))
          {
            creature.OnExitGuardArea();
          }
        }
      }
    }
    foreach (Creature creature in far_hate_creatures)
    {
      if (creature != null && !creature.gameObject.IsDestroyed())
      {
        if (creature.transform.parent == to_bg.transform || to_bg.far_hate_creatures.Contains(creature))
        {
          continue;
        }
        creature.OnExitGuardArea();
      }
    }
    foreach (ShadowCasterList objs in ex_shadow_casters)
    {
      if (objs.adj_area == to_bg)
      {
        foreach (GameObject obj in objs.list)
        {
          if (obj != null && !obj.IsDestroyed())
          {
            GameManager.Instance().AddDeletedObj(obj.transform.GetPath());
            Destroy(obj);
          }
        }
        ex_shadow_casters.Remove(objs);
        break;
      }
    }
    if (transform.parent != to_bg.transform.parent)
    {
      foreach (Transform child in transform.parent)
      {
        if (child.CompareTag("EnvMaskEx"))
        {
          child.gameObject.SetActive(false);
        }
      }
    }
    foreach (Background area in active_areas)
    {
      foreach (ShadowCasterList objs in area.ex_shadow_casters)
      {
        foreach (GameObject obj in objs.list)
        {
          if (obj != null && !obj.IsDestroyed())
          {
            obj.SetActive(false);
          }
        }
      }
      if (area == to_bg || to_bg.active_areas.Contains(area))
      {
        continue;
      }
      area.gameObject.SetActive(false);
    }
    if (!to_bg.active_areas.Contains(this))
    {
      gameObject.SetActive(false);
    }
  }
}

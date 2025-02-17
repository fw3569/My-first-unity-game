using UnityEngine;

public class Ladder : InteractivityObj
{
  public Background target_area;
  public Vector2 target_position;
  public Toward4 target_toward;
  public override string Interactive(GameObject from){
    BackgroundControl.Instance().UpdateBackground(target_area);
    Player player = GameManager.Instance().player;
    player.transform.position = target_position;
    switch (target_toward){
      case Toward4.LEFT:
        player.cur_direction = new Vector2(-1,0);
        player.cur_directionx = new Vector2(-1,0);
        player.cur_directiony = new Vector2(0,-1);
        break;
      case Toward4.RIGHT:
        player.cur_direction = new Vector2(1,0);
        player.cur_directionx = new Vector2(1,0);
        player.cur_directiony = new Vector2(0,-1);
        break;
      case Toward4.UP:
        player.cur_direction = new Vector2(0,1);
        player.cur_directionx = new Vector2(1,0);
        player.cur_directiony = new Vector2(0,1);
        break;
      case Toward4.DOWN:
        player.cur_direction = new Vector2(0,-1);
        player.cur_directionx = new Vector2(1,0);
        player.cur_directiony = new Vector2(0,-1);
        break;
      case Toward4.UNDEFINE:
      default:
        player.cur_direction = new Vector2(1,0);
        player.cur_directionx = new Vector2(1,0);
        player.cur_directiony = new Vector2(0,-1);
        break;
    }
    player.UpdateSpriteToward();
    return "";
  }
  public override void ResetInteractive(GameObject from){
  }
}
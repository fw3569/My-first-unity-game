using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour
{
  public Player player;
  public Text level_text;
  public Text exp_text;
  public Text life_text;
  public Text magic_text;
  public Text tough_text;
  public Text atk_text;
  public Text def_text;
  public Text mat_text;
  public Text mdf_text;
  public Text tat_text;

  void Update()
  {
    level_text.text = player.level.ToString();
    exp_text.text = player.cur_exp+" / "+(player.level<Player.max_level?player.next_level_exp:"-");
    life_text.text = player.life+" / "+player.life_max;
    magic_text.text = player.magic+" / "+player.magic_max;
    tough_text.text = player.tough+" / "+player.tough_max;
    atk_text.text = player.atk.ToString();
    def_text.text = player.def.ToString();
    mat_text.text = player.mat.ToString();
    mdf_text.text = player.mdf.ToString();
    tat_text.text = player.tat.ToString();
  }
}

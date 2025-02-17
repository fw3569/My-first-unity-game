using UnityEngine;

public class RecoverPoint : InteractivityObj
{
  public AudioSource audio_source;
  public AudioClip audio_clip;
  private static float audio_play_time = 2;
  public override string Interactive(GameObject from){
    GameManager.Instance().player.Recover();
    if(audio_source!=null){
      GameObject new_obj = Instantiate(audio_source.gameObject,from.transform);
      AudioSource new_audio = new_obj.GetComponent<AudioSource>();
      new_audio.clip = audio_clip;
      new_audio.Play();
      Destroy(new_obj,audio_clip.length);
      audio_play_time = audio_clip.length;
    }
    GameManager.Instance().player.ResetBirthPosition();
    GameManager.Instance().Reload(audio_play_time);
    GameManager.Instance().player.receive_input = false;
    return "";
  }
  public override void ResetInteractive(GameObject from){
  }
}

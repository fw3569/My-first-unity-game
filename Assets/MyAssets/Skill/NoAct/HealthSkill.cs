using UnityEngine;

public class HealthSkill : Skill
{
  public int health = 0;
  public AudioSource audio_source;
  public AudioClip audio_clip;
  public override bool Trigger(Vector2 pos,Vector2 dir, Quaternion rot,Creature source,GameObject target){
    source.life=Mathf.Min(source.life_max,source.life+health);
    source.UpdateGauge();
    if(audio_source!=null){
      GameObject new_obj = Instantiate(audio_source.gameObject,source.transform);
      AudioSource new_audio = new_obj.GetComponent<AudioSource>();
      new_audio.clip = audio_clip;
      new_audio.Play();
      Destroy(new_obj,new_audio.clip.length);
    }
    return true;
  }
}

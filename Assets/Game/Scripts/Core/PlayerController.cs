using System;
using Game.Core;
using MimicSpace;
using UnityEngine;
using System.Collections;
using Game.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [Header("MimicStats Script Reference")]
    [SerializeField] MimicStats _mimicStats = null;
    [Header("Timer Script Reference")]
    [SerializeField] Timer timer = null;
    [SerializeField] float respawnTimer = 2f;
    private Movement _movement;
    
  
   
    Vector3 originPos;
    bool isDead = false;

    private void Awake()
    {
        if(GetComponent<MimicStats>() != null ) _mimicStats = GetComponent<MimicStats>();
        if(GetComponent<Movement>() != null ) _movement = GetComponent<Movement>();
       
        

        if (timer == null)
        {
            timer = FindObjectOfType<Timer>();
            timer.timeRunOut += TimeRunOut;
        }

        originPos = transform.position;
    }


    void TimeRunOut(bool isTimeRunOut)
    {
        if (isTimeRunOut)
        {
            _movement.enabled = false;
        }
    }
    private void OnDestroy()
    {
        timer.timeRunOut -= TimeRunOut;
    }

   
    public void Dead(){
        isDead = true;
        //minus points
            
        StartCoroutine(playerDead());
    }
    
    public bool GetIsDead(){
        return isDead;
    }

    IEnumerator playerDead(){
        Fader fader = FindObjectOfType<Fader>();
        _movement.enabled = false;
        yield return fader.FadeOut(1f);
        Teleport(originPos);
        yield return fader.FadeIn(1f);
        _movement.enabled = true;
        yield return new WaitForSeconds(respawnTimer);
        isDead = false;
    }

    private void Teleport(Vector3 pos)
    {
        this.transform.position = pos;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    GameObject MainCamera;
    [SerializeField]
    GameObject Player;
    [SerializeField]
    GameObject Timer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !anim.IsInTransition(0))
        {
            MainCamera.SetActive(true);
            gameObject.SetActive(false);
            Player.GetComponent<PlayerController>().enabled = true;
            Timer.SetActive(true);
        }
    }
}

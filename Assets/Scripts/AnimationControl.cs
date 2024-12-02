using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public void endShoot()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("isShooting", false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

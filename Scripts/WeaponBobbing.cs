using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    public Animator weaponBob;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            weaponBob.SetTrigger("bob");
            weaponBob.ResetTrigger("stop");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            weaponBob.SetTrigger("stop");
            weaponBob.ResetTrigger("bob");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            weaponBob.SetTrigger("bob");
            weaponBob.ResetTrigger("stop");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            weaponBob.SetTrigger("stop");
            weaponBob.ResetTrigger("bob");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            weaponBob.SetTrigger("bob");
            weaponBob.ResetTrigger("stop");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            weaponBob.SetTrigger("stop");
            weaponBob.ResetTrigger("bob");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            weaponBob.SetTrigger("bob");
            weaponBob.ResetTrigger("stop");
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            weaponBob.SetTrigger("stop");
            weaponBob.ResetTrigger("bob");
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
//using BzKovSoft.ObjectSlicerSamples;

public class PlayerRigid : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isGothit = false,isGotShot=false,isGotSlamed,isdiamond;
    float v = 1;
    float i = 1;
    float j = 1;
    bool isfevermode = false;
    private FlexRun_GameManager flexRunGM;
    void Start()
    {
        isGothit = false;
        isGotShot = false;
        isGotSlamed = false;
        isdiamond = false;
        isfevermode = false;
        flexRunGM = FlexRun_GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (flexRunGM.FeverBar.fillAmount>0&&flexRunGM.isfeverModeStarted==true)
        {
            isfevermode = true;
            print("******___________FeverModeON______________*******");
        }
        if (flexRunGM.isfeverModeStarted == false&& flexRunGM.FeverBar.fillAmount == 0)
        {
            isfevermode = false;
            print("******___________FeverModOFF______________*******");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (flexRunGM.SingleSwipeControl != null)
            {
                if (flexRunGM.SingleSwipeControl.isFeverMOde == false)
                {
                    print("Hit___Obstacle");
                    isGothit = true;
                    other.GetComponent<Collider>().isTrigger = true;
                }
                else if (flexRunGM.SingleSwipeControl.isFeverMOde == true)
                {
                    Instantiate(flexRunGM.LaserDeactivateeffect, other.transform.position, other.transform.rotation);
                    other.gameObject.SetActive(false);
                }
            }
            if (flexRunGM.TwoSwipeControl != null)
            {
                if (flexRunGM.TwoSwipeControl.isFeverMOde == false)
                {
                    print("Hit___Obstacle");
                    isGothit = true;
                    other.GetComponent<Collider>().isTrigger = true;
                }
                else if (flexRunGM.TwoSwipeControl.isFeverMOde == true)
                {
                    Instantiate(flexRunGM.LaserDeactivateeffect, other.transform.position, other.transform.rotation);
                    other.gameObject.SetActive(false);
                }
            }
            //if (isfevermode == false)
            //{
            //    print("Hit___Obstacle");
            //    isGothit = true;
            //    other.GetComponent<Collider>().isTrigger = true;
            //}
            //else if (isfevermode == true)
            //{
            //    Instantiate(flexRunGM.LaserDeactivateeffect, other.transform.position, other.transform.rotation);
            //    other.gameObject.SetActive(false);
            //}
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            if (flexRunGM.SingleSwipeControl != null)
            {
                if (flexRunGM.SingleSwipeControl.isFeverMOde == false)
                {
                    print("Hit___Bullet");
                    isGotShot = true;
                    Destroy(other.gameObject);
                }
                else if (flexRunGM.SingleSwipeControl.isFeverMOde == true)
                {
                    Instantiate(flexRunGM.BulletDestroyEffect, other.transform.position, other.transform.rotation);
                    other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            if (flexRunGM.TwoSwipeControl != null)
            {
                if (flexRunGM.TwoSwipeControl.isFeverMOde == false)
                {
                    print("Hit___Bullet");
                    isGotShot = true;
                    Destroy(other.gameObject);
                }
                else if (flexRunGM.TwoSwipeControl.isFeverMOde == true)
                {
                    Instantiate(flexRunGM.BulletDestroyEffect, other.transform.position, other.transform.rotation);
                    other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            //if (isfevermode == false)
            //{
            //    print("Hit___Bullet");
            //    isGotShot = true;
            //    Destroy(other.gameObject);
            //}
            //else if (isfevermode == true)
            //{
            //    print("BulletEscape");
            //    other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //}
        }
        else if (other.gameObject.CompareTag("Enemey"))
        {
            if (flexRunGM.SingleSwipeControl != null)
            {
                if (flexRunGM.SingleSwipeControl.isFeverMOde==false)
                {
                    print("Hit___Enemey");
                    isGotSlamed = true;
                    other.GetComponent<Collider>().isTrigger = false;
                }
                else if (flexRunGM.SingleSwipeControl.isFeverMOde == true)
                {
                    print("SlamEscape");
                    other.gameObject.GetComponent<EnemeyRigid>().isEnemeyTouched = true;
                    other.GetComponent<Collider>().enabled = false;
                    if (i == 1)
                    {
                        Instantiate(flexRunGM.SmokeSlamEFfect, flexRunGM.ExternalEffectPOs.transform.position, flexRunGM.ExternalEffectPOs.transform.rotation);
                        if (AudioManager.instance != null)
                        {
                            AudioManager.instance.Play("punch");
                        }
                        i += 1;
                    }
                }
            }
           
            
            if (flexRunGM.TwoSwipeControl != null)
            {
                if (flexRunGM.TwoSwipeControl.isFeverMOde == false)
                {
                    print("Hit___Enemey");
                    isGotSlamed = true;
                    other.GetComponent<Collider>().isTrigger = false;
                }
                else if (flexRunGM.TwoSwipeControl.isFeverMOde == true)
                {
                    print("SlamEscape");
                    other.gameObject.GetComponent<EnemeyRigid>().isEnemeyTouched = true;
                    other.GetComponent<Collider>().enabled = false;
                    if (j == 1)
                    {
                        Instantiate(flexRunGM.SmokeSlamEFfect,flexRunGM.ExternalEffectPOs.transform.position, flexRunGM.ExternalEffectPOs.transform.rotation);
                        if (AudioManager.instance != null)
                        {
                            AudioManager.instance.Play("punch");
                        }
                        j +=1;
                    }
                }

            }

        }
        else if (other.gameObject.CompareTag("Car"))
        {
            if (flexRunGM.SingleSwipeControl != null)
            {
                if (flexRunGM.SingleSwipeControl.isFeverMOde == false)
                {
                    print("Hit___Car");
                    isGotSlamed = true;
                    other.GetComponent<Collider>().isTrigger = false;
                }
                else if (flexRunGM.SingleSwipeControl.isFeverMOde == true)
                {
                    print("carEscape");
                    other.gameObject.GetComponent<car>().DestroyCar();
                    //other.gameObject.GetComponent<EnemeyRigid>().isEnemeyTouched = true;
                    other.GetComponent<Collider>().enabled = false;
                    if (i == 1)
                    {
                        Instantiate(flexRunGM.SmokeSlamEFfect, flexRunGM.ExternalEffectPOs.transform.position, flexRunGM.ExternalEffectPOs.transform.rotation);
                        if (AudioManager.instance != null)
                        {
                            AudioManager.instance.Play("punch");
                        }
                        i += 1;
                    }
                }
            }
            else if (flexRunGM.TwoSwipeControl != null)
            {
                if (flexRunGM.TwoSwipeControl.isFeverMOde == false)
                {
                    print("Hit___Car");
                    isGotSlamed = true;
                    other.GetComponent<Collider>().isTrigger = false;
                }
                else if (flexRunGM.TwoSwipeControl.isFeverMOde == true)
                {
                    print("CarEscape");
                    other.gameObject.GetComponent<car>().DestroyCar();
                    //other.gameObject.GetComponent<EnemeyRigid>().isEnemeyTouched = true;
                    other.GetComponent<Collider>().enabled = false;
                    if (j == 1)
                    {
                        Instantiate(flexRunGM.SmokeSlamEFfect, flexRunGM.ExternalEffectPOs.transform.position, flexRunGM.ExternalEffectPOs.transform.rotation);
                        if (AudioManager.instance != null)
                        {
                            AudioManager.instance.Play("punch");
                        }
                        j += 1;
                    }
                }
            }
           
        }
        else if (other.gameObject.CompareTag("Diamond"))
        {
            print("CollectDiamond");
            isdiamond = true;
            Instantiate(flexRunGM.DiamondEffect, other.transform.position, other.transform.rotation);
            other.gameObject.SetActive(false);
        }
    }
}

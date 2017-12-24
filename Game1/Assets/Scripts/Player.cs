using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private bool _canMove { get { return !IsMoving && !GlobalController.IsShowingDialog && !GlobalController.IsShowingPauseMenu; } }
    public List<Item> ItemList { get; private set; }

    public new void Start()
    {
        base.Start();


        ItemList = new List<Item>(3);
    }


    #region Helper Functions

    public override void updateAnimation()
    {
        base.updateAnimation();

        if (IsMoving)
        {
            gameObject.GetComponent<AudioSource>().Play();
            // Todo waiting for real animation
            switch (Direction)
            {
                case Utility.Direction.Left:
                    //gameObject.transform.rotation = Quaternion.AngleAxis(-90, Vector3.back);
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    gameObject.GetComponent<Animator>().SetInteger("Dir", 1);
                    gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
                    break;
                case Utility.Direction.Right:
                    //gameObject.transform.rotation = Quaternion.AngleAxis (90, Vector3.back);
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    gameObject.GetComponent<Animator>().SetInteger("Dir", 1);
                    gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
                    break;
                case Utility.Direction.Up:
                    //gameObject.transform.rotation = Quaternion.AngleAxis (0, Vector3.back);
                    gameObject.GetComponent<Animator>().SetInteger("Dir", 0);
                    gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
                    break;
                case Utility.Direction.Down:
                    //gameObject.transform.rotation = Quaternion.AngleAxis (180, Vector3.back);
                    gameObject.GetComponent<Animator>().SetInteger("Dir", 2);
                    gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
                    break;
                default:
                    break;
            }
        }
        else
        {
            gameObject.GetComponent<AudioSource>().Stop();
            gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
        }
    }
    
    #endregion //Helper Functions
}

using UnityEngine;

public class AI : Character
{
    public new bool CanMove { get { return !IsMoving; }}

    public override void updateAnimation()
    {
        base.updateAnimation();

        if (IsMoving)
        {
            gameObject.GetComponents<AudioSource>()[0].Play();
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
            gameObject.GetComponents<AudioSource>()[0].Stop();
            gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
        }
    }
}

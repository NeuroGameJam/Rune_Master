using UnityEngine;

namespace Assets.Scripts
{
    public class CheckpointScript : MonoBehaviour
    {
        private bool _passed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "PlayerHand" || _passed || !other.transform.root.GetComponent<AuthorityScript>().HasAuthority())
            {
                return;
            }
            GameObject.Find("NetworkManager").GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
            if (transform.parent.GetChild(0) == transform)
            {
                if (other.GetComponent<HandScript>())
                {
                    other.GetComponent<HandScript>().UpdateHandScore();
                }
            }
            if (transform.parent.GetChild(transform.parent.childCount - 1) == transform)
            {
                other.gameObject.transform.root.GetComponent<AuthorityScript>().CmdSomething();
                if (other.GetComponent<HandScript>())
                {
                    other.GetComponent<HandScript>().End();
                    other.GetComponent<HandScript>().UpdateGlobalVar();
                }
            }
            transform.parent.GetComponent<HandTestScript>().PassCheckpoint(transform.GetSiblingIndex());
            _passed = true;
        }
    }
}

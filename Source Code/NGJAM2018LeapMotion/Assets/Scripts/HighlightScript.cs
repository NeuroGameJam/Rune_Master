using UnityEngine;

namespace Assets.Scripts
{
    public class HighlightScript : MonoBehaviour
    {
        [SerializeField]
        private bool _betOnSelf;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerHand")
            {
                transform.GetComponent<Light>().enabled=true;
                int i;
                i = _betOnSelf ? 1 : 0;
                transform.parent.GetChild(i).GetComponent<Light>().enabled = false;
                other.transform.root.GetComponent<AuthorityScript>().Choice(_betOnSelf);
            }
        }
    }
}

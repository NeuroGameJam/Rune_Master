using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class HandTestScript : NetworkBehaviour
    {
        private bool _primed = false;
        private bool[] _checkpoints;
        private bool _isOver=false;

        private MeshRenderer _myMeshRenderer;


        private void Start()
        {
            _myMeshRenderer = GetComponent<MeshRenderer>();
            _checkpoints = new bool[transform.childCount];
        }

        private void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerHand" && !_isOver)
            {
                //_myMeshRenderer.material.color = Color.green;
                _primed = true;
                if (other.GetComponent<HandScript>())
                {
                    other.GetComponent<HandScript>().StopSubtracting();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "PlayerHand" && _primed && !_isOver)
            {
                //_myMeshRenderer.material.color = Color.red;
                if (other.GetComponent<HandScript>())
                {
                    other.GetComponent<HandScript>().StartSubtracting();
                }
                other.gameObject.transform.root.GetComponent<AuthorityScript>().SetFail(true);
                _primed = false;
            }
        }

        public void PassCheckpoint(int checkpointNumber)
        {
            _checkpoints[checkpointNumber] = true;
            transform.GetChild(checkpointNumber).gameObject.SetActive(false);
            if (checkpointNumber + 1 < _checkpoints.Length)
            {
                transform.GetChild(checkpointNumber + 1).gameObject.SetActive(true);
            }
            foreach (var passed in _checkpoints)
            {
                if (!passed)
                {
                    return;
                }
            }
            _primed = false;
            _isOver = true;
        }
    }
}

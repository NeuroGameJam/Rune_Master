using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class GameManagerScript : NetworkBehaviour
    {
        [SerializeField] private GameObject[] _shapes;
        [SerializeField] private Transform[] _shapesTransform;
        private int _successes = 0;
        private int _choices = 0;
        private GameObject _currentShape;
        [SerializeField]
        private float _waitTime = 3;
        private int _currentShapeIndex = 0;
        private bool _failure;
        private bool _displayUI;
        [SerializeField]
        private GameObject _ui;
        [SerializeField]
        private GameObject _failureUi;
        [SerializeField]
        private GameObject _successUi;

        private void Start()
        {
            StartCoroutine(IPlaceShape());
        }

        [Command]
        private void CmdPlaceShape()
        {
            _currentShape = Instantiate(_shapes[_currentShapeIndex], _shapesTransform[_currentShapeIndex].position,
                _shapes[_currentShapeIndex].transform.rotation).gameObject;
            NetworkServer.Spawn(_currentShape);
        }

        private void PlaceShape()
        {
            if (!isServer)
            {
                CmdPlaceShape();
                GameObject.FindGameObjectWithTag("Player").GetComponent<AuthorityScript>().NetFunction();
            }
            else
            {
                _currentShape = Instantiate(_shapes[_currentShapeIndex], _shapesTransform[_currentShapeIndex].position,
                    _shapes[_currentShapeIndex].transform.rotation).gameObject;
                NetworkServer.Spawn(_currentShape);
                GameObject.FindGameObjectWithTag("Player").GetComponent<AuthorityScript>().NetFunction();
            }
        }

        public void GMSuccess()
        {
            _successes++;
            if (isServer && _successes >= 1)
            {
                Destroy(_currentShape);
                _currentShapeIndex++;
                GameObject.FindGameObjectWithTag("Player").GetComponent<AuthorityScript>().SetFailureState();
                if (isServer)
                {
                    RpcUpdateHandScore();
                }
                else
                {
                    CmdUpdateScore();
                }
            }
        }

        [Command]
        private void CmdUpdateScore()
        {
            RpcUpdateHandScore();
        }

        [ClientRpc]
        private void RpcUpdateHandScore()
        {
            var objs = GameObject.FindGameObjectsWithTag("PlayerHand");
            foreach (var o in objs)
            {
            }
        }

        public void GMChoice()
        {
            _choices++;
            if (_choices>=1)
            {
                StartCoroutine(IPlaceShape());
            }
        }

        private IEnumerator IPlaceShape()
        {
            yield return new WaitForSeconds(_waitTime);
            PlaceShape();
            GameObject.FindGameObjectWithTag("Player").GetComponent<AuthorityScript>().DisableUI();
        }

        public void SetChoicesNull()
        {
            _successes = 0;
            _choices = 0;
            _failure = false;

        }

        public void GMEnableUI(bool fail)
        {
            _ui.SetActive(true);
            if (fail)
            {
                _failureUi.SetActive(true);
            }
            _successUi.SetActive(true);
        }

        public void GMDisableUI()
        {
            if (isServer)
            {
                RpcDisableUIGM();
            }
            else
            {
                CmdDisableUIGM();
            }
        }

        [ClientRpc]
        private void RpcDisableUIGM()
        {
            _successUi.SetActive(false);
            _failureUi.SetActive(false);
            _ui.SetActive(false);
        }

        [Command]
        private void CmdDisableUIGM()
        {
            RpcDisableUIGM();
        }
    }
}

using UnityEngine;

namespace Assets.Scripts
{
    public class HandScript : MonoBehaviour
    {
        private int _initialScore =100;

        private float _score;

        private int _subtractSpeed = 25;

        private bool _subtracting;

        private bool _completed = false;

        private void Start()
        {
            _score = _initialScore;
        }

        private void Update()
        {
            if (!transform.root.GetComponent<AuthorityScript>().hasAuthority)
            {
                return;
            }
            if (_subtracting && !_completed)
            {
                Debug.Log("oi");
                _score -= _subtractSpeed * Time.deltaTime;
            }
        }

        public void StartSubtracting()
        {
            _subtracting = true;
        }

        public void End()
        {
            _completed = true;
            StopSubtracting();
            transform.root.GetComponent<AuthorityScript>().SetFail(false);
        }

        public void UpdateHandScore()
        {
            transform.root.GetComponent<AuthorityScript>().UpdateScore(Mathf.RoundToInt(_score));
        }

        public void StopSubtracting()
        {
            _subtracting = false;
        }

        public void ResetThisScript()
        {
            _score = _initialScore;
            _completed = false;
            _subtracting = false;
        }

        public void UpdateGlobalVar()
        {
            transform.root.GetComponent<AuthorityScript>()._scoreThisRound = _score;
            ResetThisScript();
        }
    }
}

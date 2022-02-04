using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beats
{
    public class GamePlayController : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private KeyCode _left = KeyCode.A;
        [SerializeField] private KeyCode _right = KeyCode.D;
        [SerializeField] private KeyCode _up = KeyCode.W;
        [SerializeField] private KeyCode _down = KeyCode.S;

        [Header("Inputs")]
        [Tooltip("Beats Track to play")]
        [SerializeField] private Track _track;

        /// <summary> The Current Track </summary>
        public Track track { get { return _track; } }
        
        public float secondsPerBeat { get; private set; }
        public float BeatsPerSecond { get; private set; }

        private bool _played;
        private bool _completed;

        TrackView _trackView;

        private WaitForSeconds waiAndStop;

        private static GamePlayController _instance;
        public static GamePlayController Instance {
            get {
                if (_instance == null) 
                    _instance = (GamePlayController)GameObject.FindObjectOfType(typeof(GamePlayController));
                
                return _instance;
            }
        }
        private void OnDestroy()
        {
            _instance = null;
        }
        #region MonoBehaviour Methods
        void Awake()
        {
            _instance = this;
            BeatsPerSecond = track.bpm / 60f; 
            secondsPerBeat = 60f / track.bpm;
            waiAndStop = new WaitForSeconds(BeatsPerSecond * 2);

            _trackView = FindObjectOfType<TrackView>();
            if (!_trackView) Debug.LogWarning("No TrackView Component found in current scene!");
        }

        void Start()
        {
            InvokeRepeating("NextBeat", 0f,BeatsPerSecond);
        }

        void Update()
        {
            if (_played || _completed) return;

            if (Input.GetKeyDown(_left)) PlayBeat(0);
            if (Input.GetKeyDown(_down)) PlayBeat(1);
            if (Input.GetKeyDown(_up)) PlayBeat(2);
            if (Input.GetKeyDown(_right)) PlayBeat(3);

        }
        #endregion

        #region GamePlay
        private int _current;
        public int current
        {
            get { return _current; }
            set {
                if (value != _current) {
                    _current = value;

                    if (_current == _track.beats.Count)
                    {
                        CancelInvoke("NextBeat");
                        _completed = true;

                        StartCoroutine(WaitAndStop());
                    }
                }
            }
        }

        void PlayBeat(int input) 
        {
            _played = true;

            if (_track.beats[current] == -1)
            {
                //Debug.Log(string.Format("{0} played untimely", input));
            }
            else if (_track.beats[current] == input)
            {
                Debug.Log(string.Format("{0} played right", input));
                _trackView.TriggerBeatView(current, TrackView.Trigger.Right);
            }
            else
            {
                Debug.Log(string.Format("{0} played, {1} expected", input,_track.beats[current]));
                _trackView.TriggerBeatView(current, TrackView.Trigger.Wrong);
            }
        }

        void NextBeat() {
            //Debug.Log("Tick");

            if (!_played && _track.beats[current] != -1) 
            {
                Debug.Log(string.Format("{0} missed", _track.beats[current]));
                _trackView.TriggerBeatView(current, TrackView.Trigger.Missed);
            }
            _played = false;

            current++;
        }

        private IEnumerator WaitAndStop() {
            yield return waiAndStop;
            enabled = false;
        }
        #endregion
    }
}
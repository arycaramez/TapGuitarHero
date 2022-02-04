using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Beats
{
    [CustomEditor(typeof(Track))]
    public class TrackEditor : Editor
    {
        Track track;

        Vector2 pos;
        bool _displayBeats;

        public void OnEnable()
        {
            track = (Track)target;    
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (track.beats.Count == 0) 
            {
                EditorGUILayout.HelpBox("Empty Track", MessageType.Info);
                if(GUILayout.Button("Generate Random Track", EditorStyles.miniButton))
                {
                    track.Randomize();
                }
            } 
            else 
            {
                if(GUILayout.Button("Update Random Track", EditorStyles.miniButton))
                {
                    track.Randomize();
                }

                EditorGUILayout.Space(10);
                _displayBeats = EditorGUILayout.Foldout(_displayBeats, "Display Beats");
                if (_displayBeats) {
                    EditorGUILayout.LabelField("Beat List (" + track.beats.Count + ")");
                    pos = EditorGUILayout.BeginScrollView(pos, GUILayout.Height(300));
                    for (int i = 0; i < track.beats.Count; i++)
                    {
                        track.beats[i] = EditorGUILayout.IntSlider(track.beats[i], -1, Track.inputs - 1);
                    }
                    EditorGUILayout.EndScrollView();
                }
            }

            EditorUtility.SetDirty(track);
        }
    }
}

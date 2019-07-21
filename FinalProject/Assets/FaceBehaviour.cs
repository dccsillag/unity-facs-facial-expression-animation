//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class FaceBehaviour : MonoBehaviour {
    public float speed = 500;
    public float apex = 500;
    public float intensity = 1f;

    public float talk_speed = 400;
    public bool do_talk = false;

    FaceManagerWithText fm;

    void Start() {
        fm = GetComponent<FaceManagerWithText>();

        if (do_talk)
            fm.StartSpeaking(talk_speed);

        fm.ShowExpression(FaceExpression.Anger, speed, apex, intensity);
        fm.ShowExpression(FaceExpression.Disgust, speed, apex, intensity);
        fm.ShowExpression(FaceExpression.Fear, speed, apex, intensity);
        fm.ShowExpression(FaceExpression.Happiness, speed, apex, intensity);
        fm.ShowExpression(FaceExpression.Sadness, speed, apex, intensity);
        fm.ShowExpression(FaceExpression.Surprise, speed, apex, intensity);
    }
}

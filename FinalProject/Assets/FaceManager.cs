//using System.Collections;
//using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum FaceExpression {
    Neutral,
    Happiness,
    Sadness,
    Anger,
    Disgust,
    Fear,
    Surprise
};

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class FaceManager : MonoBehaviour {
    FaceExpression current_expression;
    float current_expression_step = 0;
    float current_expression_duration = 0;
    float current_expression_duration_inv = 0;
    float current_expression_rest = 0;
    float last_expression_intensity = 0;
    float current_expression_intensity = 0;
    float speech_duration = 0;
    float speech_duration_inv = 0;
    float speech_step = 0;
    List<(FaceExpression, float, float, float)> stack = new List<(FaceExpression, float, float, float)>();
    float[] movstartweights = new float[nAUs] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    float[] maxweights = new float[nAUs] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    float[] speech_movstartweights = new float[nAUs] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    float[] speech_maxweights = new float[nAUs] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    bool showing_expression = false;
    bool speaking = false;

    SkinnedMeshRenderer skinnedMeshRenderer;
    int blendShapeCount;

    const int nAUs = 18;

    public void StartSpeaking(float speed) {
        speech_movstartweights = (float[])speech_maxweights.Clone();
        speech_maxweights = GetSpeechWeights();
        speech_step = 0;
        speech_duration = speed;
        speech_duration_inv = 1 / speech_duration;
        speaking = true;
    }

    public void StopSpeaking() {
        speaking = false;
        speech_maxweights = new float[nAUs] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    }

    public void ShowExpression(FaceExpression expression, float speed, float rest, float intensity) {
        if (showing_expression) {
            stack.Add((expression, speed, rest, intensity));
            return;
        }

        switch (expression) {
            case FaceExpression.Happiness:
                Debug.Log("Happiness");
                break;
            case FaceExpression.Sadness:
                Debug.Log("Sadness");
                break;
            case FaceExpression.Anger:
                Debug.Log("Anger");
                break;
            case FaceExpression.Disgust:
                Debug.Log("Disgust");
                break;
            case FaceExpression.Fear:
                Debug.Log("Fear");
                break;
            case FaceExpression.Surprise:
                Debug.Log("Surprise");
                break;
            case FaceExpression.Neutral:
                if (current_expression == FaceExpression.Neutral) {
                    return;
                } else {
                    Debug.Log("Neutral");
                    break;
                }
        }

        current_expression = expression;
        movstartweights = (float[])maxweights.Clone();
        last_expression_intensity = last_expression_intensity == 0 ? intensity : current_expression_intensity;
        maxweights = GetWeights(expression);
        current_expression_step = 0;
        current_expression_duration = speed;
        current_expression_duration_inv = 1 / current_expression_duration;
        current_expression_rest = rest;
        current_expression_intensity = intensity;
        showing_expression = true;
    }

    public void StopExpression() {
        showing_expression = false;
        ShowExpression(FaceExpression.Neutral, 650, 0, 1);
    }

    float[] GetSpeechWeights() {
        // Speech
        //                                    4     1     2     5     7     6     9    10    17    15    25    26    27    16    20    12    23    24
        float[] speech = new float[nAUs] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        if (speaking) {
            speech[10] = Random.Range(-0.5f, 1); // AU 25
            speech[11] = Random.Range(-0.5f, 1); // AU 26
        }

        return speech;
    }

    float[] GetWeights(FaceExpression expression) {
        List<float[]> expressions = new List<float[]>();
        switch (expression) {
            case FaceExpression.Neutral:
                //                       4     1     2     5     7     6     9    10    17    15    25    26    27    16    20    12    23    24
                expressions = new List<float[]> {
                    new float[nAUs] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
                };
                break;
            case FaceExpression.Happiness:
                //                       4     1     2     5     7     6     9    10    17    15    25    26    27    16    20    12    23    24
                expressions = new List<float[]> {
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0, 1.0f,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f,    0,    0,    0,    0,    0,    0, 0.8f,    0, 1.0f,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f,    0,    0,    0, 0.8f,    0,    0,    0,    0, 1.0f,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f,    0,    0,    0,    0, 0.8f,    0,    0,    0, 1.0f,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0, 1.0f,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f,    0,    0,    0, 0.8f,    0,    0, 0.8f,    0, 1.0f,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f,    0,    0,    0,    0, 0.8f,    0, 0.8f,    0, 1.0f,    0,    0 },
                };
                break;
            case FaceExpression.Sadness:
                //                       4     1     2     5     7     6     9    10    17    15    25    26    27    16    20    12    23    24
                expressions = new List<float[]> {
                    new float[nAUs] {    0, 1.0f,    0,    0,    0,    0,    0,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] { 0.8f, 1.0f,    0,    0,    0,    0,    0,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0, 1.0f,    0,    0, 0.8f,    0,    0,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0, 1.0f,    0,    0,    0,    0,    0,    0, 1.0f, 1.0f, 0.8f,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0, 1.0f,    0,    0,    0,    0,    0,    0, 1.0f, 1.0f,    0, 0.8f,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] { 0.8f, 1.0f,    0,    0,    0,    0,    0,    0, 1.0f, 1.0f, 0.8f,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0, 1.0f,    0,    0, 0.8f,    0,    0,    0, 1.0f, 1.0f, 0.8f,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] { 0.8f, 1.0f,    0,    0,    0,    0,    0,    0, 1.0f, 1.0f,    0, 0.8f,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0, 1.0f,    0,    0, 0.8f,    0,    0,    0, 1.0f, 1.0f,    0, 0.8f,    0,    0,    0,    0,    0,    0 },
                };
                break;
            case FaceExpression.Anger:
                //                       4     1     2     5     7     6     9    10    17    15    25    26    27    16    20    12    23    24
                expressions = new List<float[]> {
                    new float[nAUs] { 1.0f,    0, 1.0f,    0, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1.0f, 1.0f },
                    new float[nAUs] { 1.0f,    0, 1.0f,    0, 1.0f,    0,    0,    0, 0.8f,    0,    0,    0,    0,    0,    0,    0, 1.0f, 1.0f },
                    new float[nAUs] { 1.0f,    0, 1.0f,    0, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0, 0.8f,    0,    0, 1.0f, 1.0f },
                    new float[nAUs] { 1.0f,    0, 1.0f,    0, 1.0f,    0,    0,    0,    0,    0, 0.8f,    0,    0,    0,    0,    0, 1.0f, 1.0f },
                    new float[nAUs] { 1.0f,    0, 1.0f,    0, 1.0f,    0,    0,    0,    0,    0,    0, 0.8f,    0,    0,    0,    0, 1.0f, 1.0f },
                    new float[nAUs] { 1.0f,    0, 1.0f,    0, 1.0f,    0,    0,    0, 0.8f,    0, 0.8f,    0,    0,    0,    0,    0, 1.0f, 1.0f },
                    new float[nAUs] { 1.0f,    0, 1.0f,    0, 1.0f,    0,    0,    0,    0,    0, 0.8f,    0,    0, 0.8f,    0,    0, 1.0f, 1.0f },
                    new float[nAUs] { 1.0f,    0, 1.0f,    0, 1.0f,    0,    0,    0, 0.8f,    0,    0, 0.8f,    0,    0,    0,    0, 1.0f, 1.0f },
                    new float[nAUs] { 1.0f,    0, 1.0f,    0, 1.0f,    0,    0,    0,    0,    0,    0, 0.8f,    0, 0.8f,    0,    0, 1.0f, 1.0f },
                };
                break;
            case FaceExpression.Disgust:
                //                       4     1     2     5     7     6     9    10    17    15    25    26    27    16    20    12    23    24
                expressions = new List<float[]> {
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f, 1.0f, 0.8f,    0,    0,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f, 1.0f,    0,    0, 0.8f,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f, 1.0f,    0,    0,    0, 0.8f,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f, 1.0f, 0.8f,    0, 0.8f,    0,    0,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0,    0,    0,    0,    0,    0, 1.0f, 1.0f, 0.8f,    0,    0, 0.8f,    0,    0,    0,    0,    0,    0 },
                };
                break;
            case FaceExpression.Fear:
                //                       4     1     2     5     7     6     9    10    17    15    25    26    27    16    20    12    23    24
                expressions = new List<float[]> {
                    new float[nAUs] {    0, 1.0f,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1.0f,    0,    0,    0 },
                    new float[nAUs] { 0.8f, 1.0f,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1.0f,    0,    0,    0 },
                    new float[nAUs] {    0, 1.0f,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0, 0.8f,    0,    0,    0, 1.0f,    0,    0,    0 },
                    new float[nAUs] {    0, 1.0f,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0, 0.8f,    0,    0, 1.0f,    0,    0,    0 },
                    new float[nAUs] { 0.8f, 1.0f,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0, 0.8f,    0,    0,    0, 1.0f,    0,    0,    0 },
                    new float[nAUs] { 0.8f, 1.0f,    0, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0, 0.8f,    0,    0, 1.0f,    0,    0,    0 },
                };
                break;
            case FaceExpression.Surprise:
                //                       4     1     2     5     7     6     9    10    17    15    25    26    27    16    20    12    23    24
                expressions = new List<float[]> {
                    new float[nAUs] {    0, 1.0f, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0,    0, 1.0f,   0f,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0, 1.0f, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0, 1.0f, 1.0f,   0f,    0,    0,    0,    0,    0 },
                    new float[nAUs] {    0, 1.0f, 1.0f, 1.0f,    0,    0,    0,    0,    0,    0,    0, 1.0f, 0.6f,    0,    0,    0,    0,    0 },
                };
                break;
        }

        int minimum = 0;
        float norm;
        for (int i = 0; i < expressions.Count; i++) {
            norm = 0;
            for (int j = 0; j < blendShapeCount; j++) {
                norm += Mathf.Abs(maxweights[j] - expressions[i][j]);
                norm += Mathf.Clamp(speech_maxweights[j], 0, 1);
            }
            if (norm < minimum)
                minimum = i;
        }

        return expressions[minimum];
        //return expressions[minimum].Select(x => Mathf.Clamp(x, 0, 1)).ToArray();
        //return expressions[minimum].Zip(speech_maxweights, (x, y) => Mathf.Clamp(x + y, 0, 1)).ToArray();
    }

    float SmoothFunctionExpression(float t) {
        //return 0.5f*Mathf.Cos((t - 1)*Mathf.PI) + 0.5f;
        //return t;
        return -2*t*t*t + 3*t*t;
    }

    float SmoothFunctionSpeech(float t) {
        //return 0.5f*Mathf.Cos((t - 1)*Mathf.PI) + 0.5f;
        return t;
        //return -2*t*t*t + 3*t*t;
    }

    void Awake() {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
    }

    void Update() {
        if (showing_expression) {
            if (current_expression_step > current_expression_duration + current_expression_rest) {
                showing_expression = false;
            } else {
                // During rising movement
                float t;
                if (current_expression_step <= current_expression_duration) {
                    for (int i = 0; i < blendShapeCount; i++) {
                        t = ((current_expression_intensity - last_expression_intensity)*current_expression_step*current_expression_duration_inv + last_expression_intensity)
                                * ((maxweights[i] - movstartweights[i])*current_expression_step*current_expression_duration_inv + movstartweights[i]);
                        skinnedMeshRenderer.SetBlendShapeWeight(i, 100*SmoothFunctionExpression(Mathf.Clamp(t, 0, 1)));
                    }
                }
                current_expression_step += 1000*Time.deltaTime;
            }
        } else {
            if (stack.Count > 0) {
                (FaceExpression, float, float, float) cached = stack[0];
                ShowExpression(cached.Item1, cached.Item2, cached.Item3, cached.Item4);
                stack.RemoveAt(0);
            } else if (current_expression == FaceExpression.Neutral) {
                for (int i = 0; i < blendShapeCount; i++)
                    skinnedMeshRenderer.SetBlendShapeWeight(i, 0);
            } else {
                ShowExpression(FaceExpression.Neutral, 650, 0, 1);
            }
        }

        if (speaking) {
            if (speech_step > speech_duration) {
                StartSpeaking(speech_duration);
            }

            float t;
            for (int i = 0; i < blendShapeCount; i++) {
                t = skinnedMeshRenderer.GetBlendShapeWeight(i) * 0.01f;
                if (speech_maxweights[i] != 0)
                    t = SmoothFunctionSpeech(Mathf.Clamp((speech_maxweights[i] - speech_movstartweights[i])*speech_step*speech_duration_inv + speech_movstartweights[i], 0, 1));
                skinnedMeshRenderer.SetBlendShapeWeight(i, 100*Mathf.Clamp(t, 0, 1));
            }

            speech_step += 1000*Time.deltaTime;
        }
    }
}

//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class FaceManager_nospeech : MonoBehaviour {
    FaceExpression current_expression;
    float current_expression_step = 0;
    float current_expression_duration = 0;
    float current_expression_duration_inv = 0;
    float current_expression_rest = 0;
    float last_expression_intensity = 0;
    float current_expression_intensity = 0;
    List<(FaceExpression, float, float, float)> cache = new List<(FaceExpression, float, float, float)>();
    float[] movstartweights = new float[nAUs] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    float[] maxweights = new float[nAUs] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    bool showing_expression = false;

    SkinnedMeshRenderer skinnedMeshRenderer;
    int blendShapeCount;

    const int nAUs = 18;

    public void ShowExpression(FaceExpression expression, float speed, float rest, float intensity) {
        if (showing_expression) {
            cache.Add((expression, speed, rest, intensity));
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
            default:
                break;
        }

        current_expression = expression;
        movstartweights = maxweights;
        last_expression_intensity = last_expression_intensity == 0 ? intensity : current_expression_intensity;
        maxweights = GetWeights(expression);
        current_expression_step = 0;
        current_expression_duration = speed;
        current_expression_duration_inv = 1 / current_expression_duration;
        current_expression_rest = rest;
        current_expression_intensity = intensity;
        showing_expression = true;
    }

    float[] GetWeights(FaceExpression expression) {
        List<float[]> expressions = new List<float[]>();
        switch (expression) {
            case FaceExpression.Neutral:
                //                          4     1     2     5     7     6     9    10    17    15    25    26    27    16    20    12    23    24
                return new float[nAUs] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
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
            for (int j = 0; j < blendShapeCount; j++)
                norm += Mathf.Abs(maxweights[j] - expressions[i][j]);
            if (norm < minimum)
                minimum = i;
        }

        return expressions[minimum];
    }

    float InterpolationFunction(float t) {
        return Mathf.Cos((t - 1)*Mathf.PI) + 1;
    }

    void Awake() {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        blendShapeCount = skinnedMeshRenderer.sharedMesh.blendShapeCount;
        movstartweights = GetWeights(FaceExpression.Neutral);
    }

    void Update() {
        if (showing_expression) {
            if (current_expression_step > current_expression_duration + current_expression_rest) {
                showing_expression = false;
            } else {
                // During rising movement
                float t;
                if (current_expression_step <= current_expression_duration) {
                    //int nvars = blendShapeCount < nAUs ? blendShapeCount : nAUs;
                    //for (int i = 0; i < nvars; i++) {
                    for (int i = 0; i < blendShapeCount; i++) {
                        t = ((current_expression_intensity - last_expression_intensity)*current_expression_step*current_expression_duration_inv + last_expression_intensity)
                                * ((maxweights[i] - movstartweights[i])*current_expression_step*current_expression_duration_inv + movstartweights[i]);
                        skinnedMeshRenderer.SetBlendShapeWeight(i, 100*t);
                    }
                }
                current_expression_step += Mathf.RoundToInt(1000*Time.deltaTime);
            }
        } else {
            if (cache.Count > 0) {
                (FaceExpression, float, float, float) cached = cache[0];
                ShowExpression(cached.Item1, cached.Item2, cached.Item3, cached.Item4);
                cache.RemoveAt(0);
            } else {
                ShowExpression(FaceExpression.Neutral, 650, 1, 1);
            }
        }
    }
}

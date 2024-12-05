using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class Diffirent_Ground_Generator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShapeController;

    [SerializeField, Range(3f, 100f)] private int levelLenght = 50;
    [SerializeField, Range(1f, 50f)] private float xMulti = 2f;
    [SerializeField, Range(1f, 50f)] private float yMulti = 2f;
    [SerializeField, Range(0f, 1f)] private float smoothCurve = 0.5f;
    [SerializeField] private float noiseStep = 0.5f;
    [SerializeField] private float bottom = 10f;

    private Vector3 lastPosition;
    public void OnValidate()
    {
        spriteShapeController.spline.Clear();
        for(int i = 0; i < levelLenght; i++)
        {
            lastPosition = transform.position + new Vector3(i * xMulti, Mathf.PerlinNoise(0, i * noiseStep) * yMulti);
            spriteShapeController.spline.InsertPointAt(i, lastPosition);
            if (i != 0 && i != levelLenght - 1)
            {
                spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spriteShapeController.spline.SetLeftTangent(i, Vector3.left * xMulti * smoothCurve);
                spriteShapeController.spline.SetRightTangent(i, Vector3.right * xMulti * smoothCurve);
            }
        }
        spriteShapeController.spline.InsertPointAt(levelLenght, new Vector3(lastPosition.x, transform.position.y - bottom));
        spriteShapeController.spline.InsertPointAt(levelLenght+1, new Vector3(transform.position.x, transform.position.y - bottom));

    }

}

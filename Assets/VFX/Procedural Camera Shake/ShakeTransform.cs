using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShakeTransform : MonoBehaviour
{
    [System.Serializable]
    public class ShakeEvent
    {
        public float duration;
        public float timeRemaining;
        
        public ShakeTransformEventData data;

        public ShakeTransformEventData.Target target => data.target;

        Vector3 noiseOffset;
        public Vector3 noise;

        public ShakeEvent(ShakeTransformEventData d)
        {
            this.data = d;
            duration = data.duration;
            timeRemaining = duration;

            float rand = 64.0f;

            noiseOffset.x = Random.Range(0.0f, rand);
            noiseOffset.y = Random.Range(0.0f, rand);
            noiseOffset.z = Random.Range(0.0f, rand);

            noise = Vector3.zero;
        }

        public void Update()
        {
            float deltaTime = Time.deltaTime;

            timeRemaining -= deltaTime;

            float noiseOffsetDelta = deltaTime * data.frequency;

            noiseOffset += Vector3.one * noiseOffsetDelta;

            noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
            noise.y = Mathf.PerlinNoise(noiseOffset.y, 1.0f);
            noise.z = Mathf.PerlinNoise(noiseOffset.z, 2.0f);

            noise -= Vector3.one * 0.5f;

            noise *= data.amplitude;

            float agePercent = 1.0f - (timeRemaining / duration);
            noise *= data.blendOverLifetime.Evaluate(agePercent);
        }

        public bool IsAlive()
        {
            return timeRemaining > 0.0f;
        }
    }
    
    public List<ShakeEvent> shakeEvents = new List<ShakeEvent>();
    
    public void AddShakeEvent(ShakeTransformEventData data)
    {
        shakeEvents.Add(new ShakeEvent(data));
    }
    public void AddShakeEvent(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime, ShakeTransformEventData.Target target)
    {
        ShakeTransformEventData data = ShakeTransformEventData.CreateInstance<ShakeTransformEventData>();
        data.Init(amplitude, frequency, duration, blendOverLifetime, target);
    
        AddShakeEvent(data);
    }

    public void LateUpdate()
    {
        var positionOffset = Vector3.zero;
        var rotationOffset = Vector3.zero;

        for (int i = shakeEvents.Count - 1; i != -1; i--)
        {
            var se = shakeEvents[i]; se.Update();

            if (se.target == ShakeTransformEventData.Target.Position)
                positionOffset += se.noise;
            else if (se.target == ShakeTransformEventData.Target.Rotation)
                rotationOffset += se.noise;
            
            if(!se.IsAlive())
                shakeEvents.RemoveAt(i);
        }

        transform.localPosition = positionOffset;
        transform.localEulerAngles = rotationOffset;
    }
}
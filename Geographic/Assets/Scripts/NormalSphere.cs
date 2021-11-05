using UnityEngine;

    public class NormalSphere : IShapeSettings
    {
        public float radius;

        public NormalSphere(float radius)
        {
            this.radius = radius;
        }

        public Vector3 CalculatePoint(Vector3 pointOnUnitSphere)
        {
            return pointOnUnitSphere * radius;
        }
    }
using UnityEngine;

namespace HoloRobot.Utils
{
    public static class Convertor
    {
        /// <summary>
        /// Use this method if u need convert double[] to float[]
        /// </summary>
        /// <param name="value">array double</param>
        /// <returns>array float</returns>
        public static float[] DoubleToFloatArray(double[] value)
        {
            float[] result = new float[value.Length];

            for (int i = 0; i < value.Length; i++)
                result[i] = (float)value[i];

            return result;
        }

        public static Vector3[] MultiplayVectros(Vector3[] axis, float[] angle)
        {
            Vector3[] temp = new Vector3[axis.Length];

            for (int i = 0; i < axis.Length; i++)
            {
                float item = angle[i] * 180 / Mathf.PI;
                temp[i] = Vector3.Scale(axis[i], new Vector3(item, item, item));
            }
            return temp;
        }
    }
}
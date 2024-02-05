using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class WaitForSecondsPool
    {
        private static Dictionary<float, WaitForSeconds> pool = new(); 

        public static WaitForSeconds Get(float t)
        {
            if (pool.TryGetValue(t, out WaitForSeconds value)) 
                return value;

            pool[t] = new WaitForSeconds(t);
            return pool[t];
        } 
    }
}

using UnityEngine;

namespace Infrastructure
{
   public class EntryPoint : MonoBehaviour
   {
      [SerializeField] private AppStartup appStartup;

      private void Start()
      {
         if (FindAnyObjectByType<AppStartup>() == null)
         {
            Instantiate(appStartup);
         }
      }
   }
}

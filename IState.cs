using UnityEngine;

namespace SFI.GameStates
{
   public interface IState
   {
      void Tick();
      void OnEnter();
      void OnExit();
   }
}


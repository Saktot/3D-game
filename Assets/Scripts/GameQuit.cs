using UnityEngine;

namespace Amer{
    // Выход из игры. Общий для меню паузы и меню победы.
    public static class GameQuit{
        public static void Quit(){
            Time.timeScale = 1f;

#if UNITY_EDITOR
            // В редакторе Application.Quit() ничего не делает, поэтому просто останавливаем Play Mode.
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

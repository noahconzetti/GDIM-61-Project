using UnityEngine;
using UnityEngine.InputSystem;

namespace AppCore {
    public class SceneManager : MonoBehaviour {
        [SerializeField] private InputAction reloadAction;

        private void OnEnable() {
            reloadAction.Enable();
            reloadAction.performed += ReloadScene;
        }
        private void OnDisable() {
            reloadAction.Disable();
            reloadAction.performed -= ReloadScene;
        }

        private void ReloadScene(InputAction.CallbackContext _) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}

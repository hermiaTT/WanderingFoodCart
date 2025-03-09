using UnityEngine;

namespace WanderingFoodCart
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }

        public enum GameScene
        {
            MainMenu,
            Opening,
            BusToChengdu,
            Kuanzhai,
            CookingSchool,
            CookingGame,
            MasterShop,
            Suburbs
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(GameScene scene)
        {
            // 加载场景的逻辑
        }

        public bool IsSceneLoaded()
        {
            // 检查场景是否加载完成的逻辑
            return true; // 示例返回值
        }
    }
}

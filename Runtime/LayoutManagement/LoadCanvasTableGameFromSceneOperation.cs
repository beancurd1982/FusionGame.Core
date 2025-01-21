using System;
using System.Collections;
using FusionGame.Core.Controllers;
using UnityEngine.SceneManagement;

namespace FusionGame.Core.LayoutManagement
{
    public class LoadCanvasTableGameFromSceneOperation : LoadGameOperation
    {
        public string SceneName { get; }
        public uint TableId { get; }

        public LoadCanvasTableGameFromSceneOperation(GameLoader gameLoader, string sceneName, uint tableId)
            : base(gameLoader)
        {
            SceneName = sceneName;
            TableId = tableId;
        }

        public override IEnumerator LoadGame(Action<bool, ILoadableGame> onCompleted)
        {
            var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            if (operation == null)
                // the scene is not added to the build settings.
            {
                onCompleted?.Invoke(false, null);
                yield break;
            }

            yield return operation;

            var loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            if (!loadedScene.IsValid() || !loadedScene.isLoaded)
                // the scene is not loaded properly.
            {
                onCompleted?.Invoke(false, null);
                yield break;
            }

            var sceneRootObj = loadedScene.GetRootGameObjects()[0];
            var game = sceneRootObj.GetComponent<CanvasTableGame>();

            if (game == null)
                // the scene is loaded but the CanvasTableGame component is not found, we should unload the scene and return false.
            {
                yield return SceneManager.UnloadSceneAsync(loadedScene);
                onCompleted?.Invoke(false, null);
            }
            else
                // the scene is loaded and the CanvasTableGame component is found
                // we should set the scene root object as a child of the game loader and return true.
            {
                sceneRootObj.transform.SetParent(GameLoader.transform);

                // The game scene root object has been set as a child of the game loader, we can safely unload the scene now.
                yield return SceneManager.UnloadSceneAsync(loadedScene);
                onCompleted?.Invoke(true, game);
            }
        }

        public override bool Equals(object otherObject)
        {
            if (otherObject is LoadCanvasTableGameFromSceneOperation other)
            {
                return SceneName == other.SceneName && TableId == other.TableId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + (SceneName != null ? SceneName.GetHashCode() : 0);
                hash = hash * 23 + TableId.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"LoadCanvasTableGameFromSceneOperation: [SceneName={SceneName}, TableId={TableId}]";
        }
    }
}

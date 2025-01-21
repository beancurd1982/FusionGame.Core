using FusionGame.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FusionGame.Core.LayoutManagement
{
    /// <summary>
    /// The GameLoader is used to load and unload games.
    /// It also manages the loaded games.
    /// </summary>
    public class GameLoader : AutoSingletonBehaviour<GameLoader>
    {
        private readonly Dictionary<LoadGameOperation, ILoadableGame> loadedGames = new Dictionary<LoadGameOperation, ILoadableGame>();
        private Tuple<LoadGameOperation, Action<bool, ILoadableGame>> inProgressOperation;
        private readonly List<Tuple<LoadGameOperation, Action<bool, ILoadableGame>>> pendingOperations =
            new List<Tuple<LoadGameOperation, Action<bool, ILoadableGame>>>();
        private Tuple<LoadGameOperation, Action<bool>> pendingUnloadGameOperation;

        public int PendingOperationsCount => pendingOperations.Count;
        public LoadGameOperation PendingUnloadGameOperation => pendingUnloadGameOperation?.Item1;
        public bool IsIdle => inProgressOperation == null && pendingOperations.Count == 0 && pendingUnloadGameOperation == null;

        /// <summary>
        /// Load the game by providing a LoadGameOperation.
        /// When the load game operation is completed, a callback will be invoked.
        /// If the game is already loaded, the callback will be invoked immediately.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="onComplete"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void LoadGame(LoadGameOperation operation, Action<bool, ILoadableGame> onComplete)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            // Check if the game is already loaded
            if (loadedGames.TryGetValue(operation, out var loadedGame))
            {
                onComplete?.Invoke(true, loadedGame);
                return;
            }

            // Check if there is an operation in progress
            if (inProgressOperation != null)
            {
                // Queue the operation
                pendingOperations.Add(new Tuple<LoadGameOperation, Action<bool, ILoadableGame>>(operation, onComplete));
                return;
            }

            inProgressOperation = new Tuple<LoadGameOperation, Action<bool, ILoadableGame>>(operation, onComplete);
            StartCoroutine(LoadGameInternal(operation, onComplete));
        }

        /// <summary>
        /// Unload the game associated with the operation.
        /// If the load game operation is in progress, the game will be unloaded after the operation is completed.
        /// If the load game operation is pending, it will be removed from the pending list and the game won't be loaded. 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="onComplete"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UnloadGame(LoadGameOperation operation, Action<bool> onComplete)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (loadedGames.TryGetValue(operation, out var loadedGame))
                // if the game is already loaded, unload the game immediately
            {
                loadedGames.Remove(operation);
                loadedGame.OnUnload();
                Destroy(loadedGame.GetRootGameObject);
                onComplete?.Invoke(true);
            }
            else if (inProgressOperation != null && Equals(inProgressOperation.Item1, operation))
                // if the game is loading in progress, create a pending unload game operation
            {
                pendingUnloadGameOperation = new Tuple<LoadGameOperation, Action<bool>>(operation, onComplete);
            }
            else if (pendingOperations.Any(pair => Equals(pair.Item1, operation)))
                // if the game is in the pending operations, remove it from the pending operations
            {
                var pendingOperation = pendingOperations.First(pair => Equals(pair.Item1, operation));
                pendingOperations.Remove(pendingOperation);
                pendingOperation.Item2?.Invoke(false, null);

                onComplete?.Invoke(false);
            }
            else
                // if the game is not loaded, not in progress, and not in the pending operations, return true
            {
                onComplete?.Invoke(true);
            }
        }

        /// <summary>
        /// Unload a game.
        /// Return true if the game is successfully unloaded. Return false if the game was not loaded.
        /// </summary>
        /// <param name="gameToUnload"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool UnloadGame(ILoadableGame gameToUnload)
        {
            if (gameToUnload == null)
            {
                throw new ArgumentNullException(nameof(gameToUnload));
            }

            // Find the operation associated with the game to unload
            var operation = loadedGames.FirstOrDefault(pair => pair.Value == gameToUnload).Key;
            if (operation != null)
            {
                loadedGames.Remove(operation);
                gameToUnload.OnUnload();
                Destroy(gameToUnload.GetRootGameObject);
                return true;
            }

            // Game was not loaded
            return false;
        }

        /// <summary>
        /// Set a game's parent back to the GameLoader.
        /// </summary>
        /// <param name="gameToRelease"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ReleaseGame(ILoadableGame gameToRelease)
        {
            if (gameToRelease == null)
            {
                throw new ArgumentNullException(nameof(gameToRelease));
            }

            if (loadedGames.ContainsValue(gameToRelease))
            {
                gameToRelease.GetRootGameObject.transform.SetParent(transform);
                return true;
            }
            else
            {
                return false;
            }
        }

        public ILoadableGame GetLoadedGame(LoadGameOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            loadedGames.TryGetValue(operation, out var loadedGame);
            return loadedGame;
        }

        public void CleanUp(Action<bool> onComplete)
        {
            // Unload all loaded games immediately
            foreach (var loadedGame in loadedGames.Values)
            {
                loadedGame.OnUnload();
                Destroy(loadedGame.GetRootGameObject);
            }

            // Clear all loaded games and pending operations
            loadedGames.Clear();
            pendingOperations.Clear();

            // if there is an operation in progress, unload the game after the operation is completed
            if (inProgressOperation != null)
            {
                pendingUnloadGameOperation = new Tuple<LoadGameOperation, Action<bool>>(inProgressOperation.Item1, onComplete);
            }
            else
            {
                onComplete?.Invoke(true);
            }
        }

        /// <summary>
        /// Internal coroutine to load game and handle exception thrown.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        private IEnumerator LoadGameInternal(LoadGameOperation operation, Action<bool, ILoadableGame> onComplete)
        {
            Exception exceptionThrown = null;
            ILoadableGame loadedGame = null;
            var loadGameSuccess = false;

            // Run the coroutine
            IEnumerator loadGameRoutine = null;
            try
            {
                loadGameRoutine = operation.LoadGame((success, game) =>
                {
                    if (success && game == null)
                    {
                        Debug.LogError($"Load Game {operation.ToString()} success but the loaded game is Null! Make sure your load game operation always return a valid ILoadableGame reference when success.");
                        success = false;
                    }

                    loadedGame = game;
                    loadGameSuccess = success;
                });
            }
            catch (Exception e)
            {
                exceptionThrown = e;
            }

            if (exceptionThrown == null)
            {
                while (true)
                {
                    object current;
                    try
                    {
                        if (!loadGameRoutine.MoveNext())
                        {
                            break;
                        }

                        current = loadGameRoutine.Current;
                    }
                    catch (Exception e)
                    {
                        exceptionThrown = e;
                        break;
                    }

                    yield return current;  // Yield to Unity coroutine system
                }
            }

            if (exceptionThrown != null)
            {
                Debug.LogError($"Load Game {operation.ToString()} Coroutine ended due to an exception thrown.");
                Debug.LogError(exceptionThrown.StackTrace);

                onComplete?.Invoke(false, null);
            }
            else if (loadGameSuccess)
            {
                loadedGames.Add(operation, loadedGame);
                loadedGame?.OnLoaded(); // call OnLoaded() after the game is loaded, but before the onComplete callback
                onComplete?.Invoke(true, loadedGame);
            }
            else
            {
                onComplete?.Invoke(false, null);
            }

            // Clean up
            inProgressOperation = null;
        }

        private void Update()
        {
            // Check if there is any pending operation
            if (inProgressOperation == null)
            {
                if (pendingUnloadGameOperation != null)
                    // if there is a pending unload game operation, unload the game immediately
                {
                    UnloadGame(pendingUnloadGameOperation.Item1, pendingUnloadGameOperation.Item2);
                    pendingUnloadGameOperation = null;
                }
                else if (pendingOperations.Count > 0)
                    // if there is a pending load game operation, load the game immediately
                {
                    var nextOperation = pendingOperations[0];
                    pendingOperations.Remove(nextOperation);
                    LoadGame(nextOperation.Item1, nextOperation.Item2);
                }
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            pendingOperations.Clear();
        }
    }
}

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Main.View
{
    public class FrontEndQueue : MonoBehaviour
    {
        //[BoxGroup("Views")] [SerializeField] private MovementDiceRollView _movementDiceView;
        private readonly Queue<(QueuedView queuedView, Action action)> _viewActionQueue = new();
        
        private QueuedView _currentView;
/*
        private void Awake()
        {
            //_movementDiceView.Register(this);
        }

        private void OnEnable()
        {
            //_movementService.OnMovementDiceRolled += EnqueueMovementDiceRolled;
        }


        private void OnDisable()
        {
            //_movementService.OnMovementDiceRolled -= EnqueueMovementDiceRolled;
        }

        
        private void EnqueueMinigameSelection(MinigameSo minigame)
        {
            Enqueue(_newMinigameView, () => _newMinigameView.Render(minigame));
        }
        
        [Inject]
        public void Inject(MovementService movementService, BoardService boardService, RoundService roundService,
            MinigameSelectionService minigameSelectionService)
        {
            _movementService = movementService;
            _boardService = boardService;
            _roundService = roundService;
            _minigameSelectionService = minigameSelectionService;
        }*/

        private void Enqueue(QueuedView view, Action action)
        {
            _viewActionQueue.Enqueue((view, action));

            if (_currentView == null) Advance(view);
        }

        public void Advance([NotNull] QueuedView completedView)
        {
            if (_currentView != null && _currentView != completedView)
                throw new InvalidOperationException("Cannot advance ahead of queue");

#if UNITY_EDITOR
            Debug.Log($"Advancing the front end queue. Completed: {completedView}");
#endif

            if (_viewActionQueue.Count is 0)
            {
                _currentView = null;
#if UNITY_EDITOR
                Debug.Log("There are no more elements in the queue. Waiting...");
#endif
                return;
            }

            var next = _viewActionQueue.Dequeue();

#if UNITY_EDITOR
            Debug.Log($"The next element in the queue is {next}");
#endif

            _currentView = next.queuedView;
            next.action.Invoke();
        }
    }
}
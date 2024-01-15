﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChooChoo.PassengerSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.Characters;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using Timberborn.SelectionSystem;
using Timberborn.WorkSystemUI;
using UnityEngine.UIElements;

namespace ChooChoo.PassengerSystemUI
{
    internal class PassengerWagonFragment : IEntityPanelFragment
    {
        private static readonly string EmptyStationLocKey = "Tobbert.PassengerStation.EmptyStation";
        private readonly VisualElementLoader _visualElementLoader;
        private readonly PassengerViewFactory _passengerViewFactory;
        private readonly EntitySelectionService _entitySelectionService;
        private readonly ILoc _loc;
        private VisualElement _root;
        private VisualElement _workplaceUsers;
        private Label _text;
        private Button _increase;
        private Button _decrease;
        private WorkerTypeToggle _workerTypeToggle;
        private MovePassengersBehavior _movePassengersBehavior;
        private readonly List<PassengerView> _views = new();

        public PassengerWagonFragment(
            VisualElementLoader visualElementLoader,
            PassengerViewFactory passengerViewFactory,
            EntitySelectionService entitySelectionService,
            ILoc loc)
        {
            _visualElementLoader = visualElementLoader;
            _passengerViewFactory = passengerViewFactory;
            _entitySelectionService = entitySelectionService;
            _loc = loc;
        }

        public VisualElement InitializeFragment()
        {
            _root = _visualElementLoader.LoadVisualElement("Game/EntityPanel/WorkplaceFragment");
            _workplaceUsers = _root.Q<VisualElement>("WorkplaceUsers");
            _text = _root.Q<Label>("Text");
            _text.text = _loc.T(EmptyStationLocKey);
            _increase = _root.Q<Button>("Increase");
            _increase.ToggleDisplayStyle(false);
            _decrease = _root.Q<Button>("Decrease");
            _decrease.ToggleDisplayStyle(false);
            _root.ToggleDisplayStyle(false);
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _movePassengersBehavior = entity.GetComponentFast<MovePassengersBehavior>();
            if (!_movePassengersBehavior)
                return;
            AddEmptyViews();
        }

        public void ClearFragment()
        {
            _movePassengersBehavior = null;
            _root.ToggleDisplayStyle(false);
        }

        public void UpdateFragment()
        {
            if (_movePassengersBehavior)
            {
                if (_movePassengersBehavior.enabled)
                    UpdateViews();
                _workplaceUsers.ToggleDisplayStyle(_movePassengersBehavior.enabled);
                _text.ToggleDisplayStyle(!_movePassengersBehavior.Passengers.Any());
                _root.ToggleDisplayStyle(true);
            }
            else
                _root.ToggleDisplayStyle(false);
        }

        private void AddEmptyViews()
        {
            RemoveViews();
            for (var index = 0; index < 100; ++index)
                AddEmptyView();
        }

        private void RemoveViews()
        {
            _workplaceUsers.Clear();
            _views.Clear();
        }

        private void AddEmptyView()
        {
            var passengerView = _passengerViewFactory.Create();
            passengerView.ShowEmpty();
            _views.Add(passengerView);
            _workplaceUsers.Add(passengerView.Root);
        }

        private void UpdateViews()
        {
            var characters = _movePassengersBehavior.Passengers.Select(worker => worker.GetComponentFast<Character>());
            var index = 0;
            foreach (var character1 in characters)
            {
                var character = character1;
                var view = _views[index];
                var entityName = character.GetComponentFast<IEntityBadge>().GetEntityName();
                var user = character;
                Action onClick = () => _entitySelectionService.SelectAndFollow(_movePassengersBehavior);
                var description = entityName;
                view.Fill(user, onClick, description);
                ++index;
            }

            for (; index < _views.Count; ++index)
                _views[index].ShowEmpty();
        }
    }
}
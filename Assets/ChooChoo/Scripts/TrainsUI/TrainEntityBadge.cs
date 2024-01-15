﻿using Bindito.Core;
using Timberborn.AssetSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.Characters;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using UnityEngine;

namespace ChooChoo.TrainsUI
{
    public class TrainEntityBadge : BaseComponent, IModifiableEntityBadge
    {
        private readonly string _ageLocKey = "Beaver.Age";

        private IResourceAssetLoader _resourceAssetLoader;

        // private SelectionManager _selectionManager;
        private ILoc _loc;

        private Character _character;
        private Sprite _sprite;

        [Inject]
        public void InjectDependencies(
            IResourceAssetLoader resourceAssetLoader,
            // SelectionManager selectionManager,
            ILoc loc)
        {
            _resourceAssetLoader = resourceAssetLoader;
            // _selectionManager = selectionManager;
            _loc = loc;
        }

        public int EntityBadgePriority => 1;

        public void Awake()
        {
            _character = GetComponentFast<Character>();
            _sprite = _resourceAssetLoader.Load<Sprite>("tobbert.choochoo/tobbert_choochoo/ToolGroupIcon");
        }

        public string GetEntityName()
        {
            return $"<b>{_character.FirstName}</b>";
        }

        public void SetEntityName(string entityName)
        {
            _character.FirstName = entityName;
        }

        public string GetEntitySubtitle()
        {
            return Age();
        }

        public ClickableSubtitle GetEntityClickableSubtitle()
        {
            // return ClickableSubtitle.Create(() => _selectionManager.SelectAndFocusOn(trainYard), _loc.T(TrainYardDisplayNameLocKey));
            return ClickableSubtitle.CreateEmpty();
        }

        public Sprite GetEntityAvatar()
        {
            return _sprite;
        }

        private string Age()
        {
            return _loc.T(_ageLocKey, _character.Age);
        }
    }
}
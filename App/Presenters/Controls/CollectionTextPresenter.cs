using System;
using CollectionManagerExtensionsDll.Enums;
using CollectionManagerExtensionsDll.Modules.CollectionListGenerator;
using App.Interfaces;
using GuiComponents.Interfaces;

namespace App.Presenters.Controls
{
    public class CollectionTextPresenter
    {
        private ICollectionTextView _view;
        private readonly ICollectionTextModel _model;
        private ListGenerator _listGenerator = new ListGenerator();
        public CollectionTextPresenter(ICollectionTextView view, ICollectionTextModel model)
        {
            _view = view;
            _view.SetListTypes(Enum.GetValues(typeof(CollectionListSaveType)));
            _view.SaveTypeChanged += _view_SaveTypeChanged;

            _model = model;
            _model.CollectionChanged += ModelOnCollectionChanged;
        }

        private void _view_SaveTypeChanged(object sender, EventArgs e)
        {
            ModelOnCollectionChanged(this, null);
        }

        private void ModelOnCollectionChanged(object sender, EventArgs eventArgs)
        {
            CollectionListSaveType type;
            Enum.TryParse(_view.SelectedSaveType, out type);
            _view.GeneratedText = _listGenerator.GetAllMapsList(_model.Collections, type);
        }
    }
}
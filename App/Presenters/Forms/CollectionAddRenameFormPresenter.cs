using System;
using App.Interfaces;
using App.Interfaces.Forms;
using App.Models;
using App.Presenters.Controls;
using GuiComponents.Interfaces;

namespace App.Presenters.Forms
{
    public class CollectionAddRenameFormPresenter
    {
        private readonly ICollectionAddRenameForm _form;
        private readonly ICollectionAddRenameModel _model;

        public CollectionAddRenameFormPresenter(ICollectionAddRenameForm form, ICollectionAddRenameModel model)
        {
            _form = form;
            _model = model;
            
            new CollectionAddRenamePresenter(form.CollectionRenameView, model);
        }
    }
}
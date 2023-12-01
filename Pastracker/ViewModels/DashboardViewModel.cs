using Pastracker.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Pastracker.ViewModels
{
	public class DashboardViewModel : BindableBase
	{
        private readonly IRegionManager _regionManager;

        public DashboardViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            EditorCommand = new DelegateCommand(EditorCommandExecute);

        }
        public DelegateCommand EditorCommand { get; }

        private void EditorCommandExecute()
        {
            // Menu表示
            var p = new NavigationParameters();
            _regionManager.RequestNavigate("ContentRegion", nameof(Editor), p);

        }

    }
}

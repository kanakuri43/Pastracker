using Pastracker.Models;
using Pastracker.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pastracker.ViewModels
{
    public class MasterListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        private ObservableCollection<ComboBoxViewModel> _flexMasterList = new ObservableCollection<ComboBoxViewModel>();
        private int _selectedId;
        private int _masterType;
        private string _masterName;

        public DelegateCommand MouseDoubleClickCommand { get; }
        public DelegateCommand AddCommand { get; }
        public DelegateCommand CancelCommand { get; }


        public ObservableCollection<ComboBoxViewModel> FlexMastertList
        {
            get => _flexMasterList;
            set => SetProperty(ref _flexMasterList, value);
        }
        public int SelectedId
        {
            get { return _selectedId; }
            set { SetProperty(ref _selectedId, value); }
        }
        public int CurrentMasterType
        {
            get { return _masterType; }
            set { SetProperty(ref _masterType, value); }
        }
        public string CurrentMasterName
        {
            get { return _masterName; }
            set { SetProperty(ref _masterName, value); }
        }

        public MasterListViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CancelCommand = new DelegateCommand(CancelCommandExecute);
        }

        private void ShowMasterList()
        {
            switch (CurrentMasterType)
            {
                case (int)MasterType.Company:
                    CurrentMasterName = @"会社";
                    break;
                case (int)MasterType.Branch:
                    CurrentMasterName = @"支店";
                    break;
                case (int)MasterType.Employee:
                    CurrentMasterName = @"社員";
                    break; 
                default:
                    break;

            }

        }

            private void CancelCommandExecute()
        {
            var p = new NavigationParameters();
            _regionManager.RequestNavigate("ContentRegion", nameof(Editor), p);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            CurrentMasterType = navigationContext.Parameters.GetValue<int>(nameof(CurrentMasterType));
            ShowMasterList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}

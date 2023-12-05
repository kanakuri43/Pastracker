using Pastracker.Models;
using Pastracker.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Packaging;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;

namespace Pastracker.ViewModels
{
    public class EditorViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private int[] _years = new int[7];

        public int[] Years
        {
            get { return _years; }
            set { SetProperty(ref _years, value); }
        }

        public EditorViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CancelCommand = new DelegateCommand(CancelCommandExecute);
            CompanyCommand = new DelegateCommand(CompanyCommandExecute);
            BranchCommand = new DelegateCommand(BranchCommandExecute);
            EmployeeCommand = new DelegateCommand(EmployeeCommandExecute);
            DocumentCommand = new DelegateCommand(DocumentCommandExecute);

            for (int i = 0; i < 7; i++)
            {
                this.Years[i] = (DateTime.Now.Year) - i;
            }
        }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand CompanyCommand { get; }
        public DelegateCommand BranchCommand { get; }
        public DelegateCommand EmployeeCommand { get; }
        public DelegateCommand DocumentCommand { get; }

        private void CancelCommandExecute()
        {
            // Menu表示
            var p = new NavigationParameters();
            _regionManager.RequestNavigate("ContentRegion", nameof(Dashboard), p);

        }
        private void CompanyCommandExecute()
        {
            var p = new NavigationParameters();
            p.Add(nameof(MasterListViewModel.CurrentMasterType), MasterType.Company);
            _regionManager.RequestNavigate("ContentRegion", nameof(MasterList), p);

        }

        private void BranchCommandExecute()
        {
            var p = new NavigationParameters();
            p.Add(nameof(MasterListViewModel.CurrentMasterType), MasterType.Branch);
            _regionManager.RequestNavigate("ContentRegion", nameof(MasterList), p);

        }

        private void EmployeeCommandExecute()
        {
            var p = new NavigationParameters();
            p.Add(nameof(MasterListViewModel.CurrentMasterType), MasterType.Employee);
            _regionManager.RequestNavigate("ContentRegion", nameof(MasterList), p);

        }
        private void DocumentCommandExecute()
        {
            Process.Start("explorer.exe", System.AppDomain.CurrentDomain.BaseDirectory);
        }

        private void PrintCommandExecute()
        {

        }

    }
}

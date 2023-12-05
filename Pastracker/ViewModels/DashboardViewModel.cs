using Microsoft.Identity.Client;
using Pastracker.Models;
using Pastracker.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;


namespace Pastracker.ViewModels
{
	public class DashboardViewModel : BindableBase
	{
        private readonly IRegionManager _regionManager;
        private ObservableCollection<MoveContent> _moveContents;
        private ObservableCollection<Company> _companies;
        private ObservableCollection<Branch> _branches;
        private ObservableCollection<Employee> _employees;
        private int _moveContentId;
        private int _companyId;
        private int _branchId;
        private int _employeeId;

        private int[] _years = new int[7];

        public int[] Years
        {
            get { return _years; }
            set { SetProperty(ref _years, value); }
        }
        public ObservableCollection<MoveContent> MoveContents
        {
            get { return _moveContents; }
            set { SetProperty(ref _moveContents, value); }
        }
        public ObservableCollection<Company> Companies
        {
            get { return _companies; }
            set { SetProperty(ref _companies, value); }
        }
        public ObservableCollection<Branch> Branches
        {
            get { return _branches; }
            set { SetProperty(ref _branches, value); }
        }
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }
        public int MoveContentId
        {
            get { return _moveContentId; }
            set { SetProperty(ref _moveContentId, value); }
        }
        public int CompanyId
        {
            get { return _companyId; }
            set { SetProperty(ref _companyId, value); }
        }
        public int BranchId
        {
            get { return _branchId; }
            set { SetProperty(ref _branchId, value); }
        }
        public int EmployeeId
        {
            get { return _employeeId; }
            set { SetProperty(ref _employeeId, value); }
        }

        public DashboardViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            EditorCommand = new DelegateCommand(EditorCommandExecute);
            CompanyCommand = new DelegateCommand(CompanyCommandExecute);
            BranchCommand = new DelegateCommand(BranchCommandExecute);
            EmployeeCommand = new DelegateCommand(EmployeeCommandExecute);

            // ComboBox
            using (var context = new AppDbContext())
            {
                Companies = new ObservableCollection<Company>(context.Companies.ToList());
                Branches = new ObservableCollection<Branch>(context.Branches.ToList());
                Employees = new ObservableCollection<Employee>(context.Employees.ToList());
            }

            // 年
            for (int i = 0; i < 7; i++)
            {
                this.Years[i] = (DateTime.Now.Year) - i;
            }

        }
        public DelegateCommand EditorCommand { get; }
        public DelegateCommand CompanyCommand { get; }
        public DelegateCommand BranchCommand { get; }
        public DelegateCommand EmployeeCommand { get; }

        private void EditorCommandExecute()
        {
            // Menu表示
            var p = new NavigationParameters();
            _regionManager.RequestNavigate("ContentRegion", nameof(Editor), p);

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

        private void YearSelectionChangedExecute(object[] selectedItems)
        {
            try
            {
                var selectedItem = selectedItems[0];
                var year = selectedItem;
                ShowContentDetails((int)year);
            }
            catch
            {

            }
        }
        private void ShowContentDetails(int year)
        {
            using var context = new AppDbContext();
            this.MoveContents = new ObservableCollection<MoveContent>(context.MoveContents
                                                                                   .Where(j => j.PickupDate.Year == year)
                                                                                   .ToList());

        }


    }
}

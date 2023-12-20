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
	public class DashboardViewModel : BindableBase, INavigationAware
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
        private int _year;
        private int _month;

        private int?[] _years = new int?[7];
        private int?[] _months = new int?[13];

        public int?[] Years
        {
            get { return _years; }
            set { SetProperty(ref _years, value); }
        }
        public int?[] Months
        {
            get { return _months; }
            set { SetProperty(ref _months, value); }
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
        public int Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
        }
        public int Month
        {
            get { return _month; }
            set { SetProperty(ref _month, value); }
        }
        public DashboardViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            EditorCommand = new DelegateCommand(EditorCommandExecute);
            CompanyCommand = new DelegateCommand(CompanyCommandExecute);
            BranchCommand = new DelegateCommand(BranchCommandExecute);
            EmployeeCommand = new DelegateCommand(EmployeeCommandExecute);
            MoveContentsDoubleClick = new DelegateCommand(MoveContentsDoubleClickDoubleClickExecute);
            EmployeeSelectionChanged = new DelegateCommand<object[]>(EmployeeSelectionChangedExecute);
            YearSelectionChanged = new DelegateCommand<object[]>(YearSelectionChangedExecute);
            MonthSelectionChanged = new DelegateCommand<object[]>(MonthSelectionChangedExecute);

            using (var context = new AppDbContext())
            {
                // ComboBox
                Companies = new ObservableCollection<Company>(context.Companies.ToList());
                Branches = new ObservableCollection<Branch>(context.Branches.ToList());
                Employees = new ObservableCollection<Employee>(context.Employees.ToList());

                MoveContents = new ObservableCollection<MoveContent>(context.MoveContents.ToList());
            }

            // 年
            this.Years[0] = null;
            for (int i = 1; i < 7; i++)
            {
                this.Years[i] = (DateTime.Now.Year) - i + 1;
            }
            this.Months[0] = null;
            for (int i = 1; i <= 12; i++)
            {
                this.Months[i] = i;
            }
            ShowContentsList();

        }
        public DelegateCommand EditorCommand { get; }
        public DelegateCommand CompanyCommand { get; }
        public DelegateCommand BranchCommand { get; }
        public DelegateCommand EmployeeCommand { get; }
        public DelegateCommand MoveContentsDoubleClick { get; }
        public DelegateCommand<object[]> EmployeeSelectionChanged { get; }
        public DelegateCommand<object[]> YearSelectionChanged { get; }
        public DelegateCommand<object[]> MonthSelectionChanged { get; }

        private void EditorCommandExecute()
        {
            // 登録画面表示
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

        private void EmployeeSelectionChangedExecute(object[] selectedItems)
        {
            try
            {
                ShowContentsList();
            }
            catch
            {

            }
        }
        private void YearSelectionChangedExecute(object[] selectedItems)
        {
            try
            {
                var selectedItem = selectedItems[0];
                this.Year = (int)selectedItem;
                ShowContentsList();
            }
            catch
            {

            }
        }
        private void MonthSelectionChangedExecute(object[] selectedItems)
        {
            try
            {
                var selectedItem = selectedItems[0];
                this.Month = (int)selectedItem;
                ShowContentsList();
            }
            catch
            {

            }
        }

        private void ShowContentsList()
        {
            using var context = new AppDbContext();
            this.MoveContents = new ObservableCollection<MoveContent>(context.MoveContents.Where(j => j.EmployeeId == this.EmployeeId).ToList());

            //var query = context.MoveContents.AsQueryable();

            //// 年に基づくフィルタ
            //query = query.Where(j => j.PickupDate.Year == this.Year);

            //// 月が指定されている場合、その条件を追加
            //if (this.Month != 0)
            //{
            //    query = query.Where(j => j.PickupDate.Month == this.Month);
            //}

            //this.MoveContents = new ObservableCollection<MoveContent>(query.ToList());
        }

        private void MoveContentsDoubleClickDoubleClickExecute()
        {
            var p = new NavigationParameters();
            p.Add(nameof(EditorViewModel.CurrentMoveContentId), MoveContentId);
            _regionManager.RequestNavigate("ContentRegion", nameof(Editor), p);

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ShowContentsList();
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

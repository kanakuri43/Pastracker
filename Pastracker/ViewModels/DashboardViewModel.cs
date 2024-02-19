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
        private ObservableCollection<Employee> _employees;
        private int _moveContentId;
        private int _companyId;
        private int _branchId;
        private int _employeeId;
        private int _year;
        private int _month;
        private string _employeeCode;
        private string _employeeName;
        private int _selectedDistanceRangeId;
        private int _minDistance;
        private int _maxDistance;

        private int?[] _years = new int?[7];

        public int?[] Years
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
        public string EmployeeCode
        {
            get { return _employeeCode; }
            set { SetProperty(ref _employeeCode, value); }
        }
        public string EmployeeName
        {
            get { return _employeeName; }
            set { SetProperty(ref _employeeName, value); }
        }
        public int SelectedDistanceRangeId
        {
            get { return _selectedDistanceRangeId; }
            set { SetProperty(ref _selectedDistanceRangeId, value); }
        }
        public DashboardViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            EditCommand = new DelegateCommand(EditCommandExecute);
            CompanyCommand = new DelegateCommand(CompanyCommandExecute);
            EmployeeCommand = new DelegateCommand(EmployeeCommandExecute);
            MoveContentsDoubleClick = new DelegateCommand(MoveContentsDoubleClickDoubleClickExecute);
            EmployeeSelectionChanged = new DelegateCommand<object[]>(EmployeeSelectionChangedExecute);
            YearSelectionChanged = new DelegateCommand<object[]>(YearSelectionChangedExecute);
            MonthSelectionChanged = new DelegateCommand<object[]>(MonthSelectionChangedExecute);
            SearchEmployeeCommand = new DelegateCommand(SearchEmployeeCommandExecute);
            DistanceRangeSelectionChanged = new DelegateCommand<object[]>(DistanceRangeSelectionChangedExecute);

            using (var context = new AppDbContext())
            {
                // ComboBox
                Companies = new ObservableCollection<Company>(context.Companies.ToList());
                Employees = new ObservableCollection<Employee>(context.Employees.ToList());

                MoveContents = new ObservableCollection<MoveContent>(context.MoveContents.ToList());
            }

            // 年
            this.Years[0] = null;
            for (int i = 1; i < 7; i++)
            {
                this.Years[i] = (DateTime.Now.Year) - i + 1;
            }
            this.Year = DateTime.Now.Year;

            _minDistance = 0;
            _maxDistance = 999999;

            ShowContentsList();

        }
        public DelegateCommand EditCommand { get; }
        public DelegateCommand CompanyCommand { get; }
        public DelegateCommand EmployeeCommand { get; }
        public DelegateCommand MoveContentsDoubleClick { get; }
        public DelegateCommand<object[]> EmployeeSelectionChanged { get; }
        public DelegateCommand<object[]> YearSelectionChanged { get; }
        public DelegateCommand<object[]> MonthSelectionChanged { get; }
        public DelegateCommand SearchEmployeeCommand { get; }
        public DelegateCommand<object[]> DistanceRangeSelectionChanged { get; }

        private void EditCommandExecute()
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
                var month = (string)((System.Windows.Controls.ContentControl)selectedItems[0]).Content;
                if (month != "")
                {
                    this.Month = int.Parse(month);
                }
                else
                {
                    this.Month = 0;
                }

                ShowContentsList();
            }
            catch
            {

            }
        }
        private void DistanceRangeSelectionChangedExecute(object[] selectedItems)
        {
            try
            {
                switch (SelectedDistanceRangeId)
                {
                    case 0:
                        // すべて
                        _minDistance = 0;
                        _maxDistance = 999999;
                        break;
                    case 1:
                        // 0 ～ 99
                        _minDistance = 0;
                        _maxDistance = 99;
                        break;
                    case 2:
                        // 100 ～ 199
                        _minDistance = 100;
                        _maxDistance = 199;
                        break;
                    case 3:
                        // 200 ～ 299
                        _minDistance = 200;
                        _maxDistance = 299;
                        break;
                    case 4:
                        // 300 ～ 399
                        _minDistance = 300;
                        _maxDistance = 399;
                        break;
                    case 5:
                        // 400 ～
                        _minDistance = 400;
                        _maxDistance = 999999;
                        break;

                }
                ShowContentsList();
            }
            catch
            {

            }
        }


        private void ShowContentsList()
        {
            using var context = new AppDbContext();

            // 検索条件
            int? searchEmployeeID = null; // 検索しない場合はnull

            // 条件に基づいてデータをフィルタリング
            this.MoveContents = new ObservableCollection<MoveContent>(context.MoveContents
                .Where(c =>
                    (!searchEmployeeID.HasValue || c.EmployeeId == searchEmployeeID.Value) 
                    && ((c.PickupDate).Year == this.Year) 
                    && ((this.Month == 0) || (c.PickupDate).Month == this.Month) 
                    && ((c.Distance >= _minDistance) && (c.Distance <= _maxDistance))
                    )
                .OrderBy(c => c.PickupDate));

        }

        private void MoveContentsDoubleClickDoubleClickExecute()
        {
            var p = new NavigationParameters();
            p.Add(nameof(EditorViewModel.CurrentMoveContentId), MoveContentId);
            _regionManager.RequestNavigate("ContentRegion", nameof(Editor), p);

        }

        private void SearchEmployeeCommandExecute()
        {
            var p = new NavigationParameters();
            _regionManager.RequestNavigate("ContentRegion", nameof(SearchEmployee), p);

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.EmployeeId = navigationContext.Parameters.GetValue<int>(nameof(EmployeeId));

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

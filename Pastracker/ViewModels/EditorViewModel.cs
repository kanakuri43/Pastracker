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
using System.Collections.ObjectModel;

namespace Pastracker.ViewModels
{
    public class EditorViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private ObservableCollection<Company> _companies;
        private ObservableCollection<Branch> _branches;
        private ObservableCollection<Employee> _employees;
        private int _currentMoveContentId;
        private int _companyId;
        private int _branchId;
        private int _employeeId;
        private DateTime _pickupDate;
        private string _pickupName;
        private string _pickupTel;
        private string _pickupAddress1;
        private string _pickupAddress2;
        private DateTime _deliveryDate;
        private string _deliveryName;
        private string _deliveryTel;
        private string _deliveryAddress1;
        private string _deliveryAddress2;
        private int _destinationBranchId;

        private int[] _years = new int[7];

        public int[] Years
        {
            get { return _years; }
            set { SetProperty(ref _years, value); }
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
        public int CurrentMoveContentId
        {
            get { return _currentMoveContentId; }
            set { SetProperty(ref _currentMoveContentId, value); }
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
        public DateTime PickupDate
        {
            get { return _pickupDate; }
            set { SetProperty(ref _pickupDate, value); }
        }
        public string PickupName
        {
            get { return _pickupName; }
            set { SetProperty(ref _pickupName, value); }
        }
        public string PickupTel
        {
            get { return _pickupTel; }
            set { SetProperty(ref _pickupTel, value); }
        }
        public string PickupAddress1
        {
            get { return _pickupAddress1; }
            set { SetProperty(ref _pickupAddress1, value); }
        }
        public string PickupAddress2
        {
            get { return _pickupAddress2; }
            set { SetProperty(ref _pickupAddress2, value); }
        }

        public DateTime DeliveryDate
        {
            get { return _deliveryDate; }
            set { SetProperty(ref _deliveryDate, value); }
        }
        public string DeliveryName
        {
            get { return _deliveryName; }
            set { SetProperty(ref _deliveryName, value); }
        }
        public string DeliveryTel
        {
            get { return _deliveryTel; }
            set { SetProperty(ref _deliveryTel, value); }
        }
        public string DeliveryAddress1
        {
            get { return _deliveryAddress1; }
            set { SetProperty(ref _deliveryAddress1, value); }
        }
        public string DeliveryAddress2
        {
            get { return _deliveryAddress2; }
            set { SetProperty(ref _deliveryAddress2, value); }
        }
        public int DestinationBranchId
        {
            get { return _branchId; }
            set { SetProperty(ref _branchId, value); }
        }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand CompanyCommand { get; }
        public DelegateCommand BranchCommand { get; }
        public DelegateCommand EmployeeCommand { get; }
        public DelegateCommand DocumentCommand { get; }
        public DelegateCommand RegisterCommand { get; }

        public EditorViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CancelCommand = new DelegateCommand(CancelCommandExecute);
            CompanyCommand = new DelegateCommand(CompanyCommandExecute);
            BranchCommand = new DelegateCommand(BranchCommandExecute);
            EmployeeCommand = new DelegateCommand(EmployeeCommandExecute);
            DocumentCommand = new DelegateCommand(DocumentCommandExecute);
            RegisterCommand = new DelegateCommand(RegisterCommandExecute);

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

            InitializeScreen();
        }

        private void InitializeScreen()
        {
            this.PickupDate = DateTime.Now;
            this.DeliveryDate = DateTime.Now;
        }

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

        private void RegisterCommandExecute()
        {
            if (this.EmployeeId == 0 || this.BranchId == 0 || this.EmployeeId == 0)
            {
                return;
            }

            using (var context = new AppDbContext())
            {
                if (this.CurrentMoveContentId == 0)
                {
                    // Create
                    var moveContent = new MoveContent
                    {
                        CompanyId = this.CompanyId,
                        BranchId = this.BranchId,
                        EmployeeId = this.EmployeeId,
                        PickupDate = this.PickupDate,
                        PickupName = this.PickupName,
                        PickupTel = this.PickupTel,
                        PickupAddress1 = this.PickupAddress1,
                        PickupAddress2 = this.PickupAddress2,
                        DeliveryDate = this.DeliveryDate,
                        DeliveryName = this.DeliveryName,
                        DeliveryTel = this.DeliveryTel,
                        DeliveryAddress1 = this.DeliveryAddress1,
                        DeliveryAddress2 = this.DeliveryAddress2,
                    };
                    context.MoveContents.Add(moveContent);
                    context.SaveChanges();


                }
                else
                {
                    // Update
                    var moveContent = context.MoveContents.FirstOrDefault(p => p.Id == this.CurrentMoveContentId);
                    if (moveContent != null)
                    {

                        moveContent.CompanyId = this.CompanyId;
                        moveContent.BranchId = this.BranchId;
                        moveContent.EmployeeId = this.EmployeeId;
                        moveContent.PickupDate = this.PickupDate;

                        context.SaveChanges();
                    }
                }
            }
            //InitializeScreen();
            //ShowAccountJournalsTable(this.SelectedYear);
        }

        private void PrintCommandExecute()
        {

        }

        private void ShowMoveContentDetail(int MoveContentId)
        {
           

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // TODO
            // MoveContentIdを受け取った時の処理
            this.CurrentMoveContentId = navigationContext.Parameters.GetValue<int>(nameof(CurrentMoveContentId));
            ShowMoveContentDetail(CurrentMoveContentId);
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

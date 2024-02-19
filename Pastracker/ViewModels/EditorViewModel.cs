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
        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Truck> _trucks;
        private int _currentMoveContentId;
        private int _companyId;
        private int _branchId;
        private int _employeeId;
        private string _employeeName;
        private DateTime _pickupDate;
        private string _pickupAddress1;
        private string _pickupAddress2;
        private DateTime _deliveryDate;
        private string _deliveryAddress1;
        private string _deliveryAddress2;
        private int _truckId;
        private int _distance;
        private decimal _amount;
        private string _slaveName;
        private string _privateNotes;
        private string _publicNotes;

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
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }
        public ObservableCollection<Truck> Trucks
        {
            get { return _trucks; }
            set { SetProperty(ref _trucks, value); }
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
        public int EmployeeId
        {
            get { return _employeeId; }
            set { SetProperty(ref _employeeId, value); }
        }
        public string EmployeeName
        {
            get { return _employeeName; }
            set { SetProperty(ref _employeeName, value); }
        }
        public DateTime PickupDate
        {
            get { return _pickupDate; }
            set { SetProperty(ref _pickupDate, value); }
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
        public int TruckId
        {
            get { return _truckId; }
            set { SetProperty(ref _truckId, value); }
        }
        public int Distance
        {
            get { return _distance; }
            set { SetProperty(ref _distance, value); }
        }
        public decimal Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }
        public string SlaveName
        {
            get { return _slaveName; }
            set { SetProperty(ref _slaveName, value); }
        }
        public string PrivateNotes
        {
            get { return _privateNotes; }
            set { SetProperty(ref _privateNotes, value); }
        }
        public string PublicNotes
        {
            get { return _publicNotes; }
            set { SetProperty(ref _publicNotes, value); }
        }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand CompanyCommand { get; }
        public DelegateCommand BranchCommand { get; }
        public DelegateCommand EmployeeCommand { get; }
        public DelegateCommand AttachmentCommand { get; }
        public DelegateCommand RegisterCommand { get; }
        public DelegateCommand PrintCommand { get; }
        public DelegateCommand EmployeeSelectionChangedCommand { get; }

        public EditorViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CancelCommand = new DelegateCommand(CancelCommandExecute);
            CompanyCommand = new DelegateCommand(CompanyCommandExecute);
            EmployeeCommand = new DelegateCommand(EmployeeCommandExecute);
            AttachmentCommand = new DelegateCommand(AttachmentCommandExecute);
            RegisterCommand = new DelegateCommand(RegisterCommandExecute);
            PrintCommand = new DelegateCommand(PrintCommandExecute);
            EmployeeSelectionChangedCommand = new DelegateCommand(EmployeeSelectionChangedCommandExecute);

            // ComboBox
            using (var context = new AppDbContext())
            {
                Companies = new ObservableCollection<Company>(context.Companies.ToList());
                Employees = new ObservableCollection<Employee>(context.Employees.ToList());
                Trucks = new ObservableCollection<Truck>(context.Trucks.ToList());
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

            this.CompanyId = default(int);
            this.EmployeeId = default(int);
            this.EmployeeName = default(string);
            this.PickupDate = DateTime.Now;
            this.PickupAddress1 = default(string);
            this.PickupAddress2 = default(string);
            this.DeliveryDate = DateTime.Now;
            this.DeliveryAddress1 = default(string);
            this.DeliveryAddress2 = default(string);
            this.Distance = default(int);
            this.Amount = default(decimal);
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

        private void EmployeeCommandExecute()
        {
            var p = new NavigationParameters();
            p.Add(nameof(MasterListViewModel.CurrentMasterType), MasterType.Employee);
            _regionManager.RequestNavigate("ContentRegion", nameof(MasterList), p);

        }
        private void AttachmentCommandExecute()
        {
            var path = $"{System.AppDomain.CurrentDomain.BaseDirectory}attachment\\{this.EmployeeId}";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            Process.Start("explorer.exe", path);
        }

        private void RegisterCommandExecute()
        {
            if (this.EmployeeId == 0 || this.EmployeeId == 0)
            {
                return;
            }

            using (var context = new AppDbContext())
            {
                // 社員idから社員名取得
                var e = context.Employees.Find(this.EmployeeId);

                if (this.CurrentMoveContentId == 0)
                {
                    // Create
                    var moveContent = new MoveContent
                    {
                        CompanyId = this.CompanyId,
                        EmployeeId = this.EmployeeId,
                        EmployeeName = e.Name,
                        PickupDate = this.PickupDate,
                        PickupAddress1 = this.PickupAddress1,
                        PickupAddress2 = this.PickupAddress2,
                        DeliveryDate = this.DeliveryDate,
                        DeliveryAddress1 = this.DeliveryAddress1,
                        DeliveryAddress2 = this.DeliveryAddress2,
                        Distance = this.Distance,
                        Amount = this.Amount,
                        SlaveName = this.SlaveName,
                        PrivateNotes = this.PrivateNotes,
                        PublicNotes = this.PublicNotes,
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
                        moveContent.EmployeeId = this.EmployeeId;
                        moveContent.EmployeeName = e.Name;
                        moveContent.PickupDate = this.PickupDate;
                        moveContent.PickupAddress1 = this.PickupAddress1;
                        moveContent.PickupAddress2 = this.PickupAddress2;
                        moveContent.DeliveryDate = this.DeliveryDate;
                        moveContent.DeliveryAddress1 = this.DeliveryAddress1;
                        moveContent.DeliveryAddress2 = this.DeliveryAddress2;
                        moveContent.Distance = this.Distance;
                        moveContent.Amount = this.Amount;
                        moveContent.SlaveName = this.SlaveName;
                        moveContent.PrivateNotes = this.PrivateNotes;
                        moveContent.PublicNotes = this.PublicNotes;

                        context.SaveChanges();
                    }
                }
            }
            //InitializeScreen();
        }

        private void PrintCommandExecute()
        {
            ContentPaper contentPaper = new ContentPaper();
            contentPaper.DataContext = this;
            FixedPage fixedPage = new FixedPage();
            fixedPage.Children.Add(contentPaper);

            // A4縦
            fixedPage.Width = 8.27 * 96;
            fixedPage.Height = 11.69 * 96;
            PageContent pc = new PageContent();
            ((IAddChild)pc).AddChild(fixedPage);
            FixedDocument fixedDocument = new FixedDocument();
            fixedDocument.Pages.Add(pc);

            string outputDirectory = System.AppDomain.CurrentDomain.BaseDirectory + "pdf";
            string xpsFileName = @"aaa.xps"; // string.Format(outputDirectory + @"\{0}.xps", SelectedDate.ToString("yyyyMMdd"));
            string pdfFileName = @"aaa.pdf"; // string.Format(outputDirectory + @"\{0}.pdf", SelectedDate.ToString("yyyyMMdd"));
            using (Package p = Package.Open(xpsFileName, FileMode.Create))
            {
                using (XpsDocument d = new XpsDocument(p))
                {
                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(d);
                    writer.Write(fixedDocument.DocumentPaginator);
                }
            }

            PdfSharp.Xps.XpsConverter.Convert(xpsFileName, pdfFileName, 0);
            File.Delete(xpsFileName);

            // 関連付けされたソフトで開く
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pdfFileName,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void ClearContent()
        {
            this.PickupDate = DateTime.Now;
            this.PickupAddress1 = "";
            this.PickupAddress2 = "";
            this.DeliveryDate = DateTime.Now;
            this.DeliveryAddress1 = "";
            this.DeliveryAddress2 = "";

        }

        private void EmployeeSelectionChangedCommandExecute()
        {
            // 
            if(this.CurrentMoveContentId != 0)
            {
                return;
            }

            // 新規登録時のみ
            // 選択された社員の引っ越し履歴データがある場合、
            // 前回の納入先を、引取り先にデフォルト表示
            ClearContent();

            var context = new AppDbContext();
            int lastIdOfEmployee = context.MoveContents.Where(s => s.EmployeeId == this.EmployeeId) // 社員フィルタリング
                                            .OrderByDescending(s => s.PickupDate) // 引取日で降順にソート
                                            .Select(s => s.Id) 
                                            .FirstOrDefault(); // 最初の要素（最新の日付）を取得、データがない場合はデフォルト値

            if (lastIdOfEmployee != default(int))
            {
                InheritanceMoveContentDetail(lastIdOfEmployee);
            }


        }

        private void InheritanceMoveContentDetail(int MoveContentId)
        {
            using var context = new AppDbContext();
            var mc = new ObservableCollection<MoveContent>(context.MoveContents
                                                                .Where(j => j.Id == MoveContentId)
                                                                .ToList());
            if (mc.Count > 0)
            {
                this.CompanyId = mc[0].CompanyId;
                this.EmployeeId = mc[0].EmployeeId;
                this.EmployeeName = mc[0].EmployeeName;
                this.PickupDate = mc[0].DeliveryDate;
                this.PickupAddress1 = mc[0].DeliveryAddress1;
                this.PickupAddress2 = mc[0].DeliveryAddress2;
                this.DeliveryDate = DateTime.Now;
                this.DeliveryAddress1 = "";
                this.DeliveryAddress2 = "";
                

            }

        }

        private void ShowMoveContentDetail(int MoveContentId)
        {
            using var context = new AppDbContext();
            var mc = new ObservableCollection<MoveContent>(context.MoveContents
                                                                .Where(j => j.Id == MoveContentId)
                                                                .ToList());
            if (mc.Count > 0)
            {
                this.CompanyId = mc[0].CompanyId;
                this.EmployeeId = mc[0].EmployeeId;
                this.EmployeeName = mc[0].EmployeeName;
                this.PickupDate = mc[0].PickupDate;
                this.PickupAddress1 = mc[0].PickupAddress1;
                this.PickupAddress2 = mc[0].PickupAddress2;
                this.DeliveryDate = mc[0].DeliveryDate;
                this.DeliveryAddress1 = mc[0].DeliveryAddress1;
                this.DeliveryAddress2 = mc[0].DeliveryAddress2;
                this.Distance = mc[0].Distance;
                this.Amount = mc[0].Amount;
                this.SlaveName = mc[0].SlaveName;
                this.PrivateNotes = mc[0].PrivateNotes;
                this.PublicNotes = mc[0].PublicNotes;
            }

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

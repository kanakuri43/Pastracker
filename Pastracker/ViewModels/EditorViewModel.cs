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

        public EditorViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CancelCommand = new DelegateCommand(CancelCommandExecute);
            CompanyCommand = new DelegateCommand(CompanyCommandExecute);
            BranchCommand = new DelegateCommand(BranchCommandExecute);
            EmployeeCommand = new DelegateCommand(EmployeeCommandExecute);
            DocumentCommand = new DelegateCommand(DocumentCommandExecute);

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
            MovingContents reportPaper = new MovingContents();
            FixedPage fixedPage = new FixedPage();
            fixedPage.Children.Add(reportPaper);

            // A4縦
            fixedPage.Width = 8.27 * 96;
            fixedPage.Height = 11.69 * 96;
            PageContent pc = new PageContent();
            ((IAddChild)pc).AddChild(fixedPage);
            FixedDocument fixedDocument = new FixedDocument();
            fixedDocument.Pages.Add(pc);

            string outputDirectory = System.AppDomain.CurrentDomain.BaseDirectory + "pdf";
            string xpsFileName = @"abc.xps";
            string pdfFileName = @"abc.pdf";
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

    }
}

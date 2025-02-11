using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jg.wpf.core.Command;
using Jg.wpf.core.Notify;
using Jg.wpf.core.Service;
using Jg.wpf.core.Service.FileService;
using Microsoft.Win32;
using RePKG_WPF2.Model;
using RePKG_WPF2.Utility;

namespace RePKG_WPF2.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IFileService _fileService;
        private readonly string _tempFolder;
        private double _progress;

        public ObservableCollection<ExportWorkItem> WorkItems { get; }
        public string ExportPath { get; private set; }
        public double Progress
        {
            get => _progress;
            set
            {
                if (Math.Abs(value - _progress) < 0.0001) return;
                _progress = value;
                RaisePropertyChanged(nameof(Progress));
            }
        }

        public JCommand AddCommand { get; }
        public JCommand SetExportPathCommand { get; }
        public JCommand StartExportCommand { get; }
        
        public MainWindowViewModel()
        {
            _fileService = ServiceManager.GetService<IFileService>();
            _tempFolder = Path.GetTempPath();

            ExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");

            WorkItems = new ObservableCollection<ExportWorkItem>();
            WorkItems.CollectionChanged += OnWorkItemsChanged;

            AddCommand = new JCommand("AddCommand", OnAdd);
            SetExportPathCommand = new JCommand("SetExportPathCommand", OnSetExportPath);
            StartExportCommand = new JCommand("StartExportCommand", OnStartExport);
        }

        private void OnStartExport(object obj)
        {
            Progress = 0;

            Task.Run(() =>
            {
                try
                {
                    double current = 0d;
                    int total = WorkItems.Count;
                    foreach (var workItem in WorkItems)
                    {
                        var directory = workItem.ExportPath;
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        var str = CMD.RunCmd(_tempFolder + @"\RePKG.exe extract """ + workItem.Name + @""" -o """ + workItem.ExportPath + @"\" + workItem.Id) + @"""";
                        current++;
                        Progress = Math.Round(current / total * 100, 1);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
        private void OnSetExportPath(object obj)
        {
            var folder = _fileService.GetFolder();

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            ExportPath = folder;

            foreach (var exportWorkItem in WorkItems)
            {
                exportWorkItem.ExportPath = ExportPath;
            }
        }
        private void OnAdd(object obj)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "PKG文件(*.PKG)|*.pkg";
            openDialog.Multiselect = true;

            if (openDialog.ShowDialog() == true)
            {
                foreach (var file in openDialog.FileNames)
                {
                    var item = WorkItems.FirstOrDefault(w => w.Name == file);
                    if (item == null)
                    {
                        WorkItems.Add(new ExportWorkItem($"{WorkItems.Count + 1}", file, ExportPath));
                    }
                }
            }
        }
        private void OnWorkItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    for (var i = 0; i < WorkItems.Count; i++)
                    {
                        var item = WorkItems[i];
                        item.Id = $"{i + 1}";
                    }
                    break;
            }
        }
    }
}

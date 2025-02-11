using Jg.wpf.core.Notify;

namespace RePKG_WPF2.Model
{
    public class ExportWorkItem : ViewModelBase
    {
        private string _id;
        private string _exportPath;

        public string Id
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        public string Name { get; set; }

        public string ExportPath
        {
            get => _exportPath;
            set
            {
                if (value == _exportPath) return;
                _exportPath = value;
                RaisePropertyChanged(nameof(ExportPath));
            }
        }

        public ExportWorkItem(string id, string name, string exportPath)
        {
            _id = id;
            Name = name;
            _exportPath = exportPath;
        }
    }
}

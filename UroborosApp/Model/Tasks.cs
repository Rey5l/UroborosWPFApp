//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UroborosApp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Tasks : INotifyPropertyChanged
    {


        private bool _isAnswerVisible;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool IsAnswerVisible
        {
            get { return _isAnswerVisible; }
            set
            {
                if (_isAnswerVisible != value)
                {
                    _isAnswerVisible = value;
                    OnPropertyChanged(nameof(IsAnswerVisible));
                }
            }
        }

        public int id { get; set; }
        public int material_id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
    
        public virtual Material Material { get; set; }
    }
}

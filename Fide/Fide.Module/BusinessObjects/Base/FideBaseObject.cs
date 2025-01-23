using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel;

namespace Fide.Module.BusinessObjects.Base;

public class FideBaseObject : BaseObject, INotifyPropertyChanging, INotifyPropertyChanged
{
    public virtual DateTime CreationDate { get; set; }

    public override void OnCreated()
    {
        base.OnCreated();

        CreationDate = DateTime.UtcNow;
    }

    public event PropertyChangingEventHandler PropertyChanging;
    public event PropertyChangedEventHandler PropertyChanged;
}

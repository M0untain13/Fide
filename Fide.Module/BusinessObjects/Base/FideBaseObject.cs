using DevExpress.Persistent.BaseImpl.EF;

namespace Fide.Module.BusinessObjects.Base;

public class FideBaseObject : BaseObject
{
    public virtual DateTime CreationDate { get; set; }

    public override void OnCreated()
    {
        base.OnCreated();

        CreationDate = DateTime.UtcNow;
    }
}

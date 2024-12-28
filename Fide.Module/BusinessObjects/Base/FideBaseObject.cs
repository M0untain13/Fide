using DevExpress.Persistent.BaseImpl.EF;

namespace Fide.Module.BusinessObjects.Base;

public class FideBaseObject : BaseObject
{
    private readonly DateTime _creationDate;
    public DateTime CreationDate => _creationDate;

    public FideBaseObject() : base()
    {
        _creationDate = DateTime.Now;
    }
}

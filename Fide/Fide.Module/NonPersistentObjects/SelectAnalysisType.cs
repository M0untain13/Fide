using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using Fide.Module.Enums;
using System.ComponentModel;

namespace Fide.Module.NonPersistentObjects
{
    [DomainComponent]
    [DefaultProperty(nameof(AnalysisType))]
    public class SelectAnalysisType : IXafEntityObject, INotifyPropertyChanged
    {
        public SelectAnalysisType()
        {
            Oid = Guid.NewGuid();
        }

        [DevExpress.ExpressApp.Data.Key]
        [Browsable(false)]
        public Guid Oid { get; set; }

        public AnalysisEnum AnalysisType { get; set; }

        #region IXafEntityObject members (see https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.IXafEntityObject)
        void IXafEntityObject.OnCreated()
        {
            // Place the entity initialization code here.
            // You can initialize reference properties using Object Space methods; e.g.:
            // this.Address = objectSpace.CreateObject<Address>();
        }
        void IXafEntityObject.OnLoaded()
        {
            // Place the code that is executed each time the entity is loaded here.
        }
        void IXafEntityObject.OnSaving()
        {
            // Place the code that is executed each time the entity is saved here.
        }
        #endregion

        #region IObjectSpaceLink members (see https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.IObjectSpaceLink)
        // If you implement this interface, handle the NonPersistentObjectSpace.ObjectGetting event and find or create a copy of the source object in the current Object Space.
        // Use the Object Space to access other entities (see https://docs.devexpress.com/eXpressAppFramework/113707/data-manipulation-and-business-logic/object-space).
        //IObjectSpace IObjectSpaceLink.ObjectSpace {
        //    get { return objectSpace; }
        //    set { objectSpace = value; }
        //}
        #endregion

        #region INotifyPropertyChanged members (see https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-8.0&redirectedfrom=MSDN)
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
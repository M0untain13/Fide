using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Fide.Module.BusinessObjects;
using Fide.Module.NonPersistentObjects;
using Fide.Module.Services;

namespace Fide.Module.Controllers;

public partial class AnalysisController : ViewController<DetailView>
{
    private readonly PopupWindowShowAction _analysisAction;

    public AnalysisController()
    {
        TargetObjectType = typeof(ImageAnalysis);

        _analysisAction = new()
        {
            Id = "AnalysisAction",
            Caption = "Анализировать",
        };

        Actions.Add(_analysisAction);
    }

    protected override void OnActivated()
    {
        base.OnActivated();

        _analysisAction.CustomizePopupWindowParams += AnalysisAction_CustomizePopupWindowParams;
    }

    protected override void OnDeactivated()
    {
        _analysisAction.CustomizePopupWindowParams -= AnalysisAction_CustomizePopupWindowParams;

        base.OnDeactivated();
    }

    private void AnalysisAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
    {
        var localObjectSpace = Application.CreateObjectSpace(typeof(SelectImageForAnalysis));

        var detailView = Application.CreateDetailView(
            localObjectSpace,
            Application.FindDetailViewId(typeof(SelectImageForAnalysis)),
            true
        );

        var imageAnalysis = View.CurrentObject as ImageAnalysis;
        var selectImageForAnalysis = new SelectImageForAnalysis()
        {
            ObjectSpace = localObjectSpace,
            SelectedImage = imageAnalysis,
        };
        detailView.CurrentObject = selectImageForAnalysis;

        e.View = detailView;
        e.DialogController.Accepting += (sender, args) =>
        {
            var selectedImageAnalysis = selectImageForAnalysis.SelectedImage;
            var analysisService = ObjectSpace.ServiceProvider.GetService(typeof(IAnalysisService)) as IAnalysisService;
            analysisService.StartAnalysis(selectImageForAnalysis);
            ObjectSpace.CommitChanges();
            View.Refresh();
        };
    }
}

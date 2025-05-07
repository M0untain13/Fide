IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250507155720_AddAnalysisFieldAndRenameUrlField')
BEGIN

EXEC sp_rename N'[ImageLinks].[Url]', N'OriginalName', N'COLUMN';
ALTER TABLE [ImageLinks] ADD [AnalysisName] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250507155720_AddAnalysisFieldAndRenameUrlField', N'8.0.11');

END;
GO


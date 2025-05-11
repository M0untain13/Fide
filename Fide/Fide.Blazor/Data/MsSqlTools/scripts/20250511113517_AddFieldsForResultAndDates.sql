IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20250511113517_AddFieldsForResultAndDates')
BEGIN

ALTER TABLE [ImageLinks] ADD [AnalysisRequested] datetime2 NULL;
ALTER TABLE [ImageLinks] ADD [AnalysisResult] float NULL;
ALTER TABLE [ImageLinks] ADD [Created] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250511113517_AddFieldsForResultAndDates', N'8.0.11');

END;
GO



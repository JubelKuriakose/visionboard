CREATE TABLE [dbo].[ErrorLog] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [ClassName]    VARCHAR (50)  NULL,
    [MethodName]   VARCHAR (100) NULL,
    [ErrorMessage] VARCHAR (MAX) NULL,
    [DateTime]     DATETIME      NULL,
    CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);


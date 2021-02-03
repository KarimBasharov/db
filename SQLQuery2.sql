CREATE TABLE [dbo].[opilane] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [Nimi]          VARCHAR (30) NULL,
    [Perekonnanimi] VARCHAR (30) NULL,
    [Adress]          VARCHAR (30) NOT NULL,
    [Number] VARCHAR (30) NOT NULL,
    [Email]      VARCHAR (30) NULL,
    [Foto]          TEXT         NULL,
    [GruppId]       INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_opilane_ToGruppid] FOREIGN KEY ([GruppId]) REFERENCES [dbo].[Gruppid] ([Id])
);
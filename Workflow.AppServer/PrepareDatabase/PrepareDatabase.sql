CREATE SCHEMA Workflow
GO
CREATE TABLE Workflow.EventJournal (
  Ordering BIGINT IDENTITY(1,1) NOT NULL,
  PersistenceID NVARCHAR(255) NOT NULL,
  SequenceNr BIGINT NOT NULL,
  Timestamp BIGINT NOT NULL,
  IsDeleted BIT NOT NULL,
  Manifest NVARCHAR(500) NOT NULL,
  Payload VARBINARY(MAX) NOT NULL,
  Tags NVARCHAR(100) NULL,
  SerializerId INTEGER NULL
  CONSTRAINT PK_EventJournal PRIMARY KEY (Ordering),
  CONSTRAINT QU_EventJournal UNIQUE (PersistenceID, SequenceNr)
);
GO
CREATE TABLE Workflow.SnapshotStore (
  PersistenceID NVARCHAR(255) NOT NULL,
  SequenceNr BIGINT NOT NULL,
  Timestamp DATETIME2 NOT NULL,
  Manifest NVARCHAR(500) NOT NULL,
  Snapshot VARBINARY(MAX) NOT NULL,
  SerializerId INTEGER NULL
  CONSTRAINT PK_SnapshotStore PRIMARY KEY (PersistenceID, SequenceNr)
);
GO
CREATE TABLE Workflow.Metadata (
  PersistenceID NVARCHAR(255) NOT NULL,
  SequenceNr BIGINT NOT NULL,
  CONSTRAINT PK_Metadata PRIMARY KEY (PersistenceID, SequenceNr)
);
GO
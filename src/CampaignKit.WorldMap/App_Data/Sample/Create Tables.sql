-- Script Date: 2019-01-28 7:21 PM  - ErikEJ.SqlCeScripting version 3.5.2.80
DROP TABLE [Maps];
CREATE TABLE [Maps] (
  [MapId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [Name] text NULL
, [Secret] text NULL
, [IsPublic] integer DEFAULT 0 NOT NULL

, [AdjustedSize] integer NOT NULL
, [ContentType] text NULL
, [Copyright] text NULL

, [FileExtension] text NULL
, [MaxZoomLevel] integer NOT NULL
, [RepeatMapInX] integer NOT NULL

, [WorldFolderPath] text NULL
, [ThumbnailPath] text NULL

, [MarkerData] text NULL

, [UserId] text NOT NULL

, [CreationTimestamp] datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
, [UpdateTimestamp] datetime NOT NULL DEFAULT CURRENT_TIMESTAMP

);
CREATE INDEX [Maps_Maps_Maps_UserId] ON [Maps] ([UserId] ASC);
CREATE INDEX [Maps_Maps_Maps_MapSecret] ON [Maps] ([Secret] ASC);

-- Script Date: 2019-01-28 7:24 PM  - ErikEJ.SqlCeScripting version 3.5.2.80
DROP TABLE [Tiles];
CREATE TABLE [Tiles] (
  [TileId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [ZoomLevel] bigint NOT NULL
, [CreationTimestamp] datetime NOT NULL
, [CompletionTimestamp] datetime NOT NULL
, [TileSize] bigint NOT NULL
, [X] bigint NOT NULL
, [Y] bigint NOT NULL
, [MapId] bigint NOT NULL
, CONSTRAINT [FK_Tiles_0_0] FOREIGN KEY ([MapId]) REFERENCES [Maps] ([MapId]) ON DELETE CASCADE ON UPDATE NO ACTION
);
CREATE INDEX [Tiles_IX_Tiles_MapId] ON [Tiles] ([MapId] ASC);
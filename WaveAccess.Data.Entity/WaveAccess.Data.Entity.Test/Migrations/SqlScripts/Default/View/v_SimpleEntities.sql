IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_SimpleEntities]'))
DROP VIEW [dbo].[v_SimpleEntities]

GO

CREATE VIEW [dbo].[v_SimpleEntities]
AS
SELECT        Id, Name, Version
FROM    dbo.SimpleEntities
where version > 0
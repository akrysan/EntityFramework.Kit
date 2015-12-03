IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_GroupHierarchy]'))
DROP VIEW [dbo].[v_GroupHierarchy]

GO

CREATE VIEW [dbo].[v_GroupHierarchy]
AS
	WITH Tree (ID, ParentId, Depth, RootId, [Level]) AS
	(
		SELECT Id, Id, 0,
			case when ParentGroupID is NULL then Id else NULL end,
			case when ParentGroupID is NULL then 0 else NULL end
		FROM Groups
		UNION ALL
		SELECT gr.ID, Parent.ParentId, Parent.Depth + 1, Parent.RootId, Parent.[Level] + 1
		FROM Groups gr
		INNER JOIN Tree as Parent ON Parent.ID = gr.ParentGroupID
	)
	SELECT Id as ChildId, ParentId, Depth, RootId, [Level] FROM Tree

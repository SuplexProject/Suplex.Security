SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Steve Shortt
-- Create date: 08/24/2007
-- Update:		07/20/2013, added Rls resolution
-- Modified:	05/10/2016, expanding default group bitmask
-- Modified:	10/28/2016, expanding default group to varbinary(max)
-- Description:	Select User by Name
-- =============================================
create PROCEDURE [splx].[splx_dal_sel_userbyname]

	@USER_NAME				varchar(50)
	,@RESOLVE_RLS			bit = 0
	,@EXTERNAL_GROUP_LIST	varchar(max) = null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE
		splx.SPLX_USERS
	SET
		USER_LAST_LOGIN = GETDATE()
	WHERE
		USER_NAME = @USER_NAME

	IF @RESOLVE_RLS = 0
	BEGIN
		SELECT
			*
			,NULL AS 'GROUP_MEMBERSHIP_MASK'
		FROM
			splx.SPLX_USERS
		WHERE
			USER_NAME = @USER_NAME
	END
	
	ELSE
	BEGIN

		DECLARE @SPLX_USER_ID uniqueidentifier
		SELECT
			@SPLX_USER_ID = SPLX_USER_ID
		FROM
			splx.SPLX_USERS
		WHERE
			USER_NAME = @USER_NAME

		--- group membership --------------------------------------------
		CREATE TABLE #gm (
			[SPLX_GROUP_ID] [uniqueidentifier] NOT NULL
			,[GROUP_MASK] varbinary(max) NOT NULL
		);

		INSERT INTO #gm
			SELECT
				gm.SPLX_GROUP_ID
				,g.GROUP_MASK
			FROM
				splx.SPLX_GROUP_MEMBERSHIP gm
					INNER JOIN
				splx.SPLX_GROUPS g ON gm.SPLX_GROUP_ID = g.SPLX_GROUP_ID
					INNER JOIN
				splx.SPLX_USERS u ON gm.SPLX_USER_ID = u.SPLX_USER_ID
			WHERE
				gm.SPLX_USER_ID = @SPLX_USER_ID
				AND
				g.GROUP_ENABLED = 1
				AND
				u.USER_ENABLED = 1

			UNION

			SELECT
				SPLX_GROUP_ID
				,GROUP_MASK
			FROM
				splx.SPLX_GROUPS g
					INNER JOIN
				(SELECT DISTINCT value AS GROUP_NAME FROM ap_Split( @EXTERNAL_GROUP_LIST, ',' )) eg
					ON g.GROUP_NAME = eg.GROUP_NAME
			WHERE
				g.GROUP_ENABLED = 1
				AND
				g.GROUP_LOCAL = 0
		--- group membership --------------------------------------------


		--- nested group membership --------------------------------------------
		CREATE TABLE #nestedgm (
			[SPLX_GROUP_ID] [uniqueidentifier] NOT NULL
			,[GROUP_MASK] varbinary(max) NOT NULL
		);

		--recurse up from user
		WITH nested ( PARENT_GROUP_ID, GROUP_MASK )
		AS
		(
			SELECT
				ng.PARENT_GROUP_ID
				,g.GROUP_MASK
			FROM splx.SPLX_NESTED_GROUPS ng
				INNER JOIN
					#gm ON ng.CHILD_GROUP_ID = #gm.SPLX_GROUP_ID
				INNER JOIN
					splx.SPLX_GROUPS g on ng.PARENT_GROUP_ID = g.SPLX_GROUP_ID
					
				UNION ALL
				
			SELECT
				ng.PARENT_GROUP_ID
				,g.GROUP_MASK
			FROM splx.SPLX_NESTED_GROUPS ng
				INNER JOIN
					nested ON ng.CHILD_GROUP_ID = nested.PARENT_GROUP_ID
				INNER JOIN
					splx.SPLX_GROUPS g on ng.PARENT_GROUP_ID = g.SPLX_GROUP_ID
		)
		INSERT INTO #nestedgm
			SELECT DISTINCT PARENT_GROUP_ID, GROUP_MASK FROM nested
		--- nested group membership --------------------------------------------


		--- total trustees --------------------------------------------
		CREATE TABLE #trustees_mask (
			[GROUP_MASK] varbinary(max) NOT NULL
		);
		INSERT INTO #trustees_mask
				SELECT GROUP_MASK FROM #gm
					UNION
				SELECT GROUP_MASK FROM #nestedgm
		--- total trustees --------------------------------------------



		SELECT
			*
			,splx.Splx_GetTableOr( '#trustees_mask', 'GROUP_MASK' ) AS 'GROUP_MEMBERSHIP_MASK'
		FROM
			splx.SPLX_USERS
		WHERE
			USER_NAME = @USER_NAME

	END

END
ALTER FUNCTION [SPLX].[Splx_GetTableOr](@tableName [nvarchar](max), @maskFieldName [nvarchar](max))
RETURNS [varbinary](max) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [Splx_BitLib].[BinaryUtil].[GetTableOr]
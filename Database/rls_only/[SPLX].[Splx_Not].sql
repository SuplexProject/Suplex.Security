ALTER FUNCTION [SPLX].[Splx_Not](@compareValue [varbinary](max))
RETURNS [varbinary](max) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [Splx_BitLib].[BinaryUtil].[Not]
ALTER FUNCTION [SPLX].[Splx_Xor](@sourceValue [varbinary](max), @compareValue [varbinary](max))
RETURNS [varbinary](max) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [Splx_BitLib].[BinaryUtil].[Xor]
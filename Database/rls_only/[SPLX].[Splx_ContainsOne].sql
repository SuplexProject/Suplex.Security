ALTER FUNCTION [SPLX].[Splx_ContainsOne](@sourceValue [varbinary](max), @compareValue [varbinary](max))
RETURNS [bit] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [Splx_BitLib].[BinaryUtil].[ContainsOne]
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[PadString] 

(@Seq varchar(16),
@PadWith char(1),
@PadLength int
) 

RETURNS varchar(16) AS

BEGIN 

declare @curSeq varchar(16)

SELECT @curSeq = ISNULL(REPLICATE(@PadWith, @PadLength - len(ISNULL(@Seq ,0))), '') + @Seq

RETURN @curSeq

END
GO

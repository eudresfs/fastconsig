SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
create FUNCTION [dbo].[AumentaCompetencia] 

(@competencia varchar(7),
@prazo int
) 

RETURNS varchar(7) AS

BEGIN 

declare @mes varchar(7)
declare @nmes int
declare @nano int

set @mes = @competencia
while (@prazo > 1) begin
	set @prazo = @prazo - 1

	set @nano = CONVERT(int,substring(@mes,1,4))
	set @nmes = CONVERT(int,substring(@mes,6,2))
	
	if (@nmes = 12) begin
		set @nmes = 1
		set @nano = @nano +1
	end
	else begin
		set @nmes = @nmes +1	
	end
		
	set @mes = CONVERT(varchar(4),@nano)+'/'+dbo.PadString(CONVERT(varchar(2),@nmes),'0',2)
end


RETURN @mes

END
GO

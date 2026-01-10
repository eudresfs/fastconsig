GO
/****** Object:  UserDefinedFunction [dbo].[AumentaCompetencia]    Script Date: 02/29/2012 10:25:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[AnoMesCorte] 

(
) 

RETURNS varchar(7) AS

BEGIN 

declare @anomes varchar(7)
declare @dia int
declare @mes int
declare @ano int
declare @diacorte int
declare @diacortehist int

select @diacorte = CONVERT(int,valor) from Parametro where Nome = 'DiaCorte'
select @dia=DATEPART(day,getdate())
select @mes=DATEPART(month,getdate())
select @ano=DATEPART(year,getdate())

set @anomes = CONVERT(varchar(4),@ano)+'/'+dbo.PadString(CONVERT(varchar(2),@mes),'0',2)

select @diacortehist=isnull(diacorte,0) from CorteHistorico where competencia=@anomes

if (@diacortehist > 0 and @diacortehist <> @diacorte)
	set @diacorte = @diacortehist
	
if (@dia > @diacorte)
	set @anomes = dbo.AumentaCompetencia(@anomes,2)

return @anomes
END
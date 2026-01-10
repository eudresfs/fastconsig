SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE [dbo].[SP_ImportacaoAverbacao_Parcelas] @xidcontrato int, @competencia varchar(7), @parcelacorrente int
AS
BEGIN
    declare @idcontrato int
    declare @prazo int
    declare @ano_mes char(7)
    declare @mes char(7)
	declare @valor numeric(15,2)
	declare @contador int
	declare @numero int
	declare @idservicotipo int
	declare @parcelasituacao int
	
    set @contador = 1
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    DECLARE contratos_parc CURSOR FOR

	select idaverbacao,prazo,valorparcela, p.idprodutogrupo from averbacao c, produto p
	where idaverbacao = @xidcontrato and c.idproduto = p.idproduto

    OPEN contratos_parc

	FETCH NEXT FROM contratos_parc INTO @idcontrato,@prazo,@valor,@idservicotipo

	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @contador = @prazo

       delete from AverbacaoParcela where IDAverbacao = @idcontrato
          
		set @mes = @competencia

		set @competencia = '2011/11'
/*		while @contador > 0
        begin         
		    if substring(@mes,6,2) = '01'
	          set @mes = convert(varchar(4),convert(int,substring(@mes,1,4))-1)+'/12'
			else
			begin
				if convert(int,substring(@mes,6,2)) <= 10
	              set @mes = substring(@mes,1,4)+'/0'+convert(varchar(2),convert(int,substring(@mes,6,2))-1)
				else 
                  set @mes = substring(@mes,1,4)+'/'+convert(varchar(2),convert(int,substring(@mes,6,2))-1)
			end
            SET @CONTADOR = @CONTADOR - 1
        END          
        
        update Averbacao set competenciainicial = @mes where IDAverbacao = @idcontrato
  */      
		set @contador = @prazo   
		set @numero = 0       
		while @contador > 0
        begin		
            set @parcelasituacao = 1
			if (@mes < @competencia)      
               set @parcelasituacao = 3
               
			set @numero = @numero + 1
			insert into AverbacaoParcela(IDAverbacao,IDAverbacaoParcelaSituacao,numero,competencia,valor,Ativo)
			values (@idcontrato,@parcelasituacao,@numero,@mes,@valor,1)

			set @contador = @contador - 1
			
			if (@contador > 0) begin
			  if substring(@mes,6,2) = '12'
	            set @mes = convert(varchar(4),convert(int,substring(@mes,1,4))+1)+'/01'
			  else  begin
				if convert(int,substring(@mes,6,2)) < 9
	              set @mes = substring(@mes,1,4)+'/0'+convert(varchar(2),convert(int,substring(@mes,6,2))+1)
				else 
                  set @mes = substring(@mes,1,4)+'/'+convert(varchar(2),convert(int,substring(@mes,6,2))+1)
			  end
			end  
		end
        update Averbacao set competenciafinal = @mes where IDAverbacao = @idcontrato

		FETCH NEXT FROM contratos_parc INTO @idcontrato,@prazo,@ano_mes,@valor
	END
	CLOSE contratos_parc
	DEALLOCATE contratos_parc
END
GO

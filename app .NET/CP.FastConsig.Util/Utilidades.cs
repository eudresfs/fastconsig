using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CP.FastConsig.Util
{
	public static class Utilidades
	{
		public static bool ExisteItemVazio(params string[] itens)
		{
			return itens.Any(x => string.IsNullOrEmpty(x));
		}

		public static bool ValidaEmail(string email)
		{
			Regex regex = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
			return regex.IsMatch(email);
		}

		public static bool ValidaCNPJ(string cnpj)
		{
			int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

			int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

			int soma;

			int resto;

			string digito;

			string tempCnpj;

			cnpj = cnpj.Trim();

			cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

			if (cnpj.Length != 14)

				return false;

			tempCnpj = cnpj.Substring(0, 12);

			soma = 0;

			for (int i = 0; i < 12; i++)

				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

			resto = (soma % 11);

			if (resto < 2)

				resto = 0;

			else

				resto = 11 - resto;

			digito = resto.ToString();

			tempCnpj = tempCnpj + digito;

			soma = 0;

			for (int i = 0; i < 13; i++)

				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

			resto = (soma % 11);

			if (resto < 2)

				resto = 0;

			else

				resto = 11 - resto;

			digito = digito + resto.ToString();

			return cnpj.EndsWith(digito);
		}

		public static bool ValidaCPF(string cpf)
		{
			string valor = cpf;

			if (valor.Length != 11) return false;

			bool igual = true;

			for (int i = 1; i < 11 && igual; i++) if (valor[i] != valor[0]) igual = false;

			if (igual || valor == "12345678909") return false;

			int[] numeros = new int[11];

			for (int i = 0; i < 11; i++) numeros[i] = int.Parse(valor[i].ToString());

			int soma = 0;

			for (int i = 0; i < 9; i++) soma += (10 - i) * numeros[i];

			int resultado = soma % 11;

			if (resultado == 1 || resultado == 0)
			{
				if (numeros[9] != 0) return false;
			}
			else if (numeros[9] != 11 - resultado)
			{
				return false;
			}

			soma = 0;

			for (int i = 0; i < 10; i++) soma += (11 - i) * numeros[i];

			resultado = soma % 11;

			if (resultado == 1 || resultado == 0)
			{
				if (numeros[10] != 0) return false;
			}
			else if (numeros[10] != 11 - resultado)
			{
				return false;
			}

			return true;
		}

		public static string RetornaStringValor(double parcela)
		{
			return String.Format("{0:C}", parcela);
		}

		public static string CompetenciaDiminui(string competencia)
		{
			return CompetenciaDiminui(competencia, 1);
		}

		public static string CompetenciaDiminui(string competencia, int qtde)
		{
			if (string.IsNullOrEmpty(competencia) || competencia.Length != 7)
				return "";

			int ano;
			int mes;

			int.TryParse(competencia.Substring(0, 4), out ano);
			int.TryParse(competencia.Substring(5, 2), out mes);

			if (ano.Equals(0) || mes.Equals(0)) return "3523/64"; // Retorna uma data inválida qualquer.

			for (int i = 0; i < qtde; i++)
			{
				if (mes == 1)
				{
					mes = 12;
					ano--;
				}
				else
				{
					mes--;
				}
			}

			return ano.ToString() + "/" + mes.ToString().PadLeft(2, '0');
		}

		public static string CompetenciaAumenta(string anomes, int qtde)
		{
			if (string.IsNullOrEmpty(anomes) || anomes.Length != 7)
				return "";

			int ano = Convert.ToInt32(anomes.Substring(0, 4));
			int mes = Convert.ToInt32(anomes.Substring(5, 2));

			for (int i = 0; i < qtde; i++)
			{
				if (mes == 12)
				{
					mes = 1;
					ano++;
				}
				else
				{
					mes++;
				}
			}

			return ano.ToString() + "/" + mes.ToString().PadLeft(2, '0');
		}

		public static string CalculaCompetenciaFinal(string anomes, int qtde)
		{
			if (string.IsNullOrEmpty(anomes) || anomes.Length != 7)
				return "";

			return CompetenciaAumenta(anomes, qtde - 1);
		}

		public static string ConverteMesAno(string mesano)
		{
			if (string.IsNullOrEmpty(mesano) || mesano.Length != 7)
				return "";
			return mesano.Substring(5, 2) + "/" + mesano.Substring(0, 4);
		}

		public static string ConverteAnoMes(string anomes)
		{
			if (string.IsNullOrEmpty(anomes) || anomes.Length != 7)
				return "";
			return anomes.Substring(3, 4) + "/" + anomes.Substring(0, 2);
		}

		public static string RetornaStringCpf(string cpf)
		{
			string retorno = cpf.Insert(3, ".");
			retorno = retorno.Insert(7, ".");
			retorno = retorno.Insert(11, "-");
			return retorno;
		}

		public static string MascaraCPF(string cpf)
		{
			return string.Format(@"{0:000\.000\.000\-00}", cpf);
		}

		public static string MascaraCNPJ(string cnpj)
		{
			return string.Format(@"{0:00\.000\.000\/0000\-00}", cnpj);
		}

		public static string RemoveCaracteresEspeciais(string entrada)
		{
			return Regex.Replace(entrada, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
		}

		public static string RetornaStringMes(int month)
		{
			string[] meses = {
								 "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro",
								 "Outubro", "Novembro", "Dezembro"
							 };
			return meses[month - 1];
		}

		public static int SomaDiasUteis(int dias)
		{
			DateTime dia = DateTime.Today;
			int cont = 0;
			int somadias = 0;

			while (cont < dias)
			{
				dia = dia.AddDays(1);
				if (dia.DayOfWeek != DayOfWeek.Saturday && dia.DayOfWeek != DayOfWeek.Sunday)
					cont++;
				somadias++;
			}
			return somadias;
		}

		public static DateTime ObtemProximoDiaUtil(int dias)
		{
			DateTime dia = DateTime.Today;

			int contador = 0;

			while (contador < dias)
			{
				dia = dia.AddDays(1);

				if (dia.DayOfWeek == DayOfWeek.Saturday || dia.DayOfWeek == DayOfWeek.Sunday) continue;

				contador++;
			}

			return dia;
		}
	}
}